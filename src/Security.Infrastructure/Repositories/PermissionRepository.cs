using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using N5.Challenge.Services.Security.Domain.Entities;
using N5.Challenge.Services.Security.Domain.Repositories;
using N5.Challenge.Services.Security.Domain.SeedWork;

namespace N5.Challenge.Services.Security.Infrastructure.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly SecurityContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public PermissionRepository(SecurityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(Permission item)
        {
            if(_context.Permissions is null)
            {
                throw new ArgumentNullException(nameof(_context.Permissions));
            }
            await _context.Permissions.AddAsync(item);
        }

        public async Task<Permission?> FindByIdAsync(int id)
        {
            if(_context.Permissions is null)
            {
                throw new ArgumentNullException(nameof(_context.Permissions));
            }

            return await _context.Permissions.FindAsync(id);
        }

        public async Task<IEnumerable<Permission>> FindPermissionAsync(Expression<Func<Permission, bool>> predicate)
        {
            if(_context.Permissions is null)
            {
                throw new ArgumentNullException(nameof(_context.Permissions));
            }

            return await _context.Permissions.Where(predicate).ToListAsync();
        }

        public void Modify(Permission item)
        {
            if(_context.Permissions is null)
            {
                throw new ArgumentNullException(nameof(_context.Permissions));
            }

            _context.Update(item);
        }
    }
}