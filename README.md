# 🧩 CourseApp - Hata Dokümentasyonu

Bu proje, **geliştiricilerin hata bulma ve düzeltme becerilerini test etmek** amacıyla **bilinçli olarak çeşitli seviyelerde hatalar** içermektedir.  
Projedeki hatalar, *build (derleme)*, *runtime (çalışma zamanı)*, *mantıksal (logic)*, *performans* ve *mimari (architecture)* kategorilerine ayrılmıştır.

---

## 📊 Hata İstatistikleri

| Seviye | Tahmini Hata Sayısı | Tür |
|:--|:--:|:--|
| 🟢 Kolay | 20+ | Derleme (build) hataları |
| 🟡 Orta | 40+ | Runtime ve mantıksal hatalar |
| 🔴 Zor | 15+ | Mimari ve performans sorunları |
| **Toplam** | **75+** | — |

---

## 🟢 KOLAY SEVİYE HATALAR (Build Hataları)

Bu hatalar, projenin derlenmesini doğrudan engelleyen **sentaks ve isimlendirme** problemleridir.  
IDE veya derleyici çıktısı incelenerek kolayca tespit edilebilir.

### Örnek Hata Türleri:
- Noktalı virgül eksiklikleri  
- Yazım (typo) hataları — değişken, metod veya servis isimlerinde  
- Yanlış tip kullanımı  
- Eksik `using` bildirimleri  
- Servis konfigürasyonlarında yazım bozuklukları  

### Bulunabilecek Dosya Alanları:
- Controllers ("Create" ve "Update" metotları)  
- Service katmanındaki `Manager` sınıfları  
- `Program.cs` içerisindeki servis kayıt bölümü  

---

## 🟡 ORTA SEVİYE HATALAR (Runtime ve Mantıksal Hatalar)

Bu hatalar, derlemeyi engellemez ancak uygulama çalışırken beklenmedik davranışlara neden olur.  
Bazıları exception fırlatır, bazıları ise yanlış veri döndürür.

### Örnek Hata Türleri:
- **Null Reference Exception:**  
  Nesneler kullanılmadan önce null kontrolü yapılmamış.  
- **Index Out of Range Exception:**  
  Liste veya string üzerinde hatalı indis erişimleri bulunuyor.  
- **Invalid Cast Exception:**  
  Tip dönüşümleri yanlış yapılmış.  
- **Mantıksal Hatalar:**  
  Yanlış result tipleri (`ErrorResult` yerine `SuccessResult` vb.) veya yanlış mesaj dönüşleri.  

### Bulunabilecek Dosya Alanları:
- Controllers’daki CRUD işlemleri  
- `Manager` sınıflarının `CreateAsync`, `Update`, `GetByIdAsync` metotları  
- DTO dönüşümlerinin yapıldığı alanlar  

---

## 🔴 ZOR SEVİYE HATALAR (Mimari ve Performans Sorunları)

Bu seviyedeki hatalar, **uygulamanın mimarisini, veri bütünlüğünü ve performansını etkiler.**  
Sistem stabil çalışıyor görünse bile uzun vadede ciddi problemlere yol açabilir.

### Örnek Hata Türleri:
- **N+1 Query Problemleri:** Lazy loading nedeniyle her kayıt için ayrı sorgular çalışıyor.  
- **Async/Await Anti-Pattern:** `.Result`, `.Wait()` veya `GetAwaiter().GetResult()` kullanımı deadlock riski yaratıyor.  
- **Katman İhlali:** Controller katmanının doğrudan `DbContext` veya `DataAccessLayer`'a erişmesi.  
- **Memory Leak:** `DbContext` veya dosya işlemlerinde dispose edilmeyen kaynaklar.  
- **Yanlış DI Kullanımı:** `AddScoped` yerine `AddSingleton` gibi hatalı lifetime seçimleri.  

### Bulunabilecek Dosya Alanları:
- `CourseApp.ServiceLayer.Concrete` altındaki tüm Manager sınıfları  
- `Controllers` dizinindeki `Create` ve `GetAll` metotları  
- `Program.cs` konfigürasyon bölümü  

---


## 🎯 Hata Kategorileri

