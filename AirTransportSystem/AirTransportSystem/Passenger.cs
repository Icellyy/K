using System;
using System.IO;
using System.Text.Json;

//passenger
namespace AirTransportSystem
{
    class Passenger
    {
        public Passenger() {}
        public int Key { get; set; }
        public string FullName { get; set; }
        public string PassportNumber { get; set; }
        public string ContactInfo { get; set; }

        public void Print()
        {
            Console.WriteLine($"Пассажир: {FullName}");
            Console.WriteLine($"Паспорт: {PassportNumber}, Контакты: {ContactInfo}");
        }
    }
}