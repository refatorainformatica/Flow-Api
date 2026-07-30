using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.Infrastructure.Firebase
{
    public class FirebaseTokenInfo
    {
        [JsonPropertyName("sign_in_provider")]
        public string SignInProvider { get; set; } = "";

        [JsonPropertyName("identities")]
        public Identities Identities { get; set; } = null!;
    }

    public class Identities
    {
        [JsonPropertyName("email")]
        public List<string> Email { get; set; } = new List<string>();

        [JsonPropertyName("phone")]
        public List<string> Phone { get; set; } = new List<string>();
    }
}
