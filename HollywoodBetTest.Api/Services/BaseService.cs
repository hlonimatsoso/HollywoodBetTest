using HollywoodBetTest.Data.Repositories;
using HollywoodBetTest.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HollywoodBetTest.Api.Services
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        BaseRepository<T> _repo;
        public BaseService(BaseRepository<T> baseRepo)
        {
            _repo = baseRepo;
        }
        public void Delete(T entityToDelete)
        {
            _repo.Delete(entityToDelete);
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public List<T> GetAll()
        {
            return _repo.GetAll();
        }

        public T GetByID(object id)
        {
          return  _repo.GetByID(id);
        }

        public void Insert(T entity)
        {
            _repo.Insert(entity);
        }

        public void Update(T entityToUpdate)
        {
            _repo.Update(entityToUpdate);
        }
    }
}
