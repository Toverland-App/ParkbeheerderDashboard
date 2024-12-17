using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ParkbeheerderDashboard.Models
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://i558324.luna.fhict.nl")
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Attraction>> GetAttractionsAsync()
        {
            var response = await _httpClient.GetAsync("api/Attraction");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Attraction>>(json);
        }

        public async Task<Attraction> GetAttractionByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Attraction/{id}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Attraction>(json);
        }

        public async Task<bool> CreateAttractionAsync(Attraction attraction)
        {
            var json = JsonConvert.SerializeObject(attraction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Attraction", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAttractionAsync(int id, Attraction attraction)
        {
            var json = JsonConvert.SerializeObject(attraction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/Attraction/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAttractionAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Attraction/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Maintenance>> GetAllMaintenancesAsync()
        {
            var response = await _httpClient.GetAsync("api/Maintenance");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Maintenance>>(json);
        }

        public async Task<bool> AddMaintenanceAsync(int attractionId, string attractie, string status, string description)
        {
            var maintenance = new
            {
                AttractionId = attractionId,
                Attractie = attractie,
                Status = status,
                Description = description
            };
            var json = JsonConvert.SerializeObject(maintenance);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Maintenance", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAreaAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Area/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateAreaAsync(object area)
        {
            var json = JsonConvert.SerializeObject(area);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Area", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Area>> GetAreasAsync()
        {
            var response = await _httpClient.GetAsync("api/Area");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Area>>(json);
        }

        public async Task<bool> UpdateAreaAsync(int id, Area area)
        {
            var json = JsonConvert.SerializeObject(area);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/Area/{id}", content);
            return response.IsSuccessStatusCode;
        }
    }
}
