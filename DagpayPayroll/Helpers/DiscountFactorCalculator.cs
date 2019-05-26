using System;
namespace DagpayPayroll.Helpers
{
    public class DiscountFactorCalculator
    {
        public static int CalculateDiscountFactor(string firstName)
        {
            int discountFactor = 100;

            if (firstName.Substring(0, 1) == "A")
            {
                discountFactor = 90;
            }

            return discountFactor;
        }
    }
}
