using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Butler.ViewModels;

namespace Butler.Helpers
{
	public class DiskDetector
	{
		#region  Construction

		public DiskSpaceViewModel DisksView { get; set; }

		public DiskDetector()
		{
			DisksView = DiskSpaceCheck();
		}

		public static DiskSpaceViewModel DiskSpaceCheck()
		{
			var vm = new DiskSpaceViewModel { Disks = new List<DiskDiagnostic>() };

			var drives = DriveInfo.GetDrives();

			foreach ( var drive in drives )
			{
				var disk = new DiskDiagnostic { IsAvailable = true };
				try
				{
					double fspc = drive.TotalFreeSpace;
					double tspc = drive.TotalSize;
					double percent = ( fspc / tspc );
					float num = (float) percent;

					disk.DriveType = String.Format( "Type: {0}", drive.DriveType );
					disk.Name = drive.Name;
					disk.Info = String.Format( "{0} has {1:p} free", drive.Name, num );
					disk.AvailableFreeSpace = String.Format( "Space Remaining    : {0}", FormatBytes( drive.AvailableFreeSpace ) );
					disk.SpaceUsed = String.Format( "Space used         : {0}", FormatBytes( drive.TotalSize ) );
					disk.PercentFreeSpace = String.Format( "Percent Free Space : {0:p}", percent );
					vm.Disks.Add( disk );
				}
				catch ( Exception ex )
				{
					disk.Info = string.Format( "{0} - {1} ", drive.Name, ex.Message );
					disk.Name = string.Format( "X{0}X", drive.Name );
					disk.IsAvailable = false;
					vm.Disks.Add( disk );
				}
			}
			return vm;
		}

		private static string FormatBytes( long bytes )
		{
			string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
			var i = 0;
			double dblSByte = bytes;
			if ( bytes > 1024 )
				for ( i = 0; ( bytes / 1024 ) > 0; i++, bytes /= 1024 )
					dblSByte = bytes / 1024.0;
			return String.Format( "{0:0.##} {1}", dblSByte, Suffix[ i ] );
		}

		#endregion

		public bool IsDiskAvailable( string diskid )
		{
			return DisksView.Disks.Where( disk => disk.Name.Equals( diskid ) ).Any( disk => disk.IsAvailable );
		}

		public string DiskIdentifiers()
		{
			var ids = string.Empty;
			var vm = DiskSpaceCheck();
			//return vm.Disks.Aggregate( ids, ( current, d ) => current + ( d.Name + ", " ) );
			ids = vm.Disks.Where( d => d.IsAvailable ).Aggregate( ids, ( current, d ) => current + ( d.Name + ", " ) );
			return ids.Length > 2 ? ids.Remove(ids.Length - 2, 2) : ids;
		}
	}
}
