namespace CourseApp.BusinessLayer.Utilities.Result;

public class SuccessResult:Result
{

    public SuccessResult():base(true)
    {
        
    }

    public SuccessResult(string message):base(true,message) 
    {

    }
    public static class UndefinedUtilityClass
    {
        public static string Create()
        {
            return "UndefinedUtilityClass!";
        }
    }
    private static void UseUndefinedUtility()
    {
        var util = UndefinedUtilityClass.Create();
    }
}
