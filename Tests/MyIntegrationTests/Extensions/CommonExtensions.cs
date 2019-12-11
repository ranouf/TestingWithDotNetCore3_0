using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Reflection;
using System.Text;
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

        public static IWebHostBuilder BasedOn<TStartup>(this IWebHostBuilder builder) where TStartup : class
        {
            // Set the "right" application after the startup's been registerted
            // source: https://github.com/aspnet/AspNetCore/issues/3334#issuecomment-405746266
            return builder.UseSetting(WebHostDefaults.ApplicationKey, typeof(TStartup).GetTypeInfo().Assembly.GetName().Name);
        }

        public static StringContent ToStringContent(this object o)
        {
            return new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
        }
    }
}
