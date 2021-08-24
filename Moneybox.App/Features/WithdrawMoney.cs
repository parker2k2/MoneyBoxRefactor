using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;
using Moneybox.App.Domain.Enums;

namespace Moneybox.App.Features
{
    public class WithdrawMoney
    {
        private IAccountRepository accountRepository;
        private INotificationService notificationService;

        public WithdrawMoney(IAccountRepository accountRepository, INotificationService notificationService)
        {
            this.accountRepository = accountRepository;
            this.notificationService = notificationService;
        }

        public void Execute(Guid fromAccountId, decimal amount)
        {
            var from = this.accountRepository.GetAccountById(fromAccountId);

            if (from.DecreaseBalance(amount) == ENotification.FundsLow)
            {
                this.notificationService.NotifyFundsLow(from.User.Email);
            }

            from.DecreaseWithdrawn(amount);

            this.accountRepository.Update(from);
        }
    }
}