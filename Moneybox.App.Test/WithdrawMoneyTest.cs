using System;
using Moneybox.App.DataAccess;
using Moneybox.App.Domain;
using Moneybox.App.Domain.Services;
using Moneybox.App.Features;
using Moq;
using NUnit.Framework;

namespace Moneybox.App.Test
{
    public class WithdrawMoneyTest
    {
        private Mock<IAccountRepository> accountRepositoryMock;
        private Mock<INotificationService> notificationServiceMock;

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

            var withdrawMoney = new WithdrawMoney(accountRepositoryMock.Object, notificationServiceMock.Object);

            Assert.Throws(Is.TypeOf<InvalidOperationException>()
                    .And.Message.EqualTo("Insufficient funds to make transfer"),
                () => withdrawMoney.Execute(amountLargerThanBalanceGuid, 20));
        }
    }
}