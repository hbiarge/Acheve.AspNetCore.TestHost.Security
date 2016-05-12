using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders;

namespace Acheve.AspNet.TestHost.Security
{
    public class TestServerAuthenticationMiddleware
        : AuthenticationMiddleware<TestServerAuthenticationOptions>
    {
        public TestServerAuthenticationMiddleware(
            RequestDelegate next,
            TestServerAuthenticationOptions options,
            ILoggerFactory
            loggerFactory,
            IUrlEncoder encoder)
            : base(next, options, loggerFactory, encoder)
        {
        }

        protected override AuthenticationHandler<TestServerAuthenticationOptions> CreateHandler()
        {
            return new TestServerAuthenticationHandler();
        }
    }
}
