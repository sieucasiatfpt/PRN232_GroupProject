using Application.DTOs.Teacher;
using Application.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Infrastructure.ApiClients
{
    public class ClassApiClient : IClassApiClient
    {
        private readonly HttpClient _httpClient;
        private string? _jwtToken;

        public ClassApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Method to set the JWT token
        public void SetToken(string jwtToken)
        {
            _jwtToken = jwtToken;
        }

        private void AddAuthorizationHeader()
        {
            if (!string.IsNullOrEmpty(_jwtToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _jwtToken);
            }
        }

        public async Task<bool> HasActiveClassesAsync(int teacherId)
        {
            try
            {
                AddAuthorizationHeader();
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

        public async Task<List<ClassInfoDto>> GetClassesByTeacherIdAsync(int teacherId)
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync($"classes/by-teacher/{teacherId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<ClassInfoDto>>() ?? new List<ClassInfoDto>();
                }
                return new List<ClassInfoDto>();
            }
            catch
            {
                return new List<ClassInfoDto>();
            }
        }


    }
}