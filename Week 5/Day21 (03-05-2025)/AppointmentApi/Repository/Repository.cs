using System;
using AppointmentApi.Context;
using AppointmentApi.Interface;

namespace AppointmentApi.Repository;

public  abstract class Repository<K, T> : IRepository<K, T> where T:class
{
    protected readonly ClinicContext _clinicContext;

    public Repository(ClinicContext clinicContext)
    {
        _clinicContext = clinicContext;
    }
    public async Task<T> Add(T item)
    {
        _clinicContext.Add(item);
        // generate and execute the DML quries for the objects whse state is in ['added','modified','deleted'],
        await _clinicContext.SaveChangesAsync();
        return item;
    }

    public async Task<T> Delete(K key)
    {
        var item = await GetById(key);
        if (item != null)
        {
            _clinicContext.Remove(item);
            await _clinicContext.SaveChangesAsync();
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
            _clinicContext.Entry(myItem).CurrentValues.SetValues(item);
            await _clinicContext.SaveChangesAsync();
            return item;
        }
        throw new Exception("No such item found for updation");
    }
}

