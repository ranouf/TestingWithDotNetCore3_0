using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace MyIntegrationTests.Extensions
{
    public static class CommonExtensions
    {
        public static string BuildPath(this HttpClient client, string path, ITestOutputHelper output)
        {
            var builder = new UriBuilder(client.BaseAddress)
            {
                Path = path
            };
            var result = builder.Uri.PathAndQuery;
            output.WriteLine($"url:'{result}'");
            return result;
        }

        public static async Task<T> ConvertToAsync<T>(this HttpResponseMessage response, ITestOutputHelper output) where T : class
        {
            var content = await response.Content.ReadAsStringAsync();
            output.WriteLine(message: $"content: {content}");
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
