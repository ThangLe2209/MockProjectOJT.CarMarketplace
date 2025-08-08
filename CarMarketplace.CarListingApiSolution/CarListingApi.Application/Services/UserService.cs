using CarListingApi.Application.DTOs;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CarListingApi.Application.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly ResiliencePipelineProvider<string> _resiliencePipeline;

        public UserService(HttpClient httpClient, ResiliencePipelineProvider<string> resiliencePipeline)
        {
            _httpClient = httpClient;
            _resiliencePipeline = resiliencePipeline;
        }

        public async Task<UserDto?> GetUserAsync(int userId)
        {
            var retryPipeline = _resiliencePipeline.GetPipeline("my-retry-pipeline");

            return await retryPipeline.ExecuteAsync(async token =>
            {
                var response = await _httpClient.GetAsync($"/api/authentication/{userId}", token);
                if (!response.IsSuccessStatusCode)
                    return null;
                //var json = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(json); // See what you actually get

                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UserDto>>();
                return apiResponse?.Data;
            });
        }
    }
}
