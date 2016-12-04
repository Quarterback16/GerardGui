using System;

namespace Helpers
{
   public class GoogleChart
   {

      // Fields
      private string data = "";
      private string javascript;

      // Properties
      public string elementId { get; set; }

      public int height { get; set; }

      public string title { get; set; }

      public int width { get; set; }

      // ChartTypes
      public enum ChartType
      {
         BarChart,
         PieChart,
         LineChart,
         ColumnChart,
         AreaChart,
         BubbleChart,
         CandlestickChart,
         ComboChart,
         GeoChart,
         ScatterChart,
         SteppedAreaChart,
         TableChart,
         TreeMap
      }

      // Methods
      public GoogleChart()
      {
         this.title = "Google Chart";
         this.width = 730;
         this.height = 300;
         this.elementId = "container";
      }

      public void addColumn( string type, string columnName )
      {
         string data = this.data;
         this.data = data + "  data.addColumn('" + type + "', '" + columnName + "');" + Environment.NewLine;
      }

      public void addRow( string value )
      {
         this.data = this.data + "  data.addRow([" + value + "]);" + Environment.NewLine;
      }

      public string generateChart( ChartType chart )
      {
         this.javascript = "<script Language='Javascript'>" + Environment.NewLine;
         this.javascript = this.javascript + "function drawChart() {" + Environment.NewLine;
         this.javascript = this.javascript + "  var data = new google.visualization.DataTable();" + Environment.NewLine;
         this.javascript = this.javascript + this.data + Environment.NewLine;
         this.javascript = this.javascript + "  var options = {" + Environment.NewLine;
         this.javascript = this.javascript + "    'title': '" + this.title + "'," + Environment.NewLine;
         object javascript = this.javascript;
         this.javascript = string.Concat( new object[] { javascript, "    'width': ", this.width, ", 'height': ", this.height, "};" } );
         string str = this.javascript + Environment.NewLine;
         this.javascript = str + "  var chart = new google.visualization." + chart.ToString() + "(document.getElementById('" + this.elementId + "'));" + Environment.NewLine;
         this.javascript = this.javascript + "  chart.draw(data, options);" + Environment.NewLine;
         this.javascript = this.javascript + "}" + Environment.NewLine ;
         this.javascript += "google.charts.setOnLoadCallback( drawChart );" + Environment.NewLine + "</script>" + Environment.NewLine;
         return this.javascript;
      }
   }
}
