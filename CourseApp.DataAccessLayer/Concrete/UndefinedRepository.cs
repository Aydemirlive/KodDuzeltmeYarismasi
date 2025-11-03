using CourseApp.DataAccessLayer.Abstract;
using CourseApp.EntityLayer.Entity;

namespace CourseApp.DataAccessLayer.Concrete;

public class UndefinedRepository : GenericRepository<Undefined>, IUndefinedRepository
{
    public UndefinedRepository(AppDbContext context) : base(context)
    {
    }
}
