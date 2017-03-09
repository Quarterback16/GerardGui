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
#if USING_DI
      private static Container container;
#endif
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
#if USING_DI
         Bootstrap();
         Application.Run( container.GetInstance<GerardForm>());
#else
         Application.Run( new GerardForm(new TimeKeeper( null ) ) );
#endif
      }

      private static void Bootstrap()
      {
#if USING_DI
         container = new Container();

         // Register your types
         container.Register<IClock, SystemClock>( Lifestyle.Singleton );
         container.Register<IKeepTheTime, TimeKeeper>( Lifestyle.Singleton );
         container.RegisterSingleton<GerardForm>();
         // Optionally verify the container.
         container.Verify();
#endif
      }
   }
}
