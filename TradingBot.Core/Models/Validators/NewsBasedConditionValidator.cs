using FluentValidation;

namespace TradingBot.Core.Models.Validators;

public class NewsBasedConditionValidator
    : AbstractValidator<NewsBasedCondition>
{
    public NewsBasedConditionValidator()
    {
        RuleFor(c => c.SearchString).NotEmpty();
    }
}