using System;

namespace MPApp.Core.Models
{
    public class Fee
    {
        public DateTime Date { get; set; }
        public string MerchantName { get; set; }
        public decimal Amount { get; set; }

        public Fee(DateTime date, string merchantName, decimal amount)
        {
            Date = date;
            MerchantName = merchantName;
            Amount = amount;
        }
    }
}