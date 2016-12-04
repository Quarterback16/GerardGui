using Microsoft.VisualStudio.TestTools.UnitTesting;
using Helpers;

namespace Gerard.Tests
{
   [TestClass]
   public class GoogleChartTests
   {
      [TestMethod]
      public void TestQuickStats()
      {
         GoogleChart chart = new GoogleChart();
         chart.title = "Quick Stats";
         chart.width = 250;
         chart.height = 200;
         chart.addColumn( "string", "Year" );
         chart.addColumn( "number", "Value" );
         chart.addColumn( "number", "Profit" );
         chart.addRow( "'2014', 2000, 1000" );
         // asp literal
         var chartText = chart.generateChart( GoogleChart.ChartType.ColumnChart );
         Assert.IsTrue( chartText.Length > 0 );
      }
   }
}
