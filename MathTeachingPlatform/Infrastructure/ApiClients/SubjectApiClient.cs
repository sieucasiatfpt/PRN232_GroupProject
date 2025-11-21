using Application.DTOs.Subject;
using Application.DTOs.Teacher;
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

        public async Task<List<SubjectInfoDto>> GetSubjectsByTeacherIdAsync(int teacherId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"subjects/by-teacher/{teacherId}");
                if (response.IsSuccessStatusCode)
                {
                    var subjects = await response.Content.ReadFromJsonAsync<List<SubjectDto>>();
                    return subjects?.Select(s => new SubjectInfoDto
                    {
                        SubjectId = s.SubjectId,
                        Title = s.Title,
                        Description = s.Description
                    }).ToList() ?? new List<SubjectInfoDto>();
                }
                return new List<SubjectInfoDto>();
            }
            catch
            {
                return new List<SubjectInfoDto>();
            }
        }
    }
}