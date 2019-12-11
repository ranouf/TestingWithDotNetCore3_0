using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyAPI;
using MyIntegrationTests.Data;
using MyIntegrationTests.Extensions;
using MyIntegrationTests.Loggers;
using System;
using System.Net.Http;
using Xunit.Abstractions;

namespace MyIntegrationTests
{
    public class TestServerFixture : IDisposable
    {
        public IHost Host { get; }
        public TestServer Server { get; }
        public HttpClient Client { get; }
        public ITestOutputHelper Output { get; }

        public TestServerFixture()
        {

        }

        public TestServerFixture(ITestOutputHelper output)
        {
            Output = output;

            var hostBuilder = new HostBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddXunit(Output);
                })
                .ConfigureWebHost(webHost =>
                {
                    webHost
                        .UseStartup<TestStartup>()
                        .BasedOn<Startup>() //Internal extension to re set the correct ApplicationKey
                        .UseTestServer();
                });

            Host = hostBuilder.Start();
            Server = Host.GetTestServer();
            Client = Host.GetTestClient();

            using (var scope = Host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    TestDbInitializer.Seed(services, Output);
                }
                catch (Exception ex)
                {
                    Output.WriteLine("HOST: " + ex.Message);
                }
            }
        }

        public void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
            Host.Dispose();
        }
    }
}
