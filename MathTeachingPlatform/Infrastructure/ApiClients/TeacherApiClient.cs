using Application.DTOs.Teacher;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ApiClients
{
    public class TeacherApiClient : ITeacherApiClient
    {
        private readonly HttpClient _httpClient;

        public TeacherApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TeacherDto?> GetTeacherByIdAsync(int teacherId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"teachers/{teacherId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    // Log the raw JSON for debugging
                    Console.WriteLine($"Teacher API response: {json}");

                    return System.Text.Json.JsonSerializer.Deserialize<TeacherDto>(json, new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                return null;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Exception in GetTeacherByIdAsync: {ex}");
                return null;
            }
        }


        public async Task<bool> TeacherExistsAsync(int teacherId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"teachers/{teacherId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string?> GetTeacherNameAsync(int teacherId)
        {
            var teacher = await GetTeacherByIdAsync(teacherId);
            return teacher?.Name;
        }
    }
}