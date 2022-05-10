using OneOf;

namespace money_problem.Domain;

public class ConversionResult : OneOfBase<Money, MissingExchangeRate>
{
    public MissingExchangeRate MissingExchangeRate => AsT1;
    public Money Money => AsT0;

    public bool IsFailure => IsT1;

    protected ConversionResult(OneOf<Money, MissingExchangeRate> input) : base(input)
    {
    }

    public static implicit operator ConversionResult(Money _) => new(_);
    public static implicit operator ConversionResult(MissingExchangeRate _) => new(_);
}