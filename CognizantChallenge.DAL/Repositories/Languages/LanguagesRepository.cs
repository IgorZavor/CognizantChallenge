using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CognizantChallenge.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CognizantChallenge.DAL.Repositories.Languages
{
    public class LanguagesRepository: ILanguagesRepository
    {
        private DbSet<Language> _dbSet;
        private Context.Context _context;
        public LanguagesRepository(Context.Context context)
        {
            _dbSet = context.Languages;
            _context = context;
        }


        public Language GetEntity(int id)
        {
            return _dbSet.Find(id);
        }

        public async Task<List<Language>> GetAll()
        {
            var languages = await _dbSet.Select(t => t).ToListAsync();
            return languages;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}