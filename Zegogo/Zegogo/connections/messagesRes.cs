using System;
using System.Collections.Generic;
using System.Text;

namespace Zegogo.connections
{
	public class messagesRes
	{
		public List<message> messages { get; set; }
		public user interlocutor { get; set; }
		public Offer offer { get; set; }
	}
}
