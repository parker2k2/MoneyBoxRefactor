using System;
using Moneybox.App.Domain;
using Moneybox.App.Domain.Enums;
using NUnit.Framework;

namespace Moneybox.App.Test
{
    public class AccountTest
    {
        [Test]
        public void DecreaseBalance_amountLargerThanBalance_ShouldThrowInvalidOperationException()
        {
            var account = new Account(Guid.NewGuid(), null, 10, 0, 0);

            Assert.Throws(Is.TypeOf<InvalidOperationException>()
                    .And.Message.EqualTo("Insufficient funds to make transfer"),
                () => account.DecreaseBalance(20));
        }

        [Test]
        public void DecreaseBalance_nonPositiveValue_ShouldThrowArgumentException()
        {
            var account = new Account(Guid.NewGuid(), null, 10, 0, 0);

            Assert.Throws(Is.TypeOf<ArgumentException>()
                    .And.Message.EqualTo("Amount must be a positive value"),
                () => account.DecreaseBalance(-20));
        }

        [Test]
        public void DecreaseBalance_LowBalanceLimit_ShouldReturnENotificationFundsLow()
        {
            var account = new Account(Guid.NewGuid(), null, 600, 0, 0);

            var result = account.DecreaseBalance(200);

            Assert.AreEqual(ENotification.FundsLow, result);
        }

        [Test]
        public void DecreaseBalance_by10from30_ShouldEqual20()
        {
            var account = new Account(Guid.NewGuid(), null, 30, 0, 0);

            account.DecreaseBalance(10);

            Assert.AreEqual(20, account.Balance);
        }

        [Test]
        public void IncreaseBalance_by10from30_ShouldEqual40()
        {
            var account = new Account(Guid.NewGuid(), null, 30, 0, 0);

            account.IncreaseBalance(10);

            Assert.AreEqual(40, account.Balance);
        }

        [Test]
        public void IncreasePaidIn_nonPositiveValue_ShouldThrowArgumentException()
        {
            var account = new Account(Guid.NewGuid(), null, 30, 0, 0);

            Assert.Throws(Is.TypeOf<ArgumentException>()
                    .And.Message.EqualTo("Amount must be a positive value"),
                () => account.IncreasePaidIn(-20));
        }

        [Test]
        public void IncreasePaidIn_greateThanPayInLimit_ShouldInvalidOperationException()
        {
            var account = new Account(Guid.NewGuid(), null, 30, 0, 0);

            Assert.Throws(Is.TypeOf<InvalidOperationException>()
                    .And.Message.EqualTo("Account pay in limit reached"),
                () => account.IncreasePaidIn(4001));
        }

        [Test]
        public void IncreasePaidIn_approachingPayInLimitNotification_ShouldReturnENotificationApproachingPayInLimit()
        {
            var account = new Account(Guid.NewGuid(), null, 30, 0, 0);

            var result = account.IncreasePaidIn(3600);

            Assert.AreEqual(ENotification.ApproachingPayInLimit, result);
        }

        [Test]
        public void IncreasePaidIn_by10from50_ShouldEqual60()
        {
            var account = new Account(Guid.NewGuid(), null, 30, 0, 50);

            account.IncreasePaidIn(10);

            Assert.AreEqual(60, account.PaidIn);
        }
    }
}
