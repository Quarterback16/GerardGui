using System;
using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gerard.Tests
{
   [TestClass]
   public class StrengthOfScheduleTests
   {
      [TestMethod]
      public void TestStrengthOfScheduleJob()
      {
         var sut = new StrengthOfScheduleJob( new FakeTimeKeeper() );
         var outcome = sut.DoJob();
         Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
      }

      [TestMethod]
      public void TestTimetoDoStengthOfSchedule()
      {
         var sut = new StrengthOfScheduleJob( new FakeTimeKeeper() );
         string whyNot;
         Assert.IsTrue( sut.IsTimeTodo( out whyNot ) );
         Console.WriteLine( "Final:Reason for not doing>{0}", whyNot );
      }
   }
}