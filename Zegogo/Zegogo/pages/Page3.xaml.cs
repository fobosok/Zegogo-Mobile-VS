using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Zegogo.connections;

namespace Zegogo.pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Page3 : ContentPage
	{
		string secretKey = @"@YyEbrv$x4lneuBGgra$4XJ{GG1}y0e~";
		public Page3(ListOffers offers)
		{
			InitializeComponent();
			advListView.ItemsSource = null;
			advListView.ItemsSource = offers.offers;
		}
		public string paramsToString(List<Tuple<string, string>> tuples)
		{
			string res = string.Empty;
			foreach (Tuple<string, string> tuple in tuples)
			{
				res = res + tuple.Item1 + tuple.Item2;
			}
			return res;
		}
		static string GetMd5Hash(MD5 md5Hash, string input)
		{

			// Convert the input string to a byte array and compute the hash.
			byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

			// Create a new Stringbuilder to collect the bytes
			// and create a string.
			StringBuilder sBuilder = new StringBuilder();

			// Loop through each byte of the hashed data 
			// and format each one as a hexadecimal string.
			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}

			// Return the hexadecimal string.
			return sBuilder.ToString();
		}
		private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			(sender as Frame).IsEnabled = false;
			string id = (((sender as Frame).Content as StackLayout).Children[2] as Label).Text;
			try
			{
				string md5temp2 = string.Empty;
				Link link1 = new Link
				{
					Main = @"https://zegogo.com/web",
					Api = @"api",
					VersionApi = @"v1",
					Controller = @"offer",
					Action = @"index",
					Params = new List<Tuple<string, string>>(),
				};
				if (SecureStorage.GetAsync("is login").Result == "yes")
				{
					var oauthToken = await SecureStorage.GetAsync("oauth_token");
					link1.Params.Add(new Tuple<string, string>("access_token=", oauthToken));
				}
				link1.Params.Add(new Tuple<string, string>("id=", id));
				string input3 = $"{link1.Controller}{link1.Action}{paramsToString(link1.Params)}{secretKey}";
				using (MD5 md5Hash = MD5.Create())
				{
					md5temp2 = GetMd5Hash(md5Hash, input3);
				}
				HttpClient client2 = new HttpClient();
				StringBuilder sb2;
				if (SecureStorage.GetAsync("is login").Result == "yes")
				{
					var oauthToken = await SecureStorage.GetAsync("oauth_token");
					sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/offer/index?access_token={oauthToken}&id={id}&signature={md5temp2}");
				}
				else
				{
					sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/offer/index?id={id}&signature={md5temp2}");
				}
				HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
				var json5 = await response2.Content.ReadAsStringAsync();
				Debug.WriteLine(sb2);
				var res = JsonConvert.DeserializeObject<OfferRes>(json5);
				res.offer.date = DateTime.Parse(res.offer.date).ToLongDateString();
				res.offer.country_city = res.offer.country.name + ", " + res.offer.city.name;
				res.offer.id_string = "Номер обьявления: " + res.offer.id;
				if (res.offer.user.is_online == "1")
				{
					res.offer.colorLab = "#9c9c9c";
				}
				else
				{
					res.offer.colorLab = "#37A849";
				}
				foreach (var item in res.offer.options)
				{
					try
					{
						if (item.value_text.ToString()[0] == '[')
						{

							var resText = JsonConvert.DeserializeObject<string[]>(item.value_text.ToString());
							foreach (var item2 in resText)
							{
								item.value_text_string = item.value_text_string + item2 + ", ";
							}
							item.value_text_string = item.value_text_string.Substring(0, item.value_text_string.Length - 2);
							item.value_text_string = item.value_text_string + ".";
						}
						else
						{
							item.value_text_string = item.value_text.ToString();
						}

						item.value_text_string = item.value_text_string + " " + item.units;
						item.name = item.name + ":";
					}
					catch
					{

					}
				}
				if (res.offer.is_favorite == "1")
				{
					res.offer.starColor = "#BE593D";
					res.offer.Istar = "starOn";
				}
				else
				{
					res.offer.starColor = "#FFFFFF";
					res.offer.Istar = "star";
				}
				await Navigation.PushAsync(new offerPage(res.offer));
				(sender as Frame).IsEnabled = true;
			}
			catch (Exception exp)
			{
				await DisplayAlert("Get-offer", exp.Message, "Ok");
				(sender as Frame).IsEnabled = true;
			}
		}

		private async void starImg_Clicked(object sender, EventArgs e)
		{
			if (SecureStorage.GetAsync("is login").Result == "yes")
			{
				if ((sender as ImageButton).Source.ToString() == "File: star")
				{
					try
					{
						string id = (((sender as ImageButton).Parent.Parent.Parent as StackLayout).Children[2] as Label).Text;
						var oauthToken = await SecureStorage.GetAsync("oauth_token");
						string md5temp2 = string.Empty;
						Link link1 = new Link
						{
							Main = @"https://zegogo.com/web",
							Api = @"api",
							VersionApi = @"v1",
							Controller = @"favorites",
							Action = @"add",
							Params = new List<Tuple<string, string>>(),
						};
						link1.Params.Add(new Tuple<string, string>(@"access_token=", oauthToken));
						link1.Params.Add(new Tuple<string, string>(@"id=", id));
						string input3 = $"{link1.Controller}{link1.Action}{paramsToString(link1.Params)}{secretKey}";
						using (MD5 md5Hash = MD5.Create())
						{
							md5temp2 = GetMd5Hash(md5Hash, input3);
						}
						HttpClient client2 = new HttpClient();
						StringBuilder sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/favorites/add?access_token={oauthToken}&id={id}&signature={md5temp2}");
						HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
						var json5 = await response2.Content.ReadAsStringAsync();
						(sender as ImageButton).Source = "starOn";
						(sender as ImageButton).BackgroundColor = Color.FromHex("#BE593D");
						((Navigation.NavigationStack[0] as TabbedPage).Children[1] as favorites).getFav();
					}
					catch (Exception exp)
					{
						await DisplayAlert("favorit-add-error", exp.Message, "Ok");
					}
				}
				else
				{
					try
					{
						string id = (((sender as ImageButton).Parent.Parent.Parent as StackLayout).Children[2] as Label).Text;
						var oauthToken = await SecureStorage.GetAsync("oauth_token");
						string md5temp2 = string.Empty;
						Link link1 = new Link
						{
							Main = @"https://zegogo.com/web",
							Api = @"api",
							VersionApi = @"v1",
							Controller = @"favorites",
							Action = @"delete",
							Params = new List<Tuple<string, string>>(),
						};
						link1.Params.Add(new Tuple<string, string>(@"access_token=", oauthToken));
						link1.Params.Add(new Tuple<string, string>(@"id=", id));
						string input3 = $"{link1.Controller}{link1.Action}{paramsToString(link1.Params)}{secretKey}";
						using (MD5 md5Hash = MD5.Create())
						{
							md5temp2 = GetMd5Hash(md5Hash, input3);
						}
						HttpClient client2 = new HttpClient();
						StringBuilder sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/favorites/delete?access_token={oauthToken}&id={id}&signature={md5temp2}");
						HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
						var json5 = await response2.Content.ReadAsStringAsync();
						(sender as ImageButton).Source = "star";
						(sender as ImageButton).BackgroundColor = Color.FromHex("#FFFFFF");
						((Navigation.NavigationStack[0] as TabbedPage).Children[1] as favorites).getFav();
					}
					catch (Exception exp)
					{
						await DisplayAlert("favorit-del-error", exp.Message, "Ok");
					}
				}
			}
			else
			{
				(this.Parent as TabbedPage).CurrentPage = (this.Parent as TabbedPage).Children[1];
			}
		}

		private void advListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			((ListView)sender).SelectedItem = null;
		}
	}
}