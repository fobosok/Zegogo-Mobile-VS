using Newtonsoft.Json;
using Syncfusion.XForms.Buttons;
using Syncfusion.XForms.Chat;
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
	public partial class Page4 : ContentPage
	{
		string secretKey = @"@YyEbrv$x4lneuBGgra$4XJ{GG1}y0e~";
		public List<object> messages;
		public Author currentUser;
		public string offer_id;
		public string interlocutor_id;
		public string interlocutor_name;
		public string myId;
		public string myAvatar;
		public Page4(string offer_id,string interlocutor_id,string interlocutor_name, string myId, string myAvatar)
		{
			InitializeComponent();
			this.messages = new List<object>();
			this.currentUser = new Author() ;
			this.offer_id = offer_id;
			this.interlocutor_id = interlocutor_id;
			this.interlocutor_name = interlocutor_name;
			this.myId = myId;
			this.myAvatar = myAvatar;
			labelH.Text = interlocutor_name;
            sfChat.CurrentUser = currentUser;
			getDialog();
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
					Action = @"index",
					Params = new List<Tuple<string, string>>(),
				};
				string json5;
				if (SecureStorage.GetAsync("is login").Result == "yes")
				{
					var oauthToken = await SecureStorage.GetAsync("oauth_token");
					link1.Params.Add(new Tuple<string, string>(@"access_token=", oauthToken));
					link1.Params.Add(new Tuple<string, string>(@"interlocutor_id=", interlocutor_id));
					link1.Params.Add(new Tuple<string, string>(@"offer_id=", offer_id));
					var input3 = $"{link1.Controller}{link1.Action}{paramsToString(link1.Params)}{secretKey}";
					using (MD5 md5Hash = MD5.Create())
					{
						md5temp2 = GetMd5Hash(md5Hash, input3);
					}
					HttpClient client2 = new HttpClient();
					var sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/messages/index?access_token={oauthToken}&interlocutor_id={interlocutor_id}&offer_id={offer_id}&signature={md5temp2}");
					HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
					json5 = await response2.Content.ReadAsStringAsync();
					Debug.WriteLine("888" + json5);

					try
					{
						var result = JsonConvert.DeserializeObject<messagesRes>(json5);
						foreach(var item in result.messages)
						{
							if(item.sender_id == myId)
							{
								sfChat.Messages.Add(new TextMessage() { Author = currentUser, Text = item.text, DateTime = Convert.ToDateTime(item.date) });
							}
							else
							{
								sfChat.Messages.Add(new TextMessage() { Author = new Author() { Avatar = result.interlocutor.avatar }, Text = item.text, DateTime = Convert.ToDateTime(item.date) });
							}
						}
					}
					catch
					{

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

		private async void sfChat_SendMessage(object sender, SendMessageEventArgs e)
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
					link1.Params.Add(new Tuple<string, string>(@"interlocutor_id=", interlocutor_id));
					link1.Params.Add(new Tuple<string, string>(@"offer_id=", offer_id));
					link1.Params.Add(new Tuple<string, string>(@"text=", e.Message.Text));
					var input3 = $"{link1.Controller}{link1.Action}{paramsToString(link1.Params)}{secretKey}";
					using (MD5 md5Hash = MD5.Create())
					{
						md5temp2 = GetMd5Hash(md5Hash, input3);
					}
					HttpClient client2 = new HttpClient();
					var sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/messages/send?access_token={oauthToken}&interlocutor_id={interlocutor_id}&offer_id={offer_id}&text={e.Message.Text}&signature={md5temp2}");
					HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
					json5 = await response2.Content.ReadAsStringAsync();

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