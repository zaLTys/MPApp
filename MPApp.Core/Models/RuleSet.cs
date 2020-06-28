namespace MPApp.Core.Models
{
    public class RuleSet
    {
        public string MerchantName { get; set; }
        public decimal FeePercentageDiscount { get; set; }
        public decimal FixedFeePerMonth { get; set; }
        public decimal DefaultFeePercentageRate { get; set; }

        public RuleSet(string merchantName, decimal feePercentageDiscount = 0, decimal fixedFee = 0, decimal defaultFeePercentageRate = 0)
        {
            MerchantName = merchantName;
            FeePercentageDiscount = feePercentageDiscount;
            FixedFeePerMonth = fixedFee;
            DefaultFeePercentageRate = defaultFeePercentageRate;
        }
    }
}