using System;
using System.Collections.Generic;
using System.Text;

namespace Zegogo.connections
{
	public class message
	{
		public string id { get; set; }
		public string offer_id { get; set; }
		public string sender_id { get; set; }
		public string recipient_id { get; set; }
		public string text { get; set; }
		public string is_read { get; set; }
		public string archived { get; set; }
		public string date { get; set; }
	}
}
