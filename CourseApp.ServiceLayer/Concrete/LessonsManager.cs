using AutoMapper;
using CourseApp.BusinessLayer.Utilities.Result;
using CourseApp.DataAccessLayer.UnitOfWork;
using CourseApp.EntityLayer.Dto.LessonDto;
using CourseApp.EntityLayer.Entity;
using CourseApp.BusinessLayer.Abstract;
using CourseApp.BusinessLayer.Utilities.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CourseApp.EntityLayer.Dto.InstructorDto;

namespace CourseApp.BusinessLayer.Concrete;

public class LessonsManager : ILessonService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LessonsManager(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<IDataResult<IEnumerable<GetAllLessonDto>>> GetAllAsync(bool track = true)
    {
        var lessonList = await _unitOfWork.Lessons.GetAll(false).ToListAsync();
        var lessonListMapping = _mapper.Map<IEnumerable<GetAllLessonDto>>(lessonList);
        if (!lessonList.Any())
        {
            return new ErrorDataResult<IEnumerable<GetAllLessonDto>>(null, ConstantsMessages.LessonListFailedMessage);
        }
        return new SuccessDataResult<IEnumerable<GetAllLessonDto>>(lessonListMapping, ConstantsMessages.LessonListSuccessMessage);
    }

    public async Task<IDataResult<GetByIdLessonDto>> GetByIdAsync(string id, bool track = true)
    {
        // ORTA: Null check eksik - id null/empty olabilir - Completed
        if (string.IsNullOrWhiteSpace(id))
        {
            return new ErrorDataResult<GetByIdLessonDto>(null, ConstantsMessages.LessonListFailedMessage);
        }
        var hasLesson = await _unitOfWork.Lessons.GetByIdAsync(id, false);
        if (hasLesson == null)
        {
            return new ErrorDataResult<GetByIdLessonDto>(null, ConstantsMessages.LessonListFailedMessage);
        }
        // ORTA: Null reference - hasLesson null olabilir ama kontrol edilmiyor - Completed
        var hasLessonMapping = _mapper.Map<GetByIdLessonDto>(hasLesson);
        // ORTA: Mantıksal hata - yanlış mesaj döndürülüyor (Instructor yerine Lesson olmalıydı) - Completed 
        return new SuccessDataResult<GetByIdLessonDto>(hasLessonMapping, ConstantsMessages.LessonGetByIdSuccessMessage); // HATA: LessonGetByIdSuccessMessage olmalıydı - Completed
    }

    public async Task<IResult> CreateAsync(CreateLessonDto entity)
    {
        // ORTA: Null check eksik - entity null olabilir - Completed
        if (entity == null) 
        {
            return new ErrorResult(ConstantsMessages.LessonCreateFailedMessage);
        }
        var createdLesson = _mapper.Map<Lesson>(entity);
        // ORTA: Null reference - createdLesson null olabilir - Completed
        if (createdLesson == null)
        {
            return new ErrorResult(ConstantsMessages.LessonCreateFailedMessage);
        }
        var lessonName = createdLesson.Title ?? string.Empty; // Null reference riski - Completed

        // ZOR: Async/await anti-pattern - GetAwaiter().GetResult() deadlock'a sebep olabilir - Completed
        await _unitOfWork.Lessons.CreateAsync(createdLesson); // ZOR: Anti-pattern - Completed
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.LessonCreateSuccessMessage);
        }

        // KOLAY: Noktalı virgül eksikliği
        return new ErrorResult(ConstantsMessages.LessonCreateFailedMessage); // TYPO: ; eksik - Completed
    }

    public async Task<IResult> Remove(DeleteLessonDto entity)
    {
        var deletedLesson = _mapper.Map<Lesson>(entity);
        _unitOfWork.Lessons.Remove(deletedLesson);
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.LessonDeleteSuccessMessage);
        }
        return new ErrorResult(ConstantsMessages.LessonDeleteFailedMessage);
    }

    public async Task<IResult> Update(UpdateLessonDto entity)
    {
        // ORTA: Null check eksik - entity null olabilir - Completed
        if (entity == null)
        {
            return new ErrorResult(ConstantsMessages.LessonCreateFailedMessage);
        }
        var updatedLesson = _mapper.Map<Lesson>(entity);

        // ORTA: Index out of range - entity.Name null/boş olabilir - Completed
        if (!string.IsNullOrEmpty(entity?.Title))
        {
            var firstChar = entity.Title[0];
        }
        //var firstChar = !string.IsNullOrEmpty(entity?.Title); // IndexOutOfRangeException riski - Completed

        _unitOfWork.Lessons.Update(updatedLesson);
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.LessonUpdateSuccessMessage);
        }
        // ORTA: Mantıksal hata - hata durumunda SuccessResult döndürülüyor - Completed
        return new ErrorResult(ConstantsMessages.LessonUpdateFailedMessage); // HATA: ErrorResult olmalıydı - Completed
    }

    public async Task<IDataResult<IEnumerable<GetAllLessonDetailDto>>> GetAllLessonDetailAsync(bool track = true)
    {
        // ZOR: N+1 Problemi - Include kullanılmamış, lazy loading aktif - Completed
        //var lessonList = await _unitOfWork.Lessons.GetAllLessonDetails(false).ToListAsync(); - Completed
        var lessonList = await _unitOfWork.Lessons.GetAll(false)
                                        .Include(c => c.Course)
                                        .ToListAsync();
        // ZOR: N+1 - Her lesson için Course ayrı sorgu ile çekiliyor (lesson.Course?.CourseName) - Completed
        var lessonsListMapping = _mapper.Map<IEnumerable<GetAllLessonDetailDto>>(lessonList);
        if (lessonsListMapping == null || !lessonsListMapping.Any())
        {
            return new ErrorDataResult<IEnumerable<GetAllLessonDetailDto>>(lessonsListMapping, ConstantsMessages.LessonListFailedMessage);
        }
        // ORTA: Null reference - lessonsListMapping null olabilir - Completed
        var firstLesson = lessonsListMapping?.FirstOrDefault(); // Null/Empty durumunda exception - Completed

        if (firstLesson == null)
        {
            return new ErrorDataResult<IEnumerable<GetAllLessonDetailDto>>(null, ConstantsMessages.LessonListFailedMessage);
        }
        return new SuccessDataResult<IEnumerable<GetAllLessonDetailDto>>(lessonsListMapping, ConstantsMessages.LessonListSuccessMessage);
    }

    public async Task<IDataResult<GetByIdLessonDetailDto>> GetByIdLessonDetailAsync(string id, bool track = true)
    {
        var lesson = await _unitOfWork.Lessons.GetByIdLessonDetailsAsync(id, false);
        var lessonMapping = _mapper.Map<GetByIdLessonDetailDto>(lesson);
        return new SuccessDataResult<GetByIdLessonDetailDto>(lessonMapping);
    }

    public Task<IDataResult<CreateLessonDto>> GetNonExistentAsync()
    {
        return Task.FromResult<IDataResult<CreateLessonDto>>(null);
    }
    public void UseMissingHelper()
    {
        LessonHelperClass.Process();
    }
}
