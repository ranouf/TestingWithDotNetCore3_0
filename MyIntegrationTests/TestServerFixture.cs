using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyIntegrationTests.Loggers;
using System;
using System.Net.Http;
using Xunit.Abstractions;

namespace MyIntegrationTests
{
    public class TestServerFixture : WebApplicationFactory<TestStartup>
    {
        private IHost _host;
        public HttpClient Client { get; }
        public ITestOutputHelper Output { get; }

        public TestServerFixture(ITestOutputHelper output)
        {
            Output = output;
            Client = Server.CreateClient();
        }

        // never called but this is where i was previously building up the server
        //
        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
            return base.CreateServer(builder);
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            _host = builder.Build();

            using (var scope = _host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                InitializeDataBase(services, Output);
            }

            _host.Start();
            return _host;
        }

        protected override IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureLogging((hostingContext, builder) =>
                {
                    builder.Services.AddSingleton<ILoggerProvider>(new XunitLoggerProvider(Output));
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseTestServer();
                });

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseStartup<TestStartup>();
        }

        private void InitializeDataBase(IServiceProvider services, ITestOutputHelper output)
        {
            try
            {
                output.WriteLine("Starting the database initialization.");
                //here is where is feed the Test DB
                output.WriteLine("The database initialization has been done.");
            }
            catch (Exception ex)
            {
                output.WriteLine("An error occurred while initialization the database.");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
