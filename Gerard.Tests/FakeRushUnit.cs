using RosterLib;

namespace Gerard.Tests
{
	public class FakeRushUnit : RushUnit
	{
		public FakeRushUnit()
		{
			Loader = new FakeLoadRunners();
		}
	}
}