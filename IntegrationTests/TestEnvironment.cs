using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IntegrationTests
{
    internal static class TestEnvironment
    {
        public const string URL = "http://localhost:5000";
        private static Thread thread = null;
        static TestEnvironment()
        {
            thread = new Thread(() =>
            {
                SampleRestAPI.Program.Main(new string[0]);

            });
            thread.Start();
        }
        public static async Task<HttpClient> GetTestEnvironment()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    var response = await client.GetAsync("/");
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        return client;
                }
                catch { }
                await Task.Delay(500);
            }
            throw new Exception("Cannot connect by http to " + URL);
        }
    }
}
