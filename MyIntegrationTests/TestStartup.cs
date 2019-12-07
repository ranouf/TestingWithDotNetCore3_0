using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyAPI;

namespace MyIntegrationTests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void SetUpDataBase(IServiceCollection services)
        {
            // here is where I use the InMemoryDatabase 
        }
    }
}
