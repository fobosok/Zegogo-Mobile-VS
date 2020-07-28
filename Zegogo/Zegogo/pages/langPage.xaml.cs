using FormsControls.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Zegogo.connections;
using Zegogo.Resources;

namespace Zegogo.pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class langPage : ContentPage
	{
		string secretKey = @"@YyEbrv$x4lneuBGgra$4XJ{GG1}y0e~";
		public langPage()
		{
			InitializeComponent();
		}

		private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			editProfile("ru");


		}

		private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
		{
			editProfile("en");
			//SecureStorage.SetAsync("lang", "en");
			//Thread.CurrentThread.CurrentUICulture = new CultureInfo(SecureStorage.GetAsync("lang").Result);
			//AppResources.Culture = new CultureInfo(SecureStorage.GetAsync("lang").Result);
			//App.Current.MainPage = new AnimationNavigationPage(new HomePage());

		}

		private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
		{
			editProfile("et");
			//SecureStorage.SetAsync("lang", "et");
			//Thread.CurrentThread.CurrentUICulture = new CultureInfo(SecureStorage.GetAsync("lang").Result);
			//AppResources.Culture = new CultureInfo(SecureStorage.GetAsync("lang").Result);
			//App.Current.MainPage = new AnimationNavigationPage(new HomePage());

		}

		private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
		{
			editProfile("fi");
			//SecureStorage.SetAsync("lang", "fi");
			//Thread.CurrentThread.CurrentUICulture = new CultureInfo(SecureStorage.GetAsync("lang").Result);
			//AppResources.Culture = new CultureInfo(SecureStorage.GetAsync("lang").Result);
			//App.Current.MainPage = new AnimationNavigationPage(new HomePage());

		}

		private void TapGestureRecognizer_Tapped_4(object sender, EventArgs e)
		{
			editProfile("lt");
			//SecureStorage.SetAsync("lang", "lt");
			//Thread.CurrentThread.CurrentUICulture = new CultureInfo(SecureStorage.GetAsync("lang").Result);
			//AppResources.Culture = new CultureInfo(SecureStorage.GetAsync("lang").Result);
			//App.Current.MainPage = new AnimationNavigationPage(new HomePage());

		}

		private void TapGestureRecognizer_Tapped_5(object sender, EventArgs e)
		{
			editProfile("lv");
			//SecureStorage.SetAsync("lang", "lv");
			//Thread.CurrentThread.CurrentUICulture = new CultureInfo(SecureStorage.GetAsync("lang").Result);
			//AppResources.Culture = new CultureInfo(SecureStorage.GetAsync("lang").Result);
			//App.Current.MainPage = new AnimationNavigationPage(new HomePage());

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

		public async void editProfile(string lang)
		{
			try
			{
				var oauthToken = await SecureStorage.GetAsync("oauth_token");
				await SecureStorage.SetAsync("lang", lang);
				Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
				AppResources.Culture = new CultureInfo(lang);
				string md5temp2 = string.Empty;
				Link link1 = new Link
				{
					Main = @"https://zegogo.com/web",
					Api = @"api",
					VersionApi = @"v1",
					Controller = @"user",
					Action = @"edit",
					Params = new List<Tuple<string, string>>(),
				};
				link1.Params.Add(new Tuple<string, string>("access_token=", oauthToken));
				link1.Params.Add(new Tuple<string, string>("field=", "lang"));
				link1.Params.Add(new Tuple<string, string>("value=", lang));
				string input3 = $"{link1.Controller}{link1.Action}{paramsToString(link1.Params)}{secretKey}";
				using (MD5 md5Hash = MD5.Create())
				{
					md5temp2 = GetMd5Hash(md5Hash, input3);
				}
				HttpClient client2 = new HttpClient();
				StringBuilder sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/user/edit?access_token={oauthToken}&field=lang&signature={md5temp2}&value={lang}");
				Debug.WriteLine(sb2);
				HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
				var json5 = await response2.Content.ReadAsStringAsync();
			}
			catch (Exception exp)
			{
				await DisplayAlert("edit profile", exp.Message, "Ok");
			}
			finally
			{
				App.Current.MainPage = new AnimationNavigationPage(new HomePage());
			}
		}
		private async void ImageButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PopAsync(false);
		}
	}
}