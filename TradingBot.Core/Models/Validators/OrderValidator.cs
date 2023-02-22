using FluentValidation;

namespace TradingBot.Core.Models.Validators;

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        RuleFor(c => c.Symbol).NotEmpty();
        RuleFor(c => c.Id).NotEmpty();
        RuleForEach(c => c.OrderConditions).SetInheritanceValidator(
            v =>
            {
                v.Add(new NewsBasedConditionValidator());
                v.Add(new OtherStockBasedConditionValidator());
            }
        );
    }
}