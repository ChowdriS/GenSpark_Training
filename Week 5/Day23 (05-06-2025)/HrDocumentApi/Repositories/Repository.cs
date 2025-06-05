using HrDocumentApi.Contexts;
using HrDocumentApi.Models;
using HrDocumentApi.Interfaces;

namespace HrDocumentApi.Repositories
{
    public abstract class Repository<K, T> : IRepository<K, T> where T : class
    {
        protected readonly HrDocumentApiContext _HrDocumentApiContext;
        public Repository(HrDocumentApiContext HrDocumentApiContext)
        {
            _HrDocumentApiContext = HrDocumentApiContext;
        }
        public async Task<T> Add(T item)
        {
            _HrDocumentApiContext.Add(item);
            await _HrDocumentApiContext.SaveChangesAsync(); //generate and execute the DML queries for the objects where state is in ['added','modified','deleted']
            return item;
        }
        public async Task<T> Delete(K key)
        {
            var item = await Get(key);
            if (item != null)
            {
                _HrDocumentApiContext.Remove(item);
                await _HrDocumentApiContext.SaveChangesAsync();
                return item;
            }
            throw new KeyNotFoundException($"Item with key {key} not found.");
        }

        public abstract Task<T> Get(K key);
        public abstract Task<IEnumerable<T>> GetAll();

        public async Task<T> Update(K key, T item)
        {
            var myItem = await Get(key);
            if (myItem != null)
            {
                _HrDocumentApiContext.Entry(myItem).CurrentValues.SetValues(item);
                await _HrDocumentApiContext.SaveChangesAsync();
                return myItem;
            }
            throw new KeyNotFoundException($"Item with key {key} not found.");
        }
    }
}