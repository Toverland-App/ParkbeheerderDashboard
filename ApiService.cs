using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
    }
}
