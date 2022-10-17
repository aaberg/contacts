using System.Text.Json;
using Contacts.Api.Application;
using Contacts.Api.Application.Queries;
using Contacts.Api.Infrastructure;
using Contacts.Domain.Contact;
using Eventuous;
using Eventuous.Diagnostics.OpenTelemetry;
using Eventuous.EventStore;
using Eventuous.EventStore.Subscriptions;
using Eventuous.Projections.MongoDB;
using Eventuous.Subscriptions.Registrations;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Contacts.Api;

public static class Registrations
{
    public static void AddEventuous(this IServiceCollection services, IConfiguration configuration)
    {
        DefaultEventSerializer.SetDefaultSerializer(
            new DefaultEventSerializer(
                new JsonSerializerOptions(JsonSerializerDefaults.Web)
                    .ConfigureForNodaTime(DateTimeZoneProviders.Tzdb)));
        
        services.AddEventStoreClient(configuration["EventStore:ConnectionString"]);
        services.AddAggregateStore<EsdbEventStore>();
        services.AddApplicationService<ContactManagerService, Contact>();

        services.AddSingleton(Mongo.ConfigureMongo(configuration));
        services.AddCheckpointStore<MongoCheckpointStore>();
        
        services.AddSubscription<AllStreamSubscription, AllStreamSubscriptionOptions>(
            "ContactsProjection",
            options => options
                .Configure(subscriptionOptions => subscriptionOptions.ConcurrencyLimit = 2)
                .UseCheckpointStore<MongoCheckpointStore>()
                .AddEventHandler<ContactStateProjection>()
                .WithPartitioningByStream(2)
        );
    }

    public static void AddOpenTelemetry(this IServiceCollection services)
    {
        var otelEnabled = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") != null;
        services.AddOpenTelemetryMetrics(builder =>
        {
            builder
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("contacts"))
                .AddAspNetCoreInstrumentation()
                .AddEventuous()
                .AddEventuousSubscriptions()
                .AddPrometheusExporter();
            if (otelEnabled) builder.AddOtlpExporter();
        });

        services.AddOpenTelemetryTracing(builder =>
        {
            builder.AddAspNetCoreInstrumentation()
                .AddGrpcClientInstrumentation()
                .AddEventuousTracing()
                .AddMongoDBInstrumentation()
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("contacts"))
                .SetSampler(new AlwaysOnSampler());

            if (otelEnabled)
                builder.AddOtlpExporter();
            else
                builder.AddZipkinExporter();
        });
    }
}