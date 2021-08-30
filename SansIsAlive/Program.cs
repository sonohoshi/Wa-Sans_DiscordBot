using System;
using System.Configuration;
using SansIsAlive.Bot;

namespace SansIsAlive
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var token = ConfigurationManager.AppSettings["Token"];
            Console.WriteLine("SANS IS ALIVE\n");
            BotInitializer.BotMain(token).GetAwaiter().GetResult();
        }
    }
}