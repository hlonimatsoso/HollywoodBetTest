using System;
using System.Collections.Generic;
using System.Text;

namespace HollywoodBetTest.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        void Delete(T entityToDelete);
        void Delete(object id);
        T GetByID(object id);
        List<T> GetAll();
        void Insert(T entity);
        void Update(T entityToUpdate);
    }
}
