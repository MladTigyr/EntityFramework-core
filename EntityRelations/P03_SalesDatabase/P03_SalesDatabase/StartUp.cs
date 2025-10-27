using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P03_SalesDatabase
{
    using Bogus;
    using P03_SalesDatabase.Data;
    using static P03_SalesDatabase.Common.EntityValidation;

    public class StartUp
    {
        static void Main(string[] args)
        {
            SalesContext context = new SalesContext();
            context.Seed();

            

            Console.WriteLine("Hello World!");
        }
    }
}
