using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;
using Moneybox.App.Domain.Enums;

namespace Moneybox.App.Features
{
    public class TransferMoney
    {
        private IAccountRepository accountRepository;
        private INotificationService notificationService;

        public TransferMoney(IAccountRepository accountRepository, INotificationService notificationService)
        {
            this.accountRepository = accountRepository;
            this.notificationService = notificationService;
        }

        public void Execute(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            var from = this.accountRepository.GetAccountById(fromAccountId);
            var to = this.accountRepository.GetAccountById(toAccountId);

            if (from.DecreaseBalance(amount) == ENotification.FundsLow)
            {
                this.notificationService.NotifyFundsLow(from.User.Email);
            }

            from.DecreaseWithdrawn(amount);

            if (to.IncreasePaidIn(amount) == ENotification.ApproachingPayInLimit)
            {
                this.notificationService.NotifyApproachingPayInLimit(from.User.Email);
            }

            to.IncreaseBalance(amount);

            this.accountRepository.Update(from);
            this.accountRepository.Update(to);
        }
    }
}