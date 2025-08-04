# UYUMYEMEK - Online Yemek Sipariş Sistemi

Bu proje, ASP.NET Core MVC kullanılarak geliştirilmiş kapsamlı bir online yemek sipariş platformudur. Sistem, kullanıcılar, restoran sahipleri ve sistem yöneticileri için farklı yetki seviyelerinde hizmet vermektedir.

## 🚀 Proje Özellikleri

### 👥 Kullanıcı Rolleri

#### 🛡️ Admin (Sistem Yöneticisi)
- Tüm sistem verilerini görüntüleme ve yönetme
- Restoran onay/reddetme işlemleri
- Kullanıcı yönetimi
- Kategori yönetimi
- Sistem istatistikleri ve raporları
- Tüm siparişleri görüntüleme ve yönetme

#### 🏪 Restoran Sahibi
- Restoran bilgilerini güncelleme
- Menü ve ürün yönetimi
- Sipariş takip ve yönetimi
- Satış raporları
- Restoran performans istatistikleri

#### 👤 Kullanıcı (Müşteri)
- Restoran ve menü görüntüleme
- Sepet yönetimi
- Sipariş verme
- Sipariş geçmişi takibi
- Favori restoranlar

### 🛍️ Temel Özellikler

- **Çoklu Kategori Desteği**: Pizza, Burger, Tatlı, İçecek, Makarna
- **Güvenli Ödeme Sistemi**: Kredi kartı ve banka kartı desteği
- **Gerçek Zamanlı Sipariş Takibi**: Sipariş durumu güncellemeleri
- **Responsive Tasarım**: Mobil ve masaüstü uyumlu
- **Çoklu Restoran Desteği**: Farklı restoranlardan sipariş verme
- **Sepet Yönetimi**: Ürün ekleme/çıkarma, miktar güncelleme

## 🛠️ Teknolojiler

- **Framework**: ASP.NET Core MVC (.NET 6/7/8)
- **Veritabanı**: Microsoft SQL Server (MSSQL)
- **ORM**: Entity Framework Core (Database First Approach)
- **Authentication**: ASP.NET Core Cookie Authentication
- **Frontend**: HTML5, CSS3, JavaScript, Bootstrap 5
- **UI Components**: Modern ve kullanıcı dostu responsive arayüz

## 📋 Sistem Gereksinimleri

- .NET 6.0 veya üzeri
- Microsoft SQL Server 2019 veya üzeri (Express sürümü desteklenir)
- Visual Studio 2022 (önerilen) veya Visual Studio Code
- SQL Server Management Studio (SSMS) - opsiyonel
- IIS Express (geliştirme için)

## ⚙️ Kurulum

### 1. Projeyi Klonlayın
```bash
git clone https://github.com/[kullanici-adi]/uyumyemek.git
cd uyumyemek
```

### 2. SQL Server Veritabanını Hazırlayın
Proje **Database First** yaklaşımı kullanmaktadır. Veritabanı şeması önceden hazırlanmış olmalıdır.

#### Veritabanı Oluşturma Seçenekleri:
- **Seçenek A**: Proje içindeki SQL script dosyalarını kullanın
- **Seçenek B**: Backup dosyasını restore edin
- **Seçenek C**: SSMS ile manuel olarak tabloları oluşturun

### 3. Bağlantı String'ini Yapılandırın
`appsettings.json` dosyasında MSSQL connection string'ini güncelleyin:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=UyumYemekDB;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

**Farklı ortamlar için örnek connection string'ler:**

**Yerel SQL Server Express:**
```
Server=localhost\\SQLEXPRESS;Database=UyumYemekDB;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true
```

**SQL Server Authentication:**
```
Server=localhost;Database=UyumYemekDB;User Id=sa;Password=YourPassword;MultipleActiveResultSets=true;TrustServerCertificate=true
```

**Azure SQL Database:**
```
Server=tcp:yourserver.database.windows.net,1433;Database=UyumYemekDB;User ID=yourusername;Password=yourpassword;Encrypt=True;Connection Timeout=30;
```

