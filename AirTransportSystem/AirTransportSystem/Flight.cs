using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

//flight
namespace AirTransportSystem
{
    class Flight
    {
        public Flight() {}
        public int Key { get; set; } // Добавлено
        public string FlightNumber { get; set; }
        public Airport Departure { get; set; }
        public Airport Destination { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public Airplane Airplane { get; set; }
        public List<Passenger> Passengers { get; set; } = new List<Passenger>();
        public decimal Price { get; set; } // Добавлено
        public List<Airport> Landings { get; set; } = new List<Airport>(); // Добавлено

        public int FreeSeats => Airplane?.Capacity - Passengers.Count ?? 0; // Добавлено

        public void Print()
        {
            Console.WriteLine($"Рейс #{Key}: {FlightNumber}");
            Console.WriteLine($"Маршрут: {Departure?.Code} → {Destination?.Code}");
            Console.WriteLine($"Время: {DepartureTime:dd.MM.yy HH:mm} - {ArrivalTime:dd.MM.yy HH:mm}");
            Console.WriteLine($"Самолет: {Airplane?.Name} ({Airplane?.RegistrationNumber})");
            Console.WriteLine($"Цена: {Price:C}, Свободных мест: {FreeSeats}");
        }
    }
}