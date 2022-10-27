using System.Linq.Expressions;
using N5.Challenge.Services.Security.Domain.Entities;
using N5.Challenge.Services.Security.Domain.SeedWork;

namespace N5.Challenge.Services.Security.Domain.Repositories
{
    public interface IPermissionRepository: IRepository<Permission>
    {
        Task AddAsync(Permission item);
        Task<Permission?> FindByIdAsync(int id);
        Task<IEnumerable<Permission>> FindPermissionAsync(Expression<Func<Permission, bool>> predicate);
        void Modify(Permission item);

    }
}