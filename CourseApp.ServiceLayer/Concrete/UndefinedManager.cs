using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseApp.BusinessLayer.Abstract;
using CourseApp.DataAccessLayer.Abstract;
using CourseApp.ServiceLayer.Abstract;

namespace CourseApp.BusinessLayer.Concrete
{
    public class UndefinedManager : IUndefinedService
    {
        private readonly IUndefinedRepository _undefinedRepository;

        public UndefinedManager(IUndefinedRepository undefinedRepository)
        {
            _undefinedRepository = undefinedRepository;
        }

        public void UseUndefinedType()
        {
            // Repository’yi kullan
            var allUndefined = _undefinedRepository.GetAll().ToList();
            Console.WriteLine($"Toplam undefined kayıt sayısı: {allUndefined.Count}");
        }
    }
}
