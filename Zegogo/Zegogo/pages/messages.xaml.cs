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
	public partial class messages : ContentPage
	{
		string secretKey = @"@YyEbrv$x4lneuBGgra$4XJ{GG1}y0e~";
		string myId;
		string myAvatar;
		public messages()
		{
			InitializeComponent();
			getDialog();
			getDialog2();
			getDialog3();
			getProfile();
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
		public async void getProfile()
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
		public async void getDialog()
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
					Action = @"list",
					Params = new List<Tuple<string, string>>(),
				};
				string json5;
				if (SecureStorage.GetAsync("is login").Result == "yes")
				{
					var oauthToken = await SecureStorage.GetAsync("oauth_token");
					link1.Params.Add(new Tuple<string, string>(@"access_token=", oauthToken));
					link1.Params.Add(new Tuple<string, string>(@"type=", "received"));
					var input3 = $"{link1.Controller}{link1.Action}{paramsToString(link1.Params)}{secretKey}";
					using (MD5 md5Hash = MD5.Create())
					{
						md5temp2 = GetMd5Hash(md5Hash, input3);
					}
					HttpClient client2 = new HttpClient();
					var sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/messages/list?access_token={oauthToken}&type=received&signature={md5temp2}");
					HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
					json5 = await response2.Content.ReadAsStringAsync();
					Debug.WriteLine("777"+json5);
					try
					{
						var result = JsonConvert.DeserializeObject<dialogRes>(json5);
						if (result.dialogs.Count > 0)
						{
							stak1.IsVisible = false;
							list1.ItemsSource = null;
							list1.ItemsSource = result.dialogs;
						}
					}
					catch
					{
						stak1.IsVisible = true;
						list1.IsVisible = false;
						list1.ItemsSource = null;
					}

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
		public async void getDialog2()
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
					Action = @"list",
					Params = new List<Tuple<string, string>>(),
				};
				string json5;
				if (SecureStorage.GetAsync("is login").Result == "yes")
				{
					var oauthToken = await SecureStorage.GetAsync("oauth_token");
					link1.Params.Add(new Tuple<string, string>(@"access_token=", oauthToken));
					link1.Params.Add(new Tuple<string, string>(@"type=", "sended"));
					var input3 = $"{link1.Controller}{link1.Action}{paramsToString(link1.Params)}{secretKey}";
					using (MD5 md5Hash = MD5.Create())
					{
						md5temp2 = GetMd5Hash(md5Hash, input3);
					}
					HttpClient client2 = new HttpClient();
					var sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/messages/list?access_token={oauthToken}&type=sended&signature={md5temp2}");
					HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
					json5 = await response2.Content.ReadAsStringAsync();
					Debug.WriteLine("777" + json5);
					try
					{
						var result = JsonConvert.DeserializeObject<dialogRes>(json5);
						if (result.dialogs.Count > 0)
						{
							stak2.IsVisible = false;
							list2.ItemsSource = null;
							list2.ItemsSource = result.dialogs;
						}
					}
					catch
					{
						stak2.IsVisible = true;
						list2.IsVisible = false;
						list2.ItemsSource = null;
					}
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
		public async void getDialog3()
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
					Action = @"list",
					Params = new List<Tuple<string, string>>(),
				};
				string json5;
				if (SecureStorage.GetAsync("is login").Result == "yes")
				{
					var oauthToken = await SecureStorage.GetAsync("oauth_token");
					link1.Params.Add(new Tuple<string, string>(@"access_token=", oauthToken));
					link1.Params.Add(new Tuple<string, string>(@"type=", "archived"));
					var input3 = $"{link1.Controller}{link1.Action}{paramsToString(link1.Params)}{secretKey}";
					using (MD5 md5Hash = MD5.Create())
					{
						md5temp2 = GetMd5Hash(md5Hash, input3);
					}
					HttpClient client2 = new HttpClient();
					var sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/messages/list?access_token={oauthToken}&type=archived&signature={md5temp2}");
					HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
					json5 = await response2.Content.ReadAsStringAsync();
					Debug.WriteLine("777" + json5);
					try
					{
						var result = JsonConvert.DeserializeObject<dialogRes>(json5);
						if (result.dialogs.Count > 0)
						{
							stak3.IsVisible = false;
							list3.ItemsSource = null;
							list3.ItemsSource = result.dialogs;
						}
					}
					catch
					{
						stak3.IsVisible = true;
						list3.IsVisible = false;
						list3.ItemsSource = null;
					}
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
		private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			string offer_id = (((sender as Frame).Content as StackLayout).Children[2] as Label).Text;
			string interlocutor_id = (((sender as Frame).Content as StackLayout).Children[3] as Label).Text;
			string interlocutor_name = (((sender as Frame).Content as StackLayout).Children[4] as Label).Text;
			await Navigation.PushModalAsync(new Page4(offer_id,interlocutor_id, interlocutor_name, myId, myAvatar), false);
		}

		private async void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
		{
			string offer_id = (((sender as Frame).Content as StackLayout).Children[2] as Label).Text;
			string interlocutor_id = (((sender as Frame).Content as StackLayout).Children[3] as Label).Text;
			string interlocutor_name = (((sender as Frame).Content as StackLayout).Children[4] as Label).Text;
			await Navigation.PushModalAsync(new Page4(offer_id, interlocutor_id, interlocutor_name, myId, myAvatar), false);
		}

		private async void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
		{
			string offer_id = (((sender as Frame).Content as StackLayout).Children[2] as Label).Text;
			string interlocutor_id = (((sender as Frame).Content as StackLayout).Children[3] as Label).Text;
			string interlocutor_name = (((sender as Frame).Content as StackLayout).Children[4] as Label).Text;
			await Navigation.PushModalAsync(new Page4(offer_id, interlocutor_id, interlocutor_name, myId, myAvatar), false);
		}
	}
}