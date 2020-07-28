using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
	public partial class Registration : ContentPage
	{
		public Registration()
		{
			InitializeComponent();
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
		private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			await Navigation.PopAsync();
		}

		private async void Button_Clicked(object sender, EventArgs e)
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
					Action = @"register",
					Params = new List<Tuple<string, string>>(),
				};
				link1.Params.Add(new Tuple<string, string>(@"access_token", oauthToken));
				link1.Params.Add(new Tuple<string, string>(@"email", reg1.Text));
				link1.Params.Add(new Tuple<string, string>(@"password", reg2.Text));
				link1.Params.Add(new Tuple<string, string>(@"password2", reg3.Text));
				string input3 = $"{link1.Controller}{link1.Action}{paramsToString20(link1.Params)}{secretKey}";
				using (MD5 md5Hash = MD5.Create())
				{
					md5temp2 = GetMd5Hash(md5Hash, input3);
				}
				HttpClient client2 = new HttpClient();
				StringBuilder sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/user/register?{paramsToString2(link1.Params)}signature={md5temp2}");
				HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
				var json5 = await response2.Content.ReadAsStringAsync();
				Debug.WriteLine(json5);

				JObject o = JObject.Parse(json5);
				var str = o.SelectToken(@"$.result");
				//var result = JsonConvert.DeserializeObject<resultat>(json5);
				if(str.ToString() == "OK")
				{
					await DisplayAlert("", "Successful", "Ok");
					await Navigation.PopAsync();
				}
			}
			catch (Exception exp)
			{
				await DisplayAlert("", "Register error, try again", "Ok");
			}
		}
	}
}