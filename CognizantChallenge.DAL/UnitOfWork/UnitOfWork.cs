using System;
using CognizantChallenge.DAL.Repositories.Challenges;
using CognizantChallenge.DAL.Repositories.Languages;
using CognizantChallenge.DAL.Repositories.Users;

namespace CognizantChallenge.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private Context.Context _context;
        private IChallengesRepository _challengesRepository;
        private ILanguagesRepository _languagesRepository;
        private IUsersRepository _usersRepository;
        private bool _disposed;
        
        public UnitOfWork(Context.Context context)
        {
            _context= context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IChallengesRepository ChallengesRepository => _challengesRepository ?? (_challengesRepository = new ChallengesRepository(_context));

        public ILanguagesRepository LanguagesRepository => _languagesRepository ?? (_languagesRepository = new LanguagesRepository(_context));

        public IUsersRepository UsersRepository => _usersRepository ?? (_usersRepository = new UsersRepository(_context));
    }
}