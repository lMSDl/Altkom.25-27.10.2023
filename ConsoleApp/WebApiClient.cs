﻿using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ConsoleApp
{
    internal class WebApiClient : IDisposable
    {
        private HttpClient _client;
        private JsonSerializerSettings JsonSerializerSettings { get; } = new JsonSerializerSettings();

        public WebApiClient(string baseAddress)
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress)
            };
            _client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
        }

        public void Dispose()
        {
            _client.Dispose();
        }
        public async Task<string> GetStringAsync(string request)
        {
            var response = await _client.GetAsync(request);
            //response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode)
                return default;

            return await response.Content.ReadAsStringAsync();
        }
        public async Task<T> GetAsync<T>(string request)
        {
            var content = await GetStringAsync(request);
            if (content == null)
                return default;
            return JsonConvert.DeserializeObject<T>(content, JsonSerializerSettings);

        }
        public async Task<T> PostAsync<T>(string request, T payload)
        {
            var response = await _client.PostAsJsonAsync(request, payload);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        }


    }
}
