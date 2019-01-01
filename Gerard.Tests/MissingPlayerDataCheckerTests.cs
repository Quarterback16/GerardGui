using Gerard.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RosterLib;
using RosterLib.Interfaces;

namespace Gerard.Tests
{
    [TestClass]
    public class MissingPlayerDataCheckerTests
    {
        [TestMethod]
        public void PlayerChecker_SendsMessages()
        {
            var mockSender = new Mock<ISend>();
            var sut = new MissingPlayerDataChecker(
                mockSender.Object);
            sut.CheckPlayers("SF");
            mockSender.Verify(
                x => x.Send(It.IsAny<ICommand>()),
                Times.AtLeastOnce);
        }
    }
}
