using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CognizantChallenge.DAL.Repositories
{
    public interface IRepository<TEntity> where TEntity: class 
    {
        public TEntity GetEntity(int id);
        public Task<List<TEntity>> GetAll();
    }
}