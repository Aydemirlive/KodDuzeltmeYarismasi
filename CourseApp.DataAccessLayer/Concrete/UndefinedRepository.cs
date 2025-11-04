using CourseApp.DataAccessLayer.Abstract;
using CourseApp.EntityLayer.Entity;

namespace CourseApp.DataAccessLayer.Concrete;

public class UndefinedRepository : GenericRepository<Undefined>, IUndefinedRepository
{
    private readonly AppDbContext _context;
    public UndefinedRepository(AppDbContext context) : base(context)
    {
        _context = context; 
    }
}
