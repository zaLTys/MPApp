using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MPApp.Core.Models;

namespace MPApp.Core
{
    public class PaymentProcessor
    {
        private readonly IRepository _repository;
        private List<RuleSet> _feeRules;

        public PaymentProcessor(IRepository repository)
        {
            _repository = repository;
            _feeRules = new List<RuleSet>();
        }
        public IEnumerable<Payment> LoadPaymentData(string fileName)
        {
            return _repository.GetPaymentData(fileName);
        }
        public async Task<IEnumerable<RuleSet>> LoadRules(string fileName)
        {
            return await _repository.GetRulesAsync(fileName);
        }

        public async Task<List<ProcessedPayment>> ProcessPayments(string paymentsFileName, string rulesFileName)
        {
            var payments = LoadPaymentData(paymentsFileName);
            
            _feeRules = LoadRules(rulesFileName).Result.ToList();

            var processedPayments = new List<ProcessedPayment>();
            foreach (var payment in payments)
            {
                var firstPaymentOfMonthProcessed = processedPayments.Any(x => x.MerchantName == payment.MerchantName
                                                                           && payment.Date.ToString("yyyyMM") == x.Date.ToString("yyyyMM"));
                var ruleSet = _feeRules.FirstOrDefault(x => x.MerchantName == payment.MerchantName);

                var processedPayment = CalculateFeeFromPayment(payment, ruleSet, firstPaymentOfMonthProcessed);
                processedPayments.Add(processedPayment);
                PrintProcessedPaymentFee(processedPayment);
            }

            return processedPayments;
        }

        private static void PrintProcessedPaymentFee(ProcessedPayment payment)
        {
            Console.WriteLine($"{payment.Date:yyyy-MM-dd} {payment.MerchantName} {String.Format("{0:0.00}", payment.Fee)}");
        }

        public void AddRuleSet(string merchantName, decimal feePercentageDiscount, decimal fixedFee)
        {
            var ruleSet = new RuleSet(merchantName, feePercentageDiscount, fixedFee);
            _feeRules.Add(ruleSet);
        }

        public List<RuleSet> GetRules()
        {
            return _feeRules;
        }

        public ProcessedPayment CalculateFeeFromPayment(Payment payment, RuleSet ruleSet, bool firstPaymentOfMonthProcessed)
        {
            if (ruleSet == null)
            {
                var defaultFee = ApplyDefaultFee(payment, 1);
                return new ProcessedPayment(payment, defaultFee);
            }
            var fee = ApplyDefaultFee(payment, ruleSet.DefaultFeePercentageRate);
            
            fee = ApplyPercentageDiscount(ruleSet.FeePercentageDiscount, fee);
            if (!firstPaymentOfMonthProcessed)
            {
                fee = ApplyFixedFeeAmount(ruleSet.FixedFeePerMonth, fee);
            }
            return new ProcessedPayment(payment, fee);
        }

        private static decimal ApplyFixedFeeAmount(decimal fixedFeePerMonth, decimal fee)
        {
            fee += fixedFeePerMonth;
            return fee;
        }

        private static decimal ApplyPercentageDiscount(decimal feePercentageDiscount, decimal fee)
        {
            fee = fee - (fee * feePercentageDiscount / 100);
            return fee;
        }

        private static decimal ApplyDefaultFee(Payment payment, decimal defaultFeePercentageRate)
        {
            return payment.Amount * defaultFeePercentageRate / 100;
        }
    }

}