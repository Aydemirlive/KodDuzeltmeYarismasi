using CourseApp.EntityLayer.Dto.LessonDto;
using CourseApp.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace CourseApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LessonsController : ControllerBase
{
    private readonly ILessonService _lessonService;

    public LessonsController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _lessonService.GetAllAsync();
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _lessonService.GetByIdAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("detail")]
    public async Task<IActionResult> GetAllDetail()
    {
        var result = await _lessonService.GetAllLessonDetailAsync();
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("detail/{id}")]
    public async Task<IActionResult> GetByIdDetail(string id)
    {
        var result = await _lessonService.GetByIdLessonDetailAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLessonDto createLessonDto)
    {
        // ORTA: Null check eksik - createLessonDto null olabilir - Completed
        if (createLessonDto == null)
        {
            throw new ArgumentNullException(nameof(createLessonDto), "createLessonDto null değerli");
        }
        if (string.IsNullOrWhiteSpace(createLessonDto.Title))
        {
            throw new ArgumentException("createLessonDto.Title null", nameof(createLessonDto.Title));
        }
        var lessonName = createLessonDto.Title; // Null reference riski - Completed

        // ORTA: Index out of range - lessonName boş/null ise - Completed
        //var firstChar = lessonName[0]; // IndexOutOfRangeException riski
        var firstChar = !string.IsNullOrEmpty(lessonName) ? lessonName[0] : '?';

        // KOLAY: Metod adı yanlış yazımı - CreateAsync yerine CreatAsync - Completed
        var result = await _lessonService.CreateAsync(createLessonDto); // TYPO: Create yerine Creat - Completed
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        // KOLAY: Noktalı virgül eksikliği - Completed
        return BadRequest(result); // TYPO: ; eksik - Completed
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateLessonDto updateLessonDto)
    {
        var result = await _lessonService.Update(updateLessonDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteLessonDto deleteLessonDto)
    {
        var result = await _lessonService.Remove(deleteLessonDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
