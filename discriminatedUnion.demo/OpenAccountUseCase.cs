using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace discriminatedUnion.demo;

public class OpenAccountUseCase : IUseCase<OpenAccountRequest, OpenAccountResponse>
{
    public OpenAccountResponse Handle(OpenAccountRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName))
        {
            throw new ValidationException("First name should not be Empty");
        }

        if (string.IsNullOrWhiteSpace(request.LastName))
        {
            throw new ValidationException("First name should not be Empty");
        }
        
        var balance = Balance.From(request.Balance);
        var bankAccount = new BankAccount("New Account Number", request.FirstName, request.LastName, balance);

        return new OpenAccountResponse(bankAccount.AccountNumber);
    }
}