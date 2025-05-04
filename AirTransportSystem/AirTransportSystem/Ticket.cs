using System;

namespace AirTransportSystem
{
    class Ticket
    {
        public int Key { get; set; }
        public int CashRegisterNumber { get; set; }
        public string FlightNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public DateTime SaleTime { get; set; }

        public void Print()
        {
            Console.WriteLine($"Билет #{Key} на рейс {FlightNumber}");
            Console.WriteLine($"Продан: {SaleDate:dd.MM.yy} в {SaleTime:HH:mm} кассой #{CashRegisterNumber}");
        }
    }
}