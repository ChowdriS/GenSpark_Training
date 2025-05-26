using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidPrinciples.Model;

namespace SolidPrinciples.Interface
{
    public interface IRepository<TKey, TEntity>
    {
        void Add(TEntity entity);
        List<TEntity> GetAll();
        TEntity GetById(TKey id);
    }
}
