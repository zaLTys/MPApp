using MPApp.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MPApp.tests
{
    public class FeeCalculatorShould
    {
        //private readonly IRepository _repository;
        //private readonly FeeCalculator _feeCalculator;

        public FeeCalculatorShould()
        {
           // _repository = repository;
            //_feeCalculator = feeCalculator;
        }

        [Theory]
        [InlineData("2018-09-01", "7-ELEVEN", 100)]
        [InlineData("2018-09-04","CIRCLE_K",100)]
        [InlineData("2018-09-07","TELIA",100)]
        [InlineData("2018-09-09","NETTO",100)]
        [InlineData("2018-09-13","CIRCLE_K",100)]
        [InlineData("2018-09-16","TELIA",100)]
        [InlineData("2018-09-19","7-ELEVEN",100)]
        [InlineData("2018-09-22","CIRCLE_K",100)]
        [InlineData("2018-09-25","TELIA",100)]
        [InlineData("2018-09-28","7-ELEVEN",100)]
        [InlineData("2018-09-30","CIRCLE_K",100)]


        public void RetrieveDataFromFile(string dateString, string merchantName, decimal fee) //format - Date merchantName fee 
        {
            var sut = new FeeCalculator(new TextRepository());
            var result = sut.LoadData().Result.ToList();
            Assert.True(result.Exists(x => x.Date == Convert.ToDateTime(dateString) && x.Fee == fee && x.MerchantName == merchantName));
            var payment = result.Single(x => x.Date == Convert.ToDateTime(dateString) && x.Fee == fee && x.MerchantName == merchantName);
            
            AssertPaymentFields(dateString, merchantName, fee, payment);
        }

        private static void AssertPaymentFields(string dateString, string merchantName, decimal fee, Payment payment)
        {
            Assert.Equal(Convert.ToDateTime(dateString), payment.Date);
            Assert.Equal(merchantName, payment.MerchantName);
            Assert.Equal(fee, payment.Fee);
        }
    }


}
