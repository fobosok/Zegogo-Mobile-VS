using Syncfusion.XForms.ComboBox;
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
	public partial class settingsPage : ContentPage
	{
		string secretKey = @"@YyEbrv$x4lneuBGgra$4XJ{GG1}y0e~";
		string contactPerson = null;
		string city = null;
		string country = null;
		string phone = null;
		string phone4notify = null;
		string password = null;
		string passwordRepeat = null;
		string lang = null;
		public settingsPage()
		{
			InitializeComponent();
			//List<lang> langs = new List<lang>();
			//langs.Add(new lang { icon = "ru.png", name = "Russia" });
			//langs.Add(new lang { icon = "en.png", name = "English" });
			//langs.Add(new lang { icon = "et.png", name = "Estonian" });
			//langs.Add(new lang { icon = "fi.png", name = "Finnish" });
			//langs.Add(new lang { icon = "lt.png", name = "Lithuanian" });
			//langs.Add(new lang { icon = "lv.png", name = "Latvian" });
			//comboBox.DataSource = langs;
			//comboBox.DisplayMemberPath = "name";
			//Subscribe();
		}
		//private void Subscribe()
		//{
		//	MessagingCenter.Subscribe<location>(this, "sendLocation2settings", (sender) => { mestLab.Text = sender.text; city = sender.city; country = sender.country; });
		//}
		private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{

		}

		private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new langPage(), false);
		}

		private async void ImageButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PopAsync(false);
		}

		private async void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
		{
			await(sender as Frame).ScaleTo(1.1, 50);
			await(sender as Frame).ScaleTo(1, 50);
			await Navigation.PushAsync(new locPage("settings"), false);
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
		private async void SfButton_Clicked(object sender, EventArgs e)
		{
			var oauthToken = await SecureStorage.GetAsync("oauth_token");
			try
			{
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
				link1.Params.Add(new Tuple<string, string>("city=", city));
				link1.Params.Add(new Tuple<string, string>("contact_person=", contactPerson));
				link1.Params.Add(new Tuple<string, string>("country=", country));
				link1.Params.Add(new Tuple<string, string>("lang=", lang));
				link1.Params.Add(new Tuple<string, string>("notify_phone=", phone4notify));
				link1.Params.Add(new Tuple<string, string>("password=", password));
				link1.Params.Add(new Tuple<string, string>("password2=", passwordRepeat));
				link1.Params.Add(new Tuple<string, string>("phone=", phone));
				string input3 = $"{link1.Controller}{link1.Action}{paramsToString(link1.Params)}{secretKey}";
				using (MD5 md5Hash = MD5.Create())
				{
					md5temp2 = GetMd5Hash(md5Hash, input3);
				}
				HttpClient client2 = new HttpClient();
				StringBuilder sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/user/edit?access_token={oauthToken}&city={city}&contact_person={contactPerson}&country={country}&lang={lang}&notify_phone={phone4notify}&password={password}&password2={passwordRepeat}&phone={phone}&signature={md5temp2}");
				Debug.WriteLine(sb2);
				HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
				var json5 = await response2.Content.ReadAsStringAsync();
				await DisplayAlert("1", json5, "1");
			}
			catch (Exception exp)
			{
				await DisplayAlert("edit profile", exp.Message, "Ok");
			}
		}

		private void comboBox_ValueChanged(object sender, Syncfusion.XForms.ComboBox.ValueChangedEventArgs e)
		{
			if((sender as SfComboBox).SelectedIndex == 0)
			{
				lang = "ru";
			}
			else if((sender as SfComboBox).SelectedIndex == 1)
			{
				lang = "en";
			}
			else if ((sender as SfComboBox).SelectedIndex == 2)
			{
				lang = "et";
			}
			else if ((sender as SfComboBox).SelectedIndex == 3)
			{
				lang = "fi";
			}
			else if ((sender as SfComboBox).SelectedIndex == 4)
			{
				lang = "lt";
			}
			else if ((sender as SfComboBox).SelectedIndex == 5)
			{
				lang = "lv";
			}
		}

		private async void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
		{
			await(sender as Frame).ScaleTo(1.1, 50);
			await(sender as Frame).ScaleTo(1, 50);
			//await Navigation.PushAsync(new passCh(), false);
		}
	}
}