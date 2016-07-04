namespace Butler.Interfaces
{
   public interface IHoldList
	{
		bool Contains(string holdItem);
		void LoadFromXml(string xmlFile);
	}
}
