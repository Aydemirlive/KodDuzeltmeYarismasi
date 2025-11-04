using CourseApp.DataAccessLayer.Abstract;
using CourseApp.EntityLayer.Entity;

namespace CourseApp.DataAccessLayer.Concrete;

public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
    private readonly AppDbContext _context;
    public StudentRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
