using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class ToDoTests
    {
        [Fact]
        public async Task TodoListingAll()
        {
            var client = await TestEnvironment.GetTestEnvironment();

            var response = await client.GetAsync("/ToDo/");
            response.GetBody<IEnumerable<Dictionary<string, object>>>();

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task TodoCreation()
        {
            var client = await TestEnvironment.GetTestEnvironment();
            var obj = new Dictionary<string, object>();
            obj["title"] = "Simple title";
            obj["description"] = "Simple description";

            var response = await client.PostAsync("/ToDo/", new StringContent(JsonSerializer.Serialize(obj), System.Text.Encoding.UTF8, "application/json"));
            var responseObj = await response.GetBody<Dictionary<string, JsonElement>>();
            var deleteResponse = await client.DeleteAsync("/ToDo/" + responseObj["id"].GetInt32());

            Assert.Equal(JsonValueKind.Number, responseObj["id"].ValueKind);
            Assert.Equal(JsonValueKind.Null, responseObj["expiration"].ValueKind);
            Assert.Equal(obj["title"], responseObj["title"].GetString());
            Assert.Equal(obj["description"], responseObj["description"].GetString());
            Assert.Equal(0, responseObj["completion"].GetInt32());
            Assert.Equal("waiting", responseObj["status"].GetString());
            Assert.Equal(System.Net.HttpStatusCode.OK, deleteResponse.StatusCode);
        }
        [Fact]
        public async Task TodoUpdate()
        {
            var client = await TestEnvironment.GetTestEnvironment();
            var sample = await CreateSampleObject();
            try
            {
                sample["description"] = JsonDocument.Parse("\"Updated value\"").RootElement;

                var response1 = await client.PutAsync("/ToDo/" + sample["id"].ToString(), new StringContent(JsonSerializer.Serialize(sample), System.Text.Encoding.UTF8, "application/json"));
                var response2 = await client.GetAsync("/ToDo/" + sample["id"].ToString());
                var response2Obj = await response2.GetBody<Dictionary<string, JsonElement>>();

                Assert.Equal(System.Net.HttpStatusCode.OK, response1.StatusCode);
                Assert.True(DictionaryEquals(response2Obj, sample));
            }
            finally
            {
                RemoveSampleObject(sample);
            }
        }
        [Fact]
        public async Task TodoSetAsDone()
        {
            var client = await TestEnvironment.GetTestEnvironment();
            var sample = await CreateSampleObject();
            try
            {
                var response1 = await client.PutAsync("/ToDo/" + sample["id"].ToString() + "/done", null);
                var response2 = await client.GetAsync("/ToDo/" + sample["id"].ToString());
                var response2Obj = await response2.GetBody<Dictionary<string, JsonElement>>();

                Assert.Equal(System.Net.HttpStatusCode.OK, response1.StatusCode);
                Assert.Equal("done", response2Obj["status"].ToString());
            }
            finally
            {
                RemoveSampleObject(sample);
            }
        }
        [Fact]
        public async Task TodoSetPercentage()
        {
            var client = await TestEnvironment.GetTestEnvironment();
            var sample = await CreateSampleObject();
            try
            {
                var response1 = await client.PutAsync("/ToDo/" + sample["id"].ToString() + "/completion", new StringContent("12.34", System.Text.Encoding.UTF8, "application/json"));
                var response2 = await client.GetAsync("/ToDo/" + sample["id"].ToString());
                var response2Obj = await response2.GetBody<Dictionary<string, JsonElement>>();

                Assert.Equal(System.Net.HttpStatusCode.OK, response1.StatusCode);
                Assert.Equal("12.34", response2Obj["completion"].ToString());
            }
            finally
            {
                RemoveSampleObject(sample);
            }
        }
        [Fact]
        public async Task TodoListingAllContainsNew()
        {
            var client = await TestEnvironment.GetTestEnvironment();

            var response1 = await client.GetAsync("/ToDo/");
            var response1Obj = await response1.GetBody<IEnumerable<Dictionary<string, JsonElement>>>();

            var sample = await CreateSampleObject();

            var response2 = await client.GetAsync("/ToDo/");
            var response2Obj = await response2.GetBody<IEnumerable<Dictionary<string, JsonElement>>>();

            await RemoveSampleObject(sample);

            var response3 = await client.GetAsync("/ToDo/");
            var response3Obj = await response3.GetBody<IEnumerable<Dictionary<string, JsonElement>>>();

            Assert.False(response1Obj.Any(x => DictionaryEquals(x, sample)));
            Assert.True(response2Obj.Any(x => DictionaryEquals(x, sample)));
            Assert.False(response3Obj.Any(x => DictionaryEquals(x, sample)));
        }


        private bool DictionaryEquals(Dictionary<string, JsonElement> a, Dictionary<string, JsonElement> b)
        {
            return a["id"].GetInt32() == b["id"].GetInt32() && a["description"].GetString() == b["description"].GetString();
        }
        [Fact]
        public async Task TodoDelete()
        {
            var client = await TestEnvironment.GetTestEnvironment();
            var sample = await CreateSampleObject();
            try
            {
                var deleteResponse1 = await client.DeleteAsync("/ToDo/" + sample["id"].GetInt32());
                var deleteResponse2 = await client.DeleteAsync("/ToDo/" + sample["id"].GetInt32());

                Assert.Equal(HttpStatusCode.OK, deleteResponse1.StatusCode);
                Assert.Equal(HttpStatusCode.NotFound, deleteResponse2.StatusCode);
            }
            finally
            {
                RemoveSampleObject(sample);
            }
        }
        [Fact]
        public async Task TodoGetOne()
        {
            var client = await TestEnvironment.GetTestEnvironment();
            var sample = await CreateSampleObject();

            try
            {
                var response = await client.GetAsync("/ToDo/" + sample["id"].GetInt32());
                var responseObj = await response.GetBody<Dictionary<string, JsonElement>>();

                Assert.True(DictionaryEquals(sample, responseObj));
            }
            finally
            {
                RemoveSampleObject(sample);
            }
        }
        [Fact]
        public async Task TodoGetOneThatNotExists()
        {
            var client = await TestEnvironment.GetTestEnvironment();

            var response = await client.GetAsync("/ToDo/-1");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        private async Task<Dictionary<string, JsonElement>> CreateSampleObject()
        {
            var client = await TestEnvironment.GetTestEnvironment();
            var obj = new Dictionary<string, object>();
            obj["title"] = "Simple title";
            obj["description"] = Guid.NewGuid().ToString();

            var response = await client.PostAsync("/ToDo/", new StringContent(JsonSerializer.Serialize(obj), System.Text.Encoding.UTF8, "application/json"));
            return await response.GetBody<Dictionary<string, JsonElement>>();
        }
        private async Task RemoveSampleObject(Dictionary<string, JsonElement> sample)
        {
            var client = await TestEnvironment.GetTestEnvironment();
            await client.DeleteAsync("/ToDo/" + sample["id"].GetInt32());
        }
    }
}