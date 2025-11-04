using AutoMapper;
using CourseApp.DataAccessLayer.UnitOfWork;
using CourseApp.EntityLayer.Dto.RegistrationDto;
using CourseApp.EntityLayer.Dto.StudentDto;
using CourseApp.EntityLayer.Entity;
using CourseApp.BusinessLayer.Abstract;
using CourseApp.BusinessLayer.Utilities.Constants;
using CourseApp.BusinessLayer.Utilities.Result;
using Microsoft.EntityFrameworkCore;
using CourseApp.EntityLayer.Dto.LessonDto;
using Microsoft.IdentityModel.Tokens;

namespace CourseApp.BusinessLayer.Concrete;

public class StudentManager : IStudentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public StudentManager(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<IEnumerable<GetAllStudentDto>>> GetAllAsync(bool track = true)
    {
        var studentList = await _unitOfWork.Students.GetAll(track).ToListAsync();
        var studentListMapping = _mapper.Map<IEnumerable<GetAllStudentDto>>(studentList);
        if (!studentList.Any())
        {
            return new ErrorDataResult<IEnumerable<GetAllStudentDto>>(null, ConstantsMessages.StudentListFailedMessage);
        }
        return new SuccessDataResult<IEnumerable<GetAllStudentDto>>(studentListMapping, ConstantsMessages.StudentListSuccessMessage);
    }

    public async Task<IDataResult<GetByIdStudentDto>> GetByIdAsync(string id, bool track = true)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return new ErrorDataResult<GetByIdStudentDto>(null, ConstantsMessages.StudentGetByIdFailedMessage);
        }
        // ORTA: Null check eksik - id null/empty olabilir - Completed
        // ORTA: Null reference exception - hasStudent null olabilir ama kontrol edilmiyor - Completed
        var hasStudent = await _unitOfWork.Students.GetByIdAsync(id, false);
        if (hasStudent == null)
        {
            return new ErrorDataResult<GetByIdStudentDto>(null, ConstantsMessages.StudentGetByIdFailedMessage);
        }
        var hasStudentMapping = _mapper.Map<GetByIdStudentDto>(hasStudent);
        // ORTA: Null reference - hasStudentMapping null olabilir ama kullanılıyor - Completed
        if (hasStudentMapping == null)
        {
            return new ErrorDataResult<GetByIdStudentDto>(null, ConstantsMessages.StudentGetByIdFailedMessage);
        }
        var name = hasStudentMapping.Name ?? string.Empty; // Null reference riski - Completed
        return new SuccessDataResult<GetByIdStudentDto>(hasStudentMapping, ConstantsMessages.StudentGetByIdSuccessMessage);
    }

    public async Task<IResult> CreateAsync(CreateStudentDto entity)
    {
        if(entity == null) return new ErrorResult("Null");

        // ORTA: Tip dönüşüm hatası - string'i int'e direkt cast - Completed
        // ORTA: InvalidCastException - string int'e dönüştürülemez - Completed
        if (int.TryParse(entity.TC, out int tcNumber))
        {
            var invalidConversion = tcNumber;
        }
        var createdStudent = _mapper.Map<Student>(entity);
        // ORTA: Null reference - createdStudent null olabilir - Completed
        if (createdStudent == null)
        {
            return new ErrorResult(ConstantsMessages.StudentCreateFailedMessage);
        }
        var studentName = createdStudent?.Name; // Null check yok - Completed

        await _unitOfWork.Students.CreateAsync(createdStudent);
        // ZOR: Async/await anti-pattern - .Result kullanımı deadlock'a sebep olabilir - Completed
        var result = await _unitOfWork.CommitAsync(); // ZOR: Anti-pattern - Completed
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.StudentCreateSuccessMessage);
        }

        return new ErrorResult(ConstantsMessages.StudentCreateFailedMessage);
    }

    public async Task<IResult> Remove(DeleteStudentDto entity)
    {
        var deletedStudent = _mapper.Map<Student>(entity);
        _unitOfWork.Students.Remove(deletedStudent);
        var result = _unitOfWork.CommitAsync().GetAwaiter().GetResult();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.StudentDeleteSuccessMessage);
        }
        return new ErrorResult(ConstantsMessages.StudentDeleteFailedMessage);
    }

    public async Task<IResult> Update(UpdateStudentDto entity)
    {
        // ORTA: Null check eksik - entity null olabilir - Completed
        if (entity == null)
        {
            return new ErrorResult(ConstantsMessages.StudentUpdateFailedMessage);
        }
        var updatedStudent = _mapper.Map<Student>(entity);

        // ORTA: Index out of range - entity.TC null/boş olabilir - Completed
        //var tcFirstDigit = entity.TC[0]; // IndexOutOfRangeException riski - Completed
        if (!string.IsNullOrEmpty(entity?.TC))
        {
            var tcFirstDigit = entity.TC[0];
        }
        _unitOfWork.Students.Update(updatedStudent);
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            // ORTA: Mantıksal hata - başarılı durumda yanlış mesaj döndürülüyor - Completed
            return new SuccessResult(ConstantsMessages.StudentListSuccessMessage); // HATA: UpdateSuccessMessage olmalıydı - Completed
        }
        // ORTA: Mantıksal hata - hata durumunda SuccessResult döndürülüyor
        return new ErrorResult(ConstantsMessages.StudentUpdateFailedMessage); // HATA: ErrorResult olmalıydı - Completed
    }

    public void MissingImplementation()
    {
        var x = UnknownClass.StaticMethod();
    }
}
