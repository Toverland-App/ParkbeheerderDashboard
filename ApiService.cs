using System.Collections.Generic;
using System.Dynamic;
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

            var response = await _httpClient.PostAsync($"https://localhost:7129/api/AttractionMaintenance/AddMaintenance?attractionId={attractionId}", content);

            return response.IsSuccessStatusCode;
        }
    }
}
