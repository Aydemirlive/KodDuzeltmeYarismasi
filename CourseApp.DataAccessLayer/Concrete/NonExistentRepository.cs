using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseApp.DataAccessLayer.Abstract;
using CourseApp.EntityLayer.Entity;

namespace CourseApp.DataAccessLayer.Concrete
{
    public class NonExistentRepository : GenericRepository<NonExistent>, INonExistentRepository
    {
        public NonExistentRepository(AppDbContext context) : base(context)
        {
        }
    }
}
