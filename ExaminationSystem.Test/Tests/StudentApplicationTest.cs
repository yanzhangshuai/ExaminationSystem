using ExaminationSystem.Application.Student;

namespace ExaminationSystem.Test.Tests;

public class StudentApplicationTest(IStudentService studentService)
{
    [Fact]
    public async Task TestGenerate()
    {
        var res = await studentService.Generate();
        Assert.NotNull(res);
        var collection = res as string[] ?? res.ToArray();
        Assert.NotEmpty(collection);
        Assert.True(collection.Any());
    }
    
    
    [Fact]
    public async Task ListTest()
    {
        var res = await studentService.List();
        Assert.NotNull(res);
        var collection = res as string[] ?? res.ToArray();
        
        Assert.NotEmpty(collection);
        
    }
}