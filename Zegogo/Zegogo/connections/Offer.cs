using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Zegogo.connections
{
	public class Offer
	{
		public string id { get; set; }
		public string id_string { get; set; }
		public category category { get; set; }
		public string  offer_seek { get; set; }
		public string title { get; set; }
		public string text { get; set; }
		public string price_type { get; set; }
		public string price { get; set; }
		public string arranged_price { get; set; }
		public country country { get; set; }
		public city city { get; set; }
		public string country_city { get; set; }
		public string phone { get; set; }
		public string coords { get; set; }
		public string top { get; set; }
		public string pushup { get; set; }
		public string vip { get; set; }
		public string status { get; set; }
		public string views { get; set; }
		public string phone_views { get; set; }
		public string active_to { get; set; }
		public string deactivation_notified { get; set; }
		public string date { get; set; }
		public user user { get; set; }
		public string favorites_count { get; set; }
		public region region { get; set; }
		public string price_formated { get; set; }
		public List<photo> photos { get; set; }
		public bool isVis { get; set; }
		public bool isVisTop { get; set; }
		public DateTime dateString { get; set; }
		public List<option> options { get; set; }
		public string colorLab { get; set; }
		public string is_favorite { get; set; }
		public string Istar { get; set; }
		public string starColor { get; set; }
	}
}
