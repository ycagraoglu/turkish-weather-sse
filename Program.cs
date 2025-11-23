using System.Text.Json;
using turkish_weather_sse.Services;

var builder = WebApplication.CreateBuilder(args);

// CORS policy ekle
// Not: Üretim ortamında AllowAnyOrigin yerine belirli origin'ler kullanılmalıdır
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// HttpClient ve WeatherService'i DI container'a ekle
builder.Services.AddHttpClient<WeatherService>();

var app = builder.Build();

// Static files ve default files middleware
app.UseDefaultFiles();
app.UseStaticFiles();

// CORS aktif
app.UseCors();

/// <summary>
/// SSE endpoint: Her 10 saniyede bir 3 şehir için hava durumu gönderir
/// Content-Type: text/event-stream
/// Format: id: X\ndata: {...}\n\n
/// </summary>
app.MapGet("/api/weather/stream", async (HttpContext context, WeatherService weatherService, ILogger<Program> logger) =>
{
    // SSE için gerekli header'lar
    context.Response.Headers.ContentType = "text/event-stream";
    context.Response.Headers.CacheControl = "no-cache";
    context.Response.Headers.Connection = "keep-alive";
    
    logger.LogInformation("SSE bağlantısı kuruldu: {ConnectionId}", context.Connection.Id);

    var cancellationToken = context.RequestAborted;
    var eventId = 0;

    try
    {
        // Sonsuz döngü - her 10 saniyede bir veri gönder
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                // Tüm şehirler için hava durumu verilerini al (paralel)
                var weatherData = await weatherService.GetAllCitiesWeatherAsync();

                if (weatherData.Any())
                {
                    // JSON serialize et
                    var json = JsonSerializer.Serialize(weatherData, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    // SSE formatında gönder: id: X\ndata: {...}\n\n
                    await context.Response.WriteAsync($"id: {eventId}\n");
                    await context.Response.WriteAsync($"data: {json}\n\n");
                    await context.Response.Body.FlushAsync(cancellationToken);

                    logger.LogInformation("Hava durumu verisi gönderildi. Event ID: {EventId}", eventId);
                    eventId++;
                }

                // 10 saniye bekle
                await Task.Delay(10000, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                // Bağlantı kesildi, normal durum
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "SSE veri gönderimi sırasında hata");
                // Hata durumunda da devam et
                await Task.Delay(10000, cancellationToken);
            }
        }
    }
    catch (OperationCanceledException)
    {
        logger.LogInformation("SSE bağlantısı kapatıldı: {ConnectionId}", context.Connection.Id);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "SSE endpoint hatası");
    }
});

// Sağlık kontrolü endpoint'i
app.MapGet("/api/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.Now }));

// Logger'ı al ve başlangıç mesajlarını yazdır
var startupLogger = app.Services.GetRequiredService<ILogger<Program>>();
startupLogger.LogInformation("Uygulama başlatılıyor...");
startupLogger.LogInformation("SSE Endpoint: /api/weather/stream");
startupLogger.LogInformation("Ana sayfa: http://localhost:5000");

app.Run();
