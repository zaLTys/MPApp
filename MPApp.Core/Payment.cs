using System;

namespace MPApp.Core
{
    public class Payment
    {
        public DateTime Date { get; set; }
        public string MerchantName { get; set; }
        public decimal Fee { get; set; }

        public Payment(DateTime date, string merchantName, decimal fee)
        {
            Date = date;
            MerchantName = merchantName;
            Fee = fee;
        }
    }
}