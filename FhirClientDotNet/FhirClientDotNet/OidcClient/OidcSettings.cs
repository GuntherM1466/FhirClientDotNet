using System.Collections.Generic;

namespace WpfOidcClient.OidcClient
{
    public class OidcSettings
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }
        public string Scope { get; set; }

        public bool LoadUserProfile { get; set; }
        public bool FilterClaims { get; set; }

        public List<string> FilterClaimTypes { get; set; }

        public OidcSettings()
        {
            FilterClaimTypes = new List<string>
            {
                "iss",
                "exp",
                "nbf",
                "aud",
                "nonce",
                "c_hash",
                "iat",
                "auth_time"
            };
            FilterClaims = true;
            LoadUserProfile = false;
        }
    }
}