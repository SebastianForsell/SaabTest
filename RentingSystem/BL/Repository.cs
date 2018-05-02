using RentingSystem.Models;
using System;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Security.Cryptography;

namespace RentingSystem.BL
{
    public static class Repository
    {
        public static bool StoreVehicleToDatabase(Rental_Registration rentCarData)
        {
            rentCarData.bookingNumber = GenerateBookingNumber();
            //SetConnectionString();

            using (SaabTestEntities database = new SaabTestEntities())
            {
                database.Rental_Registration.Add(rentCarData);
                try
                {
                    database.SaveChanges();
                    Console.WriteLine($"Data sent successfully.\n\rYour booking number is: {rentCarData.bookingNumber}\n\r\n\rPress any key to continue ...");
                    Console.ReadLine();
                    return true;
                }
                catch (Exception exception)
                {

                    Console.WriteLine("Error: " + exception.ToString());
                    Console.ReadLine();
                    return false;
                }
                
            }
        }

        private static int GenerateBookingNumber()
        {
            

           int bookingNumber = Guid.NewGuid().GetHashCode();
            while (bookingNumber < 0)
            {
                bookingNumber = Guid.NewGuid().GetHashCode();
            }
            return bookingNumber;
            
            
            //using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            //{
            //    int bookingNumber = -1;
            //    while (bookingNumber < 0)
            //    {
            //        bookingNumber = rng.GetHashCode();
            //    }
            //    return bookingNumber;
            //}
        }

        internal static Rental_Registration ReturnVehicle(int number)
        {
            using (SaabTestEntities database = new SaabTestEntities())
            {
                Rental_Registration reservation = new Rental_Registration();
                reservation = database.Rental_Registration.Where(x => x.bookingNumber == number).FirstOrDefault();
                if (reservation != null)
                {
                    reservation.rentEndDate = DateTime.Now.AddDays(10);
                    Random random = new Random();
                    reservation.CurrentMilageKm = random.Next(500);
                    try
                    {
                        database.SaveChanges();
                        return reservation;
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("Error: " + exception.InnerException.Message);
                        Console.ReadLine();
                        return reservation;
                    }
                }
                else
                {
                    return reservation;
                }
                
                
            }
        }

        private static void SetConnectionString()
        {
            var originalConnectionString = ConfigurationManager.ConnectionStrings["SaabTestEntities"].ConnectionString;
            var entityBuilder = new EntityConnectionStringBuilder(originalConnectionString);
            var factory = DbProviderFactories.GetFactory(entityBuilder.Provider);
            var providerBuilder = factory.CreateConnectionStringBuilder();

            providerBuilder.ConnectionString = entityBuilder.ProviderConnectionString;

            providerBuilder.Add("password", "SHIMAshinenKOUEN150");

            entityBuilder.ProviderConnectionString = providerBuilder.ToString();
        }
    }
}
