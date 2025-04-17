using ExaminationSystem.Application.Student;
using ExaminationSystem.Web.Utils.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Web.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("[controller]/[action]")]
public class StudentController(IStudentService studentService) : ControllerBase
{

    [HttpGet]
    public async Task<IEnumerable<string>> List()
    {
        return await studentService.List();
    }

    [HttpPost]
    public async Task<IEnumerable<string>> Generate()
    {
        return await studentService.Generate();
    }
}
