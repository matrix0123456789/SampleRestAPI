using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntegrationTests
{
    internal static class TestHelpers
    {
        public static async Task<T> GetBody<T>(this HttpResponseMessage responseMessage)
        {
            var responseString = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseString);
        }
        public static async Task<T> GetBody<T>(this Task<HttpResponseMessage> responseMessage)
        {
            return await GetBody<T>(await responseMessage);
        }
    }
}
