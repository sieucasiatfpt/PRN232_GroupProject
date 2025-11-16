using Application.DTOs.Subject;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.ApiClients
{
    public class SubjectApiClient : ISubjectApiClient
    {
        private readonly HttpClient _httpClient;

        public SubjectApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SubjectDto?> GetSubjectByIdAsync(int subjectId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"subjects/{subjectId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<SubjectDto>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> SubjectExistsAsync(int subjectId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"subjects/{subjectId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string?> GetSubjectTitleAsync(int subjectId)
        {
            var subject = await GetSubjectByIdAsync(subjectId);
            return subject?.Title;
        }

    }
}