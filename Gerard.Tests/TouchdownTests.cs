using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using RosterLib.Models;
using System;
using System.Data;
using System.Collections.Generic;

namespace Gerard.Tests
{
   [TestClass]
   public class TouchdownTests
   {
      [TestMethod]
      public void TestTouchdownInstantiation()
      {
         var td = new Touchdown();
         Assert.IsNotNull( td );
      }

      [TestMethod]
      public void TestTouchdownFactory()
      {
         var ds = Utility.TflWs.ScoresDs( "2014", String.Format( "{0:0#}", 14 ) );
         var list = new List<Touchdown>();
         foreach ( DataRow dr in ds.Tables[0].Rows )
         {
            var td = TouchdownFactory.FromDr( dr );
            if ( td != null )
               list.Add( td );
         }
         Assert.IsTrue( list.Count > 0 );
      }

      [TestMethod]
      public void TestScoreIsNotATouchdown()
      {
         Assert.IsTrue(TouchdownFactory.ScoreIsATouchdown(Constants.K_SCORE_FUMBLE_RETURN));
         Assert.IsTrue(TouchdownFactory.ScoreIsATouchdown(Constants.K_SCORE_TD_PASS));
         Assert.IsFalse(TouchdownFactory.ScoreIsATouchdown( "S" ) );

      }
   }
}
