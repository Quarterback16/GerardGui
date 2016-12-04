using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using Helpers;
using System.IO;

namespace Gerard.Tests
{
   [TestClass]
   public class HtmlFileTests
   {
      [TestMethod]
      public void EmptyFileTest()
      {
         var testFile = ".\\Output\\test1.html";
         var cut = new HtmlFile( testFile,"Test Html File" );
         cut.AddTopScript( "src='https://www.gstatic.com/charts/loader.js'" );
         cut.AddTopScript( "src='https://www.google.com/jsapi'" );
         cut.AddTopScript( "google.charts.load('current', {packages: ['treemap']});" );
         var chart = new GoogleChart();
         chart.title = "Quick Stats";
         chart.width = 250;
         chart.height = 200;
         chart.addColumn( "string", "Year" );
         chart.addColumn( "number", "Value" );
         chart.addColumn( "number", "Profit" );
         chart.addRow( "'2014', 2000, 1000" );
         var gcText = chart.generateChart( GoogleChart.ChartType.TreeMap );

         cut.AddScript( gcText );
         cut.Render();
         Assert.IsTrue( File.Exists( testFile ) );

      }
   }
}
