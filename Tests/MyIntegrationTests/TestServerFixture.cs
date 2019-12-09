using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyAPI;
using MyIntegrationTests.Loggers;
using System.IO;
using System.Net.Http;
using Xunit.Abstractions;

namespace MyIntegrationTests
{
    public class TestServerFixture : WebApplicationFactory<Startup>
    {
        public HttpClient Client { get; }
        public ITestOutputHelper Output { get; set; }

        protected override IHostBuilder CreateHostBuilder()
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            var builder = Host.CreateDefaultBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddXunit(Output);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureTestServices((services) =>
                        {
                            services
                                .AddControllers()
                                .AddApplicationPart(typeof(Startup).Assembly);
                        });
                });

            return builder;
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseStartup<TestStartup>();

            base.ConfigureWebHost(builder);
        }

        public TestServerFixture SetOutPut(ITestOutputHelper output)
        {
            Output = output;
            return this;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Output = null;
        }
    }
}
