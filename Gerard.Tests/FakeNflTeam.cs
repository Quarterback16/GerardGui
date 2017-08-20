using RosterLib;

namespace Gerard.Tests
{
	internal class FakeNflTeam : NflTeam
	{
		public FakeNflTeam()
		{
		}

		public FakeNflTeam(string teamCode)
		{
			TeamCode = teamCode;
		}
	}
}