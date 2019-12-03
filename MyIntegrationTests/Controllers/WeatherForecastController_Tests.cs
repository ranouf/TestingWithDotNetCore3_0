using MyAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static MyAPI.Constants;

namespace MyIntegrationTests.Controllers
{
    [Collection(Constants.TEST_COLLECTION)]
    public class WeatherForecastController_Tests : IClassFixture<TestServerFixture>
    {
        public HttpClient Client { get; private set; }
        public ITestOutputHelper Output { get; private set; }

        public WeatherForecastController_Tests(TestServerFixture testServerFixture, ITestOutputHelper output)
        {
            Client = testServerFixture.Client;
            Output = output;
        }


        [Fact]
        public async Task Should_Get_WeatherForecast()
        {
            var response = await Client.GetAsync(
                BuildPath(API.WeatherForecast.Url)
            );
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var weatherForecasts = await ConvertToAsync<IEnumerable<WeatherForecast>>(response);
            Assert.True(weatherForecasts.Any());

        }

        #region Private
        private string BuildPath(string path)
        {
            var builder = new UriBuilder(Client.BaseAddress)
            {
                Path = path
            };
            var result = builder.Uri.PathAndQuery;
            Output.WriteLine($"url:'{result}'");
            return result;
        }

        public async Task<T> ConvertToAsync<T>(HttpResponseMessage response) where T : class
        {
            var content = await response.Content.ReadAsStringAsync();
            Output.WriteLine(message: $"content: {content}");
            return JsonConvert.DeserializeObject<T>(content);
        }
        #endregion
    }
}
