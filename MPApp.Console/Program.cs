using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MPApp.Core;

namespace MPApp.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddTransient<IRepository, TextRepository>()
                
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IRepository>();

            var calc = new PaymentProcessor(repo);

            System.Console.WriteLine("Enter name of payments data file, leave blank for default value");
            var paymentsFileName =  System.Console.ReadLine();
            if (String.IsNullOrEmpty(paymentsFileName))
                paymentsFileName = "testData";
            System.Console.WriteLine("Enter name of fee rules data file, leave blank for default value");
            var rulesFileName = System.Console.ReadLine();
            if (String.IsNullOrEmpty(rulesFileName))
                rulesFileName = "rules";
            await calc.ProcessPayments(paymentsFileName, rulesFileName);

            System.Console.Read();
        }
    }
}
