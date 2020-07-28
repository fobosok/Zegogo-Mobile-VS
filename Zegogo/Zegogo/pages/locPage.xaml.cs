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
	public partial class locPage : ContentPage
	{
		string secretKey = @"@YyEbrv$x4lneuBGgra$4XJ{GG1}y0e~";
		string temp_settings = string.Empty;
		public locPage()
		{
			InitializeComponent();
		}
		public locPage(string settings)
		{
			InitializeComponent();
			temp_settings = settings;
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
		private async void ImageButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PopAsync(false);
		}

		private async void Entry_TextChanged(object sender, TextChangedEventArgs e)
		{
			if((sender as Entry).Text.Length>=2)
			{
				try
				{
					string temp = (sender as Entry).Text;
					string md5temp2 = string.Empty;
					var oauthToken = await SecureStorage.GetAsync("oauth_token");
					Link link1 = new Link
					{
						Main = @"https://zegogo.com/web",
						Api = @"api",
						VersionApi = @"v1",
						Controller = @"location",
						Action = @"search",
						Params = new List<Tuple<string, string>>(),
					};
					link1.Params.Add(new Tuple<string, string>(@"access_token=", oauthToken));
					link1.Params.Add(new Tuple<string, string>(@"s=", temp));
					string input3 = $"{link1.Controller}{link1.Action}{paramsToString(link1.Params)}{secretKey}";
					using (MD5 md5Hash = MD5.Create())
					{
						md5temp2 = GetMd5Hash(md5Hash, input3);
					}
					HttpClient client2 = new HttpClient();
					StringBuilder sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/location/search?access_token={oauthToken}&s={temp}&signature={md5temp2}");
					HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
					var json5 = await response2.Content.ReadAsStringAsync();
					var listLocations = JsonConvert.DeserializeObject<locations>(json5);
					listLoc.ItemsSource = null;
					listLoc.ItemsSource = listLocations.items;
				}
				catch (Exception exp)
				{
					await DisplayAlert("Location page error", exp.Message, "Ok");
				}
			}
		}

		private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			if(temp_settings == "settings")
			{
				MessagingCenter.Send(new location {text = (((sender as Frame).Content as StackLayout).Children[0] as Label).Text, city = (((sender as Frame).Content as StackLayout).Children[1] as Label).Text, country = (((sender as Frame).Content as StackLayout).Children[2] as Label).Text }, "sendLocation2settings");
				await Navigation.PopAsync();
			}
			else
			{
				List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();
				var tuple1 = new Tuple<string, string>("country", (((sender as Frame).Content as StackLayout).Children[2] as Label).Text);
				var tuple2 = new Tuple<string, string>("city", (((sender as Frame).Content as StackLayout).Children[1] as Label).Text);
				tuples.Add(tuple1);
				tuples.Add(tuple2);
				MessagingCenter.Send(tuples, "sendLocation");
				MessagingCenter.Send((((sender as Frame).Content as StackLayout).Children[0] as Label).Text, "sendLocationText");
				await Navigation.PopAsync();
			}
		}
	}
}