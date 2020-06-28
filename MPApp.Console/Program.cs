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

            var calc = new FeeCalculator(repo);

            await calc.ProcessPayments("testData", "rules");

            System.Console.Read();
        }
    }
}