### 4. Bağımlılıkları Yükleyin
```bash
dotnet restore
```

### 5. Entity Framework Scaffold (Gerekirse)
Eğer model sınıfları yoksa veya veritabanı değişikliklerini yansıtmak istiyorsanız:
```bash
dotnet ef dbcontext scaffold "YourConnectionString" Microsoft.EntityFrameworkCore.SqlServer -o Models -c UyumYemekContext --force
```

### 6. Projeyi Çalıştırın
```bash
dotnet run
```

### 7. İlk Kullanım
- Tarayıcınızda `https://localhost:5001` adresine gidin
- Admin hesabı oluşturmak için `/Account/Register` sayfasını ziyaret edin

## 📊 Veritabanı Yapısı (Database First)

Bu proje **Database First** yaklaşımı kullanmaktadır. Veritabanı şeması önceden tasarlanmış ve Entity Framework Core ile mevcut veritabanından model sınıfları türetilmiştir.

### 🗄️ Ana Tablolar

#### Kullanıcı Yönetimi
- **Users**: Kullanıcı bilgileri, roller ve kimlik doğrulama
- **UserRoles**: Kullanıcı rol ilişkileri

#### Restoran Yönetimi
- **Restaurants**: Restoran bilgileri, sahiplik ve onay durumu
- **RestaurantCategories**: Restoran-kategori ilişkileri

#### Ürün Yönetimi
- **Categories**: Ürün kategorileri (Pizza, Burger, Tatlı, vb.)
- **Products**: Menü ürünleri, fiyatlar ve açıklamalar
- **ProductCategories**: Ürün-kategori ilişkileri

#### Sipariş Yönetimi
- **Orders**: Sipariş ana bilgileri
- **OrderItems**: Sipariş detayları (ürün, miktar, fiyat)
- **OrderStatus**: Sipariş durum takibi

#### Sepet Yönetimi
- **CartItems**: Kullanıcı sepet içeriği
- **Sessions**: Oturum yönetimi

### 🔗 İlişkiler
- **Users** → **Restaurants** (1:N - Bir kullanıcının birden fazla restoranı olabilir)
- **Restaurants** → **Products** (1:N - Bir restoranın birden fazla ürünü)
- **Users** → **Orders** (1:N - Bir kullanıcının birden fazla siparişi)
- **Orders** → **OrderItems** (1:N - Bir siparişin birden fazla kalemi)

### 📝 Örnek Veritabanı Script

```sql
-- Ana kategoriler
INSERT INTO Categories (Name, Description) VALUES 
('Pizza', 'Lezzetli pizzalar'),
('Burger', 'Nefis burgerler'),
('Tatlı', 'Tatlı çeşitleri'),
('İçecek', 'Soğuk ve sıcak içecekler'),
('Makarna', 'İtalyan makarna çeşitleri');

-- Örnek admin kullanıcı
INSERT INTO Users (Email, Name, Role, PasswordHash) VALUES 
('admin@uyumyemek.com', 'Admin User', 'Admin', 'HashedPassword');
```

## 🔐 Kimlik Doğrulama ve Yetkilendirme

### Cookie Authentication
Proje, ASP.NET Core Cookie Authentication kullanır:
- Güvenli oturum yönetimi
- Rol tabanlı erişim kontrolü
- Otomatik yönlendirme

### Roller
```csharp
public static class Roles
{
    public const string Admin = "Admin";
    public const string RestaurantOwner = "RestaurantOwner";
    public const string User = "User";
}
```

## 📱 Kullanıcı Arayüzü

### Ana Sayfa
- Popüler restoranlar
- Kategori filtreleme
- Arama fonksiyonu

### Sepet Yönetimi
- Dinamik fiyat hesaplama
- Teslimat ücreti
- Ödeme seçenekleri

### Sipariş Takibi
- Gerçek zamanlı durum güncellemeleri
- Sipariş geçmişi
- Detaylı fatura görüntüleme

## 👨‍💼 Admin Paneli

