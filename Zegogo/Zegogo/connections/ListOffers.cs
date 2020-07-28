using System;
using System.Collections.Generic;
using System.Text;

namespace Zegogo.connections
{
	public class ListOffers
	{
		public List<Offer> offers_top { get; set; }
		public List<Offer> offers { get; set; }
		public string pages_count { get; set; }
	}
}
