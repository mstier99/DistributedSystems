namespace MicroRabbit.Banking.Domain.Models;

public class CreateAccount // én adtam hozzá
{
    public int AccountType { get; set; }
    public decimal AccountBalance { get; set; }
}