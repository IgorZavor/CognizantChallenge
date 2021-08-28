using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CognizantChallenge.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CognizantChallenge.DAL.Repositories.Users
{
    public class UsersRepository: IUsersRepository
    {
        private readonly DbSet<User> _dbSet;
        public UsersRepository(Context.Context context)
        {
            _dbSet = context.Users;
        }
        
        public void Insert(User entity)
        {
            var user = entity;
            if (user == null) return;
            _dbSet.Add(user);
        }

        public async Task<IEnumerable<User>> GetTop3()
        {
            return await _dbSet
                .Select(user => user)
                .OrderByDescending(user => user.Scores)
                .Take(3)
                .ToListAsync();
        }

        public User GetEntity(int id)
        {
            return _dbSet.Find(id);
        }

        public async Task<List<User>> GetAll()
        {
            var tasks = await _dbSet.Select(t => t).ToListAsync();
            return tasks;
        }
    }
}