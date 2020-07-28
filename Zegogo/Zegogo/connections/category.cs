using System;
using System.Collections.Generic;
using System.Text;

namespace Zegogo.connections
{
	public class category
	{
		public string id { get; set; }
		public string parent_id { get; set; }
		public string icon_app { get; set; }
		public bool isSealed { get; set; }
		public bool isVisible { get; set; }
		public string name { get; set; }
	}
}
