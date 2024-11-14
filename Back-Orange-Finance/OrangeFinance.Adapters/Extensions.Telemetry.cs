using System.Diagnostics;
using System.Runtime.CompilerServices;

using RabbitMQ.Client;

namespace OrangeFinance.Adapters;

internal static partial class Extension
{
    public static Activity SafeStartActivity(this ActivitySource activitySource, [CallerMemberName] string name = "", ActivityKind kind = ActivityKind.Internal)
    {
        ArgumentNullException.ThrowIfNull(activitySource);
        Activity activity = activitySource.StartActivity(name, kind) ?? new Activity(name);
        activity.SetStartTime(DateTime.UtcNow);
        return activity;
    }
    public static IBasicProperties SetTelemetry(this IBasicProperties basicProperties, Activity activity)
    {
        ArgumentNullException.ThrowIfNull(basicProperties);
        if (activity != null)
        {
            basicProperties
                .SetSpanId(activity.SpanId)
                .SetTraceId(activity.TraceId);
        }
        return basicProperties;
    }

    private static IBasicProperties SetSpanId(this IBasicProperties basicProperties, ActivitySpanId? activitySpanId)
    {
        ArgumentNullException.ThrowIfNull(basicProperties);
        if (activitySpanId != null)
        {
            basicProperties.Headers ??= new Dictionary<string, object>();
            basicProperties.Headers["SpanId"] = activitySpanId.ToString();
        }
        return basicProperties;
    }

    private static IBasicProperties SetTraceId(this IBasicProperties basicProperties, ActivityTraceId? activityTraceId)
    {
        ArgumentNullException.ThrowIfNull(basicProperties);
        if (activityTraceId != null)
        {
            basicProperties.Headers ??= new Dictionary<string, object>();
            basicProperties.Headers["TraceId"] = activityTraceId.ToString();
        }
        return basicProperties;
    }

    public static ActivityTraceId GetTraceId(this IBasicProperties basicProperties)
    {
        if (basicProperties is null) throw new ArgumentNullException(nameof(basicProperties));
        return basicProperties.Headers != null && basicProperties.Headers.ContainsKey("TraceId")
            ? ActivityTraceId.CreateFromString(basicProperties.Headers.AsString("TraceId"))
            : default;
    }

    public static ActivitySpanId GetSpanId(this IBasicProperties basicProperties)
    {
        if (basicProperties is null) throw new ArgumentNullException(nameof(basicProperties));
        if (basicProperties.Headers != null && basicProperties.Headers.ContainsKey("SpanId"))
        {
            return ActivitySpanId.CreateFromString(basicProperties.Headers.AsString("SpanId"));
        }
        return default;
    }
}
