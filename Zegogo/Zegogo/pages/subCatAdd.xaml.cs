using FormsControls.Base;
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
	public partial class subCatAdd : ContentPage , IAnimationPage
	{
		public string subId;
		public IPageAnimation PageAnimation { get; } = new SlidePageAnimation
		{
			Duration = AnimationDuration.Short,
			Subtype = AnimationSubtype.FromRight
		};
		public void OnAnimationStarted(bool isPopAnimation)
		{
			// Put your code here but leaving empty works just fine
		}

		public void OnAnimationFinished(bool isPopAnimation)
		{
			// Put your code here but leaving empty works just fine
		}
		string secretKey = @"@YyEbrv$x4lneuBGgra$4XJ{GG1}y0e~";
		public subCatAdd(ListCategory listCategory, string title, string subId)
		{
			InitializeComponent();
			this.subId = subId;
			if (title != string.Empty)
			{
				SubTitle.Text = title;
			}
			SubCategoryListView.ItemsSource = null;
			SubCategoryListView.ItemsSource = listCategory.categories;
		}

		private async void ImageButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PopAsync();
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

		private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			var oauthToken = await SecureStorage.GetAsync("oauth_token");
			try
			{
				string catId = (((sender as Frame).Content as StackLayout).Children[2] as Label).Text;
				string title = (((sender as Frame).Content as StackLayout).Children[0] as Label).Text;
				await (sender as Frame).ScaleTo(1.1, 50);
				await (sender as Frame).ScaleTo(1, 50);
				if ((((sender as Frame).Content as StackLayout).Children[1] as Image).IsVisible == false)
				{
					MessagingCenter.Send<string>(title, "LabelChange2");
					MessagingCenter.Send<string>(catId, "sendID");
					await Navigation.PopToRootAsync();
				}
				else
				{
					string md5temp2 = string.Empty;
					Link link1 = new Link
					{
						Main = @"https://zegogo.com/web",
						Api = @"api",
						VersionApi = @"v1",
						Controller = @"category",
						Action = @"list",
						Params = new List<Tuple<string, string>>(),
					};
					link1.Params.Add(new Tuple<string, string>("access_token=", oauthToken));
					link1.Params.Add(new Tuple<string, string>(@"parent_id=", catId));
					string input3 = $"{link1.Controller}{link1.Action}{paramsToString(link1.Params)}{secretKey}";
					using (MD5 md5Hash = MD5.Create())
					{
						md5temp2 = GetMd5Hash(md5Hash, input3);
					}
					HttpClient client2 = new HttpClient();
					StringBuilder sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/category/list?access_token={oauthToken}&parent_id={catId}&signature={md5temp2}");
					HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
					var json5 = await response2.Content.ReadAsStringAsync();
					Debug.WriteLine(json5);
					var ListCategory = JsonConvert.DeserializeObject<ListCategory>(json5);
					foreach (var item in ListCategory.categories)
					{
						if (item.isSealed == true)
						{
							item.isVisible = false;
						}
						else
						{
							item.isVisible = true;
						}
					}
					await Navigation.PushAsync(new subCatAdd(ListCategory, title, catId));
				}
			}
			catch (Exception exp)
			{
				await DisplayAlert("Category-List", exp.Message, "Ok");
			}
		}
	}
}