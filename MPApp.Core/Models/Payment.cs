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

    public class ProcessedPayment : Payment
    {
        public decimal Fee { get; set; }
        public ProcessedPayment(Payment payment, decimal fee) : base(payment.Date, payment.MerchantName, payment.Amount)
        {
            Fee = fee;
        }
    }
}