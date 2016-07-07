﻿using System;
using System.Security.Claims;

namespace WpfOidcClient.OidcClient
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public ClaimsPrincipal User { get; set; }

        public string AccessToken { get; set; }
        public string IdentityToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }

        public LoginResult()
        {
            Success = false;
        }
    }
}