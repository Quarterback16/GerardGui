using Butler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gerard.Tests
{
    [TestClass]
    public class HomeLocatorTests
    {
        [TestMethod]
        public void HomeLocator_OnInstantiationWithNoParameter_DefaultsToRegina()
        {
            var cut = new HomeLocator();
            Assert.IsTrue(
                cut.HomeFolder.Equals(@"\\\\Regina\books\"),
                $"Actual home directory is {cut.HomeFolder}");
        }

        [TestMethod]
        public void HomeLocator_OnInstantiation_LoadsFolders()
        {
            var cut = new HomeLocator(@".\Test_IT_folder\");
            Assert.IsTrue(cut.ITFolderCollection.Count > 0);
        }

        [TestMethod]
        public void HomeLocator_ReginaOnInstantiation_LoadsITFolders()
        {
            var cut = new HomeLocator();
            Assert.IsTrue(cut.ITFolderCollection.Count > 60);
        }

        [TestMethod]
        public void HomeLocator_OnInstantiation_ContainsBitcoin()
        {
            var cut = new HomeLocator(@".\Test_IT_folder\");
            var testString = "Bitcoin";
            Assert.IsTrue( cut.ITFolderCollection.Contains(testString) );
        }

        [TestMethod]
        public void HomeFor_BitcoinedExplained_ResultsInBitcoin()
        {
            var cut = new HomeLocator(@".\Test_IT_folder\");
            Assert.AreEqual(
                expected: $@"{cut.HomeFolder}IT\Bitcoin\Bitcoin Explained.epub",
                actual: cut.HomeFor("Bitcoin Explained.epub"));
        }

        [TestMethod]
        public void HomeLocator_ForRegina_DoesNotInclude_S()
        {
            var cut = new HomeLocator();
            var testString = "S";
            Assert.IsFalse(cut.ITFolderCollection.Contains(testString));
        }

        [TestMethod]
        public void HomeFor_SkillsSoGood_ResultsInNoHome()
        {
            var cut = new HomeLocator();
            Assert.AreEqual(expected: string.Empty,
                actual: cut.HomeFor("So Good They Cant Ignore You.epub"));
        }

        [TestMethod]
        public void HomeFor_SimplyNigella_ResultsCookingHome()
        {
            var cut = new HomeLocator();
            Assert.AreEqual(
                expected: $@"{cut.HomeFolder}Cooking\Simply Nigella.pdf",
                actual: cut.HomeFor("Simply Nigella.pdf"));
        }

        [TestMethod]
        public void HomeFor_SherlockHolmes_ResultsSherlockHomes()
        {
            var fileName = "The Sherlock Holmes Book Big Ideas.tgz";
            var cut = new HomeLocator();
            Assert.AreEqual(
                expected: $@"{cut.HomeFolder}Sherlock Holmes\{fileName}",
                actual: cut.HomeFor(fileName));
        }

        [TestMethod]
        public void HomeFor_BuildingMaintainableSoftware_ResultsCSharp()
        {
            var fileName = "Building Maintainable Software, C# Edition.pdf";
            var cut = new HomeLocator();
            Assert.AreEqual(
                expected: $@"{cut.HomeFolder}IT\C#\{fileName}",
                actual: cut.HomeFor(fileName));
        }

        [TestMethod]
        public void HomeFor_Price_ResultsCSharp()
        {
            var fileName = "Price M. C# 9 and .NET 5.  Modern Cross-Platform Development 5ed 2020.pdf";
            var cut = new HomeLocator();
            Assert.AreEqual(
                expected: $@"{cut.HomeFolder}IT\C#\{fileName}",
                actual: cut.HomeFor(fileName));
        }
    }
}
