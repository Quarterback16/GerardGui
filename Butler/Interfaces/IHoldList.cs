using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Butler.Interfaces
{
	public interface IHoldList
	{
		bool Contains(string holdItem);
		void LoadFromXml(string xmlFile);
	}
}
