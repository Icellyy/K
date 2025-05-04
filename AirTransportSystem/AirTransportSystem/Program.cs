using System;
using System.Collections.Generic;
using System.Linq;

namespace AirTransportSystem
{
    class Program
    {
        static List<Flight> flights = new List<Flight>();
        static List<Airplane> airplanes = new List<Airplane>();
        static List<Airport> airports = new List<Airport>();
        static List<Ticket> tickets = new List<Ticket>();
        static int nextFlightKey = 1;
        static int nextTicketKey = 1;

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "✈ Система управления авиаперевозками";

            while (true)
            {
                Console.Clear();
                ShowMenu();
                Console.Write("→ Ваш выбор: ");
                string choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1": AddFlight(); break;
                    case "2": AddAirplane(); break;
                    case "3": AddAirport(); break;
                    case "4": ShowAllFlights(); break;
                    case "5": EditFlight(); break;
                    case "6": RemoveFlight(); break;
                    case "7": AddPassenger(); break;
                    case "8": SellTicket(); break;
                    case "9": CheckFreeSeats(); break;
                    case "10": FindNonStopFlights(); break;
                    case "11": FindFlightsByPlane(); break;
                    case "12": ShowFlightLoad(); break;
                    case "13": ShowMostExpensiveFlight(); break;
                    case "14": FindReplaceablePlanes(); break;
                    case "15": return;
                    default: ShowError("Неверный выбор"); break;
                }
            }
        }

        static void ShowMenu()
        {
            WriteCenter("╔════════════════════════════════════════╗");
            WriteCenter("║   СИСТЕМА УПРАВЛЕНИЯ АВИАПЕРЕВОЗКАМИ   ║");
            WriteCenter("╠════════════════════════════════════════╣");
            WriteCenter("║ 1.  Добавить рейс                      ║");
            WriteCenter("║ 2.  Добавить самолет                   ║");
            WriteCenter("║ 3.  Добавить аэропорт                  ║");
            WriteCenter("║ 4.  Показать все рейсы                 ║");
            WriteCenter("║ 5.  Редактировать рейс                 ║");
            WriteCenter("║ 6.  Удалить рейс                       ║");
            WriteCenter("║ 7.  Добавить пассажира                 ║");
            WriteCenter("║ 8.  Продать билет                      ║");
            WriteCenter("║ 9.  Проверить свободные места          ║");
            WriteCenter("║ 10. Рейсы без пересадок                ║");
            WriteCenter("║ 11. Рейсы по самолету                  ║");
            WriteCenter("║ 12. Загруженность рейсов               ║");
            WriteCenter("║ 13. Самый дорогой рейс                 ║");
            WriteCenter("║ 14. Заменяемые самолеты                ║");
            WriteCenter("║ 15. Выход                              ║");
            WriteCenter("╚════════════════════════════════════════╝");
            Console.WriteLine();
        }

        static void AddFlight()
        {
            var flight = new Flight { Key = nextFlightKey++ };
            WriteCenter("=== НОВЫЙ РЕЙС ===\n");
            Console.Write("Номер рейса: ");
            flight.FlightNumber = Console.ReadLine();

            Console.WriteLine("\nВылет");
            flight.Departure = SelectAirportOrCreate();

            Console.WriteLine("\nПрилёт");
            flight.Destination = SelectAirportOrCreate();

            Console.WriteLine();
            Console.Write("Время вылета (дд.мм.гггг чч:мм): ");
            string departureInput = Console.ReadLine();
            if (DateTime.TryParse(departureInput, out DateTime depTime))
                flight.DepartureTime = depTime;

            Console.WriteLine();
            Console.Write("Время прибытия (дд.мм.гггг чч:мм): ");
            string arrivalInput = Console.ReadLine();
            if (DateTime.TryParse(arrivalInput, out DateTime arrTime))
                flight.ArrivalTime = arrTime;

            flight.Airplane = SelectAirplaneOrCreate();
            Console.WriteLine();
            Console.Write("Цена рейса: ");
            decimal price;
            if (decimal.TryParse(Console.ReadLine(), out price))
                flight.Price = price;

            Console.WriteLine();
            Console.WriteLine("Промежуточные посадки (-1 - завершить):");
            while (true)
            {
                var airport = SelectAirportOrCreate(allowExit: true);
                if (airport == null) break;
                flight.Landings.Add(airport);
            }

            flights.Add(flight);
            ShowSuccess("=== Рейс добавлен! ===");
        }

        static void SellTicket()
        {
            ShowAllFlights();
            Console.Write("Введите номер рейса: ");
            var flight = flights.FirstOrDefault(f => f.FlightNumber == Console.ReadLine());

            if (flight == null || flight.FreeSeats <= 0)
            {
                ShowError("Рейс не найден или нет мест");
                return;
            }

            var ticket = new Ticket
            {
                Key = nextTicketKey++,
                CashRegisterNumber = new Random().Next(1, 10),
                FlightNumber = flight.FlightNumber,
                SaleDate = DateTime.Now.Date,
                SaleTime = DateTime.Now
            };

            tickets.Add(ticket);
            flight.Passengers.Add(new Passenger { Key = flight.Passengers.Count + 1 });
            ShowSuccess($"=== Билет #{ticket.Key} продан! ===");
        }

        static void CheckFreeSeats()
        {
            ShowAllFlights();

            Console.Write("Номер рейса: ");
            var flight = flights.FirstOrDefault(f => f.FlightNumber == Console.ReadLine());

            if (flight == null)
            {
                ShowError("=== Рейс не найден ===");
                return;
            }

            WriteCenter($"=== Свободных мест: {flight.FreeSeats} ===");
            Console.ReadLine();
        }

        static void FindNonStopFlights()
        {
            WriteCenter("=== РЕЙСЫ БЕЗ ПЕРЕСАДОК ===");
            var result = flights.Where(f => !f.Landings.Any()).ToList();

            if (!result.Any())
                WriteCenter("=== РЕЙСОВ БЕЗ ПЕРЕСАДОК НЕТ ===");
            else
                foreach (var f in result)
                    f.Print();

            Console.ReadLine();
        }

        static void FindFlightsByPlane()
        {
            var plane = SelectAirplane();
            if (plane == null) return;

            var result = flights.Where(f => f.Airplane.Key == plane.Key).ToList();
            ShowFlights(result);
        }

        static void ShowFlightLoad()
        {
            WriteCenter("=== ЗАГРУЖЕННОСТЬ РЕЙСОВ ===");
            ShowAllFlights();
            Console.Write("Номер рейса: ");
            var flight = flights.FirstOrDefault(f => f.FlightNumber == Console.ReadLine());

            if (flight == null)
            {
                ShowError("=== Рейс не найден ===");
                return;
            }

            Console.WriteLine($"Загруженность рейса {flight.FlightNumber}:");
            Console.WriteLine($"Всего мест: {flight.Airplane.Capacity}");
            Console.WriteLine($"Занято: {flight.Passengers.Count}");
            Console.WriteLine($"Свободно: {flight.FreeSeats}");
            Console.ReadLine();
        }

        static void ShowMostExpensiveFlight()
        {
            var flight = flights.OrderByDescending(f => f.Price).FirstOrDefault();

            if (flight == null)
                WriteCenter("=== Рейсов нет ===");
            else
                flight.Print();

            Console.ReadLine();
        }

        static void FindReplaceablePlanes()
        {
            var result = flights.Where(f => f.FreeSeats > f.Airplane.Capacity * 0.3).ToList();

            if (!result.Any())
                WriteCenter("=== РЕЙСОВ С ВОЗМОЖНОСТЬЮ ЗАМЕНЫ НЕТ ===");
            else
                foreach (var f in result)
                    f.Print();

            Console.ReadLine();
        }

        static void AddAirplane()
        {
            var airplane = new Airplane
            {
                Key = airplanes.Count + 1,
                Name = "Boeing " + (airplanes.Count + 700)
            };
            
            WriteCenter("=== САМОЛЕТ ===\n");

            Console.Write("Модель: ");
            airplane.Model = Console.ReadLine();

            Console.Write("Регистрационный номер: ");
            airplane.RegistrationNumber = Console.ReadLine();

            Console.Write("Категория: ");
            airplane.Category = Console.ReadLine();

            Console.Write("Вместимость: ");
            int capacity;
            if (int.TryParse(Console.ReadLine(), out capacity))
                airplane.Capacity = capacity;

            airplanes.Add(airplane);
            ShowSuccess("=== Самолет добавлен! ===");
        }

        static void AddAirport()
        {
            var airport = new Airport
            {
                Key = airports.Count + 1
            };

            WriteCenter("=== АЭРОПОРТ ===\n");

            Console.Write("Код аэропорта: ");
            airport.Code = Console.ReadLine().ToUpper();

            Console.Write("Название: ");
            airport.Name = Console.ReadLine();

            Console.Write("Город: ");
            airport.City = Console.ReadLine();

            Console.Write("Страна: ");
            airport.Country = Console.ReadLine();

            airports.Add(airport);
            ShowSuccess("=== Аэропорт добавлен! ===");
        }

        static void ShowAllFlights()
        {
            WriteCenter("=== СПИСОК РЕЙСОВ ===");
            if (!flights.Any())
            {
                WriteCenter("=== Рейсов нет ===");
            }
            else
            {
                foreach (var flight in flights)
                {
                    flight.Print();
                    Console.WriteLine(new string('-', 40));
                }
            }
            Console.WriteLine("\nНажмите Enter...");
            Console.ReadLine();
        }

        static void EditFlight()
        {
            ShowAllFlights();
            Console.Write("Номер рейса: ");
            var flight = flights.FirstOrDefault(f => f.FlightNumber == Console.ReadLine());

            if (flight == null)
            {
                ShowError("=== Рейс не найден ===");
                return;
            }

            WriteCenter("╔════════════════════════════════════════╗");
            WriteCenter("║ 1.  Изменить номер                     ║");
            WriteCenter("║ 2.  Изменить аэропорт отправления      ║");
            WriteCenter("║ 3.  Изменить аэропорт назначения       ║");
            WriteCenter("║ 4.  Изменить время вылета              ║");
            WriteCenter("║ 5.  Изменить время прибытия            ║");
            WriteCenter("║ 6.  Изменить самолет                   ║");
            WriteCenter("║ 7.  Изменить цену                      ║");
            WriteCenter("╚════════════════════════════════════════╝");
            Console.Write("\n→ Ваш выбор: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("Новый номер: ");
                    flight.FlightNumber = Console.ReadLine();
                    break;
                case "2":
                    flight.Departure = SelectAirportOrCreate();
                    break;
                case "3":
                    flight.Destination = SelectAirportOrCreate();
                    break;
                case "4":
                    Console.Write("Новое время вылета: ");
                    DateTime.TryParse(Console.ReadLine(), out DateTime newDep);
                    flight.DepartureTime = newDep;
                    break;
                case "5":
                    Console.Write("Новое время прибытия: ");
                    DateTime.TryParse(Console.ReadLine(), out DateTime newArr);
                    flight.ArrivalTime = newArr;
                    break;
                case "6":
                    flight.Airplane = SelectAirplaneOrCreate();
                    break;
                case "7":
                    Console.Write("Новая цена: ");
                    decimal.TryParse(Console.ReadLine(), out decimal newPrice);
                    flight.Price = newPrice;
                    break;
            }
            ShowSuccess("=== Рейс обновлен! ===");
        }

        static void RemoveFlight()
        {
            ShowAllFlights();
            Console.Write("Номер рейса: ");
            var flight = flights.FirstOrDefault(f => f.FlightNumber == Console.ReadLine());

            if (flight != null)
            {
                flights.Remove(flight);
                ShowSuccess("=== Рейс удален! ===");
            }
            else
            {
                ShowError("=== Рейс не найден ===");
            }
        }

        static void AddPassenger()
        {
            ShowAllFlights();
            Console.Write("Номер рейса: ");
            var flight = flights.FirstOrDefault(f => f.FlightNumber == Console.ReadLine());

            if (flight == null)
            {
                ShowError("=== Рейс не найден ===");
                return;
            }

            if (flight.FreeSeats <= 0)
            {
                ShowError("=== Нет свободных мест ===");
                return;
            }

            var passenger = new Passenger
            {
                Key = flight.Passengers.Count + 1
            };

            Console.Write("ФИО: ");
            passenger.FullName = Console.ReadLine();

            Console.Write("Паспорт: ");
            passenger.PassportNumber = Console.ReadLine();

            Console.Write("Контакты: ");
            passenger.ContactInfo = Console.ReadLine();

            flight.Passengers.Add(passenger);
            ShowSuccess("=== Пассажир добавлен! ===");
        }

        static Airport SelectAirportOrCreate(bool allowExit = false)
        {
            while (true)
            {
                Console.WriteLine("Выберите аэропорт:");
                for (int i = 0; i < airports.Count; i++)
                    Console.WriteLine($"{i + 1}. {airports[i].Name} ({airports[i].Code})");

                if (allowExit)
                    Console.WriteLine("-1. Завершить выбор");
                else
                    Console.WriteLine("0. Создать новый");

                Console.Write("\n→ Ваш выбор: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice == -1 && allowExit)
                        return null;
                    if (choice == 0 && !allowExit)
                    {
                        AddAirport();
                        return airports.Last();
                    }
                    if (choice > 0 && choice <= airports.Count)
                        return airports[choice - 1];
                }
                ShowError("=== Неверный выбор ===");
            }
        }

        static Airplane SelectAirplaneOrCreate()
        {
            while (true)
            {
                Console.WriteLine("Выберите самолет:");
                for (int i = 0; i < airplanes.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {airplanes[i].Name} ({airplanes[i].RegistrationNumber})");
                }
                Console.WriteLine("0. Создать новый");

                Console.Write("\n→ Ваш выбор: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice == 0)
                    {
                        AddAirplane();
                        return airplanes.Last();
                    }
                    if (choice > 0 && choice <= airplanes.Count)
                    {
                        return airplanes[choice - 1];
                    }
                }
                ShowError("=== Неверный выбор ===");
            }
        }

        static Airplane SelectAirplane()
        {
            while (true)
            {
                Console.WriteLine("Выберите самолет:");
                for (int i = 0; i < airplanes.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {airplanes[i].Name} ({airplanes[i].RegistrationNumber})");
                }

                Console.Write("\n→ Ваш выбор: ");

               
                if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= airplanes.Count)
                {
                    return airplanes[choice - 1];
                }
                ShowError("=== Неверный выбор ===");
            }
        }

        static void ShowFlights(List<Flight> flightsToShow)
        {
            foreach (var flight in flightsToShow)
            {
                flight.Print();
                Console.WriteLine(new string('-', 40));
            }
            Console.ReadLine();
        }

        static void WriteCenter(string text)
        {
            int leftPadding = (Console.WindowWidth - text.Length) / 2;
            Console.WriteLine(new string(' ', leftPadding) + text);
        }

        static void ShowSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            WriteCenter($"✓ {message}");
            Console.ResetColor();
            Console.WriteLine("\nНажмите Enter...");
            Console.ReadLine();
        }

        static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            WriteCenter($"⚠ {message}");
            Console.ResetColor();
            Console.WriteLine("\nНажмите Enter...");
            Console.ReadLine();
        }
    }
}