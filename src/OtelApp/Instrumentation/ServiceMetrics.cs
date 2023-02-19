using System.Diagnostics.Metrics;

namespace OtelApp.Instrumentation;

public static class ServiceMetrics
{
    private static readonly Meter Meter = new(OtelExtensions.ServiceName);
    private static readonly Counter<long> Requests = Meter.CreateCounter<long>("requests_total");

    public static void RequestHandled()
    {
        Requests.Add(1);
    }
}
