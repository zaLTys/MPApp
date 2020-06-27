namespace MPApp.Core.Models
{
    public class FeeRuleSet
    {
        public string MerchantName { get; set; }
        public decimal FeePercentageDiscount { get; set; }
        public decimal FixedFeePerMonth { get; set; }

        public FeeRuleSet(string merchantName, decimal feePercentageDiscount = 0, decimal fixedFee = 0)
        {
            MerchantName = merchantName;
            FeePercentageDiscount = feePercentageDiscount;
            FixedFeePerMonth = fixedFee;
        }
    }
}