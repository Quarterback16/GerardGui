using RosterLib;

namespace Gerard.Tests
{
	public class FakeNFLPlayer : NFLPlayer
	{
		public FakeNFLPlayer(string role, string posDesc, string name)
		{
			PlayerName = name;
			PlayerRole = role;
			PlayerPos = posDesc;
		}
	}
}