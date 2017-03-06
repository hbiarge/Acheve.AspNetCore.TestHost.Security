using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.Extensions.Primitives;
using System.Linq;

namespace Acheve.AspNetCore.TestHost.Security
{
    public class TestServerAuthenticationHandler : AuthenticationHandler<TestServerAuthenticationOptions>
    {
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            StringValues authHeaderString;
            var existAuthorizationHeader =
                Context.Request.Headers.TryGetValue(Constants.AuthenticationHeaderName, out authHeaderString);

            if (existAuthorizationHeader == false)
            {
                return Task.FromResult(AuthenticateResult.Fail("No Authorization header present"));
            }

            AuthenticationHeaderValue authHeader;
            var canParse = AuthenticationHeaderValue.TryParse(authHeaderString[0], out authHeader);

            if (canParse == false || authHeader.Scheme != TestServerAuthenticationDefaults.AuthenticationScheme)
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization header not valid"));
            }

            var headerClaims = DefautClaimsEncoder.Decode(authHeader.Parameter).ToArray();

            if (headerClaims.Length == 0)
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization header with no claims"));
            }

            var identity = new ClaimsIdentity(
                claims: Options.CommonClaims.Union(headerClaims),
                authenticationType: Options.AuthenticationScheme,
                nameType: Options.NameClaimType,
                roleType: Options.RoleClaimType);

            var ticket = new AuthenticationTicket(
                new ClaimsPrincipal(identity),
                new AuthenticationProperties(),
                Options.AuthenticationScheme);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
