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

        private static bool HasMissingExchangeRates(List<ConversionResult> convertedMoneys)
            => convertedMoneys.Exists(_ => _.IsFailure);

        private static double ToAmountInCurrency(IEnumerable<ConversionResult> convertedMoneys)
            => convertedMoneys.Aggregate(0d, (acc,
                money) => acc + money.Match(success => success.Amount, _ => 0));

        private static MissingExchangeRate[] ToMissingExchangeRates(List<ConversionResult> convertedMoneys)
            => convertedMoneys
                .Where(_ => _.IsFailure)
                .Select(_ => _.MissingExchangeRate)
                .ToArray();
    }
}