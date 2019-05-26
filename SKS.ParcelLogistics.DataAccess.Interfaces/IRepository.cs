using System;
using System.Collections.Generic;
using System.Text;

namespace SKS.ParcelLogistics.DataAccess.Interfaces
{
    public interface IRepository<T>
    {
        void Create(T t);
        void Update(T t);
        T Get(int id);
        IEnumerable<T> GetAll();
        void Delete(T t);
        void DeleteAll();
    }
}
