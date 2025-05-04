using System;
using System.IO;
using System.Text.Json;
namespace AirTransportSystem
{
    class Airport
    {
        public Airport() {}
        public int Key { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public void Print()
        {
            Console.WriteLine($"Аэропорт {Name} ({Code})");
            Console.WriteLine($"Местоположение: {City}, {Country}");
        }
    }
}