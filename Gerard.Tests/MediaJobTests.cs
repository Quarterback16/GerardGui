using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gerard.Tests
{
   [TestClass]
   public class MediaJobTests
   {
      [TestMethod]
      public void TestMediaJob()
      {
         var sut = new MediaJob();
         sut.DoJob();
         Assert.IsTrue( sut.Candidates.Count > 0 );
      }

      [TestMethod]
      public void TestMediaJobCandidates()
      {
         var sut = new MediaJob();
         sut.GetCandidates();
         Assert.IsTrue( sut.Candidates.Count > 0 );
         Console.WriteLine( " {0} candidates found in {1}", sut.Candidates.Count, sut.DownloadFolder );
      }

      [TestMethod]
      public void TestGetDownloadFolder()
      {
         var sut = new MediaJob();
         var folder = MediaJob.GetDownloadFolder();
         Assert.IsFalse( string.IsNullOrEmpty( folder ) );
      }

      [TestMethod]
      public void TestGetMagazineFolder()
      {
         var sut = new MediaJob();
         var folder = MediaJob.GetMagazineFolder();
         Assert.IsFalse(string.IsNullOrEmpty(folder));
      }

      [TestMethod]
      public void TestGetMagazineDestinationFolder()
      {
         var sut = new MediaJob();
         var folder = MediaJob.GetMagazineDestinationFolder();
         Assert.IsFalse(string.IsNullOrEmpty(folder));
      }
   }
}
