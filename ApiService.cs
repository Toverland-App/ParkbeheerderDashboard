using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkbeheerderDashboard.Models;

namespace ParkbeheerderDashboard
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Attraction>> GetAttractionsAsync()
        {
            var response = await _httpClient.GetStringAsync("https://localhost:7129/api/Attraction");
            return JsonConvert.DeserializeObject<List<Attraction>>(response);
        }

        public async Task<List<Maintenance>> GetMaintenanceAsync()
        {
            var response = await _httpClient.GetStringAsync("https://localhost:7129/api/Maintenance");
            return JsonConvert.DeserializeObject<List<Maintenance>>(response);
        }

        public async Task<List<AttractionStatus>> GetAttractionStatusesAsync()
        {
            var response = await _httpClient.GetStringAsync("https://localhost:7129/api/AttractionMaintenance/GetAllMaintenances");
            return JsonConvert.DeserializeObject<List<AttractionStatus>>(response);
        }

        public async Task<bool> AddMaintenanceAsync(int attractionId, string attractie, string status, string opmerkingen)
        {
            var maintenanceData = new
            {
                Attractie = attractie,
                Status = status,
                Opmerkingen = opmerkingen
            };

            var json = JsonConvert.SerializeObject(maintenanceData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync($"https://localhost:7129/api/AttractionMaintenance/AddMaintenance?attractionId={attractionId}", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode}, Content: {errorContent}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAttractionStatusAsync(int attractionId, string status)
        {
            var statusData = new
            {
                Status = status
            };

            var json = JsonConvert.SerializeObject(statusData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PutAsync($"https://localhost:7129/api/AttractionMaintenance/UpdateStatus?attractionId={attractionId}", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode}, Content: {errorContent}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Maintenance>> GetAllMaintenancesAsync()
        {
            var response = await _httpClient.GetStringAsync("https://localhost:7129/api/AttractionMaintenance/GetAllMaintenances");
            return JsonConvert.DeserializeObject<List<Maintenance>>(response);
        }
    }
}
