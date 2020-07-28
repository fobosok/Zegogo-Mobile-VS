using System;
using System.Collections.Generic;
using System.Text;

namespace Zegogo.connections
{
	public class filter
	{
		public string id { get; set; }
		public string name { get; set; }
		public string name_min { get; set; }
		public string name_max { get; set; }
		public string units { get; set; }
		public string key { get; set; }
		public string type { get; set; }
		public string in_search { get; set; }
		public List<opt> options { get; set; }
	}
}
