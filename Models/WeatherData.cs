namespace turkish_weather_sse.Models;

/// <summary>
/// Şehir hava durumu verisi modeli
/// </summary>
public class WeatherData
{
    /// <summary>
    /// Şehir adı (İstanbul, Bursa, Eskişehir)
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Şehir emoji simgesi
    /// </summary>
    public string CityIcon { get; set; } = string.Empty;

    /// <summary>
    /// Sıcaklık (°C)
    /// </summary>
    public double Temperature { get; set; }

    /// <summary>
    /// Rüzgar hızı (km/h)
    /// </summary>
    public double WindSpeed { get; set; }

    /// <summary>
    /// Nem oranı (%)
    /// </summary>
    public int Humidity { get; set; }

    /// <summary>
    /// Hava durumu açıklaması (Açık, Bulutlu, vb.)
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Hava durumu emoji simgesi
    /// </summary>
    public string WeatherIcon { get; set; } = string.Empty;

    /// <summary>
    /// Güncelleme zamanı
    /// </summary>
    public DateTime Timestamp { get; set; }
}

/// <summary>
/// Open-Meteo API yanıt modeli
/// </summary>
public class OpenMeteoResponse
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public CurrentWeather? Current { get; set; }
}

/// <summary>
/// Open-Meteo API mevcut hava durumu modeli
/// </summary>
public class CurrentWeather
{
    public double Temperature_2m { get; set; }
    public double Windspeed_10m { get; set; }
    public int Relativehumidity_2m { get; set; }
    public int Weathercode { get; set; }
}
