using CarSharing.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace CarSharing.Service
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Province>> GetProvincesAsync()
        {
            var response = await _httpClient.GetAsync("https://provinces.open-api.vn/api/p");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var provinces = JsonConvert.DeserializeObject<List<Province>>(content);
                return provinces;
            }

            return null;
        }

        public async Task<List<District>> GetDistrictsByProvinceAsync(int provinceCode)
        {
            var url = $"https://provinces.open-api.vn/api/p/{provinceCode}?depth=2";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var province = JsonConvert.DeserializeObject<Province>(content);

                if (province != null)
                {
                    var districts = province.Districts.ToList();
                    return districts;
                }
            }
            return null;
        }

    }
}