| Kategori | Açıklama |
|:--|:--|
| **Build Hataları** | Derleme aşamasında IDE veya compiler tarafından yakalanan hatalar. |
| **Runtime Hataları** | Uygulama çalışırken ortaya çıkan istisnalar veya beklenmeyen davranışlar. |
| **Mantıksal Hatalar** | Kod doğru çalışsa da yanlış sonuçlar üretir. |
| **Performans Sorunları** | N+1, gereksiz async beklemeleri veya yetersiz caching nedeniyle yavaşlama. |
| **Mimari Sorunlar** | Katman bağımlılıklarının ihlali veya SOLID prensiplerine aykırı yapılar. |

---

## 🔍 Hata Bulma İpuçları

- **Build hataları:** IDE veya terminal çıktısından compiler mesajlarını takip edin.  
- **Runtime hataları:** Exception loglarını ve stack trace’leri inceleyin.  
- **Mantıksal hatalar:** Test senaryoları yazın veya debug modunda kodu adım adım izleyin.  
- **Performans sorunları:** SQL Profiler, dotTrace veya Application Insights gibi profiler aracıları kullanın.  
- **Mimari sorunlar:** Katman bağımlılıklarını, servis kayıtlarını ve kod yapısının SOLID prensiplerine uygunluğunu kontrol edin.  

---

## ⚠️ Not

Bu projedeki hatalar **tamamen kasıtlı** olarak eklenmiştir.  
Her hata, ilgili satır yakınında **yorum satırı (// [BugSeed])** etiketiyle işaretlenmiştir.  
Katılımcıların görevi, bu hataları bulup düzeltmek ve projeyi başarıyla derleyip çalışır hale getirmektir.


📅 **Son Güncelleme:** 2025-02-11  
📦 **Toplam Hata Sayısı:** 75+  

💪 **Başarılar dileriz — iyi kod avı!**

ORTA SEVİYE HATALAR

1.Null Reference Exception
Örnek: var courseName = createCourseDto?.CourseName ?? "Değer Null!";  Nesnenin null kontrolü yapılmış ve eğer nullsa default değer verilmiştir.

2.Index Out of Range Exception:
Örnek: if (!string.IsNullOrEmpty(courseName))
{
    firstChar = courseName[0];
} Nesnenin null kontrolü yapılmış ve eğer boş değilse ilk değerin ataması yapılmıştır.

3.Invalid Cast Exception:
Örnek: if (int.TryParse(createRegistrationDto.Price.ToString(), out int price))
{
    var invalidPrice = price;
} Burada tip dönüştürme başarılıysa true döner price değerine atanır false ise hata vererek kodun çalışmasını bozmaz.

4. Mantıksal Hatalar
Örnek: if (result > 0)
{
    return new SuccessResult(ConstantsMessages.RegistrationUpdateSuccessMessage);
}
return new ErrorResult(ConstantsMessages.RegistrationUpdateFailedMessage); Burada başarılı sonuçta SuccessResult hatada ise ErrorResult döndürülerek çözüldü.

ZOR SEVİYE HATALAR

1.N+1 Query Problemleri
Örnek: var registrationData = await _unitOfWork.Registrations.GetAll(false)
                                .Include(c => c.Course)
                                .Include(c=>c.Student)
                                .ToListAsync(); Burada Include() metodu ile EF Core sayesinde ilişkili tabloları tek seferde çekerek hatayı çözdük.
2.Async/Await Anti-Pattern
Örnek: // await _unitOfWork.Registrations.CreateAsync(createdRegistration); Burada asenkron metod çağırıldığı için wait kullanıldı thread bloklanmadan işlem doğal olarak devam ettirildi deadlock riski ortadan kalktı.

3.Katman İhlali
Örnek: //using CourseApp.DataAccessLayer.Concrete; // ZOR: Katman ihlali - Controller'dan direkt DataAccessLayer'a erişim Burada API Controllerdan direkt DataLyer'a referans kaldırıldı ve BusinessLayerda ilgili interface (private readonly IStudentService _studentService;) kullanıldı.

4.Memory Leak
Örnek: public StudentsController(IStudentService studentService)
{
    _studentService = studentService;
} Burada AppDbContext öğesini katman ihlali dolayısıyla kaldırdığımızdan buradaki işlemi BusinessLayer'a taşıdık bu yüzden context dispose etmeye gerek kalmadı ve memory leak ortadak kaldırıldı.

5.DbUpdateException 
örnek:public IQueryable<Course>? Courses { get; set; } Burada EF Core navigation property’leri ICollection<T> olarak beklerken biz IQueryable<Course> olarak kullandığımızdan foreign key i çoğulluyor bu yüzden public ICollection<Course>? Courses { get; set; } = []; olarak güncelleyip hatayı kaldırıyoruz.
