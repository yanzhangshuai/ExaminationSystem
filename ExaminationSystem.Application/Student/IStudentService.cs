namespace ExaminationSystem.Application.Student;

public interface IStudentService
{
    Task<IEnumerable<string>> Generate();

    Task<IEnumerable<string>> List();
}