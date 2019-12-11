using MyAPI.Services.Entities;
using MyIntegrationTests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static MyAPI.Constants;

namespace MyIntegrationTests.Controllers
{
    [Collection(Constants.TEST_COLLECTION)]
    public class MyController_Tests
    {
        public TestServerFixture TestServerFixture { get; private set; }
        public ITestOutputHelper Output { get { return TestServerFixture.Output; } }

        public MyController_Tests(ITestOutputHelper output)
        {
            TestServerFixture = new TestServerFixture(output);
        }

        [Fact]
        public async Task Should_Get_My_Entities()
        {
            // Get all
            var response = await TestServerFixture.Client.GetAsync(
                TestServerFixture.Client.BuildPath(API.My.Url, Output)
            );
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var myEntities = await response.ConvertToAsync<IEnumerable<MyEntity>>(Output);
            Assert.True(myEntities.Any());
        }

        [Fact]
        public async Task Should_Create_An_Entity()
        {
            // Create entity
            var dto = new MyEntity() { Name = "New Entity" };
            var response = await TestServerFixture.Client.PostAsync(
                TestServerFixture.Client.BuildPath(API.My.Url, Output),
                dto.ToStringContent()
            );
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var myEntity = await response.ConvertToAsync<MyEntity>(Output);
            Assert.NotNull(myEntity);
            Assert.NotEqual(Guid.Empty, myEntity.Id);
            Assert.Equal("New Entity", myEntity.Name);
        }

        [Fact]
        public async Task Should_Update_An_Entity()
        {
            // Get all
            var response = await TestServerFixture.Client.GetAsync(
                TestServerFixture.Client.BuildPath(API.My.Url, Output)
            );
            var myEntities = await response.ConvertToAsync<IEnumerable<MyEntity>>(Output);
            var myEntity = myEntities.First();

            // Update entity
            myEntity.Name = "Entity Updated";
            response = await TestServerFixture.Client.PutAsync(
                TestServerFixture.Client.BuildPath($"{API.My.Url}/{myEntity.Id}", Output),
                myEntity.ToStringContent()
            );
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            myEntity = await response.ConvertToAsync<MyEntity>(Output);
            Assert.NotNull(myEntity);
            Assert.Equal("Entity Updated", myEntity.Name);
        }

        [Fact]
        public async Task Should_Delete_One_Entity()
        {
            // Get all
            var response = await TestServerFixture.Client.GetAsync(
                TestServerFixture.Client.BuildPath(API.My.Url, Output)
            );
            var myEntities = await response.ConvertToAsync<IEnumerable<MyEntity>>(Output);
            var myEntitiesCount = myEntities.Count();
            var myEntity = myEntities.First();

            // Delete an entity
            response = await TestServerFixture.Client.DeleteAsync(
                TestServerFixture.Client.BuildPath($"{API.My.Url}/{myEntity.Id}", Output)
            );
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Confirm than one is missing
            response = await TestServerFixture.Client.GetAsync(
                TestServerFixture.Client.BuildPath(API.My.Url, Output)
            );
            myEntities = await response.ConvertToAsync<IEnumerable<MyEntity>>(Output);
            Assert.Equal(myEntitiesCount - 1, myEntities.Count());
        }
    }
}
