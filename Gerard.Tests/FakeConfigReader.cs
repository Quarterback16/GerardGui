using Helpers.Interfaces;


namespace Gerard.Tests
{
	public class FakeConfigReader : IConfigReader
	{
		public string GetSetting( string settingKey )
		{
			//  we could fake results for all the keys, doing the minimum first
			return "1";
		}
	}
}
