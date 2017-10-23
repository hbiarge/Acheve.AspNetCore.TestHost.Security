# Acheve.AspNetCore.TestHost.Security
NuGet package to manage authenticated requests in AspNetCore TestServer

Unit testing your Mvc controllers is not enougth to verify the correctness of your WebApi. Are the filters working? The correct status code is sent when that condition is reached? Is the user authorized to request that endpoint? 


The NuGet package [Microsoft.AspNetCore.TestHost](https://www.nuget.org/packages/Microsoft.AspNetCore.TestHost/) allows you to create an in memory server that exposes an HttpClient to be able to send request to the server. All in memory, all in the same process. Fast. It's the best way to create integration test in your Mvc application.

But when your Mvc application requires authenticated request it could be a little more dificult...

What if you have an easy way to indicate the claims in the request? 

This package implements an authentication middleware and several extension methods to easiy indicate
the claims for authenticated calls to the WebApi.

In the TestServer startup class you shoud incude the authentication service and add the .Net Core new AUthentication middleware:

     public class TestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = TestServerAuthenticationDefaults.AuthenticationScheme;
                })
                .AddTestServerAuthentication();

            var mvcCoreBuilder = services.AddMvcCore();
            ApiConfiguration.ConfigureCoreMvc(mvcCoreBuilder);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();
        }
    }

And in your tests you can use an HttpClient with default credentials or build 
the request with the server RequestBuilder and with the specified claims:

    public class VauesWithDefaultUserTests : IDisposable
    {
        private readonly TestServer _server;
        private readonly HttpClient _userHttpCient;

        public VauesWithDefaultUserTests()
        {
            // Build the test server
            var host = new WebHostBuilder()
                .UseStartup<TestStartup>();

            _server = new TestServer(host);

            // You can create an HttpClient instance with a default identity
            _userHttpCient = _server.CreateClient()
                .WithDefaultIdentity(Identities.User);
        }

        [Fact]
        public async Task WithHttpClientWithDefautIdentity()
        {
            var response = await _userHttpCient.GetAsync("api/values");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task WithRequestBuilder()
        {
            // Or you can create a request and assign the identity to the RequestBuilder
            var response = await _server.CreateRequest("api/values")
                .WithIdentity(Identities.User)
                .GetAsync();

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Anonymous()
        {
            var response = await _server.CreateRequest("api/values/public")
                .GetAsync();

            response.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            _server.Dispose();
            _userHttpCient.Dispose();
        }
    }

Both methods (`WithDefaultIdentity` and `WithIdentity`) accept as the only parameter an IEnumerabe&lt;Claim&gt; that should include the desired user claims in the request.

    public static class Identities
    {
        public static readonly IEnumerable<Claim> User = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Name, "User"),
        };

        public static readonly IEnumerable<Claim> Empty = new Claim[0];
    }

You can find a complete example in the [samples](https://github.com/hbiarge/Acheve.AspNetCore.TestHost.Security/tree/master/Acheve.AspNet.TestHost.Security/samples) directory.