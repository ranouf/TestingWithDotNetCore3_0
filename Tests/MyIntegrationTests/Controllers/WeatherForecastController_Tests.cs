using MyAPI;
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
    public class WeatherForecastController_Tests
    {
        public TestServerFixture TestServerFixture { get; private set; }
        public ITestOutputHelper Output { get { return TestServerFixture.Output; } }

        public WeatherForecastController_Tests(ITestOutputHelper output)
        {
            TestServerFixture = new TestServerFixture(output);
        }

        [Fact]
        public async Task Should_Get_WeatherForecast()
        {
            var response = await TestServerFixture.Client.GetAsync(
                TestServerFixture.Client.BuildPath(API.WeatherForecast.Url, Output)
            );
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var weatherForecasts = await response.ConvertToAsync<IEnumerable<WeatherForecast>>(Output);
            Assert.True(weatherForecasts.Any());
        }
    }
}
