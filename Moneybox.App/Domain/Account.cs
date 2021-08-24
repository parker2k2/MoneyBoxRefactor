using System;
using Moneybox.App.Domain.Enums;

namespace Moneybox.App.Domain
{
    public class Account
    {
        public Account(Guid id, User user, decimal balance, decimal withdrawn, decimal paidIn)
        {
            Id = id;
            User = user;
            Balance = balance;
            Withdrawn = withdrawn;
            PaidIn = paidIn;
        }

        private const decimal PayInLimit = 4000m;
        private const decimal ApproachingPayInLimit = 500m;
        private const decimal LowBalanceLimit = 500m;

        private const string PositiveValueError = "Amount must be a positive value";
        private const string InsufficientFundsError = "Insufficient funds to make transfer";
        private const string AccountPayInLimitError = "Account pay in limit reached";

        public Guid Id { get; private set; }

        public User User { get; private set; }

        public decimal Balance { get; private set; }

        public decimal Withdrawn { get; private set; }

        public decimal PaidIn { get; private set; }

        public ENotification DecreaseBalance(decimal amount)
        {
            if (amount <= 0m)
            {
                throw new ArgumentException(PositiveValueError);
            }

            if (Balance - amount < 0m)
            {
                throw new InvalidOperationException(InsufficientFundsError);
            }

            Balance -= amount;

            if (Balance < LowBalanceLimit)
            {
                return ENotification.FundsLow;
            }

            return ENotification.None;
        }

        public void DecreaseWithdrawn(decimal amount)
        {
            if (amount <= 0m)
            {
                throw new ArgumentException(PositiveValueError);
            }

            Withdrawn -= amount;
        }

        public void IncreaseBalance(decimal amount)
        {
            if (amount <= 0m)
            {
                throw new ArgumentException(PositiveValueError);
            }

            Balance += amount;
        }

        public ENotification IncreasePaidIn(decimal amount)
        {
            if (amount <= 0m)
            {
                throw new ArgumentException(PositiveValueError);
            }

            if (PaidIn + amount > PayInLimit)
            {
                throw new InvalidOperationException(AccountPayInLimitError);
            }

            PaidIn += amount;

            if (PayInLimit - PaidIn < ApproachingPayInLimit)
            {
                return ENotification.ApproachingPayInLimit;
            }

            return ENotification.None;
        }
    }
}