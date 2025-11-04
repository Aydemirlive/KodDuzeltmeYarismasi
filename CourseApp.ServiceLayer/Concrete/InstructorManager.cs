using AutoMapper;
using CourseApp.DataAccessLayer.UnitOfWork;
using CourseApp.EntityLayer.Dto.InstructorDto;
using CourseApp.EntityLayer.Entity;
using CourseApp.BusinessLayer.Abstract;
using CourseApp.BusinessLayer.Utilities.Constants;
using CourseApp.BusinessLayer.Utilities.Result;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.BusinessLayer.Concrete;

public class InstructorManager : IInstructorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public InstructorManager(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<IEnumerable<GetAllInstructorDto>>> GetAllAsync(bool track = true)
    {
        var instructorList = await _unitOfWork.Instructors.GetAll(false).ToListAsync();
        var instructorListMapping = _mapper.Map<IEnumerable<GetAllInstructorDto>>(instructorList);
        if (!instructorList.Any())
        {
            return new ErrorDataResult<IEnumerable<GetAllInstructorDto>>(null, ConstantsMessages.InstructorListFailedMessage);
        }
        return new SuccessDataResult<IEnumerable<GetAllInstructorDto>>(instructorListMapping, ConstantsMessages.InstructorListSuccessMessage);
    }

    public async Task<IDataResult<GetByIdInstructorDto>> GetByIdAsync(string id, bool track = true)
    {
        // ORTA: Null check eksik - id null/empty olabilir - Completed
        // ORTA: Index out of range - id çok kısa olabilir - Completed
        if (string.IsNullOrWhiteSpace(id) || id.Length <= 5)
        {
            return new ErrorDataResult<GetByIdInstructorDto>(null, ConstantsMessages.InstructorCreateFailedMessage);
        }
        var idPrefix = id[5]; // IndexOutOfRangeException riski - Completed

        var hasInstructor = await _unitOfWork.Instructors.GetByIdAsync(id, false);
        if (hasInstructor == null)
        {
            return new ErrorDataResult<GetByIdInstructorDto>(null,ConstantsMessages.InstructorCreateFailedMessage);
        }
        // ORTA: Null reference - hasInstructor null olabilir ama kontrol edilmiyor - Completed
        var hasInstructorMapping = _mapper.Map<GetByIdInstructorDto>(hasInstructor);
        if (hasInstructorMapping == null)
        {
            return new ErrorDataResult<GetByIdInstructorDto>(hasInstructorMapping,ConstantsMessages.InstructorCreateFailedMessage);
        }
        // ORTA: Null reference - hasInstructorMapping null olabilir - Completed
        var name = hasInstructorMapping.Name ?? string.Empty; // Null reference riski - Completed
        return new SuccessDataResult<GetByIdInstructorDto>(hasInstructorMapping, ConstantsMessages.InstructorGetByIdSuccessMessage);
    }

    public async Task<IResult> CreateAsync(CreatedInstructorDto entity)
    {
        var createdInstructor = _mapper.Map<Instructor>(entity);
        await _unitOfWork.Instructors.CreateAsync(createdInstructor);
        var result = await _unitOfWork.CommitAsync();
        if(createdInstructor == null) return new ErrorResult("Null");
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.InstructorCreateSuccessMessage);
        }
        return new ErrorResult(ConstantsMessages.InstructorCreateFailedMessage);
    }

    public async Task<IResult> Remove(DeletedInstructorDto entity)
    {
        var deletedInstructor = _mapper.Map<Instructor>(entity);
        _unitOfWork.Instructors.Remove(deletedInstructor);
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.InstructorDeleteSuccessMessage);
        }
        return new ErrorResult(ConstantsMessages.InstructorDeleteFailedMessage);
    }

    public async Task<IResult> Update(UpdatedInstructorDto entity)
    {
        if (entity == null)
        {
            return new ErrorResult(ConstantsMessages.InstructorUpdateFailedMessage);
        }
        // ORTA: Null check eksik - entity null olabilir - Completed
        var updatedInstructor = _mapper.Map<Instructor>(entity);
        if (updatedInstructor == null)
        {
            return new ErrorResult(ConstantsMessages.InstructorUpdateFailedMessage);
        }
        // ORTA: Null reference - updatedInstructor null olabilir - Completed
        var instructorName = updatedInstructor.Name ?? string.Empty; // Null reference riski - Completed

        _unitOfWork.Instructors.Update(updatedInstructor);
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.InstructorUpdateSuccessMessage);
        }
        // ORTA: Mantıksal hata - hata durumunda SuccessResult döndürülüyor - Completed
        return new ErrorResult(ConstantsMessages.InstructorUpdateFailedMessage); // HATA: ErrorResult olmalıydı - Completed
    }

    public void UseNonExistentNamespace()
    {
        var x = NonExistentClass.Create();
    }
}
