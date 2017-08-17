using System;
using System.Collections.Generic;
using Butler.Models;
using System.Linq;

namespace Butler
{
	public class JobLog
	{
		private readonly List<Job> _jobList;
		private readonly List<string> _output;

		public JobLog(List<Job> jobList)
		{
			_jobList = jobList;
			_output = new List<string>();
		}
		public List<String> Generate()
		{
			foreach ( var job in _jobList.OrderByDescending( x => x.ElapsedTimeSpan ).ToList() )
			{
				if ( !job.OnHold()  )
				   _output.Add( $"Job: {job.Name} ran for {job.ElapsedTimeSpan}" );
			};
			return _output;
		}
	}
}
