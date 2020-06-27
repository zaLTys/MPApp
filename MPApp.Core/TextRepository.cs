using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MPApp.Core
{
    public class TextRepository : IRepository
    {
        public async Task<List<Payment>> GetPaymentData()
        {
            string line;
            List<Payment> payments = new List<Payment>();
            System.IO.StreamReader file =
                new System.IO.StreamReader(@"c:\transactions.txt");
            while ((line = file.ReadLine()) != null)
            {
                string[] data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                payments.Add(new Payment(Convert.ToDateTime(data[0]) , data[1], Convert.ToDecimal(data[2])));
            }

            file.Close();
            return payments;

        }
    }
}