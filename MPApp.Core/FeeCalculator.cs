using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MPApp.Core.Models;

namespace MPApp.Core
{
    public class FeeCalculator
    {
        private readonly IRepository _repository;
        private readonly List<FeeRuleSet> _feeRules;

        public FeeCalculator(IRepository repository)
        {
            _repository = repository;
            _feeRules = new List<FeeRuleSet>();
        }
        public async Task<IEnumerable<Payment>> LoadData()
        {
            return await _repository.GetPaymentDataAsync();
        }

        public async Task<List<Fee>> PrintFees()
        {
            var payments = await LoadData();
            var fees = new List<Fee>();

            foreach (var payment in payments)
            {
                var ruleSet = _feeRules.FirstOrDefault(x => x.MerchantName == payment.MerchantName);

                var fee = CalculateFeeFromPayment(payment, ruleSet);
                fees.Add(fee);
                PrintFee(fee);
            }

            return fees;
        }

        private static void PrintFee(Fee fee)
        {
            Console.WriteLine($"{fee.Date:yyyy-MM-dd} {fee.MerchantName} {fee.Amount}");
        }

        public void AddRuleSet(string merchantName, decimal feePercentageDiscount, decimal fixedFee)
        {
            var ruleSet = new FeeRuleSet(merchantName, feePercentageDiscount, fixedFee);
            _feeRules.Add(ruleSet);
        }

        public List<FeeRuleSet> GetRules()
        {
            return _feeRules;
        }

        public Fee CalculateFeeFromPayment(Payment payment, FeeRuleSet ruleSet)
        {
            
            var fee = payment.Amount * 1 / 100;
            fee = ApplyRuleSetToPayment(ruleSet, fee);
            return new Fee(payment.Date, payment.MerchantName, fee);
        }

        private static decimal ApplyRuleSetToPayment(FeeRuleSet ruleSet, decimal fee)
        {
            if (ruleSet != null)
            {
                fee = fee - (fee * ruleSet.FeePercentageDiscount / 100);
            }

            return fee;
        }
    }

}