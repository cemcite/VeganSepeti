using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using VeganSepeti.CartApi;

namespace VeganSepeti.IntegrationTest
{
    public class TestClientProvider
    {
        public readonly HttpClient Client;

        public TestClientProvider()
        {
            var testServer = new TestServer(new WebHostBuilder()
             .UseStartup<Startup>());
            Client = testServer.CreateClient();
        }
    }
}
