using System.Diagnostics;

namespace App.Infrastructure.OpenTelemetry;

public static class ActivitySourceProvider
{
    public static ActivitySource Source = default!;
}
