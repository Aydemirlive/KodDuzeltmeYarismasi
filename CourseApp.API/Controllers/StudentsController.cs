using CourseApp.EntityLayer.Dto.StudentDto;
using CourseApp.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;// KOLAY: Eksik using - System.Text.Json kullanılıyor ama using yok
//using CourseApp.DataAccessLayer.Concrete; // ZOR: Katman ihlali - Controller'dan direkt DataAccessLayer'a erişim

namespace CourseApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    // ZOR: Katman ihlali - Presentation katmanından direkt DataAccess katmanına erişim - Completed
    //private readonly AppDbContext _dbContext;
    // ORTA: Değişken tanımlandı ama asla kullanılmadı ve null olabilir - Completed
    private List<GetAllStudentDto>_cachedStudents = new();

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
        //_dbContext = dbContext; // ZOR: Katman ihlali - Completed
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // ORTA: Null reference exception riski - _cachedStudents null - Completed
        if (_cachedStudents != null && _cachedStudents.Any())
        {
            return Ok(_cachedStudents); // Mantıksal hata: cache kontrolü yanlış - Completed
        }
        
        var result = await _studentService.GetAllAsync();
        // KOLAY: Metod adı yanlış yazımı - Success yerine Succes - Completed
        /*if (result.IsSuccess) // TYPO: Success yerine Succes
        {
            return Ok(result);
        }*/
        if (result != null && result.IsSuccess && result.Data != null)
        {
            _cachedStudents = result.Data.ToList();
            return Ok(_cachedStudents);
        }
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        // ORTA: Null check eksik - id null/empty olabilir - Completed
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest();
        }
        // ORTA: Index out of range riski - string.Length kullanımı yanlış olabilir - Completed
        if (id.Length > 10)
        {
            char? studentIdChar = id[10];
        }
        var studentId = id[10]; // ORTA: id 10 karakterden kısa olursa IndexOutOfRangeException - Completed

        var result = await _studentService.GetByIdAsync(id);
        // ORTA: Null reference exception - result.Data null olabilir - Completed
        if (result == null || result.Data == null)
        {
            return BadRequest(); ;
        }
        var studentName = result.Data.Name ?? " "; // Null check yok - Completed
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStudentDto createStudentDto)
    {
        // ORTA: Null check eksik - Completed
        // ORTA: Tip dönüşüm hatası - string'i int'e direkt atama - Completed
        //var invalidAge = (int)createStudentDto.Name; // ORTA: InvalidCastException - string int'e dönüştürülemez - Completed
        if (createStudentDto == null || string.IsNullOrWhiteSpace(createStudentDto.Name))
        {
            // DTO veya Name boş ise default değer ver veya hata döndür  - Completed
            return BadRequest();
        }
        // ZOR: Katman ihlali - Controller'dan direkt DbContext'e erişim (Business Logic'i bypass ediyor) - Completed
        //var directDbAccess = await _studentService.CreateAsync(createStudentDto);

        var result = await _studentService.CreateAsync(createStudentDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        // KOLAY: Noktalı virgül eksikliği - Completed
        return BadRequest(result); // TYPO: ; eksik - Completed
    } 

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateStudentDto updateStudentDto)
    {
        // KOLAY: Değişken adı typo - updateStudentDto yerine updateStudntDto - Completed
        var name = updateStudentDto.Name; // TYPO
        
        var result = await _studentService.Update(updateStudentDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteStudentDto deleteStudentDto)
    {
        // ORTA: Null reference - deleteStudentDto null olabilir - Completed
        if (deleteStudentDto == null)
        {
            return BadRequest();
        }
        var id = deleteStudentDto.Id; // Null check yok - Completed
        if (deleteStudentDto == null || string.IsNullOrWhiteSpace(deleteStudentDto.Id))
        {
            return BadRequest();
        }
        // ZOR: Memory leak - DbContext Dispose edilmiyor - Completed
        //var tempContext = new AppDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<AppDbContext>());
        //tempContext.Students.ToList(); // Dispose edilmeden kullanılıyor - Completed

        var result = await _studentService.Remove(deleteStudentDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
