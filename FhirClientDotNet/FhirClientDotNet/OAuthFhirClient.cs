using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Rest;

namespace FhirClientDotNet
{
    public class OAuthFhirClient : FhirClient
    {
        private string _token;

        /// <summary>
        /// Creates a new client using a default endpoint
        /// If the endpoint does not end with a slash (/), it will be added.
        /// </summary>
        /// <param name="endpoint">
        /// The URL of the server to connect to.<br/>
        /// If the trailing '/' is not present, then it will be appended automatically
        /// </param>
        /// <param name="verifyFhirVersion">
        /// If parameter is set to true the first time a request is made to the server a 
        /// conformance check will be made to check that the FHIR versions are compatible.
        /// When they are not compatible, a FhirException will be thrown.
        /// </param>
        public OAuthFhirClient(Uri endpoint, string token, bool verifyFhirVersion = false)
            : base(endpoint, verifyFhirVersion)
        {
            _token = token;
        }


        /// <summary>
        /// Creates a new client using a default endpoint
        /// If the endpoint does not end with a slash (/), it will be added.
        /// </summary>
        /// <param name="endpoint">
        /// The URL of the server to connect to.<br/>
        /// If the trailing '/' is not present, then it will be appended automatically
        /// </param>
        /// <param name="verifyFhirVersion">
        /// If parameter is set to true the first time a request is made to the server a 
        /// conformance check will be made to check that the FHIR versions are compatible.
        /// When they are not compatible, a FhirException will be thrown.
        /// </param>
        public OAuthFhirClient(string endpoint, string token, bool verifyFhirVersion = false)
            : this(new Uri(endpoint), token, verifyFhirVersion)
        {
        }

        /// <summary>
        /// Inspect or modify the HttpWebRequest just before the FhirClient issues a call to the server
        /// </summary>
        /// <param name="rawRequest">The request as it is about to be sent to the server</param>
        /// <param name="body">The data in the body of the request as it is about to be sent to the server</param>
        protected override void BeforeRequest(HttpWebRequest rawRequest, byte[] body)
        {
            rawRequest.Headers.Add("Authorization", "Bearer " + _token);
        }
    }
}
