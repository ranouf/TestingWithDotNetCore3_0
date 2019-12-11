using Microsoft.Extensions.DependencyInjection;
using MyAPI.EntityFramework;
using MyAPI.Services;
using System;
using Xunit.Abstractions;

namespace MyIntegrationTests.Data
{
    public static class TestDbInitializer
    {
        public static void Seed(IServiceProvider services, ITestOutputHelper output)
        {
            try
            {
                var context = services.GetRequiredService<MyDbContext>();
                context.Database.EnsureCreated();

                var myService = services.GetRequiredService<IMyService>();
                new MyEntitiesDataBuilder(context, myService, output).Seed();

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                output.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
