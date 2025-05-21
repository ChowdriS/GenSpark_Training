using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Interface;
using BookingApp.UserDefinedException;

namespace BookingApp.Repository
{
    public abstract class Repository<K, T> : IRepository<K, T> where T : class
    {
        protected List<T> _items = new List<T>();
        protected abstract K GenerateID();
        public abstract ICollection<T> GetAll();
        public abstract T GetById(K id);
        public abstract ICollection<Appointment> GetUpcomingAppointments();
        public abstract ICollection<Appointment> GetPastAppointments();

        public T Add(T item)
        {
            var id = GenerateID();
            var prop = typeof(T).GetProperty("Id");
            prop?.SetValue(item, id);

            if (_items.Contains(item))
                throw new DuplicateEntityException("Item already exists");

            _items.Add(item);
            return item;
        }

        public T Update(T item)
        {
            var index = _items.IndexOf(item);
            if (index == -1) throw new KeyNotFoundException("Item not found");
            _items[index] = item;
            return item;
        }

        public T Delete(K id)
        {
            var item = GetById(id);
            _items.Remove(item);
            return item;
        }

    }

}
