using RosterLib;
using RosterLib.Helpers;
using RosterLib.Interfaces;
using SimpleInjector;
using System;
using System.Windows.Forms;

namespace GerardGui
{
    static class Program
    {
      private static Container container;
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Bootstrap();
         Application.Run( container.GetInstance<GerardForm>());
      }

      private static void Bootstrap()
      {
         container = new Container();

         // Register your types
         container.Register<IClock, SystemClock>( Lifestyle.Singleton );
         container.Register<IKeepTheTime, TimeKeeper>( Lifestyle.Singleton );
         container.RegisterSingleton<GerardForm>();
         // Optionally verify the container.
         container.Verify();
      }
   }
}
