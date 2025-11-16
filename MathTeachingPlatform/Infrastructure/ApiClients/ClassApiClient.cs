using Application.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.ApiClients
{
    public class ClassApiClient : IClassApiClient
    {
        private readonly HttpClient _httpClient;

        public ClassApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> HasActiveClassesAsync(int teacherId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"classes/teachers/{teacherId}/active-classes");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<bool>(content);
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}