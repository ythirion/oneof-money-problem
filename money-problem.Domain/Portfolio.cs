using OneOf;

namespace money_problem.Domain
{
    public record Portfolio(params Money[] Moneys)
    {
        public OneOf<Money, MissingExchangeRate[]> Evaluate(
            Bank bank,
            Currency toCurrency)
        {
            var convertedMoneys =
                Moneys
                    .Select(money => bank.Convert(money, toCurrency))
                    .ToList();

            return !convertedMoneys.Exists(_ => _.IsT1)
                ? new Money(convertedMoneys.Aggregate(0d, (acc, money) => acc + money.Match(
                    success => success.Amount,
                    _ => 0)), toCurrency)
                : convertedMoneys.Where(_ => _.IsT1).Select(_ => _.AsT1).ToArray();
        }
    }
}