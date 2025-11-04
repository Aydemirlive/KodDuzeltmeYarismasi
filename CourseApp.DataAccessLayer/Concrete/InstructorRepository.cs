using CourseApp.DataAccessLayer.Abstract;
using CourseApp.EntityLayer.Entity;

namespace CourseApp.DataAccessLayer.Concrete;

public class InstructorRepository : GenericRepository<Instructor>, IInstructorRepository
{
    private readonly AppDbContext _context;
    public InstructorRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
