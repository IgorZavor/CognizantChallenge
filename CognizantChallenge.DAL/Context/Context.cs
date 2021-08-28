using CognizantChallenge.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CognizantChallenge.DAL.Context
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<Language> Languages { get; set; }

        public Context(DbContextOptions<Context> options): base(options) {}
    }
}