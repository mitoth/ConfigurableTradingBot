using FluentValidation;

namespace TradingBot.Core.Models.Validators;

public class OtherStockBasedConditionValidator 
    : AbstractValidator<OtherStockBasedCondition>
{
    public OtherStockBasedConditionValidator()
    {
        RuleFor(c => c.StockName).NotEmpty();
    }
}