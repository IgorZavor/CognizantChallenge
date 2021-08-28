using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CognizantChallenge.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CognizantChallenge.DAL.Repositories.Challenges
{
    public class ChallengesRepository: IChallengesRepository
    {
        private DbSet<Challenge> _dbSet;
        private Context.Context _context;
        public ChallengesRepository(Context.Context context)
        {
            _context = context;
            _dbSet = _context.Challenges;
        }

        public Challenge GetEntity(int id)
        {
            return  _dbSet.Find(id);
        }

        public async Task<List<Challenge>> GetAll()
        {
            var tasks = await _dbSet.Select(t => t).ToListAsync();
            return tasks;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}