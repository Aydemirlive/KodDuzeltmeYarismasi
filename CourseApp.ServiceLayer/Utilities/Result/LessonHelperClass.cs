using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApp.BusinessLayer.Utilities.Result
{
    public static class LessonHelperClass
    {
        public static string Process()
        {
            return "LessonHelperClass!";
        }
    }
    public static class MissingMethodHelper
    {
        public static string Execute()
        {
            return "MissingMethodHelper!";
        }
    }
    public static class NonExistentClass
    {
        public static string Create()
        {
            return "NonExistentClass!";
        }
    }
    public static class UnknownClass
    {
        public static string StaticMethod()
        {
            return "UnknownClass!";
        }
    }
}
