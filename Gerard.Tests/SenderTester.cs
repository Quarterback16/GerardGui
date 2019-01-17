using Butler.Messaging;
using Gerard.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;

namespace Gerard.Tests
{
    [TestClass]
    public class SenderTester
    {
        private ShuttleSender _sut;

        [TestInitialize]
        public void Setup()
        {
            _sut = new ShuttleSender(
                LogManager.GetCurrentClassLogger());
        }

        [TestMethod]
        public void Sender_SendsMessageToQueue()
        {
            _sut.Send(
                new DataFixCommand
                {
                    FirstName = "Steve",
                    LastName = "Colonna",
                    TeamCode = "SF"
                });
        }
    }
}
