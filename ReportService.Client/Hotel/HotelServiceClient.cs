using System.Net.Http;
using System.Text.Json;
using App.Core.Results;
using ReportService.Client.Hotel.Model;

namespace ReportService.Client.Hotel;

public class HotelServiceClient : IHotelServiceClient
{
    private readonly HttpClient _httpClient;

    public HotelServiceClient(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("HotelServiceClient");
    }

    public async Task<Result<HotelDto[]>> GetHotelByLocationAsync(string location)
    {
        var response = await _httpClient.GetAsync($"/api/hotel/location/{location}");


        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error fetching hotel data: {response.StatusCode}");
        }
        
        var responseData = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result<HotelDto[]>>(responseData, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}
