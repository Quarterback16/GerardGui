using RosterLib;
using RosterLib.Interfaces;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;

namespace GerardGui
{
   public partial class GerardForm : Form
   {
      public string LastMessage { get; set; }

      private readonly IKeepTheTime TimeKeeper;

      public GerardForm(IKeepTheTime timeKeeper )
      {
         TimeKeeper = timeKeeper;
         InitializeComponent();
         backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
         backgroundWorker1.DoWork += backgroundWorker1_DoWork;
         var versionInfo = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
         var startDate = new DateTime( 2000, 1, 1 );
         var diffDays = versionInfo.Build;
         var computedDate = startDate.AddDays( diffDays );
         Text = string.Format( "Gerard the Butler {0} g built {1}", versionInfo.ToString(), computedDate.ToLongDateString() );
         PreFlightTests();
      }

      private void PreFlightTests()
      {
         CheckSetting( "PeakStartHour" );
         CheckSetting( "PeakFinishHour" );
         CheckSetting( "TvFolder" );
         CheckSetting( "ViewQueueFolder");
         CheckSetting( "DownloadFolder");
         CheckSetting( "NflFolder" );
         CheckSetting( "SoccerFolder" );
         CheckDirExists( "output", Utility.OutputDirectory() );
         CheckDirExists("xml", Utility.XmlDirectory());
      }

      private void CheckSetting( string setting )
      {
         var theConfigSetting = ConfigurationManager.AppSettings.Get( setting );
         InsertMessage( ! string.IsNullOrEmpty( theConfigSetting )
                           ? string.Format( "{0} set to {1}", setting, theConfigSetting )
                           : string.Format( "{0} does not exist", setting ) );
      }

      private void CheckDirExists(string setting, string dir)
      {
         InsertMessage(System.IO.Directory.Exists(dir)
                           ? string.Format("{0} set to {1}", setting, dir)
                           : string.Format("{0} does not exist", dir));
      }

      private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
      {
         var helperBW = sender as BackgroundWorker;
         var rs = new Butler.Butler(Text, this.TimeKeeper )
            {
               Pollinterval = Int32.Parse(ConfigurationManager.AppSettings["PollInterval"]),
               Verbose = ConfigurationManager.AppSettings["Verbose"] == "true",
            };
         rs.Go(helperBW, e);
      }

      private void button1_Click(object sender, EventArgs e)
      {
         try
         {
            button1.Enabled = false;
            label1.Text = string.Empty;
            startDateLabel.Text += DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();

            if (backgroundWorker1.IsBusy != true)
            {
               backgroundWorker1.RunWorkerAsync();
            }
         }
         catch (Exception ex)
         {
            textBox1.Text += ex.StackTrace;
            throw;
         }
      }

      private void backgroundWorker1_ProgressChanged(object sender,
          ProgressChangedEventArgs e)
      {
         var msg = e.UserState.ToString();
	      if (msg == LastMessage) return;

	      label1.Text = msg;
	      if (e.ProgressPercentage.Equals(99))
		      InsertMessage(msg);
	      LastMessage = msg;
      }

      private void InsertMessage(string msg)
      {
         textBox1.Text = string.Format( "{0} {1}", DateTime.Now.ToString("hh:mm"), msg ) + Environment.NewLine + textBox1.Text;
      }

      private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
         if (e.Cancelled)
         {
            textBox1.Text += "The task has been cancelled";
         }
         else if (e.Error != null)
         {
            textBox1.Text += "Error. Details: " + (e.Error as Exception).ToString();
         }
         else
         {
            textBox1.Text += "The task has been completed. Results: " + e.Result.ToString();
         }
      }
   }
}