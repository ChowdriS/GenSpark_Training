using System;
using shop_api.Context;
using shop_api.Interfaces;

namespace shop_api.Repository;

public  abstract class Repository<K, T> : IRepository<K, T> where T:class
{
    protected readonly ShopContext _shopContext;

    public Repository(ShopContext shopContext)
    {
        _shopContext = shopContext;
    }
    public async Task<T> Add(T item)
    {
        _shopContext.Add(item);
        await _shopContext.SaveChangesAsync();
        return item;
    }

    public async Task<T> Delete(K key)
    {
        var item = await GetById(key);
        if (item != null)
        {
            _shopContext.Remove(item);
            await _shopContext.SaveChangesAsync();
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
            _shopContext.Entry(myItem).CurrentValues.SetValues(item);
            await _shopContext.SaveChangesAsync();
            return item;
        }
        throw new Exception("No such item found for updation");
    }
}

