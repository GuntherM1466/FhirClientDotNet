namespace WpfOidcClient.OidcClient
{
    class ValidationResult
    {
        public bool Success { get; set; } 
        public string ErrorMessage { get; set; }

        public LoginResult LoginResult { get; set; }

        public ValidationResult()
        {
            Success = false;
        }
    }
}