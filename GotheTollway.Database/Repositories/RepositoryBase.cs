using GotheToll.Database.Context;
using GotheTollway.Domain.Repositories;

namespace GotheTollway.Database.Repositories
{
    public abstract class RepositoryBase : IRepositoryBase
    {
        protected readonly GotheTollWayContext _context;
        public RepositoryBase(GotheTollWayContext context)
        {
            _context = context;
        }
            
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
