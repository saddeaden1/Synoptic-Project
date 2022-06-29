using System.Linq;
using MembershipSystem.Domain.Exceptions;
using MembershipSystem.Domain.Interfaces;
using MembershipSystem.Repository;
using MembershipSystem.Repository.DbModels;

namespace MembershipSystem.Domain
{
    public class TransactionService : ITransactionService
    {
        private readonly SharedContext _context;

        public TransactionService(SharedContext context)
        {
            _context = context;
        }

        public TransactionService()
        {
            _context = new SharedContext();
        }

        public void AddMoney(string sessionToken, string amount)
        {
            var card = GetMembershipCard(sessionToken);
            var newCard = new MembershipCardDbModel()
            {
                Balance = card.Balance,
                MembershipCardId = card.MembershipCardId,
                EmployeeDbModel = _context.Employees.Single(s=>s.MembershipCardDbModelId == card.MembershipCardId)
            };

            try
            {
                newCard.Balance += double.Parse(amount);
                _context.MembershipCards.Remove(card);
                _context.MembershipCards.Add(newCard);
                _context.SaveChanges();

            }
            catch
            {
                throw new FailedTransactionException("Adding Money Failed");
            }

        }

        public void BuySomething(string sessionToken, string amount)
        {

            var card = GetMembershipCard(sessionToken);
            var newCard = new MembershipCardDbModel()
            {
                Balance = card.Balance,
                MembershipCardId = card.MembershipCardId,
                EmployeeDbModel = _context.Employees.Single(s => s.MembershipCardDbModelId == card.MembershipCardId)
            };

            if (card.Balance > double.Parse(amount))
            {
                try
                {
                    newCard.Balance -= double.Parse(amount);
                    _context.MembershipCards.Remove(card);
                    _context.MembershipCards.Add(newCard);
                    _context.SaveChanges();

                }
                catch
                {

                    throw new FailedTransactionException("Adding Money Failed");
                }
            }
            else
            {
                throw new InsufficientFundsException();
            }
        }

        public double CheckBalance(string sessionToken)
        {
            var card = GetMembershipCard(sessionToken);

            return card.Balance;
        }

        private MembershipCardDbModel GetMembershipCard(string sessionToken)
        {
            var session = _context.ActiveSessions.SingleOrDefault(s => s.SessionToken == sessionToken);

            var employee = _context.Employees.SingleOrDefault(s => s.EmployeeId == session.EmployeeId);

            var card = _context.MembershipCards.Single(s => s.MembershipCardId == employee.MembershipCardDbModelId);
            return card;
        }
    }
}