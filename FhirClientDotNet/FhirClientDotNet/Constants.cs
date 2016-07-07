namespace Sample
{
    public static class Constants
    {
        public const string BaseAddress = "http://g-601322-w10:20295";
        
        public const string AuthorizeEndpoint = BaseAddress + "/connect/authorize";
        public const string LogoutEndpoint = BaseAddress + "/connect/endsession";
        public const string TokenEndpoint = BaseAddress + "/connect/token";
        public const string UserInfoEndpoint = BaseAddress + "/connect/userinfo";
        public const string IdentityTokenValidationEndpoint = BaseAddress + "/connect/identitytokenvalidation";
        public const string TokenRevocationEndpoint = BaseAddress + "/connect/revocation";

        public const string AspNetWebApiSampleApi = "http://localhost:2727/";
        public const string AspNetWebApiSampleApiUsingPoP = "http://localhost:46613/";
    }
}