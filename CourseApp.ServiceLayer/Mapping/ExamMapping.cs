using AutoMapper;
using CourseApp.EntityLayer.Dto.ExamDto;
using CourseApp.EntityLayer.Entity;

namespace CourseApp.BusinessLayer.Mapping;

public class ExamMapping:Profile
{
    public ExamMapping()
    {
        CreateMap<Exam,GetAllExamDto>().ReverseMap();
        CreateMap<Exam,CreateExamDto>().ReverseMap();
        CreateMap<Exam,DeleteExamDto>().ReverseMap();
        CreateMap<Exam,UpdateExamDto>().ReverseMap();
    }
}