### Dashboard
- Sistem genel durumu
- Günlük/aylık istatistikler
- Performans metrikleri

### Yönetim Özellikleri
- Restoran onaylama sistemi
- Kullanıcı yönetimi
- Kategori ve ürün yönetimi
- Sipariş takibi

## 🏪 Restoran Paneli

### Restoran Yönetimi
- Restoran bilgileri güncelleme
- Menü düzenleme
- Ürün fiyat yönetimi

### Sipariş Yönetimi
- Gelen siparişler
- Sipariş durumu güncelleme
- Müşteri bilgileri

## 📈 İstatistikler ve Raporlama

- Günlük/haftalık/aylık satış raporları
- Popüler ürün analizleri
- Müşteri davranış istatistikleri
- Gelir analizleri

## 🔧 Yapılandırma

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=UyumYemekDB;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "AllowedHosts": "*",
  "Authentication": {
    "CookieTimeout": 60
  }
}
```

### Entity Framework Configuration
```csharp
// Program.cs veya Startup.cs
services.AddDbContext<UyumYemekContext>(options =>
    options.UseSqlServer(connectionString));
```

### Database First Model Update
Veritabanında değişiklik yaptığınızda modelleri güncellemek için:
```bash
dotnet ef dbcontext scaffold "YourConnectionString" Microsoft.EntityFrameworkCore.SqlServer -o Models -c UyumYemekContext --force
```

## 🚨 Güvenlik

- XSS koruması
- CSRF token koruması
- SQL Injection koruması
- Güvenli password hashing
- Role-based authorization

## 📞 İletişim ve Destek

- **E-posta**: destek@uyumyemek.com
- **Telefon**: +90 555 333 1122
- **Adres**: Moda Cad. No:4

## 🤝 Katkıda Bulunma

1. Fork edin
2. Feature branch oluşturun (`git checkout -b feature/YeniOzellik`)
3. Değişikliklerinizi commit edin (`git commit -am 'Yeni özellik eklendi'`)
4. Branch'inizi push edin (`git push origin feature/YeniOzellik`)
5. Pull Request oluşturun

## 📝 Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için `LICENSE` dosyasına bakınız.

## 🆕 Sürüm Geçmişi

### v1.0.0 (Mevcut)
- Temel sipariş sistemi
- Çoklu kullanıcı desteği
- Admin ve restoran panelleri
- Cookie authentication
- Responsive tasarım

## 🔮 Gelecek Özellikler

- [ ] Mobil uygulama
- [ ] SignalR ile gerçek zamanlı bildirimler
- [ ] Sosyal medya entegrasyonu
- [ ] Puanlama ve yorum sistemi
- [ ] Coğrafi konum tabanlı teslimat
- [ ] Çoklu ödeme yöntemleri (PayPal, Stripe)
- [ ] Redis cache implementasyonu
- [ ] Elasticsearch entegrasyonu

## ⚠️ Bilinen Sorunlar

- Büyük sipariş hacimlerinde performans optimizasyonu gerekebilir
- Database First yaklaşımda model değişikliklerinde dikkat edilmesi gereken noktalar
- Bazı eski tarayıcılarda görsel sorunlar olabilir

## 🗄️ Veritabanı Yönetimi

### Backup ve Restore
```sql
-- Database Backup
BACKUP DATABASE UyumYemekDB 
TO DISK = 'C:\Backup\UyumYemekDB.bak'

-- Database Restore
RESTORE DATABASE UyumYemekDB 
FROM DISK = 'C:\Backup\UyumYemekDB.bak'
```

### Performance Tips
- İndeksleri düzenli kontrol edin
- Büyük tablolar için partitioning düşünün
- Connection pooling optimize edin

## 📚 Ek Kaynaklar

- [ASP.NET Core Dokümantasyonu](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core Rehberi](https://docs.microsoft.com/ef/core)
- [Bootstrap Dokümantasyonu](https://getbootstrap.com/docs)

---

**Not**: Bu README dosyası projenin güncel durumunu yansıtmaktadır. Güncellemeler için düzenli olarak kontrol ediniz.
