using System.Diagnostics;

namespace OtelApp.Instrumentation;

public static class ServiceTraces
{
    private static readonly ActivitySource Source = new(OtelExtensions.ServiceName);

    public static void RequestHandled()
    {
        using var activity = Source.StartActivity(nameof(RequestHandled), kind: ActivityKind.Internal);
        activity?.SetTag("customTag", "customValue");
        activity?.AddEvent(new("ChildEvent"));
    }
}
