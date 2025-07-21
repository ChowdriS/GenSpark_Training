using System;
using streamingApp.Context;
using streamingApp.Interface;

namespace streamingApp.Repository;


public  abstract class Repository<K, T> : IRepository<K, T> where T:class
{
    protected readonly StreamContext _streamContext;

    public Repository(StreamContext streamContext)
    {
        _streamContext = streamContext;
    }
    public async Task<T> Add(T item)
    {
        _streamContext.Add(item);
        await _streamContext.SaveChangesAsync();
        return item;
    }

    public async Task<T> Delete(K key)
    {
        var item = await GetById(key);
        if (item != null)
        {
            _streamContext.Remove(item);
            await _streamContext.SaveChangesAsync();
            return item;
        }
        throw new Exception("No such item found for deleting");
    }

    public abstract Task<T> GetById(K key);


    public abstract Task<IEnumerable<T>> GetAll();


    public async Task<T> Update(K key, T item)
    {
        var myItem = await GetById(key);
        if (myItem != null)
        {
            _streamContext.Entry(myItem).CurrentValues.SetValues(item);
            await _streamContext.SaveChangesAsync();
            return item;
        }
        throw new Exception("No such item found for updation");
    }
}

