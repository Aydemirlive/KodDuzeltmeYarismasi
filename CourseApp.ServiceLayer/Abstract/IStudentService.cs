using CourseApp.EntityLayer.Dto.StudentDto;
using CourseApp.BusinessLayer.Utilities.Result;

namespace CourseApp.BusinessLayer.Abstract;

public interface IStudentService
{
    Task<IDataResult<IEnumerable<GetAllStudentDto>>> GetAllAsync(bool track = true);
    Task<IDataResult<GetByIdStudentDto>> GetByIdAsync(string id, bool track = true);
    Task<IResult> CreateAsync(CreateStudentDto entity);
    Task<IResult> Update(UpdateStudentDto entity);
    Task<IResult> Remove(DeleteStudentDto entity);
    public void MissingImplementation();
}
