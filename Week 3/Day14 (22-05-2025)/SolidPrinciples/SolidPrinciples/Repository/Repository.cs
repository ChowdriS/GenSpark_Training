using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidPrinciples.Interface;
using SolidPrinciples.Model;

namespace SolidPrinciples.Repository
{
    public abstract class Repository<K, T> : IRepository<K, T> where T : class
    {
        protected readonly Dictionary<K, T> _items = new Dictionary<K, T>();

        public abstract void Add(T entity);
        public List<T> GetAll() => new List<T>(_items.Values);
        public T GetById(K id)
        {
            if (_items.ContainsKey(id))
            {
                return _items[id];
            }
            else
            {
                return null;
            }
        }
    }
}
