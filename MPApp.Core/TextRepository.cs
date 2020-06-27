using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MPApp.Core.Models;

namespace MPApp.Core
{
    public class TextRepository : IRepository
    {
        public async Task<List<Payment>> GetPaymentDataAsync()
        {
            return Task.Run(GetPaymentData).Result.ToList();

        }

        private IEnumerable<Payment> GetPaymentData()
        {
            string line;
            List<Payment> payments = new List<Payment>();
            System.IO.StreamReader sr =
                new System.IO.StreamReader(@"c:\transactions.txt");
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (!String.IsNullOrEmpty(line))
                {
                    string[] data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    yield return new Payment(Convert.ToDateTime(data[0]), data[1], Convert.ToDecimal(data[2]));
                }
            }

            sr.Close();

        }
    }
}