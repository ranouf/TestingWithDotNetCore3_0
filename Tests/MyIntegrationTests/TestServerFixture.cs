using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyAPI;
using MyAPI.Modules;
using MyAPI.Services;
using MyIntegrationTests.Loggers;
using System;
using System.IO;
using System.Net.Http;
using Xunit.Abstractions;

namespace MyIntegrationTests
{
    public class TestServerFixture : WebApplicationFactory<Startup>
    {
        private HttpClient _client;
        public HttpClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = CreateClient();
                }
                return _client;
            }
        }
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
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule<MyAPIModule>();
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

        protected override void ConfigureClient(HttpClient client)
        {
            using (var scope = Services.CreateScope())
            {
                try
                {
                    // Here you can get all the injected services including services from Autofac
                    // for example for seeding your local DB
                    var myService = scope.ServiceProvider.GetRequiredService<IMyService>();
                }
                catch (Exception e)
                {
                    Output.WriteLine(e.Message);
                }
            }
            base.ConfigureClient(client);
        }

        public TestServerFixture WithOutPut(ITestOutputHelper output)
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
