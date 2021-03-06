﻿using System;
using IdentityModel.Client;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FhirClientDotNet;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Newtonsoft.Json;
using WpfOidcClient.OidcClient;

namespace WpfOidcClient
{
    public partial class MainWindow : Window
    {
        LoginWebView _login;
        private string _accessToken;

        public MainWindow()
        {
            InitializeComponent();

            _login = new LoginWebView();
            _login.Done += _login_Done;

            Loaded += MainWindow_Loaded;
            //IdentityTextBox.Visibility = Visibility.Hidden;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _login.Owner = this;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var settings = new OidcSettings
            {
                Authority = "https://tw171.open.allscripts.com/authorization",
                ClientId = "775285A0-C93A-4E89-985F-3D2AE947E2E4",
                ClientSecret = "D8933844EEF5",
                RedirectUri = "urn:ietf:wg:oauth:2.0:oob",
                Scope = "openid patient/*.read launch/patient",
                LoadUserProfile = true
            };

            await _login.LoginAsync(settings);
        }

        private async void GetPatientButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IdentityTextBox.Text = "Retrieving ...";

                // We are wrapping this in a async block to make the UI more responsive
                var patient = await Task.Run(() => GetPatient());

                // display the patient on the UI
                var serializedFHIR = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(patient);
                var formattedSerializedFHIR = FormatJson(serializedFHIR);

                var sb = new StringBuilder(128);
                sb.AppendLine("Fhir Response");
                sb.AppendLine();
                sb.AppendLine(formattedSerializedFHIR);
                IdentityTextBox.Text = sb.ToString();
            }
            catch (Exception ex)
            {

                IdentityTextBox.Text = ex.ToString();
            }
        }

        private Patient GetPatient()
        {
            string strUri = "http://tw171.open.allscripts.com/fhir/";
            var client = new OAuthFhirClient(strUri, _accessToken);
            client.PreferredFormat = ResourceFormat.Json;

            var patient = (Patient) client.Get("Patient/19");
            
            return patient;
        }

        void _login_Done(object sender, LoginResult e)
        {
            if (e.Success)
            {
                var sb = new StringBuilder(128);

                sb.AppendLine("Access token: " + e.AccessToken);
                sb.AppendLine();
                sb.AppendLine("Access token expiration: " + e.AccessTokenExpiration);

                _accessToken = e.AccessToken;

                IdentityTextBox.Text = sb.ToString();

                button_get_patient.IsEnabled = true;
            }
            else
            {
                IdentityTextBox.Text = e.ErrorMessage;
            }
        }

        private string FormatJson(string json)
        {
            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }
    }
}