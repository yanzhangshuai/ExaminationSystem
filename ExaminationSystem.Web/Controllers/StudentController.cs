using ExaminationSystem.Application.Student;
using ExaminationSystem.Web.Utils.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Web.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class StudentController(IStudentService studentService) : ControllerBase
{

    [HttpGet]
    public async Task<IEnumerable<string>> List()
    {
        throw new CustomException(401,"自定义异常");
        return await studentService.List();
    }
    
    [HttpPost]
    public async Task<IEnumerable<string>> Generate()
    {
        return await studentService.Generate();
    }
}