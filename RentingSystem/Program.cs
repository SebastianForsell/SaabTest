using RentingSystem.BL;
using RentingSystem.Classes;
using RentingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            int number = 0;
            
            string input;
            string menuText = "1. Rent car\n\r2. Return car\n\r3. Exit";
            Console.WriteLine(menuText);
            while ((input = Console.ReadLine()) != null)
            {
                if (int.TryParse(input, out number))
                {
                    switch (number)
                    {
                        case 1:
                            RentCar();
                            break;

                        case 2:
                            ReturnCar();
                            break;

                        case 3:
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid number.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Please, type only a number.");
                }
                Console.Clear();
                Console.WriteLine(menuText);
            }
        }
        public static void RentCar()
        {
            int number;
            string input;
            Console.Clear();
            SmallCarClass smallCar = new SmallCarClass();
            VanClass van = new VanClass();
            MiniBusClass miniBus = new MiniBusClass();
            Rental_Registration renting = new Rental_Registration();
            DateTime customerBirthday;
            bool dateInput = true;
            Console.WriteLine("What is your date of birth? (YYYY-MM-DD)");
            while (dateInput)
            {
                string customerDateInput = Console.ReadLine();
                dateInput = DateTime.TryParse(customerDateInput, out customerBirthday);
                if (dateInput)
                {
                    renting.CustomerBirthDate = customerBirthday;
                    dateInput = false;
                    continue;
                }
                else
                {
                    Console.WriteLine("Not correct format");
                    dateInput = true;
                }
            }
            Console.WriteLine("Which vehicle would you like?");
            Console.WriteLine("1. Small car\n\r2. Van\n\r3. Mini Bus");
            input = Console.ReadLine();
            dateInput = true;
            while (dateInput)
            {
                if (int.TryParse(input, out number))
                {
                    switch (number)
                    {
                        case 1:
                            renting.VehicleType = smallCar.Type;
                            dateInput = false;
                            break;

                        case 2:
                            renting.VehicleType = van.Type;
                            dateInput = false;
                            break;

                        case 3:
                            renting.VehicleType = miniBus.Type;
                            dateInput = false;
                            break;
                        default:
                            Console.WriteLine("Something went wrong.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Use only a number.");
                }
            }
            renting.rentStartDate = DateTime.Today;
            renting.CurrentMilageKm = 0;
            Console.Write("Ok, sending data ...");
            if (Repository.StoreVehicleToDatabase(renting))
            {
            }
            else
            {
                Console.WriteLine("Could not send data.");
            }
        }
        public static void ReturnCar()
        {
            Console.Clear();
            int number;
            double price = 0;
            string input;
            bool dataNeeded = true;
            Rental_Registration reservation = new Rental_Registration();
            Console.WriteLine("What is your booking number?");
            while (dataNeeded)
            {
                input = Console.ReadLine();
                if (int.TryParse(input, out number))
                {

                    reservation = Repository.ReturnVehicle(number);
                    if (reservation != null)
                    {
                        if (!reservation.rentEndDate.HasValue)
                        {
                            Console.WriteLine("Vehicle returned. Calculating price ...");
                            dataNeeded = false;
                            price = CalculatePrice(reservation, price);
                            Console.WriteLine($"The price is: {price}" + "\n\r\n\r");
                            Console.Write("Press any key to continue ...");
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("Vehicle already returned!");
                            Console.ReadLine();
                            dataNeeded = false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Reservation does not exist!");
                        Console.ReadLine();
                        dataNeeded = false;
                    }
                }
                else
                {
                    Console.WriteLine("Write only numbers.");
                }
            }
            
        }
        public static double CalculatePrice(Rental_Registration reservation, double price)
        {
            
            switch (reservation.VehicleType)
            {
                case "Small car":
                    SmallCarClass car = new SmallCarClass();
                    DateTime endTimeCar = (DateTime)reservation.rentEndDate;
                    TimeSpan totalDaysCar = endTimeCar.Subtract(reservation.rentStartDate);
                    price = car.BaseDayRental * totalDaysCar.Days;
                    break;
                case "Van":
                    SmallCarClass van = new SmallCarClass();
                    DateTime endTimeVan = (DateTime)reservation.rentEndDate;
                    TimeSpan totalDaysVan = endTimeVan.Subtract(reservation.rentStartDate);
                    price = van.BaseDayRental * totalDaysVan.Days * (1.2 + van.KmPrice) * (double)reservation.CurrentMilageKm;
                    break;

                case "Mini bus":
                    SmallCarClass miniBus = new SmallCarClass();
                    DateTime endTimeBus = (DateTime)reservation.rentEndDate;
                    TimeSpan totalDaysBus = endTimeBus.Subtract(reservation.rentStartDate);
                    price = miniBus.BaseDayRental * totalDaysBus.Days * (1.7 + (miniBus.KmPrice * (double)reservation.CurrentMilageKm * 1.5));
                    break;
                default:
                    break;
            }
            return price;
        }
    }
}
