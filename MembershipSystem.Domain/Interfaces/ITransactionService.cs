namespace MembershipSystem.Domain.Interfaces
{
    public interface ITransactionService
    {
        void AddMoney(string sessionToken, string amount);
        void BuySomething(string sessionToken, string amount);
        double CheckBalance(string sessionToken);
    }
}