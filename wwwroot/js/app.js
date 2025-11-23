// SSE bağlantısı ve durum yönetimi
let eventSource = null;
let reconnectAttempts = 0;
const maxReconnectAttempts = 5;
const reconnectDelay = 3000;

// Sayfa yüklendiğinde SSE bağlantısını başlat
document.addEventListener('DOMContentLoaded', () => {
    console.log('🚀 Uygulama başlatılıyor...');
    connectToSSE();
});

/**
 * SSE bağlantısını başlatır
 */
function connectToSSE() {
    try {
        // EventSource ile SSE endpoint'e bağlan
        eventSource = new EventSource('/api/weather/stream');

        // Bağlantı açıldığında
        eventSource.onopen = () => {
            console.log('✅ SSE bağlantısı kuruldu');
            updateConnectionStatus('connected', 'Bağlı');
            reconnectAttempts = 0;
        };

        // Mesaj alındığında
        eventSource.onmessage = (event) => {
            try {
                console.log('📨 Veri alındı:', event.data);
                const weatherData = JSON.parse(event.data);
                updateWeatherCards(weatherData);
            } catch (error) {
                console.error('❌ Veri parse hatası:', error);
            }
        };

        // Hata oluştuğunda
        eventSource.onerror = (error) => {
            console.error('❌ SSE bağlantı hatası:', error);
            updateConnectionStatus('disconnected', 'Bağlantı Kesildi');
            
            // Bağlantıyı kapat ve yeniden bağlanmayı dene
            eventSource.close();
            attemptReconnect();
        };

    } catch (error) {
        console.error('❌ SSE başlatma hatası:', error);
        updateConnectionStatus('disconnected', 'Hata');
        attemptReconnect();
    }
}

/**
 * Yeniden bağlanma denemesi
 */
function attemptReconnect() {
    if (reconnectAttempts < maxReconnectAttempts) {
        reconnectAttempts++;
        updateConnectionStatus('connecting', `Yeniden bağlanıyor... (${reconnectAttempts}/${maxReconnectAttempts})`);
        
        console.log(`🔄 Yeniden bağlanma denemesi ${reconnectAttempts}/${maxReconnectAttempts}`);
        
        setTimeout(() => {
            connectToSSE();
        }, reconnectDelay);
    } else {
        updateConnectionStatus('disconnected', 'Bağlantı Başarısız');
        console.error('❌ Maksimum yeniden bağlanma denemesi aşıldı');
    }
}

/**
 * Bağlantı durumunu günceller
 */
function updateConnectionStatus(status, text) {
    const indicator = document.getElementById('statusIndicator');
    const statusText = document.getElementById('statusText');
    
    // Mevcut tüm sınıfları kaldır
    indicator.classList.remove('connected', 'disconnected', 'connecting');
    
    // Yeni durumu ekle
    if (status === 'connected') {
        indicator.classList.add('connected');
    } else if (status === 'disconnected') {
        indicator.classList.add('disconnected');
    }
    
    statusText.textContent = text;
}

/**
 * Türkçe karakterleri İngilizce karşılıklarına dönüştürür
 */
function normalizeTurkishChars(text) {
    const turkishChars = { 'ı': 'i', 'ş': 's', 'ğ': 'g', 'ü': 'u', 'ö': 'o', 'ç': 'c', 'İ': 'i', 'Ş': 's', 'Ğ': 'g', 'Ü': 'u', 'Ö': 'o', 'Ç': 'c' };
    return text.toLowerCase().split('').map(char => turkishChars[char] || char).join('');
}

/**
 * Hava durumu kartlarını günceller
 */
function updateWeatherCards(weatherDataArray) {
    weatherDataArray.forEach(data => {
        const cityKey = normalizeTurkishChars(data.city);
        const card = document.getElementById(`${cityKey}-card`);
        
        if (card) {
            // Güncelleme animasyonu ekle
            card.classList.add('updating');
            setTimeout(() => card.classList.remove('updating'), 1000);
            
            // Hava durumu ikonu
            const weatherIcon = card.querySelector('.weather-icon');
            weatherIcon.textContent = data.weatherIcon;
            
            // Sıcaklık
            const temperature = card.querySelector('.temperature');
            temperature.textContent = `${data.temperature}°C`;
            
            // Açıklama
            const description = card.querySelector('.weather-description');
            description.textContent = data.description;
            
            // Rüzgar
            const windValue = card.querySelectorAll('.detail-value')[0];
            windValue.textContent = `${data.windSpeed} km/h`;
            
            // Nem
            const humidityValue = card.querySelectorAll('.detail-value')[1];
            humidityValue.textContent = `${data.humidity}%`;
            
            // Son güncelleme zamanı
            const lastUpdate = card.querySelector('.last-update');
            const updateTime = new Date(data.timestamp);
            lastUpdate.textContent = `Son güncelleme: ${formatTime(updateTime)}`;
            
            console.log(`✅ ${data.city} kartı güncellendi`);
        }
    });
}

/**
 * Zamanı formatlar (HH:MM:SS)
 */
function formatTime(date) {
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const seconds = String(date.getSeconds()).padStart(2, '0');
    return `${hours}:${minutes}:${seconds}`;
}

/**
 * Sayfa kapatılırken bağlantıyı temizle
 */
window.addEventListener('beforeunload', () => {
    if (eventSource) {
        eventSource.close();
        console.log('🔌 SSE bağlantısı kapatıldı');
    }
});
