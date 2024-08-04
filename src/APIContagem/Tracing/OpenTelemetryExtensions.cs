namespace APIContagem.Tracing;

public static class OpenTelemetryExtensions
{
    public static string ServiceName { get; }
    public static string ServiceVersion { get; }

    static OpenTelemetryExtensions()
    {
        ServiceName = "APIContagemPostgreSQL";
        ServiceVersion = typeof(OpenTelemetryExtensions).Assembly.GetName().Version!.ToString();
    }
}