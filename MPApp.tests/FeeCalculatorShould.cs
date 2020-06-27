using Moq;
using MPApp.Core;
using MPApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MPApp.tests
{
    public class FeeCalculatorShould
    {
        private readonly Mock<IRepository> _repositoryMock;

        public FeeCalculatorShould()
        {
            _repositoryMock = new Mock<IRepository>();
        }

        [Theory]
        [InlineData("2018-09-01", "7-ELEVEN", 100)]
        [InlineData("2018-09-04", "CIRCLE_K", 100)]
        [InlineData("2018-09-07", "TELIA", 100)]
        [InlineData("2018-09-09", "NETTO", 100)]
        [InlineData("2018-09-13", "CIRCLE_K", 100)]
        [InlineData("2018-09-16", "TELIA", 100)]
        [InlineData("2018-09-19", "7-ELEVEN", 100)]
        [InlineData("2018-09-22", "CIRCLE_K", 100)]
        [InlineData("2018-09-25", "TELIA", 100)]
        [InlineData("2018-09-28", "7-ELEVEN", 100)]
        [InlineData("2018-09-30", "CIRCLE_K", 100)]
        public void RetrieveDataFromFile(string dateString, string merchantName, decimal paymentAmount)
        {
            var sut = new FeeCalculator(new TextRepository());
            var result = sut.LoadData().Result.ToList();
            Assert.True(result.Exists(x => x.Date == Convert.ToDateTime(dateString) && x.Amount == paymentAmount && x.MerchantName == merchantName));

            var payment = result.Single(x => x.Date == Convert.ToDateTime(dateString) && x.Amount == paymentAmount && x.MerchantName == merchantName);
            AssertPaymentFields(dateString, merchantName, paymentAmount, payment);
        }

        [Fact]
        public void AddAndRetrieveRuleSet()
        {
            var sut = new FeeCalculator(new TextRepository());
            sut.AddRuleSet("CIRCLE_K", 15, 5);

            var rules = sut.GetRules();
            Assert.Single(rules);

            var set = rules.Single();
            Assert.Equal("CIRCLE_K", set.MerchantName);
            Assert.Equal(15, set.FeePercentageDiscount);
            Assert.Equal(5, set.FixedFeePerMonth);
        }

        [Theory]
        [InlineData("2018-09-04", "CIRCLE_K", 100, 0.8)]
        [InlineData("2018-09-13", "CIRCLE_K", 120, 0.96)]
        [InlineData("2018-09-22", "CIRCLE_K", 200, 1.60)]
        [InlineData("2018-09-30", "CIRCLE_K", 300, 2.40)]
        [InlineData("2018-09-30", "CIRCLE_K", 150, 1.2)]
        public void ApplyPercentageDiscount(string dateString, string merchantName, decimal paymentAmount, decimal expectedFeeAmount)
        {
            var sut = new FeeCalculator(new TextRepository());
            var ruleSet = new FeeRuleSet("CIRCLE_K", 20, 0);

            var paymentDate = Convert.ToDateTime(dateString);
            var payment = new Payment(paymentDate, merchantName, paymentAmount);
            var fee = sut.CalculateFeeFromPayment(payment, ruleSet);

            AssertFeeFields(dateString, merchantName, expectedFeeAmount, fee);
        }

        //[Theory]
        //[InlineData("2018-09-02", "7-ELEVEN", 120, 30.20)]
        //[InlineData("2018-09-04", "NETTO", 200, 31.00)]
        //[InlineData("2018-10-22", "7-ELEVEN", 300, 32.00)]
        //[InlineData("2018-10-29", "7-ELEVEN", 150, 1.50)]
        [Fact]
        public void ApplyFixedFee()//string dateString, string merchantName, decimal paymentAmount, decimal expectedFeeAmount)
        {
            var testData = new List<Payment>()
            {
                new Payment(Convert.ToDateTime("2018-09-02"), "7-ELEVEN", 120),
                new Payment(Convert.ToDateTime("2018-09-04"), "NETTO", 120),
                new Payment(Convert.ToDateTime("2018-10-22"), "7-ELEVEN", 120),
                new Payment(Convert.ToDateTime("2018-10-29"), "7-ELEVEN", 120)
            };
            _repositoryMock.Setup(x => x.GetPaymentDataAsync()).Returns(Task.FromResult(testData));

            var sut = new FeeCalculator(_repositoryMock.Object);

            sut.AddRuleSet("7-ELEVEN", 0, 29);
            sut.AddRuleSet("NETTO", 0, 29);

            var result = sut.PrintFees().Result;

            
        }


        private static void AssertFeeFields(string dateString, string merchantName, decimal expectedFeeAmount, Fee fee)
        {
            Assert.Equal(dateString, fee.Date.ToString("yyyy-MM-dd"));
            Assert.Equal(merchantName, fee.MerchantName);
            Assert.Equal(expectedFeeAmount, fee.Amount);
        }

        private static void AssertPaymentFields(string dateString, string merchantName, decimal fee, Payment payment)
        {
            Assert.Equal(Convert.ToDateTime(dateString), payment.Date);
            Assert.Equal(merchantName, payment.MerchantName);
            Assert.Equal(fee, payment.Amount);
        }

    }
}
