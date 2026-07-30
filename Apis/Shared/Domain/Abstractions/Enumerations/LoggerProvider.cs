namespace Shared.Domain.Abstractions.Enumerations
{
    public enum LoggerProvider
    {
        ApplicationInsights,
        Seq,
        ElasticSearch,
        Console,
        Debug,
        File,
    }
}
