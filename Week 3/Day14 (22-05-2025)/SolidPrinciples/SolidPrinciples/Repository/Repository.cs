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
        protected readonly Dictionary<K, T> _storage = new Dictionary<K, T>();

        public abstract void Add(T entity);
        public List<T> GetAll() => new List<T>(_storage.Values);
        public T GetById(K id) => _storage.ContainsKey(id) ? _storage[id] : null;
    }
}
