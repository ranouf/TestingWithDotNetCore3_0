using MyAPI.Services.Entities;
using MyIntegrationTests.Extensions;
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
            var response = await TestServerFixture.Client.GetAsync(
                TestServerFixture.Client.BuildPath(API.My.Url, Output)
            );
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var myEntities = await response.ConvertToAsync<IEnumerable<MyEntity>>(Output);
            Assert.True(myEntities.Any());
        }

        //[Fact]
        //public async Task Should_Update_My_Entity()
        //{
        //    var response = await Client.GetAsync(
        //        BuildPath(API.My.Url)
        //    );
        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        //    var weatherForecasts = await ConvertToAsync<IEnumerable<WeatherForecast>>(response);
        //    Assert.True(weatherForecasts.Any());
        //}
    }
}
