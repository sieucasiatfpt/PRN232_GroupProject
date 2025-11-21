using Application.DTOs.Student;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ApiClients
{
    public class StudentApiClient : IStudentApiClient
    {
        private readonly HttpClient _httpClient;

        public StudentApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<StudentDto?> GetStudentByIdAsync(int studentId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/student/{studentId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<StudentDto>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> StudentExistsAsync(int studentId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/student/{studentId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string?> GetStudentNameAsync(int studentId)
        {
            var student = await GetStudentByIdAsync(studentId);
            return student?.Name;
        }
    }
}