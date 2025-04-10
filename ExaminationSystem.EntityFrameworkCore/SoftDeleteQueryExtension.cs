using System.Linq.Expressions;
using System.Reflection;
using ExaminationSystem.Model.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExaminationSystem.EntityFrameworkCore
{
    public static class SoftDeleteQueryExtension
    {
        public static void AddSoftDeleteQueryFilter(this IMutableEntityType type)
        {
            var methodToCall = typeof(SoftDeleteQueryExtension).GetMethod(nameof(GetSoftDeleteFilter),
                    BindingFlags.NonPublic | BindingFlags.Static)
                ?.MakeGenericMethod(type.ClrType);
            var filter = methodToCall?.Invoke(null, []);
            type.SetQueryFilter((LambdaExpression)filter!);
        }
        private static LambdaExpression GetSoftDeleteFilter<TEntity>()
            where TEntity:class,ISoftDelete
        {
            Expression<Func<TEntity, bool>> filter = x => !x.Deleted;
            return filter;
        }
    }
}