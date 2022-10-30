using System;

namespace DeliveryDatesGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var x = DateTime.UtcNow.DayOfWeek;
            var y = DayOfWeek.Sunday;
            Console.WriteLine("Hello, World!");
        }
    }
}