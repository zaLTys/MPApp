using System;
using Microsoft.Extensions.DependencyInjection;
using MPApp.Core;

namespace MPApp.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddTransient<IRepository, TextRepository>()
                
                .BuildServiceProvider();

            //var repo = serviceProvider.GetService<IRepository>();
            //repo.GetPaymentData();

        }
    }
}
