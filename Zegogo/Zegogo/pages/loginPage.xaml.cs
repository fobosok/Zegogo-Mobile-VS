using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
	public partial class loginPage : ContentPage
	{
		private static Random random = new Random();
		string secretKey = @"@YyEbrv$x4lneuBGgra$4XJ{GG1}y0e~";
		public string pass = null;
		public loginPage()
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
		private async void Button_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new Registration());
		}

		private async void Button_Clicked_1(object sender, EventArgs e)
		{
			try
			{
				string login = username.Text;
				string sData = pass;

				string rand1 = random.NextString(5);
				string rand2 = random.NextString(5);

				byte[] bytes = Encoding.ASCII.GetBytes(rand2 + sData);
				var temp = Convert.ToBase64String(bytes);
				string hashPass = rand1 + temp;

				string md5temp2 = string.Empty;
				Link link1 = new Link
				{
					Main = @"https://zegogo.com/web",
					Api = @"api",
					VersionApi = @"v1",
					Controller = @"user",
					Action = @"login",
					Params = new List<Tuple<string, string>>(),
				};
				link1.Params.Add(new Tuple<string, string>(@"password=", hashPass));
				link1.Params.Add(new Tuple<string, string>(@"username=", login));
				link1.Params.Sort();
				string input3 = $"{link1.Controller}{link1.Action}{paramsToString(link1.Params)}{secretKey}";
				using (MD5 md5Hash = MD5.Create())
				{
					md5temp2 = GetMd5Hash(md5Hash, input3);
				}
				HttpClient client2 = new HttpClient();
				StringBuilder sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/user/login?password={hashPass}&username={login}&signature={md5temp2}");
				HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
				var json5 = await response2.Content.ReadAsStringAsync();
				JObject o = JObject.Parse(json5);
				var token = JsonConvert.DeserializeObject<resultat>(o.ToString());
				await SecureStorage.SetAsync("oauth_token", token.result.access_token);
				await SecureStorage.SetAsync("is login", "yes");
				TabbedPage tempTP = Parent as TabbedPage;
				(Parent as TabbedPage).Children.Clear();

				tempTP.Children.Add(new advertisement());
				tempTP.Children.Add(new favorites()); 
				tempTP.Children.Add(new add());
				tempTP.Children.Add(new messages());
				tempTP.Children.Add(new profile());
			}
			catch (Exception exp)
			{
				userInput.HasError = true;
				passInput.HasError = true;
				await Task.Delay(2000);
				userInput.HasError = false;
				passInput.HasError = false;
			}
		}

		private void password_TextChanged(object sender, TextChangedEventArgs e)
		{
			pass = (sender as Entry).Text;
		}
	}
}