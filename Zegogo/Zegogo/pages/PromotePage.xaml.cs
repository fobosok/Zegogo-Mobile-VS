using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
	public partial class PromotePage : ContentPage
	{
		string secretKey = @"@YyEbrv$x4lneuBGgra$4XJ{GG1}y0e~";
		double price = 0;
		double price2 = 0;
		double price3 = 0;
		bool pr1 = false;
		bool pr2 = false;
		bool pr3 = false;
		bool pr4 = false;
		bool pr5 = false;
		bool pr6 = false;
		string idOffer;
		public PromotePage(offerAddResult result)
		{
			InitializeComponent();
			idOffer = result.offer_id;
			IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
			price3 = double.Parse(result.price, formatter);
			payBut.Text = $"{AppResources.To_pay}({price3})";
		}
		public PromotePage(string id)
		{
			InitializeComponent();
			idOffer = id;
		}
		private async void ImageButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PopModalAsync(false);
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
		public string paramsToString20(List<Tuple<string, string>> tuples)
		{
			string res = string.Empty;
			foreach (Tuple<string, string> tuple in tuples)
			{
				res = res + tuple.Item1 + "=" + tuple.Item2;
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

		private async void Button_Clicked(object sender, EventArgs e)
		{
			try
			{
				var oauthToken = await SecureStorage.GetAsync("oauth_token");
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
				link1.Params.Add(new Tuple<string, string>(@"offer_id", idOffer));
				var tempList = new List<Tuple<string, string>>();
				if(pr1 == true)
				{
					tempList.Add(new Tuple<string, string>(@"services[]", "1"));
				}
				else if(pr2 == true)
				{
					tempList.Add(new Tuple<string, string>(@"services[]", "2"));
				}
				else if(pr3 == true)
				{
					tempList.Add(new Tuple<string, string>(@"services[]", "3"));
				}
				if(pr4 == true)
				{
					tempList.Add(new Tuple<string, string>(@"services[]", "4"));
				}
				if(pr5 == true)
				{
					tempList.Add(new Tuple<string, string>(@"services[]", "6"));
				}
				if(pr6 == true)
				{
					tempList.Add(new Tuple<string, string>(@"services[]", "7"));
				}
				var tempList2 = new List<Tuple<string, string>>();
				for (int i = 0; i < tempList.Count; i++)
				{
					tempList2.Add(new Tuple<string, string>(tempList[i].Item1.Replace("[]", $"[{i}]"), tempList[i].Item2));
				}
				link1.Params.AddRange(tempList2);
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
				await Navigation.PopModalAsync(false);
			}
			catch
			{
				await Navigation.PopModalAsync(false);
			}
		}


		private void Button_Clicked_1(object sender, EventArgs e)
		{

		}

		private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			if(pr1 == false)
			{
				(sender as Frame).BorderColor = Color.FromHex("#BE593D");
				((sender as Frame).Content as Frame).BackgroundColor = Color.FromHex("#FFF9F8");
				pr1 = true;
				pr2 = false;
				pr3 = false;
				prFrame2.BorderColor = Color.LightGray;
				(prFrame2.Content as Frame).BackgroundColor = Color.White;
				prFrame3.BorderColor = Color.LightGray;
				(prFrame3.Content as Frame).BackgroundColor = Color.White;
				price = 1.99;
				
			}
			else
			{
				(sender as Frame).BorderColor = Color.LightGray; ;
				((sender as Frame).Content as Frame).BackgroundColor = Color.White;
				pr1 = false;
				pr2 = false;
				pr3 = false;
				prFrame2.BorderColor = Color.LightGray;
				(prFrame2.Content as Frame).BackgroundColor = Color.White;
				prFrame3.BorderColor = Color.LightGray;
				(prFrame3.Content as Frame).BackgroundColor = Color.White;
				price = 0;
			}
			payBut.Text = $"{AppResources.To_pay}({price + price2 + price3} Euro)";
		}

		private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
		{
			if(pr2 == false)
			{
				(sender as Frame).BorderColor = Color.FromHex("#BE593D");
				((sender as Frame).Content as Frame).BackgroundColor = Color.FromHex("#FFF9F8");
				pr1 = false;
				pr2 = true;
				pr3 = false;
				prFrame1.BorderColor = Color.LightGray;
				(prFrame1.Content as Frame).BackgroundColor = Color.White;
				prFrame3.BorderColor = Color.LightGray;
				(prFrame3.Content as Frame).BackgroundColor = Color.White;
				price = 7.99;
			}
			else
			{
				(sender as Frame).BorderColor = Color.LightGray; ;
				((sender as Frame).Content as Frame).BackgroundColor = Color.White;
				pr1 = false;
				pr2 = false;
				pr3 = false;
				prFrame1.BorderColor = Color.LightGray;
				(prFrame1.Content as Frame).BackgroundColor = Color.White;
				prFrame3.BorderColor = Color.LightGray;
				(prFrame3.Content as Frame).BackgroundColor = Color.White;
				price = 0;
			}
			payBut.Text = $"{AppResources.To_pay}({price + price2 + price3} Euro)";
		}

		private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
		{
			if(pr3 == false)
			{
				(sender as Frame).BorderColor = Color.FromHex("#BE593D");
				((sender as Frame).Content as Frame).BackgroundColor = Color.FromHex("#FFF9F8");
				pr1 = false;
				pr2 = false;
				pr3 = true;
				prFrame1.BorderColor = Color.LightGray;
				(prFrame1.Content as Frame).BackgroundColor = Color.White;
				prFrame2.BorderColor = Color.LightGray;
				(prFrame2.Content as Frame).BackgroundColor = Color.White;
				price = 59.99;
			}
			else
			{
				pr1 = false;
				pr2 = false;
				pr3 = false;
				(sender as Frame).BorderColor = Color.LightGray; ;
				((sender as Frame).Content as Frame).BackgroundColor = Color.White;
				prFrame1.BorderColor = Color.LightGray;
				(prFrame1.Content as Frame).BackgroundColor = Color.White;
				prFrame2.BorderColor = Color.LightGray;
				(prFrame2.Content as Frame).BackgroundColor = Color.White;
				price = 0;
			}
			payBut.Text = $"{AppResources.To_pay}({price + price2 + price3} Euro)";
		}

		private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
		{
			if (pr4 == false)
			{
				(sender as Frame).BorderColor = Color.FromHex("#BE593D");
				((sender as Frame).Content as Frame).BackgroundColor = Color.FromHex("#FFF9F8");
				pr4 = true;
				price2 = price2 + 5.99;
			}
			else
			{
				(sender as Frame).BorderColor = Color.LightGray; ;
				((sender as Frame).Content as Frame).BackgroundColor = Color.White;
				pr4 = false;
				price2 = price2 - 5.99;
			}
			payBut.Text = $"{AppResources.To_pay}({price + price2 + price3} Euro)";
		}

		private void TapGestureRecognizer_Tapped_4(object sender, EventArgs e)
		{
			if (pr5==false)
			{
				(sender as Frame).BorderColor = Color.FromHex("#BE593D");
				((sender as Frame).Content as Frame).BackgroundColor = Color.FromHex("#FFF9F8");
				pr5 = true;
				price2 = price2 + 3.99;
			}
			else
			{
				(sender as Frame).BorderColor = Color.LightGray; ;
				((sender as Frame).Content as Frame).BackgroundColor = Color.White;
				pr5 = false;
				price2 = price2 - 3.99;
			}
			payBut.Text = $"{AppResources.To_pay}({price + price2 + price3} Euro)";
		}

		private void TapGestureRecognizer_Tapped_5(object sender, EventArgs e)
		{
			if (pr6 == false)
			{
				(sender as Frame).BorderColor = Color.FromHex("#BE593D");
				((sender as Frame).Content as Frame).BackgroundColor = Color.FromHex("#FFF9F8");
				pr6 = true;
				price2 = price2 + 19.99;
			}
			else
			{
				(sender as Frame).BorderColor = Color.LightGray; ;
				((sender as Frame).Content as Frame).BackgroundColor = Color.White;
				pr6 = false;
				price2 = price2 - 19.99;
			}
			payBut.Text = $"{AppResources.To_pay}({price + price2 + price3} Euro)";
		}
	}
}