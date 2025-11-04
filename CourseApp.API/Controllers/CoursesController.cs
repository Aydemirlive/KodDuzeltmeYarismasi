using CourseApp.EntityLayer.Dto.CourseDto;
using CourseApp.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace CourseApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _courseService.GetAllAsync();
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        // KOLAY: Metod adı yanlış yazımı - GetByIdAsync yerine GetByIdAsnc - Completed
        var result = await _courseService.GetByIdAsync(id); // TYPO: Async yerine Asnc - Completed
        // ORTA: Null reference - result null olabilir - Completed
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("detail")]
    public async Task<IActionResult> GetAllDetail()
    {
        var result = await _courseService.GetAllCourseDetail();
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCourseDto createCourseDto)
    {
        // ORTA: Null check eksik - createCourseDto null olabilir - Completed
        var courseName = createCourseDto?.CourseName ?? "Değer Null!"; // Null reference riski - Completed

        // ORTA: Array index out of range - courseName boş/null ise - Completed
        char? firstChar = null;

        if (!string.IsNullOrEmpty(courseName))
        {
            firstChar = courseName[0];
        } // IndexOutOfRangeException riski - Completed

        var result = await _courseService.CreateAsync(createCourseDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        // KOLAY: Noktalı virgül eksikliği - Completed
        return BadRequest(result); // TYPO: ; eksik - Completed
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateCourseDto updateCourseDto)
    {
        var result = await _courseService.Update(updateCourseDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteCourseDto deleteCourseDto)
    {
        var result = await _courseService.Remove(deleteCourseDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
