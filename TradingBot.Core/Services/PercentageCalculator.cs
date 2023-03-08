namespace TradingBot.Core.Services;

/// <summary>
/// Helper class for calculating percentage differences
/// </summary>
public static class PercentageCalculator
{
    /// <summary>
    /// Compute the increase/decrease between 2 numbers in percentage
    /// </summary>
    /// <param name="startValue">Reference value</param>
    /// <param name="currentValue">Current value</param>
    /// <returns>Increase in percentage from reference value to current value; negative number if decrease</returns>
    public static decimal ComputeIncrease(decimal startValue, decimal currentValue)
    {
        var difference = currentValue - startValue;
        return difference * 100 / startValue;
    }
    
    /// <summary>
    /// Checks if the expected price changed happened
    /// </summary>
    /// <param name="expectedChangePercentage">The expected change in percentage</param>
    /// <param name="symbolPriceAtReferencePoint"></param>
    /// <param name="symbolPriceNow"></param>
    /// <returns></returns>
    public static bool ExpectedPriceChangeHappened(decimal expectedChangePercentage,
        decimal symbolPriceAtReferencePoint, decimal symbolPriceNow)
    {
        var differenceInPercentage = PercentageCalculator.ComputeIncrease(symbolPriceAtReferencePoint, symbolPriceNow);
        if (expectedChangePercentage < 0)
        {
            return expectedChangePercentage >= differenceInPercentage;
        }

        return expectedChangePercentage <= differenceInPercentage;
    }
}