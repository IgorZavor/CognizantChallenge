using System;
using CognizantChallenge.DAL.Repositories.Challenges;
using CognizantChallenge.DAL.Repositories.Languages;
using CognizantChallenge.DAL.Repositories.Users;

namespace CognizantChallenge.DAL.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        IChallengesRepository ChallengesRepository { get; }
        ILanguagesRepository LanguagesRepository { get; }
        IUsersRepository UsersRepository { get; }

        public void Save();
    }
}