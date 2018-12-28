using Gerard.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
    [TestClass]
    public class MissingPlayerDataCheckerTests
    {
        [TestMethod]
        public void PlayerChecker_ChecksDob()
        {
            var sut = new MissingPlayerDataChecker(
                new FakeSender());
            sut.CheckPlayers("SF");
        }
    }
}
