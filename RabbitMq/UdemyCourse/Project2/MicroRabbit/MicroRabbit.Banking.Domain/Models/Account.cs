namespace MicroRabbit.Banking.Domain.Models;

public class Account
{
    public int Id { get; set; }
    public int AccountType { get; set; }
    public decimal AccountBalance { get; set; }
}
