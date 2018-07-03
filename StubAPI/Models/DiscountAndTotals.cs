using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StubAPI.Models
{
    public class DiscountAndTotals
    {
          public decimal TotalMontlyIcomeAfterAdjustment { get; set; }
          public decimal TotalGleAfterAdjustment { get; set; }
          public decimal TotalMonthlyExpensesAfterAdjustment { get; set; }
          public decimal TotalExpensesAfterAdjustmentHemInclusive { get; set; }
          public decimal NetDisposableIncomePreInstallment { get; set; }
          public decimal NetDisposableIncomePercentagePreInstallment { get; set; }
          public decimal PrimaryBorrowerDiscountedRent { get; set; }
          public decimal PrimaryBorrowerDiscountedOvertimeBonus { get; set; }
          public decimal PrimaryBorrowerDiscountedOtherIncome { get; set; }
          public decimal CoBorrowerDiscountedRent { get; set; }
          public decimal CoBorrowerDiscountedOvertimeBonus { get; set; }
          public decimal CoBorrowerDiscountedOtherIncome { get; set; }
          public decimal NonBorrowerContributionAmountUsed { get; set; }     

    }
}