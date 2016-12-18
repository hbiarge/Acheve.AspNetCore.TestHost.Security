using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Acheve.AspNetCore.TestHost.Security
{
    public class TestServerAuthenticationMiddleware
        : AuthenticationMiddleware<TestServerAuthenticationOptions>
    {
        public TestServerAuthenticationMiddleware(
            RequestDelegate next,
            IOptions<TestServerAuthenticationOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder)
            : base(next, options, loggerFactory, encoder)
        {
        }

        protected override AuthenticationHandler<TestServerAuthenticationOptions> CreateHandler()
        {
            return new TestServerAuthenticationHandler();
        }
    }
}
