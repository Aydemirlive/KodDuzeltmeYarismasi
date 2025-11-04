namespace CourseApp.BusinessLayer.Utilities.Result;

public class ErrorDataResult<T>:DataResult<T>
{
    public ErrorDataResult(T data,string message):base(data,false,message)
    {
        
    }
    public ErrorDataResult(EntityLayer.Entity.Instructor? hasInstructor, T data) : base(data,false,default)
    {
        
    }

}
