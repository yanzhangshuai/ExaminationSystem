namespace ExaminationSystem.Web.Utils.Exceptions;

public class CustomException(int code, string message) : System.Exception(message)
{
    public int Code { get; private set; } = code;

    public override string Message { get; } = message;
}    
