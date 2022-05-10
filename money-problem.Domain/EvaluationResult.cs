using OneOf;

namespace money_problem.Domain;

public class EvaluationResult : OneOfBase<Money, MissingExchangeRate[]>
{
    public MissingExchangeRate[] MissingExchangeRates => AsT1;
    public Money Money => AsT0;

    protected EvaluationResult(OneOf<Money, MissingExchangeRate[]> input) : base(input)
    {
    }

    public static implicit operator EvaluationResult(Money _) => new(_);
    public static implicit operator EvaluationResult(MissingExchangeRate[] _) => new(_);
}