using CourseApp.EntityLayer.Dto.InstructorDto;
using CourseApp.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace CourseApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstructorsController : ControllerBase
{
    private readonly IInstructorService _instructorService;

    public InstructorsController(IInstructorService instructorService)
    {
        _instructorService = instructorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _instructorService.GetAllAsync();
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _instructorService.GetByIdAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatedInstructorDto createdInstructorDto)
    {
        // ORTA: Null check eksik - createdInstructorDto null olabilir - Completed
        var instructorName = createdInstructorDto.Name; // Null reference riski - Completed
        if (createdInstructorDto != null)
        {
            instructorName = createdInstructorDto.Name;
        }
        else
        {
            return BadRequest("Instructor Null!"); 
        }

        // ORTA: Index out of range - instructorName boş/null ise - Completed
        //var firstChar = instructorName[0]; // IndexOutOfRangeException riski
        char? firstChar = !string.IsNullOrEmpty(instructorName) ? instructorName[0] : (char?)null;
        // ORTA: Tip dönüşüm hatası - string'i int'e direkt cast - Completed
        //var invalidAge = (int)instructorName; // ORTA: InvalidCastException - Completed
        var age = int.Parse(instructorName);
        var result = await _instructorService.CreateAsync(createdInstructorDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdatedInstructorDto updatedInstructorDto)
    {
        var result = await _instructorService.Update(updatedInstructorDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeletedInstructorDto deletedInstructorDto)
    {
        var result = await _instructorService.Remove(deletedInstructorDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
