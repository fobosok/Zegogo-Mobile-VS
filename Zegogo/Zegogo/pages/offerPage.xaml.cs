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
using Zegogo.Resources;

namespace Zegogo.pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class offerPage : ContentPage
	{
		Offer Offer;
		string myId;
		string myAvatar;
		string secretKey = @"@YyEbrv$x4lneuBGgra$4XJ{GG1}y0e~";
		public offerPage(Offer offer)
		{
			InitializeComponent();
			getProfile();
			this.Offer = offer;
			this.BindingContext = offer;
		}

		private void Button_Clicked(object sender, EventArgs e)
		{
			DisplayAlert("1", Offer.category.id, "1");
		}

		private async void ImageButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PopAsync();
		}

		//private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		//{
		//	textOffer.MaxLines = -1;
		//	showBut.HeightRequest = 0;
		//	showBut.IsVisible = false;
		//}
		public async void getProfile()
		{
			try
			{
				string secretKey = @"@YyEbrv$x4lneuBGgra$4XJ{GG1}y0e~";
				var oauthToken = await SecureStorage.GetAsync("oauth_token");
				string md5temp2 = string.Empty;
				Link link1 = new Link
				{
					Main = @"https://zegogo.com/web",
					Api = @"api",
					VersionApi = @"v1",
					Controller = @"user",
					Action = @"index",
					Params = new List<Tuple<string, string>>(),
				};
				link1.Params.Add(new Tuple<string, string>(@"access_token=", oauthToken));
				string input3 = $"{link1.Controller}{link1.Action}{paramsToString(link1.Params)}{secretKey}";
				using (MD5 md5Hash = MD5.Create())
				{
					md5temp2 = GetMd5Hash(md5Hash, input3);
				}
				HttpClient client2 = new HttpClient();
				StringBuilder sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/user/index?access_token={oauthToken}&signature={md5temp2}");
				HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
				var json5 = await response2.Content.ReadAsStringAsync();
				var result = JsonConvert.DeserializeObject<user>(json5);
				this.myId = result.id;
				this.myAvatar = result.avatar;

			}
			catch (Exception ex)
			{
				await DisplayAlert("Get-myId-Error", ex.Message, "Ok");
			}
		}
		private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
		{
			try
			{
				string id = Offer.id;
				var oauthToken = await SecureStorage.GetAsync("oauth_token");
				string md5temp2 = string.Empty;
				Link link1 = new Link
				{
					Main = @"https://zegogo.com/web",
					Api = @"api",
					VersionApi = @"v1",
					Controller = @"offer",
					Action = @"other",
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
				StringBuilder sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/offer/other?access_token={oauthToken}&id={id}&signature={md5temp2}");
				HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
				var json5 = await response2.Content.ReadAsStringAsync();
				Debug.WriteLine(json5);
				var offers = JsonConvert.DeserializeObject<ListOffers>(json5);
				foreach (var item in offers.offers)
				{
					if (item.is_favorite == "0")
					{
						item.Istar = "star";
						item.starColor = "#FFFFFF";
					}
					else
					{
						item.Istar = "starOn";
						item.starColor = "#BE593D";
					}
				}
				
				await Navigation.PushAsync(new Page2(offers),false);
			}
			catch (Exception exp)
			{
				await DisplayAlert("other-offer-error", exp.Message, "Ok");
			}
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
		private async void ImageButton_Clicked_1(object sender, EventArgs e)
		{
			if (((sender as Frame).Content as Image).Source.ToString() == "File: star")
			{
				try
				{
					string id = Offer.id;
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
					((sender as Frame).Content as Image).Source = "starOn";
					(sender as Frame).BackgroundColor = Color.FromHex("#BE593D");
					((Navigation.NavigationStack[0] as TabbedPage).Children[1] as favorites).getFav();
					((Navigation.NavigationStack[0] as TabbedPage).Children[0] as advertisement).getVip();
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
					string id = Offer.id;
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
					((sender as Frame).Content as Image).Source = "star";
					(sender as Frame).BackgroundColor = Color.FromHex("#FFFFFF");
					((Navigation.NavigationStack[0] as TabbedPage).Children[1] as favorites).getFav();
					((Navigation.NavigationStack[0] as TabbedPage).Children[0] as advertisement).getVip();
				}
				catch (Exception exp)
				{
					await DisplayAlert("favorit-del-error", exp.Message, "Ok");
				}
			}

		}

		private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			try
			{
				string id = Offer.id;
				var oauthToken = await SecureStorage.GetAsync("oauth_token");
				string md5temp2 = string.Empty;
				Link link1 = new Link
				{
					Main = @"https://zegogo.com/web",
					Api = @"api",
					VersionApi = @"v1",
					Controller = @"offer",
					Action = @"similar",
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
				StringBuilder sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/offer/similar?access_token={oauthToken}&id={id}&signature={md5temp2}");
				HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
				var json5 = await response2.Content.ReadAsStringAsync();
				Debug.WriteLine(json5);
				var offers = JsonConvert.DeserializeObject<ListOffers>(json5);
				foreach (var item in offers.offers)
				{
					if (item.is_favorite == "0")
					{
						item.Istar = "star";
						item.starColor = "#FFFFFF";
					}
					else
					{
						item.Istar = "starOn";
						item.starColor = "#BE593D";
					}
				}

				await Navigation.PushAsync(new Page3(offers), false);
			}
			catch (Exception exp)
			{
				await DisplayAlert("other-offer-error", exp.Message, "Ok");
			}
		}

		private async void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
		{
			try
			{
				string action = await DisplayActionSheet(Offer.user.phone, null, null, AppResources.call, AppResources.write_sms);
				if (action == AppResources.call)
				{
					PhoneDialer.Open(Offer.user.phone);
				}
			}
			catch
			{
				await DisplayAlert(null, AppResources.user_did_not_provide, AppResources.ok);
			}
		}

		private async void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
		{
			try
			{
				string md5temp2 = string.Empty;
				Link link1 = new Link
				{
					Main = @"https://zegogo.com/web",
					Api = @"api",
					VersionApi = @"v1",
					Controller = @"messages",
					Action = @"send",
					Params = new List<Tuple<string, string>>(),
				};
				string json5;
				if (SecureStorage.GetAsync("is login").Result == "yes")
				{
					var oauthToken = await SecureStorage.GetAsync("oauth_token");
					link1.Params.Add(new Tuple<string, string>(@"access_token=", oauthToken));
					link1.Params.Add(new Tuple<string, string>(@"interlocutor_id=", Offer.user.id));
					link1.Params.Add(new Tuple<string, string>(@"offer_id=", Offer.id));
					link1.Params.Add(new Tuple<string, string>(@"text=", mesEntry.Text));
					var input3 = $"{link1.Controller}{link1.Action}{paramsToString(link1.Params)}{secretKey}";
					using (MD5 md5Hash = MD5.Create())
					{
						md5temp2 = GetMd5Hash(md5Hash, input3);
					}
					HttpClient client2 = new HttpClient();
					var sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/messages/send?access_token={oauthToken}&interlocutor_id={Offer.user.id}&offer_id={Offer.id}&text={mesEntry.Text}&signature={md5temp2}");
					HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
					json5 = await response2.Content.ReadAsStringAsync();
					Debug.WriteLine("888" + json5);
					await Navigation.PushModalAsync(new Page4(Offer.id, Offer.user.id, Offer.user.email, myId, myAvatar), false);
				}
				else
				{

				}


			}
			catch (Exception exp)
			{
				await DisplayAlert("get-mess-error", exp.Message, "Ok");
			}
		}
	}
}