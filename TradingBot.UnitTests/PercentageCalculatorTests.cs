using NUnit.Framework;
using TradingBot.Core.Services;
using FluentAssertions;

namespace TradingBot.UnitTests;

public class PercentageCalculatorTests
{
    [Test]
    public void TestPercentageCalculatorWithGain()
    {
        var increase = PercentageCalculator.ComputeIncrease(10, 15);
        increase.Should().Be(50);
        Assert.That(increase, Is.EqualTo(50));
    }
    
    [Test]
    public void TestPercentageCalculatorWithLoss()
    {
        var increase = PercentageCalculator.ComputeIncrease(20, 10);
        increase.Should().Be(-50);
        Assert.AreEqual(-50, increase);
    }

    [TestCase(1, 10, 20, true)]
    [TestCase(1, 10, 9, false)]
    [TestCase(-1, 10, 20, false)]
    [TestCase(-1, 10, 2, true)]
    [TestCase(0, 10, 10, true)]
    [TestCase(50, 10, 15, true)]
    [TestCase(50, 10, 16, true)]
    [TestCase(50, 10, 14.9, false)]
    [TestCase(-50, 10, 5, true)]
    [TestCase(-50, 10, 4, true)]
    [TestCase(-50, 10, 5.1, false)]
    public void TestExpectedChangeHappened(decimal changeInPercentage, decimal referenceValue, decimal currentValue, 
        bool expectedChangeShouldHappen)
    {
        var expectedPriceChangeHappened =
            PercentageCalculator.ExpectedPriceChangeHappened(changeInPercentage, referenceValue, currentValue);
        expectedChangeShouldHappen.Should().Be(expectedPriceChangeHappened);
    }
}