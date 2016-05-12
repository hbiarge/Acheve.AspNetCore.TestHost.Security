using System;
using Microsoft.AspNet.Builder;

namespace Acheve.AspNet.TestHost.Security
{
    public static class TestAppBuilderExtensions
    {
        public static IApplicationBuilder UseTestServerAuthentication(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<TestServerAuthenticationMiddleware>(new TestServerAuthenticationOptions());
        }

        public static IApplicationBuilder UseTestServerAuthentication(this IApplicationBuilder app, Action<TestServerAuthenticationOptions> configureOptions)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var options = new TestServerAuthenticationOptions();

            configureOptions?.Invoke(options);

            return app.UseMiddleware<TestServerAuthenticationMiddleware>(options);
        }
    }
}
