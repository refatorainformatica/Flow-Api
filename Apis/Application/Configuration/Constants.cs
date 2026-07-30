using System.Diagnostics.CodeAnalysis;

namespace Application.Configuration
{
    [ExcludeFromCodeCoverage]
    internal static class Constants
    {
        internal const string Services = "Services";

        internal const string HealtCheckURLLiveness = "/health/liveness";
        internal const string HealtCheckURLReadness = "/health/readness";
        internal const string HealtCheckURL = "/hc";
        internal const string HealtCheckUIURL = "/hcui";

        internal const string ApiName = "'Genial.Energia.Mono'";
        internal const string ApiDescription = "'Genial.Energia.Mono'";
        internal const string TagReadness = "Readness";

        internal const string EnvironmentVariables = "EnvironmentVariables";

        internal static string TYPE_STATUS_200 = "https://www.genial.com.vc/techdocs/httpcodes#200";
        internal static string TYPE_STATUS_201 = "https://www.genial.com.vc/techdocs/httpcodes#201";
        internal static string TYPE_STATUS_204 = "https://www.genial.com.vc/techdocs/httpcodes#204";
        internal static string TYPE_STATUS_400 = "https://www.genial.com.vc/techdocs/httpcodes#400";
        internal static string TYPE_STATUS_401 = "https://www.genial.com.vc/techdocs/httpcodes#401";
        internal static string TYPE_STATUS_403 = "https://www.genial.com.vc/techdocs/httpcodes#403";
        internal static string TYPE_STATUS_404 = "https://www.genial.com.vc/techdocs/httpcodes#404";
        internal static string TYPE_STATUS_406 = "https://www.genial.com.vc/techdocs/httpcodes#406";
        internal static string TYPE_STATUS_500 = "https://www.genial.com.vc/techdocs/httpcodes#500";
        internal static string TYPE_STATUS_503 = "https://www.genial.com.vc/techdocs/httpcodes#503";

        internal const string ConnectionStringsSettings = "ConnectionStrings";
        internal const string HttpClient = "HttpClient";
    }
}
