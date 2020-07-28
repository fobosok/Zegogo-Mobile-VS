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
	public partial class myOffers : ContentPage
	{
		public myOffers(ListOffers listOffers)
		{
			InitializeComponent();
			myOffersList.ItemsSource = null;
			myOffersList.ItemsSource = listOffers.offers;
		}

		private async void ImageButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PopAsync(false);
		}

		private void myOffersList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{

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
		public string paramsToString2(List<Tuple<string, string>> tuples)
		{
			string res = string.Empty;
			foreach (Tuple<string, string> tuple in tuples)
			{
				res = res + tuple.Item1 + "=" + tuple.Item2;
				res = res + "&";
			}
			return res;
		}
		public string paramsToString20(List<Tuple<string, string>> tuples)
		{
			string res = string.Empty;
			foreach (Tuple<string, string> tuple in tuples)
			{
				res = res + tuple.Item1 + "=" + tuple.Item2;
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
			var oauthToken = await SecureStorage.GetAsync("oauth_token");
			string secretKey = @"@YyEbrv$x4lneuBGgra$4XJ{GG1}y0e~";
			var id = (((sender as Frame).Content as StackLayout).Children[2] as Label).Text;
			try
			{
				string md5temp2 = string.Empty;
				Link link1 = new Link
				{
					Main = @"https://zegogo.com/web",
					Api = @"api",
					VersionApi = @"v1",
					Controller = @"promote",
					Action = @"index",
					Params = new List<Tuple<string, string>>(),
				};
				link1.Params.Add(new Tuple<string, string>(@"access_token", oauthToken));
				link1.Params.Add(new Tuple<string, string>(@"filter[is_pack]", "0"));
				link1.Params.Add(new Tuple<string, string>(@"offer_id", id));

				string input3 = $"{link1.Controller}{link1.Action}{paramsToString20(link1.Params)}{secretKey}";
				using (MD5 md5Hash = MD5.Create())
				{
					md5temp2 = GetMd5Hash(md5Hash, input3);
				}

				HttpClient client2 = new HttpClient();
				StringBuilder sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/promote/index?{paramsToString2(link1.Params)}signature={md5temp2}");
				HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
				var json5 = await response2.Content.ReadAsStringAsync();

				Debug.WriteLine(json5);
			}
			catch (Exception exp)
			{
				await DisplayAlert("Promote error", exp.Message, "Ok");
			}
		}

		private void ImageButton_Clicked_1(object sender, EventArgs e)
		{

		}
		public async void getUserOffers()
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
					Controller = @"offer",
					Action = @"user-offers",
					Params = new List<Tuple<string, string>>(),
				};
				link1.Params.Add(new Tuple<string, string>(@"access_token=", oauthToken));
				string input3 = $"{link1.Controller}{link1.Action}{paramsToString(link1.Params)}{secretKey}";
				using (MD5 md5Hash = MD5.Create())
				{
					md5temp2 = GetMd5Hash(md5Hash, input3);
				}
				HttpClient client2 = new HttpClient();
				StringBuilder sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/offer/user-offers?access_token={oauthToken}&signature={md5temp2}");
				HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
				var json5 = await response2.Content.ReadAsStringAsync();
				var result = JsonConvert.DeserializeObject<ListOffers>(json5);
				Debug.WriteLine(json5);
				myOffersList.ItemsSource = null;
				myOffersList.ItemsSource = result.offers;
			}
			catch (Exception ex)
			{
				await DisplayAlert("Get user offers", ex.Message, "Ok");
			}

		}
		
		private async void ImageButton_Clicked_2(object sender, EventArgs e)
		{
			
			
		}

		private async void Button_Clicked(object sender, EventArgs e)
		{
			string action = await DisplayActionSheet(null, AppResources.cancel, null, AppResources.edit, AppResources.delete);
			if(action == AppResources.delete)
			{
				bool answer = await DisplayAlert(null, AppResources.are_u_sure, AppResources.yes, AppResources.no);
				if (answer == true)
				{
					var oauthToken = await SecureStorage.GetAsync("oauth_token");
					string secretKey = @"@YyEbrv$x4lneuBGgra$4XJ{GG1}y0e~";
					var id = ((((sender as Button).Parent.Parent as StackLayout).Children[0] as StackLayout).Children[2] as Label).Text;
					try
					{
						string md5temp2 = string.Empty;
						Link link1 = new Link
						{
							Main = @"https://zegogo.com/web",
							Api = @"api",
							VersionApi = @"v1",
							Controller = @"offer",
							Action = @"delete",
							Params = new List<Tuple<string, string>>(),
						};
						link1.Params.Add(new Tuple<string, string>(@"access_token", oauthToken));
						link1.Params.Add(new Tuple<string, string>(@"id", id));
						string input3 = $"{link1.Controller}{link1.Action}{paramsToString20(link1.Params)}{secretKey}";
						using (MD5 md5Hash = MD5.Create())
						{
							md5temp2 = GetMd5Hash(md5Hash, input3);
						}

						HttpClient client2 = new HttpClient();
						StringBuilder sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/offer/delete?{paramsToString2(link1.Params)}signature={md5temp2}");
						HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
						var json5 = await response2.Content.ReadAsStringAsync();
						getUserOffers();

					}
					catch (Exception exp)
					{
						await DisplayAlert("Delete error", exp.Message, "Ok");
					}
				}
			}
			else if(action == AppResources.edit)
			{
				var id = ((((sender as Button).Parent.Parent as StackLayout).Children[0] as StackLayout).Children[2] as Label).Text;
				await Navigation.PushAsync(new Page1(id), false);
			}
		}

		private async void Button_Clicked_1(object sender, EventArgs e)
		{
			try
			{
				var id = ((((sender as Button).Parent.Parent as StackLayout).Children[0] as StackLayout).Children[2] as Label).Text;
				var oauthToken = await SecureStorage.GetAsync("oauth_token");
				string secretKey = @"@YyEbrv$x4lneuBGgra$4XJ{GG1}y0e~";
				string md5temp2 = string.Empty;
				Link link1 = new Link
				{
					Main = @"https://zegogo.com/web",
					Api = @"api",
					VersionApi = @"v1",
					Controller = @"promote",
					Action = @"pay",
					Params = new List<Tuple<string, string>>(),
				};
				link1.Params.Add(new Tuple<string, string>(@"access_token", oauthToken));
				link1.Params.Add(new Tuple<string, string>(@"offer_id", "id"));
				link1.Params.Add(new Tuple<string, string>(@"services[0]", "8"));
				link1.Params.Sort();
				string input3 = $"{link1.Controller}{link1.Action}{paramsToString20(link1.Params)}{secretKey}";
				using (MD5 md5Hash = MD5.Create())
				{
					md5temp2 = GetMd5Hash(md5Hash, input3);
				}
				HttpClient client2 = new HttpClient();
				StringBuilder sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/promote/pay?{paramsToString2(link1.Params)}signature={md5temp2}");
				HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
				var json5 = await response2.Content.ReadAsStringAsync();
				var res = JsonConvert.DeserializeObject<urlRes>(json5);
				await Browser.OpenAsync(new Uri(res.pay_url), BrowserLaunchMode.SystemPreferred);
			}
			catch (Exception exp)
			{
				await DisplayAlert("Get pay error", exp.Message, "Ok");
			}
		}

		private async void Button_Clicked_2(object sender, EventArgs e)
		{
			var id = ((((sender as Button).Parent.Parent as StackLayout).Children[0] as StackLayout).Children[2] as Label).Text;
			await Navigation.PushModalAsync(new PromotePage(id), false);
		}

		private void myOffersList_ItemSelected_1(object sender, SelectedItemChangedEventArgs e)
		{
			((ListView)sender).SelectedItem = null;
		}
	}
}