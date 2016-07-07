using IdentityModel;
using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols;
using mshtml;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.IdentityModel.Tokens;
using WpfOidcClient.OidcClient;
using SecurityToken = System.IdentityModel.Tokens.SecurityToken;

namespace WpfOidcClient
{
    public partial class LoginWebView : Window
    {
        public event EventHandler<LoginResult> Done;

        OpenIdConnectConfiguration _config;
        OidcSettings _settings;

        string _nonce;
        string _state;

        public LoginWebView()
        {
            InitializeComponent();

            webView.Navigating += webView_Navigating;
            Closing += LoginWebView_Closing;
        }

        public async Task LoginAsync(OidcSettings settings)
        {
            _settings = settings;

            if (_config == null)
            {
                //await LoadOpenIdConnectConfigurationAsync();
            }

            this.Visibility = Visibility.Visible;
            webView.Navigate(CreateUrl());
        }

        private string CreateUrl()
        {
            _nonce = CryptoRandom.CreateUniqueId(32);
            _state = CryptoRandom.CreateUniqueId(32);

            var request = new AuthorizeRequest(_settings.Authority + "/connect/authorize");

            return request.CreateAuthorizeUrl(
                clientId: _settings.ClientId,
                responseType: "code",
                scope: _settings.Scope,
                redirectUri: _settings.RedirectUri,
                state: _state,
                nonce: _nonce
                );
        }

        void LoginWebView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }

        private async void webView_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            AuthorizeResponse response = null;

            if (e.Uri.ToString().StartsWith(_settings.RedirectUri))
            {
                if (e.Uri.AbsoluteUri.Contains("code"))
                {
                    response = new AuthorizeResponse(e.Uri.AbsoluteUri);
                }

                e.Cancel = true;
                this.Visibility = Visibility.Hidden;

                var loginResult = await ValidateResponseAsync(response);
                Done.Invoke(this, loginResult);
            }
        }

        private async Task<LoginResult> ValidateResponseAsync(AuthorizeResponse response)
        {
            // exchange the code for a token
            var tokenClient = new TokenClient(
                _settings.Authority + "/connect/token",
                _settings.ClientId,
                _settings.ClientSecret);

            var tokenResponse = await tokenClient.RequestAuthorizationCodeAsync(
                code: response.Code,
                redirectUri: _settings.RedirectUri);

            if (tokenResponse.IsError)
            {
                return new LoginResult {ErrorMessage = tokenResponse.Error};
            }

            return new LoginResult
            {
                Success = true,
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken,
                AccessTokenExpiration = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn)
            };
        }
    }
}