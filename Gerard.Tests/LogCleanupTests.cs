using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gerard.Tests
{
   [TestClass]
   public class LogCleanupTests
   {
      [TestMethod]
      public void TestDoLogCleanupJob()
      {
         var sut = new LogCleanupJob();
         var outcome = sut.DoJob();
         Assert.IsTrue( sut.LogDirectories.Count > 0 );
      }

      [TestMethod]
      public void TestDoLogConfigLoad()
      {
         var sut = new LogCleanupJob();
         var outcome = sut.LoadLogDirectoriesFromConfig();
         Assert.IsTrue( sut.LogDirectories.Count > 0 );
         Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
         Console.WriteLine( outcome );
      }

      [TestMethod]
      public void TestLogFileRecognition()
      {
         var sut = new LogCleanupJob();
         Assert.IsTrue( sut.IsLogFile( "log-directory1" ) );
         Assert.IsTrue( sut.IsLogFile( "log-directory2" ) );
         Assert.IsTrue( sut.IsLogFile( "log-directory01" ) );
         Assert.IsTrue( sut.IsLogFile( "log-directory-01" ) );
         Assert.IsFalse( sut.IsLogFile( "SoccerFolder" ) );
      }

   }
}
