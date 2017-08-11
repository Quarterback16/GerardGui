using RosterLib;

namespace Gerard.Tests
{
	public class FakePassUnit : PassUnit
	{
		public FakePassUnit()
		{
			Loader = new FakeLoadPassUnit();
		}
	}
}