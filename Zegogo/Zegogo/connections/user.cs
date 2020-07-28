using System;
using System.Collections.Generic;
using System.Text;

namespace Zegogo.connections
{
	public class user
	{
		public string id { get; set; }
		public string email { get; set; }
		public string avatar { get; set; }
		public string contact_person { get; set; }
		public country country { get; set; }
		public city city { get; set; }
		public string lang { get; set; }
		public string phone { get; set; }
		public string notify_phone { get; set; }
		public string cv { get; set; }
		public string role { get; set; }
		public string last_activity { get; set; }
		public string date { get; set; }
		public string big_avatar { get; set; }
		public region region { get; set; }
		public string reg_period { get; set; }
		public string is_online { get; set; }

	}
}
