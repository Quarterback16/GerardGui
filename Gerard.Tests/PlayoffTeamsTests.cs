using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;

namespace Gerard.Tests
{
   [TestClass]
   public class PlayoffTeamsTests
   {
      [TestMethod]
      public void TestDoPlayoffTeamsJob()  //   
      {
         var sut = new PlayOffTeamsJob(new TimeKeeper());
         var outcome = sut.DoJob();
         Assert.IsFalse(string.IsNullOrEmpty(outcome));
      }
   }
}
