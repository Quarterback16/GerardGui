using System.Data;

namespace RosterLib
{
   class TeamReport
   {
      private DataTable data;

      #region  Accessors
      
      public DataTable Data
      {
         get { return data; }
         set { data = value; }
      }

      #endregion

      private void Render( SimpleTableReport r, string header )
      {
         r.LoadBody( Data );
         r.RenderAsHtml(string.Format("{0}{1}.htm", Utility.OutputDirectory(), header), true);
      }
   }
}