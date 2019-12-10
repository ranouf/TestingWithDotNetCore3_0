using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyAPI.Services;
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
                .ConfigureServices(services =>
                {
                    services.AddAutofac();
                    services.AddSingleton<IMyService, MyService>();
                })
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterType<MyService>().As<IMyService>().InstancePerLifetimeScope();
                })
                .ConfigureWebHost(webHost =>
                {
                    // Add TestServer
                    webHost
                        .UseStartup<TestStartup>()
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                            services.AddSingleton<IMyService, MyService>();
                            services.AddAutofac();
                            services
                                .AddControllers()
                                .AddApplicationPart(typeof(TestStartup).Assembly);
                        })
                        .ConfigureTestServices(services =>
                        {
                            services.AddSingleton<IMyService, MyService>();
                            services.AddAutofac();
                            services
                                .AddControllers()
                                .AddApplicationPart(typeof(TestStartup).Assembly);
                        })
                        .ConfigureTestContainer<ContainerBuilder>(builder =>
                        {
                            builder.RegisterType<MyService>().As<IMyService>().InstancePerLifetimeScope();
                        });
                });

            Host = hostBuilder.Start();
            Server = Host.GetTestServer();
            Client = Host.GetTestClient();

            using (var scope = Host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var myService = services.GetRequiredService<MyService>();
                }
                catch (Exception ex)
                {
                    Output.WriteLine("HOST: " + ex.Message);
                }
            }
            using (var scope = Server.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var myService = services.GetRequiredService<MyService>();
                }
                catch (Exception ex)
                {
                    Output.WriteLine("SERVER: " + ex.Message);
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

    //public class TestServerFixture : WebApplicationFactory<Startup>
    //{
    //    private HttpClient _client;
    //    public HttpClient Client
    //    {
    //        get
    //        {
    //            if (_client == null)
    //            {
    //                _client = CreateClient();
    //            }
    //            return _client;
    //        }
    //    }
    //    public ITestOutputHelper Output { get; set; }

    //    protected override IHostBuilder CreateHostBuilder()
    //    {
    //        var currentDirectory = Directory.GetCurrentDirectory();

    //        var builder = Host.CreateDefaultBuilder()
    //            .ConfigureLogging(logging =>
    //            {
    //                logging.ClearProviders();
    //                logging.AddXunit(Output);
    //            })
    //            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    //            .ConfigureContainer<ContainerBuilder>(builder =>
    //            {
    //                builder.RegisterModule<MyAPIModule>();
    //            })
    //            .ConfigureWebHost(builder =>
    //            {
    //                builder.UseTestServer();
    //                builder
    //                    .ConfigureTestServices((services) =>
    //                    {
    //                        services
    //                            .AddControllers()
    //                            .AddApplicationPart(typeof(Startup).Assembly);
    //                    });
    //            });

    //        return builder;
    //    }
    //    protected override void ConfigureWebHost(IWebHostBuilder builder)
    //    {
    //        builder.UseStartup<TestStartup>();
    //        base.ConfigureWebHost(builder);
    //    }

    //    protected override void ConfigureClient(HttpClient client)
    //    {
    //        base.ConfigureClient(client);
    //    }

    //    public TestServerFixture WithOutPut(ITestOutputHelper output)
    //    {
    //        Output = output;
    //        return this;
    //    }
    //    public TestServerFixture WithData()
    //    {
    //        using (var scope = Server.Services.CreateScope())
    //        {
    //            var services = scope.ServiceProvider;
    //            try
    //            {
    //                Output.WriteLine("Starting the database initialization.");
    //                TestDbInitializer.Seed(services, Output);
    //                Output.WriteLine("The database initialization has been done.");
    //            }
    //            catch (Exception e)
    //            {
    //                Output.WriteLine(e.Message);
    //            }
    //        }
    //        return this;
    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        base.Dispose(disposing);
    //        Output = null;
    //    }
    //}
}
