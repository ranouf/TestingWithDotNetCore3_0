using MyAPI.EntityFramework;
using MyAPI.Services;
using MyAPI.Services.Entities;
using Xunit.Abstractions;

namespace MyIntegrationTests.Data
{
    internal class MyEntitiesDataBuilder
    {
        private readonly MyDbContext _context;
        private readonly ITestOutputHelper _output;
        private readonly IMyService _myService;

        public MyEntitiesDataBuilder(MyDbContext context, IMyService myService, ITestOutputHelper output)
        {
            _context = context;
            _myService = myService;
            _output = output;
        }

        internal void Seed()
        {
            var myEntities = new MyEntity[]
            {
                new MyEntity() {Name ="MyEntity 1"},
                new MyEntity() {Name ="MyEntity 2"},
                new MyEntity() {Name ="MyEntity 3"} 
            };

            foreach (var myEntity in myEntities)
            {
                _myService.Create(myEntity.Name);
            }
            _output.WriteLine($"{myEntities.Length} entities have been created.");
        }
    }
}