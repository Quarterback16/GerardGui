using Butler.Models;
using System;

namespace Gerard.Tests.Fakes
{
	public class FakeJob : Job
	{
		private readonly bool _onHold;

		public FakeJob()
		{
			Name = "Fake Job";
			ElapsedTimeSpan = new TimeSpan( 1, 0, 0 );
			_onHold = false;
		}

		public FakeJob(string name, TimeSpan timeSpan)
		{
			Name = name;
			ElapsedTimeSpan = timeSpan;
			_onHold = false;
		}

		public FakeJob( string name, TimeSpan timeSpan, bool onHold )
		{
			Name = name;
			ElapsedTimeSpan = timeSpan;
			_onHold = onHold;
		}

		public override bool OnHold()
		{
			return _onHold;
		}
	}
}
