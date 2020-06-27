using System;

namespace MPApp.Core.Models
{
    public class Payment
    {
        public DateTime Date { get; set; }
        public string MerchantName { get; set; }
        public decimal Amount { get; set; }

        public Payment(DateTime date, string merchantName, decimal amount)
        {
            Date = date;
            MerchantName = merchantName;
            Amount = amount;
        }
    }
}