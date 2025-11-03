using CourseApp.DataAccessLayer.Abstract;
using CourseApp.EntityLayer.Entity;

namespace CourseApp.DataAccessLayer.Concrete;

public class ExamRepository : GenericRepository<Exam>, IExamRepository
{
    public ExamRepository(AppDbContext context) : base(context)
    {
    }
    public static class ExamHelperUtility
    {
        public static string Execute()
        {
            // Yardımcı işlem: örnek
            return "Exam işlemi tamamlandı!";
        }
    }
    public void InvalidMethod()
    {
        var x = ExamHelperUtility.Execute();
    }
}
