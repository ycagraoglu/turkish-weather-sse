using System.Text.Json;
using turkish_weather_sse.Models;

namespace turkish_weather_sse.Services;

/// <summary>
/// Open-Meteo API kullanarak hava durumu verilerini getiren servis
/// </summary>
public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WeatherService> _logger;

    // Şehir bilgileri: Ad, Enlem, Boylam, Emoji
    private readonly List<(string Name, double Lat, double Lon, string Icon)> _cities = new()
    {
        ("İstanbul", 41.0082, 28.9784, "🏙️"),
        ("Bursa", 40.1826, 29.0665, "🌳"),
        ("Eskişehir", 39.7767, 30.5206, "🎓")
    };

    public WeatherService(HttpClient httpClient, ILogger<WeatherService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// Tüm şehirler için hava durumu verilerini paralel olarak getirir
    /// </summary>
    public async Task<List<WeatherData>> GetAllCitiesWeatherAsync()
    {
        try
        {
            // Paralel API çağrıları (Task.WhenAll)
            var tasks = _cities.Select(city => GetCityWeatherAsync(city.Name, city.Lat, city.Lon, city.Icon));
            var results = await Task.WhenAll(tasks);
            return results.OfType<WeatherData>().ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hava durumu verileri alınırken hata oluştu");
            return new List<WeatherData>();
        }
    }

    /// <summary>
    /// Belirli bir şehir için hava durumu verisini Open-Meteo API'den getirir
    /// </summary>
    private async Task<WeatherData?> GetCityWeatherAsync(string cityName, double latitude, double longitude, string cityIcon)
    {
        try
        {
            var url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude:F4}&longitude={longitude:F4}&current=temperature_2m,windspeed_10m,relativehumidity_2m,weathercode&timezone=Europe/Istanbul";
            
            _logger.LogInformation("API isteği gönderiliyor: {City}", cityName);
            
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<OpenMeteoResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (apiResponse?.Current == null)
            {
                _logger.LogWarning("API'den geçersiz yanıt alındı: {City}", cityName);
                return null;
            }

            var (description, icon) = GetWeatherDescription(apiResponse.Current.Weathercode);

            return new WeatherData
            {
                City = cityName,
                CityIcon = cityIcon,
                Temperature = Math.Round(apiResponse.Current.Temperature_2m, 1),
                WindSpeed = Math.Round(apiResponse.Current.Windspeed_10m, 1),
                Humidity = apiResponse.Current.Relativehumidity_2m,
                Description = description,
                WeatherIcon = icon,
                Timestamp = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Şehir hava durumu alınırken hata: {City}", cityName);
            return null;
        }
    }

    /// <summary>
    /// Weather code'u Türkçe açıklama ve emoji'ye çevirir
    /// WMO Weather interpretation codes
    /// </summary>
    private (string Description, string Icon) GetWeatherDescription(int weatherCode)
    {
        return weatherCode switch
        {
            0 => ("Açık", "☀️"),
            1 or 2 or 3 => ("Parçalı Bulutlu", "⛅"),
            45 or 48 => ("Sisli", "🌫️"),
            >= 51 and <= 67 => ("Yağmurlu", "🌧️"),
            >= 71 and <= 77 => ("Karlı", "❄️"),
            >= 80 and <= 99 => ("Fırtınalı", "⛈️"),
            _ => ("Bilinmeyen", "🌈")
        };
    }
}
