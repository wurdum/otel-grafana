using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace OtelApp.Instrumentation;

public static class OtelExtensions
{
    public const string ServiceName = "otel-app";
    public const string ServiceVersion = "1.0.0";
    private const string Endpoint = "http://otel-collector:4317";
    private const OtlpExportProtocol Protocol = OtlpExportProtocol.Grpc;

    public static OpenTelemetryBuilder WithOtelResource(this OpenTelemetryBuilder builder)
    {
        return builder.ConfigureResource(resource => resource
            .AddService(ServiceName, serviceVersion: ServiceVersion)
            .AddTelemetrySdk());
    }

    public static OpenTelemetryBuilder WithOtelMetrics(this OpenTelemetryBuilder builder)
    {
        return builder.WithMetrics(provider => provider
            .AddMeter(ServiceName)
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddOtlpExporter((exporter, reader) =>
            {
                reader.PeriodicExportingMetricReaderOptions = new()
                {
                    ExportIntervalMilliseconds = 15000,
                    ExportTimeoutMilliseconds = 3000
                };

                exporter.Protocol = Protocol;
                exporter.Endpoint = new(Endpoint);
            }));
    }

    public static OpenTelemetryBuilder WithOtelTraces(this OpenTelemetryBuilder builder)
    {
        return builder.WithTracing(provider => provider
            .AddSource(ServiceName)
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter(exporter =>
            {
                exporter.Protocol = Protocol;
                exporter.Endpoint = new(Endpoint);
            }));
    }

    public static ILoggingBuilder WithOtelLogging(this ILoggingBuilder logging)
    {
        return logging.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(ResourceBuilder
                .CreateDefault()
                .AddService(ServiceName, serviceVersion: ServiceVersion));

            options.IncludeScopes = true;
            options.ParseStateValues = true;
            options.IncludeFormattedMessage = true;

            options.AddOtlpExporter(exporter =>
            {
                exporter.Protocol = Protocol;
                exporter.Endpoint = new(Endpoint);
            });
        });
    }
}
