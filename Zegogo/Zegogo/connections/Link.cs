using System;
using System.Collections.Generic;
using System.Text;

namespace Zegogo.connections
{
	public class Link
	{
		public string Main { get; set; }
		public string Api { get; set; }
		public string VersionApi { get; set; }
		public string Controller { get; set; }
		public string Action { get; set; }
		public List<Tuple<string, string>> Params { get; set; }
	}
}
