using System;

namespace AirTransportSystem
{
    class Airplane
    {
        public int Key { get; set; } // Добавлено
        public string Name { get; set; } // Добавлено
        public string Model { get; set; }
        public string RegistrationNumber { get; set; }
        public string Category { get; set; }
        public int Capacity { get; set; } // Переименовано из SeatCount

        public void Print()
        {
            Console.WriteLine($"Самолет {Name} ({Model}) - Рег.номер: {RegistrationNumber}");
            Console.WriteLine($"Категория: {Category}, Вместимость: {Capacity}");
        }
    }
}