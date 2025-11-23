# 🌤️ Turkish Weather SSE - Türkiye Hava Durumu (Gerçek Zamanlı)

**ASP.NET Core 8** ve **Server-Sent Events (SSE)** kullanarak Türkiye'nin 3 büyük şehrinin (İstanbul, Bursa, Eskişehir) hava durumunu gerçek zamanlı olarak takip eden modern bir web uygulaması.

[English version below](#english-version)

---

## 🇹🇷 Türkçe Versiyon

### ✨ Özellikler

- 🌍 **3 Şehir**: İstanbul 🏙️, Bursa 🌳, Eskişehir 🎓
- 📡 **Gerçek Zamanlı**: Server-Sent Events (SSE) ile WebSocket kullanmadan canlı veri akışı
- 🔄 **Otomatik Güncelleme**: Her 10 saniyede bir hava durumu verileri güncellenir
- 🎨 **Modern Tasarım**: Glassmorphism efektli, responsive ve kullanıcı dostu arayüz
- 🆓 **Ücretsiz API**: Open-Meteo API (API key gerektirmez)
- 🔌 **Otomatik Yeniden Bağlanma**: Bağlantı kesildiğinde otomatik olarak yeniden bağlanır
- 🌡️ **Detaylı Bilgi**: Sıcaklık, rüzgar hızı, nem oranı ve hava durumu açıklaması
- 📱 **Responsive**: Mobil, tablet ve masaüstü cihazlarda mükemmel görünüm

### 🛠️ Teknoloji Stack

- **Backend**: ASP.NET Core 8 (Minimal API)
- **Frontend**: Vanilla JavaScript (Framework kullanılmadı)
- **API**: Open-Meteo API (Ücretsiz hava durumu API'si)
- **SSE**: EventSource API
- **Styling**: Modern CSS (Glassmorphism)

### 📋 SSE Nedir?

**Server-Sent Events (SSE)**, sunucudan istemciye tek yönlü, gerçek zamanlı veri akışı sağlayan bir teknoloji. HTTP protokolü üzerinden çalışır ve WebSocket'e göre daha basittir.

#### SSE vs WebSocket Karşılaştırması

| Özellik | SSE | WebSocket |
|---------|-----|-----------|
| **Yön** | Tek yönlü (Sunucu → İstemci) | Çift yönlü |
| **Protokol** | HTTP | WS/WSS |
| **Komplekslik** | Basit | Daha karmaşık |
| **Tarayıcı Desteği** | Tüm modern tarayıcılar | Tüm modern tarayıcılar |
| **Auto-reconnect** | Yerleşik | Manuel |
| **Veri Formatı** | Text (genelde JSON) | Text/Binary |
| **Kullanım Senaryosu** | Haber akışları, canlı skorlar, hava durumu | Chat, oyunlar, işbirliği araçları |

### 📦 Kurulum

#### Gereksinimler
- .NET 8 SDK ([İndir](https://dotnet.microsoft.com/download/dotnet/8.0))

#### Adımlar

1. **Repoyu klonlayın**
```bash
git clone https://github.com/ycagraoglu/turkish-weather-sse.git
cd turkish-weather-sse
```

2. **Bağımlılıkları yükleyin**
```bash
dotnet restore
```

3. **Uygulamayı çalıştırın**
```bash
dotnet run
```

4. **Tarayıcıda açın**
```
http://localhost:5000
```

### 🏗️ Proje Yapısı

```
turkish-weather-sse/
├── Program.cs                 # Ana uygulama ve SSE endpoint
├── Models/
│   └── WeatherData.cs        # Hava durumu veri modelleri
├── Services/
│   └── WeatherService.cs     # Open-Meteo API entegrasyonu
├── wwwroot/
│   ├── index.html            # Ana HTML sayfası
│   ├── css/
│   │   └── style.css         # Glassmorphism tasarım
│   └── js/
│       └── app.js            # SSE istemci ve UI mantığı
├── turkish-weather-sse.csproj
├── appsettings.json
├── .gitignore
├── README.md
└── LICENSE
```

### 🌐 API Endpoint'leri

#### SSE Stream
```
GET /api/weather/stream
Content-Type: text/event-stream
```

**Response Format:**
```
id: 0
data: [{"city":"İstanbul","cityIcon":"🏙️","temperature":15.3,"windSpeed":12.5,"humidity":65,"description":"Açık","weatherIcon":"☀️","timestamp":"2025-01-15T14:30:00"}]

id: 1
data: [{"city":"İstanbul",...}, {"city":"Bursa",...}, {"city":"Eskişehir",...}]
```

#### Sağlık Kontrolü
```
GET /api/health
```

### 🎯 Hava Durumu Kodları

| Kod | Açıklama | Icon |
|-----|----------|------|
| 0 | Açık | ☀️ |
| 1-3 | Parçalı Bulutlu | ⛅ |
| 45-48 | Sisli | 🌫️ |
| 51-67 | Yağmurlu | 🌧️ |
| 71-77 | Karlı | ❄️ |
| 80-99 | Fırtınalı | ⛈️ |

### 🧪 Test

```bash
# Build kontrolü
dotnet build

# Uygulamayı çalıştır
dotnet run

# Tarayıcıda test et
# 1. http://localhost:5000 adresini aç
# 2. Bağlantı durumunun "Bağlı" olduğunu kontrol et
# 3. Kartların güncellenmesini izle (her 10 saniye)
# 4. Mobil responsive testi için tarayıcı geliştirici araçlarını kullan
```

### 📸 Ekran Görüntüleri

Uygulama çalışır durumda modern glassmorphism tasarımlı kartlar gösterir. Her kart:
- Şehir adı ve emoji simgesi
- Büyük hava durumu ikonu
- Sıcaklık değeri
- Hava durumu açıklaması
- Rüzgar hızı ve nem oranı
- Son güncelleme zamanı

### 🤝 Katkıda Bulunma

Katkılarınız her zaman memnuniyetle karşılanır! Lütfen pull request göndermekten çekinmeyin.

### 📄 Lisans

Bu proje [MIT Lisansı](LICENSE) altında lisanslanmıştır.

### 👨‍💻 Geliştirici

**ycagraoglu**

---

## 🇬🇧 English Version

### ✨ Features

- 🌍 **3 Cities**: Istanbul 🏙️, Bursa 🌳, Eskişehir 🎓
- 📡 **Real-time**: Live data streaming with Server-Sent Events (SSE) without WebSocket
- 🔄 **Auto-update**: Weather data refreshes every 10 seconds
- 🎨 **Modern Design**: Glassmorphism effects, responsive and user-friendly interface
- 🆓 **Free API**: Open-Meteo API (no API key required)
- 🔌 **Auto-reconnect**: Automatically reconnects when connection is lost
- 🌡️ **Detailed Info**: Temperature, wind speed, humidity and weather description
- 📱 **Responsive**: Perfect view on mobile, tablet and desktop devices

### 🛠️ Tech Stack

- **Backend**: ASP.NET Core 8 (Minimal API)
- **Frontend**: Vanilla JavaScript (No framework)
- **API**: Open-Meteo API (Free weather API)
- **SSE**: EventSource API
- **Styling**: Modern CSS (Glassmorphism)

### 📋 What is SSE?

**Server-Sent Events (SSE)** is a technology that provides unidirectional, real-time data streaming from server to client. It works over HTTP protocol and is simpler than WebSocket.

#### SSE vs WebSocket Comparison

| Feature | SSE | WebSocket |
|---------|-----|-----------|
| **Direction** | Unidirectional (Server → Client) | Bidirectional |
| **Protocol** | HTTP | WS/WSS |
| **Complexity** | Simple | More complex |
| **Browser Support** | All modern browsers | All modern browsers |
| **Auto-reconnect** | Built-in | Manual |
| **Data Format** | Text (usually JSON) | Text/Binary |
| **Use Cases** | News feeds, live scores, weather | Chat, games, collaboration tools |

### 📦 Installation

#### Requirements
- .NET 8 SDK ([Download](https://dotnet.microsoft.com/download/dotnet/8.0))

#### Steps

1. **Clone the repository**
```bash
git clone https://github.com/ycagraoglu/turkish-weather-sse.git
cd turkish-weather-sse
```

2. **Restore dependencies**
```bash
dotnet restore
```

3. **Run the application**
```bash
dotnet run
```

4. **Open in browser**
```
http://localhost:5000
```

### 🏗️ Project Structure

```
turkish-weather-sse/
├── Program.cs                 # Main application and SSE endpoint
├── Models/
│   └── WeatherData.cs        # Weather data models
├── Services/
│   └── WeatherService.cs     # Open-Meteo API integration
├── wwwroot/
│   ├── index.html            # Main HTML page
│   ├── css/
│   │   └── style.css         # Glassmorphism design
│   └── js/
│       └── app.js            # SSE client and UI logic
├── turkish-weather-sse.csproj
├── appsettings.json
├── .gitignore
├── README.md
└── LICENSE
```

### 🌐 API Endpoints

#### SSE Stream
```
GET /api/weather/stream
Content-Type: text/event-stream
```

**Response Format:**
```
id: 0
data: [{"city":"İstanbul","cityIcon":"🏙️","temperature":15.3,"windSpeed":12.5,"humidity":65,"description":"Açık","weatherIcon":"☀️","timestamp":"2025-01-15T14:30:00"}]

id: 1
data: [{"city":"İstanbul",...}, {"city":"Bursa",...}, {"city":"Eskişehir",...}]
```

#### Health Check
```
GET /api/health
```

### 🎯 Weather Codes

| Code | Description | Icon |
|------|-------------|------|
| 0 | Clear | ☀️ |
| 1-3 | Partly Cloudy | ⛅ |
| 45-48 | Foggy | 🌫️ |
| 51-67 | Rainy | 🌧️ |
| 71-77 | Snowy | ❄️ |
| 80-99 | Stormy | ⛈️ |

### 🧪 Testing

```bash
# Build check
dotnet build

# Run the application
dotnet run

# Test in browser
# 1. Open http://localhost:5000
# 2. Check connection status shows "Connected"
# 3. Watch cards update (every 10 seconds)
# 4. Use browser dev tools for mobile responsive test
```

### 🤝 Contributing

Contributions are always welcome! Please feel free to submit a pull request.

### 📄 License

This project is licensed under the [MIT License](LICENSE).

### 👨‍💻 Developer

**ycagraoglu**

---

Made with ❤️ using ASP.NET Core 8 and Server-Sent Events
