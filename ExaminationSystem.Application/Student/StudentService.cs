using EntityFrameworkCore.UnitOfWork.Interfaces;

namespace ExaminationSystem.Application.Student;

public class StudentService(IUnitOfWork unitOfWork) : IStudentService
{
    public async Task<IEnumerable<string>> Generate()
    {
        var result = new List<string>();

        var repository = unitOfWork.Repository<Model.Student>();

        var query = repository.MultipleResultQuery().Select(s => s.Id);
        var ids = await repository.SearchAsync(query);
        await repository.RemoveAsync(x => ids.Contains(x.Id));


        var list = new List<Model.Student>();
        var count = new Random().Next(20, 50);
        for (var i = 0; i < count; i++)
        {
            var name = "考生" + (i + 1);
            list.Add(new Model.Student { Name = name });
            result.Add(name);
        }

        await repository.AddRangeAsync(list);
        await unitOfWork.SaveChangesAsync();

        return result;
    }

    public async Task<IEnumerable<string>> List()
    {
        var result = new List<string>();

        var repository = unitOfWork.Repository<Model.Student>();

        var query = repository.MultipleResultQuery().OrderBy(x => x.Id).Select(x => x.Name);
        var list = await repository.SearchAsync(query);

        int left = 0,
            right = list.Count - 1;

        while (left <= right)
        {
            if (left == right)
            {
                result.Add(list[left]);
            }
            else
            {
                result.Add(list[left]);
                result.Add(list[right]);
            }

            left++;
            right--;
        }

        return result;
    }
}
