using System;
using System.Collections.Generic;
using System.Text;

namespace Zegogo.connections
{
	public class option
	{
		public string key { get; set; }
		public string type { get; set; }
		public string name { get; set; }
		public string units { get; set; }
		public object value_text { get; set; }
		public string value_text_string { get; set; }
		public object value_key { get; set; }
		public string min { get; set; }
		public string max { get; set; }
		public string period { get; set; }
	}
}
