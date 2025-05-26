using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.FlyWeight_DesignPattern
{
    public interface ICar
    {
        void Display(string NumberPlate);
    }

    public class CarModel : ICar
    {
        private string _brand;
        private string _model;

        public CarModel(string brand, string model)
        {
            _brand = brand;
            _model = model;
        }

        public void Display(string licensePlate)
        {
            Console.WriteLine($"Car: {_brand} {_model}, License Plate: {licensePlate}");
        }
    }

    public class CarFactory
    {
        private Dictionary<string, ICar> _cars = new Dictionary<string, ICar>();

        public ICar GetCarModel(string brand, string model)
        {
            string key = brand + "_" + model;

            if (!_cars.ContainsKey(key))
            {
                _cars[key] = new CarModel(brand, model);
            }

            return _cars[key];
        }
    }

    public class FlyWeight
    {
        public void Run()
        {
            CarFactory factory = new CarFactory();

            ICar car1 = factory.GetCarModel("Toyota", "Camry");
            car1.Display("TN01AB1234");

            ICar car2 = factory.GetCarModel("Toyota", "Camry");
            car2.Display("TN01CD5678");

            ICar car3 = factory.GetCarModel("Honda", "Civic");
            car3.Display("TN05XY9999");

        }
    }
}
