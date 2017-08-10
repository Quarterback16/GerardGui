using RosterLib;

namespace Gerard.Tests
{
	public class FakeNFLPlayer : NFLPlayer
	{
		public FakeNFLPlayer(
			string id, string role, string posDesc, string name,
			string injury = "0" )
		{
			PlayerCode = id;
			PlayerName = name;
			PlayerRole = role;
			PlayerPos = posDesc;
			Injury = injury;
		}
	}
}