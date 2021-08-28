using System.Collections.Generic;
using System.Threading.Tasks;
using CognizantChallenge.DAL.Models;

namespace CognizantChallenge.DAL.Repositories.Users
{
    public interface IUsersRepository: IRepository<User>
    {
        public void Insert(User entity);

        public Task<IEnumerable<User>> GetTop3();
    }
}