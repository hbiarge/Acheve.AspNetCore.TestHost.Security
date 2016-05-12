using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNet.TestHost;

namespace Acheve.AspNet.TestHost.Security
{
    public static class TestServerAuthenticationExtensions
    {
        public static HttpClient WithDefaultIdentity(this HttpClient httpClient, IEnumerable<Claim> claims)
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    TestServerAuthenticationDefaults.AuthenticationScheme,
                    DefautClaimsEncoder.Encode(claims));

            return httpClient;
        }

        public static RequestBuilder WithIdentity(this RequestBuilder requestBuilder, IEnumerable<Claim> claims)
        {
            requestBuilder.AddHeader(
                Constants.AuthenticationHeaderName,
                $"{TestServerAuthenticationDefaults.AuthenticationScheme} {DefautClaimsEncoder.Encode(claims)}");

            return requestBuilder;
        }
    }
}
