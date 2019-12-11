using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyAPI;
using MyAPI.EntityFramework;

namespace MyIntegrationTests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void SetUpDataBase(IServiceCollection services)
        {
            services.AddDbContext<MyDbContext>(options => options
                .UseInMemoryDatabase("MyDb")
                .EnableSensitiveDataLogging()
            );
        }
    }
}
