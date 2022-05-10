using OneOf;

namespace money_problem.Domain
{
    public record Portfolio(params Money[] Moneys)
    {
        public EvaluationResult Evaluate(
            Bank bank,
            Currency toCurrency)
        {
            var convertedMoneys =
                Moneys
                    .Select(money => bank.Convert(money, toCurrency))
                    .ToList();

            return !HasMissingExchangeRates(convertedMoneys)
                ? new Money(ToAmountInCurrency(convertedMoneys), toCurrency)
                : ToMissingExchangeRates(convertedMoneys);
        }

        private static bool HasMissingExchangeRates(List<OneOf<Money, MissingExchangeRate>> convertedMoneys)
            => convertedMoneys.Exists(_ => _.IsT1);

        private static double ToAmountInCurrency(List<OneOf<Money, MissingExchangeRate>> convertedMoneys)
            => convertedMoneys.Aggregate(0d, (acc,
                money) => acc + money.Match(success => success.Amount, _ => 0));

        private static MissingExchangeRate[] ToMissingExchangeRates(
            List<OneOf<Money, MissingExchangeRate>> convertedMoneys)
            => convertedMoneys.Where(_ => _.IsT1).Select(_ => _.AsT1).ToArray();
    }
}