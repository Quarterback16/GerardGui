namespace Gerard.Tests
{
   [TestClass]
   public class CompressionTests
   {
      [TestMethod]
      public void TestUnpackingTgz()
      {
		 var oututDir = ".\Extract\";
	     var inputDir = "..\Compression\";
		 ZipFile.ExtractToDirectory( inputDir + "C# Deconstructed.tgz", outputDirectory);
         Assert.IsTrue( Directory.Exists( outputDir ) );
      }

   }
}