using System;
using Moneybox.App.DataAccess;
using Moneybox.App.Domain;
using Moneybox.App.Domain.Services;
using Moneybox.App.Features;
using Moq;
using NUnit.Framework;

namespace Moneybox.App.Test
{
    public class TransferMoneyTest
    {
        private Mock<IAccountRepository> accountRepositoryMock;
        private Mock<INotificationService> notificationServiceMock;

        private TransferMoney transferMoney;

        [SetUp]
        public void Setup()
        {
            accountRepositoryMock = new Mock<IAccountRepository>();
            notificationServiceMock = new Mock<INotificationService>();
        }

        [Test]
        public void Execute_amountLargerThanBalance_ShouldThrowInvalidOperationException()
        {
            var amountLargerThanBalanceGuid = Guid.Parse("D878AF17-2D27-45D5-96E4-4CE8562ED72D");
            accountRepositoryMock.Setup(x => x.GetAccountById(amountLargerThanBalanceGuid)).Returns(new Account(amountLargerThanBalanceGuid, null, 10, 0, 0));

            transferMoney = new TransferMoney(accountRepositoryMock.Object, notificationServiceMock.Object);

            Assert.Throws(Is.TypeOf<InvalidOperationException>()
                    .And.Message.EqualTo("Insufficient funds to make transfer"),
                () => transferMoney.Execute(amountLargerThanBalanceGuid, amountLargerThanBalanceGuid, 20));
        }

        [Test]
        public void Execute_amountLargerThanPayInLimit_ShouldThrowInvalidOperationException()
        {
            var amountLargerThanPayInLimitGuid = Guid.Parse("99FD19E8-DA39-4B70-B98A-AB1E96CAE7EC");
            accountRepositoryMock.Setup(x => x.GetAccountById(amountLargerThanPayInLimitGuid)).Returns(new Account(amountLargerThanPayInLimitGuid, null, 10000, 0, 3981));

            transferMoney = new TransferMoney(accountRepositoryMock.Object, notificationServiceMock.Object);

            Assert.Throws(Is.TypeOf<InvalidOperationException>()
                    .And.Message.EqualTo("Account pay in limit reached"),
                () => transferMoney.Execute(amountLargerThanPayInLimitGuid, amountLargerThanPayInLimitGuid, 20));
        }

    }
}