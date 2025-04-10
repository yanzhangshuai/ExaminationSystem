using EntityFrameworkCore.UnitOfWork.Interfaces;

namespace ExaminationSystem.Application.Student;

public class StudentService(IUnitOfWork unitOfWork) : IStudentService
{

    public async Task<IEnumerable<string>> Generate()
    {
        var repository = unitOfWork.Repository<Model.Student>();
        
        // 删除所有数据
        var ids = await repository.SearchAsync(repository.MultipleResultQuery().Select(s => s.Id));
        if (ids != null && ids.Any())
        {
            foreach (var id in ids)
            {
                repository.Remove(x => x.Id == id);
            }
            await unitOfWork.SaveChangesAsync();
        }
        
        var result = new List<string>();
        
        var list = new List<Model.Student>();

        var random = new Random();
        var count = random.Next(20, 50);
        for (var i = 0; i < count; i++)
        {
            var name = "考生" + (i + 1);
            result.Add(name);
            list.Add(new Model.Student { Name = name });
        }

        await repository.AddRangeAsync(list);
        await unitOfWork.SaveChangesAsync();

        return result;
    }

    public async Task<IEnumerable<string>> List()
    {
        var repository = unitOfWork.Repository<Model.Student>();
        var query = repository.MultipleResultQuery().OrderBy(x => x.Id).Select(x => x.Name);
        var names = await repository.SearchAsync(query);

        var result = new List<string>();
        
        int left = 0, 
            right = names.Count - 1;

        while (left <= right)
        {
            if (left == right)
            {
                result.Add(names[left]);
            }
            else
            {
                result.Add(names[left]);
                result.Add(names[right]);
            }

            left++;
            right--;
        }

        return result;
    }
}