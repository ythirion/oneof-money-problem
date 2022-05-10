using OneOf;

namespace money_problem.Domain
{
    public record Portfolio(params Money[] Moneys)
    {
        public OneOf<Money, MissingExchangeRate[]> Evaluate(
            Bank bank,
            Currency toCurrency)
        {
            var missingExchangeRates = new List<MissingExchangeRate>();
            var convertedMoneys = new List<Money>();

            foreach (var money in Moneys)
            {
                bank.Convert(money, toCurrency)
                    .Switch(
                        converted => convertedMoneys.Add(converted),
                        missingExchangeRate => missingExchangeRates.Add(missingExchangeRate)
                    );
            }

            return !missingExchangeRates.Any()
                ? new Money(convertedMoneys.Aggregate(0d, (acc, money) => acc + money.Amount), toCurrency)
                : missingExchangeRates.ToArray();
        }
    }
}