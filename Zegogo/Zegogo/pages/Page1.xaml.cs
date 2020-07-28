using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
	public partial class Page1 : ContentPage
	{
		MultipartFormDataContent profile_imgTemp;
		MultipartFormDataContent profile_imgTemp2del;
		private static Random random = new Random();
		string secretKey = @"@YyEbrv$x4lneuBGgra$4XJ{GG1}y0e~";
		string catID;
		string priceType = "price";
		List<Tuple<string, string>> tuplesList = new List<Tuple<string, string>>();
		List<string> photoList = new List<string>();
		View temp1;
		View temp2;
		View temp3;
		View temp4;
		View temp5;
		View temp6;
		View temp7;
		Grid tempPrice;
		OfferRes OfferRes = new OfferRes();
		public Page1(string id)
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			getOffer(id);
			temp1 = frame1.Content;
			temp2 = frame2.Content;
			temp3 = frame3.Content;
			temp4 = frame4.Content;
			temp5 = frame5.Content;
			temp6 = frame6.Content;
			temp7 = frame7.Content;
			tempPrice = p2;
			Subscribe();
			profile_imgTemp2del = new MultipartFormDataContent();
			profile_imgTemp = new MultipartFormDataContent();
		}
		public async void getOffer(string id)
		{
			try
			{
				string md5temp2 = string.Empty;
				Link link1 = new Link
				{
					Main = @"https://zegogo.com/web",
					Api = @"api",
					VersionApi = @"v1",
					Controller = @"offer",
					Action = @"index",
					Params = new List<Tuple<string, string>>(),
				};
				if (SecureStorage.GetAsync("is login").Result == "yes")
				{
					var oauthToken = await SecureStorage.GetAsync("oauth_token");
					link1.Params.Add(new Tuple<string, string>("access_token=", oauthToken));
				}
				link1.Params.Add(new Tuple<string, string>("id=", id));
				string input3 = $"{link1.Controller}{link1.Action}{paramsToString(link1.Params)}{secretKey}";
				using (MD5 md5Hash = MD5.Create())
				{
					md5temp2 = GetMd5Hash(md5Hash, input3);
				}
				HttpClient client2 = new HttpClient();
				StringBuilder sb2;
				if (SecureStorage.GetAsync("is login").Result == "yes")
				{
					var oauthToken = await SecureStorage.GetAsync("oauth_token");
					sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/offer/index?access_token={oauthToken}&id={id}&signature={md5temp2}");
				}
				else
				{
					sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/offer/index?id={id}&signature={md5temp2}");
				}
				HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
				var json5 = await response2.Content.ReadAsStringAsync();
				Debug.WriteLine(sb2);
				var res = JsonConvert.DeserializeObject<OfferRes>(json5);
				res.offer.date = DateTime.Parse(res.offer.date).ToLongDateString();
				res.offer.country_city = res.offer.country.name + ", " + res.offer.city.name;
				res.offer.id_string = "Номер обьявления: " + res.offer.id;
				if (res.offer.user.is_online == "1")
				{
					res.offer.colorLab = "#9c9c9c";
				}
				else
				{
					res.offer.colorLab = "#37A849";
				}
				foreach (var item in res.offer.options)
				{
					try
					{
						if (item.value_text.ToString()[0] == '[')
						{

							var resText = JsonConvert.DeserializeObject<string[]>(item.value_text.ToString());
							foreach (var item2 in resText)
							{
								item.value_text_string = item.value_text_string + item2 + ", ";
							}
							item.value_text_string = item.value_text_string.Substring(0, item.value_text_string.Length - 2);
							item.value_text_string = item.value_text_string + ".";
						}
						else
						{
							item.value_text_string = item.value_text.ToString();
						}

						item.value_text_string = item.value_text_string + " " + item.units;
						item.name = item.name + ":";
					}
					catch
					{

					}
				}
				if (res.offer.is_favorite == "1")
				{
					res.offer.starColor = "#BE593D";
					res.offer.Istar = "starOn";
				}
				else
				{
					res.offer.starColor = "#FFFFFF";
					res.offer.Istar = "star";
				}
				zagEntry.Text = res.offer.title;
				chooseCat.Text = res.offer.category.name;
				if (res.offer.category.id == "421" || res.offer.category.id == "422" || res.offer.category.id == "423" || res.offer.category.id == "424") { p1.IsVisible = false; p2.IsVisible = false; p3.IsVisible = false; gridPrice.IsVisible = false; } else { p1.IsVisible = true; p2.IsVisible = true; p3.IsVisible = true; gridPrice.IsVisible = true; } catID = res.offer.category.id; optFr.HeightRequest = 45; optStack.IsVisible = true; optLine.IsVisible = true; rombImg.Source = "RombOk"; 
				if (res.offer.price_type == "price")
				{
					b1.BackgroundColor = Color.FromHex("#D86241");
					b2.TextColor = Color.FromHex("#D86241");
					b2.BackgroundColor = Color.White;
					b2.IsEnabled = true;
					b3.TextColor = Color.FromHex("#D86241");
					b3.BackgroundColor = Color.White;
					b3.IsEnabled = true;
					gridPrice.IsVisible = true;
					gridPrice.HeightRequest = 45;
					b1.TextColor = Color.White;

					priceEntry.Text = res.offer.price;
				}
				else if(res.offer.price_type == "free")
				{
					b2.BackgroundColor = Color.FromHex("#D86241");
					b1.TextColor = Color.FromHex("#D86241");
					b1.BackgroundColor = Color.White;
					b1.IsEnabled = true;
					b3.TextColor = Color.FromHex("#D86241");
					b3.BackgroundColor = Color.White;
					b3.IsEnabled = true;
					gridPrice.IsVisible = false;
					gridPrice.HeightRequest = 0;
					b2.TextColor = Color.White;
				}
				else if(res.offer.price_type == "exchange")
				{
					b3.BackgroundColor = Color.FromHex("#D86241");
					b2.TextColor = Color.FromHex("#D86241");
					b2.BackgroundColor = Color.White;
					b2.IsEnabled = true;
					b1.TextColor = Color.FromHex("#D86241");
					b1.BackgroundColor = Color.White;
					b1.IsEnabled = true;
					gridPrice.IsVisible = false;
					gridPrice.HeightRequest = 0;
					b3.TextColor = Color.White;
				}
				opEntry.Text = res.offer.text;
				mestLab.Text = res.offer.country_city;
				nomEntry.Text = res.offer.phone;
				mestRomb.Source = "RombOk";
				OfferRes = res;
				photosRefresh(res);
				check_rombs();
			}
			catch (Exception exp)
			{
				await DisplayAlert("Get-offer", exp.Message, "Ok");
			}
		}
		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);

			double colsCount = gridPhoto.ColumnDefinitions.Count;
			double rowsCount = gridPhoto.RowDefinitions.Count;
			if (colsCount > 0 & rowsCount > 0)
			{
				gridPhoto.HeightRequest = width / 2.5;
			}
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

		private void refresh()
		{
			profile_imgTemp = new MultipartFormDataContent();
			catID = string.Empty;
			nomEntry.Text = string.Empty;
			priceEntry.Text = string.Empty;
			priceType = "price";
			opEntry.Text = string.Empty;
			zagEntry.Text = string.Empty;
			chooseCat.Text = "Выбрать рубрику";
			chooseCat.TextColor = Color.FromHex("#8D8D8D");
			rombImg.Source = "rombS";
			frame1.Content = temp1;
			frame2.Content = temp2;
			frame3.Content = temp3;
			frame4.Content = temp4;
			frame5.Content = temp5;
			frame6.Content = temp6;
			frame7.Content = temp7;
			mestRomb.Source = "rombS";
			tuplesList.Clear();
			optLab.TextColor = Color.FromHex("#8D8D8D");
			mestLab.TextColor = Color.FromHex("#8D8D8D");
			mestLab.Text = "Местоположение";
			p2 = tempPrice;
			p1.IsVisible = true;
			p2.IsVisible = true;
			p3.IsVisible = true;
			gridPrice.IsVisible = true;
			photoList.Clear();
			optFr.HeightRequest = 0; optStack.IsVisible = false; optLine.IsVisible = false;
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
					Controller = @"offer",
					Action = @"edit",
					Params = new List<Tuple<string, string>>(),
				};
				link1.Params.Add(new Tuple<string, string>(@"access_token", oauthToken));
				link1.Params.Add(new Tuple<string, string>(@"category", catID));
				link1.Params.Add(new Tuple<string, string>(@"phone", nomEntry.Text));
				if (priceType == "price")
				{
					link1.Params.Add(new Tuple<string, string>(@"price", priceEntry.Text));
				}
				link1.Params.Add(new Tuple<string, string>(@"price_type", priceType));
				link1.Params.Add(new Tuple<string, string>(@"text", opEntry.Text));
				link1.Params.Add(new Tuple<string, string>(@"title", zagEntry.Text));
				link1.Params.AddRange(tuplesList);
				link1.Params.Sort();
				string input3 = $"{link1.Controller}{link1.Action}{paramsToString20(link1.Params)}{secretKey}";
				using (MD5 md5Hash = MD5.Create())
				{
					md5temp2 = GetMd5Hash(md5Hash, input3);
				}
				StringBuilder sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/offer/add?{paramsToString2(link1.Params)}signature={md5temp2}");
				var client = new HttpClient();
				var res = await client.PostAsync(sb2.ToString(), profile_imgTemp);
				var res2 = await res.Content.ReadAsStringAsync();
				var result = JsonConvert.DeserializeObject<offerAddResult>(res2);
				if (result.result == "OK")
				{
					refresh();
					await Navigation.PushModalAsync(new PromotePage(result), false);
				}
				else
				{
					profile_imgTemp = new MultipartFormDataContent();
					frame1.Content = temp1;
					frame2.Content = temp2;
					frame3.Content = temp3;
					frame4.Content = temp4;
					frame5.Content = temp5;
					frame6.Content = temp6;
					frame7.Content = temp7;
					photoList.Clear();
				}

			}
			catch (Exception ex)
			{
				await DisplayAlert("Offer Add", ex.Message, "Ok");
			}
		}
		private void Subscribe()
		{
			MessagingCenter.Subscribe<string>(this, "LabelChange2", (sender) => { chooseCat.Text = sender; chooseCat.TextColor = Color.Black; });
			MessagingCenter.Subscribe<string>(this, "sendID", (sender) => { if (sender == "421" || sender == "422" || sender == "423" || sender == "424") { p1.IsVisible = false; p2.IsVisible = false; p3.IsVisible = false; gridPrice.IsVisible = false; } else { p1.IsVisible = true; p2.IsVisible = true; p3.IsVisible = true; gridPrice.IsVisible = true; } catID = sender; optFr.HeightRequest = 45; optStack.IsVisible = true; optLine.IsVisible = true; rombImg.Source = "RombOk"; check_rombs(); });
			MessagingCenter.Subscribe<List<Tuple<string, string>>>(this, "sendListOptions", (sender) => { indexes(sender); optLab.TextColor = Color.Black; });
			MessagingCenter.Subscribe<List<Tuple<string, string>>>(this, "sendLocation", (sender) => { tuplesList.AddRange(sender); mestRomb.Source = "RombOk"; check_rombs(); });
			MessagingCenter.Subscribe<string>(this, "sendLocationText", (sender) => { mestLab.Text = sender; mestLab.TextColor = Color.Black; });
		}
		private void indexes(List<Tuple<string, string>> temp)
		{
			var temp2 = temp;
			var temp3 = new List<Tuple<string, string>>();
			var temp4 = new List<Tuple<string, string>>();
			var temp5 = new List<Tuple<string, string>>();
			int ii = 0;
			foreach (var item in temp2)
			{
				if (item.Item1.IndexOf("[]") != -1)
				{
					temp3.Add(item);
				}
				else
				{
					temp4.Add(item);
				}
			}
			string str = string.Empty;
			for (int i = 0; i < temp3.Count; i++)
			{
				if (i > 0)
				{
					if (temp3[i].Item1 == str)
					{

						temp4.Add(new Tuple<string, string>(temp3[i].Item1.Replace("[]", $"[{ii}]"), temp3[i].Item2));
						ii++;
					}
					else
					{
						ii = 0;
						str = temp3[i].Item1;
						temp4.Add(new Tuple<string, string>(temp3[i].Item1.Replace("[]", $"[{ii}]"), temp3[i].Item2));
						ii++;
					}
				}
				else
				{
					str = temp3[i].Item1;
					temp4.Add(new Tuple<string, string>(temp3[i].Item1.Replace("[]", $"[{ii}]"), temp3[i].Item2));
					ii++;
				}
			}
			tuplesList.AddRange(temp4);
		}
		private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			try
			{
				await (sender as Frame).ScaleTo(1.1, 50);
				await (sender as Frame).ScaleTo(1, 50);
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

				string input3 = $"{link1.Controller}{link1.Action}{secretKey}";
				using (MD5 md5Hash = MD5.Create())
				{
					md5temp2 = GetMd5Hash(md5Hash, input3);
				}
				HttpClient client2 = new HttpClient();
				StringBuilder sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/category/list?signature={md5temp2}");
				HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
				var json5 = await response2.Content.ReadAsStringAsync();
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
				await Navigation.PushAsync(new CatAdd(ListCategory));
			}
			catch (Exception exp)
			{
				await DisplayAlert("Category-List-ADD error", exp.Message, "Ok");
			}
		}

		private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
		{
			try
			{
				await (sender as Frame).ScaleTo(1.1, 50);
				await (sender as Frame).ScaleTo(1, 50);
				var oauthToken = await SecureStorage.GetAsync("oauth_token");
				string md5temp2 = string.Empty;
				Link link1 = new Link
				{
					Main = @"https://zegogo.com/web",
					Api = @"api",
					VersionApi = @"v1",
					Controller = @"category",
					Action = @"filters",
					Params = new List<Tuple<string, string>>(),
				};
				link1.Params.Add(new Tuple<string, string>(@"access_token=", oauthToken));
				link1.Params.Add(new Tuple<string, string>(@"category_id=", catID));
				link1.Params.Add(new Tuple<string, string>(@"in_search=", "false"));
				string input3 = $"{link1.Controller}{link1.Action}{paramsToString(link1.Params)}{secretKey}";
				using (MD5 md5Hash = MD5.Create())
				{
					md5temp2 = GetMd5Hash(md5Hash, input3);
				}
				HttpClient client2 = new HttpClient();
				StringBuilder sb2 = new StringBuilder($@"https://zegogo.com/web/api/v1/category/filters?access_token={oauthToken}&category_id={catID}&in_search=false&signature={md5temp2}");
				HttpResponseMessage response2 = await client2.GetAsync(sb2.ToString());
				var json5 = await response2.Content.ReadAsStringAsync();
				Debug.WriteLine(json5);
				var filters = JsonConvert.DeserializeObject<Filters>(json5);
				await Navigation.PushAsync(new options(filters));
			}
			catch (Exception ex)
			{
				await DisplayAlert("Get filters error", ex.Message, "Ok");
			}
		}
		private void check_rombs()
		{
			//await DisplayAlert("1", opRomb.Source.ToString(), "1");
			if (zagRomb.Source.ToString() == "File: RombOk" && opRomb.Source.ToString() == "File: RombOk" && nomRomb.Source.ToString() == "File: RombOk" && mestRomb.Source.ToString() == "File: RombOk" && rombImg.Source.ToString() == "File: RombOk")
			{
				addBut.IsEnabled = true;
				addBut.BackgroundColor = Color.FromHex("#BE593D");
			}
			else
			{
				addBut.IsEnabled = false;
				addBut.BackgroundColor = Color.LightGray;
			}
		}
		private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
		{
			zagEntry.Focus();
		}

		private void zagEntry_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (zagEntry.Text.Length > 3)
			{
				zagRomb.Source = "RombOk";
			}
			else
			{
				zagRomb.Source = "rombS";
			}
			check_rombs();
		}

		private void b1_Clicked(object sender, EventArgs e)
		{
			priceType = "price";
			b1.BackgroundColor = Color.FromHex("#D86241");
			b2.TextColor = Color.FromHex("#D86241");
			b2.BackgroundColor = Color.White;
			b2.IsEnabled = true;
			b3.TextColor = Color.FromHex("#D86241");
			b3.BackgroundColor = Color.White;
			b3.IsEnabled = true;
			gridPrice.IsVisible = true;
			gridPrice.HeightRequest = 45;
			b1.TextColor = Color.White;
		}

		private void b2_Clicked(object sender, EventArgs e)
		{
			priceType = "free";
			b2.BackgroundColor = Color.FromHex("#D86241");
			b1.TextColor = Color.FromHex("#D86241");
			b1.BackgroundColor = Color.White;
			b1.IsEnabled = true;
			b3.TextColor = Color.FromHex("#D86241");
			b3.BackgroundColor = Color.White;
			b3.IsEnabled = true;
			gridPrice.IsVisible = false;
			gridPrice.HeightRequest = 0;
			b2.TextColor = Color.White;
		}

		private void b3_Clicked(object sender, EventArgs e)
		{
			priceType = "exchange";
			b3.BackgroundColor = Color.FromHex("#D86241");
			b2.TextColor = Color.FromHex("#D86241");
			b2.BackgroundColor = Color.White;
			b2.IsEnabled = true;
			b1.TextColor = Color.FromHex("#D86241");
			b1.BackgroundColor = Color.White;
			b1.IsEnabled = true;
			gridPrice.IsVisible = false;
			gridPrice.HeightRequest = 0;
			b3.TextColor = Color.White;
		}

		private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
		{
			opEntry.Focus();
		}

		private async void TapGestureRecognizer_Tapped_4(object sender, EventArgs e)
		{
			//autocomplete.Focus();
			await (sender as Frame).ScaleTo(1.1, 50);
			await (sender as Frame).ScaleTo(1, 50);
			await Navigation.PushAsync(new locPage(), false);
		}

		private void TapGestureRecognizer_Tapped_5(object sender, EventArgs e)
		{
			nomEntry.Focus();
		}

		private void TapGestureRecognizer_Tapped_6(object sender, EventArgs e)
		{
			//emailEntry.Focus();
		}

		private void TapGestureRecognizer_Tapped_7(object sender, EventArgs e)
		{
			//imyaEntry.Focus();
		}

		private void opEntry_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (opEntry.Text.Length > 20)
			{
				opRomb.Source = "RombOk";
			}
			else
			{
				opRomb.Source = "rombS";
			}
			check_rombs();
		}

		private void nomEntry_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (nomEntry.Text.Length > 0)
			{
				nomRomb.Source = "RombOk";
			}
			else
			{
				nomRomb.Source = "rombS";
			}
			check_rombs();
		}

		private void emailEntry_TextChanged(object sender, TextChangedEventArgs e)
		{
			//if (emailEntry.Text.Length > 0)
			//{
			//	emailRomb.Source = "RombOk";
			//}
			//else
			//{
			//	emailRomb.Source = "rombS";
			//}
			//check_rombs();
		}

		private void imyaEntry_TextChanged(object sender, TextChangedEventArgs e)
		{
			//if (imyaEntry.Text.Length > 0)
			//{
			//	imyaRomb.Source = "RombOk";
			//}
			//else
			//{
			//	imyaRomb.Source = "rombS";
			//}
			//check_rombs();
		}


		private async void TapGestureRecognizer_Tapped_9(object sender, EventArgs e)
		{
			var action = await DisplayActionSheet(null, null, null, "Gallery", "Camera");
			if (action == "Gallery")
			{
				if (CrossMedia.Current.IsPickPhotoSupported)
				{

					var photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
					{
						PhotoSize = PhotoSize.MaxWidthHeight,
						RotateImage = true,
						CompressionQuality = 80
					});


					if (photo != null)
					{
						var streamContent = new StreamContent(File.OpenRead(photo.Path));
						var profile_img = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
						profile_img.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
						profile_imgTemp.Add(profile_img, "photos[]", photo.Path);
						if (photoList.Count == 0)
						{
							frame1.Content = new Image { Source = photo.Path, Aspect = Aspect.AspectFill };
							photoList.Add(photo.Path);
						}
						else if (photoList.Count == 1)
						{
							frame2.Content = new Image { Source = photo.Path, Aspect = Aspect.AspectFill };
							photoList.Add(photo.Path);
						}
						else if (photoList.Count == 2)
						{
							frame3.Content = new Image { Source = photo.Path, Aspect = Aspect.AspectFill };
							photoList.Add(photo.Path);
						}
						else if (photoList.Count == 3)
						{
							frame4.Content = new Image { Source = photo.Path, Aspect = Aspect.AspectFill };
							photoList.Add(photo.Path);
						}
						else if (photoList.Count == 4)
						{
							frame5.Content = new Image { Source = photo.Path, Aspect = Aspect.AspectFill };
							photoList.Add(photo.Path);
						}
						else if (photoList.Count == 5)
						{
							frame6.Content = new Image { Source = photo.Path, Aspect = Aspect.AspectFill };
							photoList.Add(photo.Path);
						}
						else if (photoList.Count == 6)
						{
							frame7.Content = new Image { Source = photo.Path, Aspect = Aspect.AspectFill };
							photoList.Add(photo.Path);
						}

					}
					else
					{
						return;
					}

				}
				else
				{
					return;
				}
			}
			else if (action == "Camera")
			{
				if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
				{
					MediaFile photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
					{
						SaveToAlbum = true,
						Directory = "Sample",
						Name = $"{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.jpg",
						PhotoSize = PhotoSize.MaxWidthHeight,
						MaxWidthHeight = 250,
						RotateImage = true,
						CompressionQuality = 80
					});
					if (photo != null)
					{
						var streamContent = new StreamContent(File.OpenRead(photo.Path));
						var profile_img = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
						profile_img.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
						profile_imgTemp.Add(profile_img, "photos[]", photo.Path);
						if (photoList.Count == 0)
						{
							frame1.Content = new Image { Source = photo.Path, Aspect = Aspect.AspectFill };
							photoList.Add(photo.Path);
						}
						else if (photoList.Count == 1)
						{
							frame2.Content = new Image { Source = photo.Path, Aspect = Aspect.AspectFill };
							photoList.Add(photo.Path);
						}
						else if (photoList.Count == 2)
						{
							frame3.Content = new Image { Source = photo.Path, Aspect = Aspect.AspectFill };
							photoList.Add(photo.Path);
						}
						else if (photoList.Count == 3)
						{
							frame4.Content = new Image { Source = photo.Path, Aspect = Aspect.AspectFill };
							photoList.Add(photo.Path);
						}
						else if (photoList.Count == 4)
						{
							frame5.Content = new Image { Source = photo.Path, Aspect = Aspect.AspectFill };
							photoList.Add(photo.Path);
						}
						else if (photoList.Count == 5)
						{
							frame6.Content = new Image { Source = photo.Path, Aspect = Aspect.AspectFill };
							photoList.Add(photo.Path);
						}
						else if (photoList.Count == 6)
						{
							frame7.Content = new Image { Source = photo.Path, Aspect = Aspect.AspectFill };
							photoList.Add(photo.Path);
						}
					}
				}
				else
				{
					return;
				}
			}
		}
		public async void photosRefresh(OfferRes OfferRes)
		{
			try
			{

				frame1.Content = temp1;
				frame2.Content = temp2;
				frame3.Content = temp3;
				frame4.Content = temp4;
				frame5.Content = temp5;
				frame6.Content = temp6;
				frame7.Content = temp7;
				profile_imgTemp = new MultipartFormDataContent();
				if (OfferRes.offer.photos.Count == 1)
				{
					frame1.Content = new Image { Source = OfferRes.offer.photos[0].original, Aspect = Aspect.AspectFill };
					var streamContent = new StreamContent(File.OpenRead((frame1.Content as Image).Source.ToString()));
					await DisplayAlert("1", OfferRes.offer.photos[0].original, "q1");
					var profile_img = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
					profile_img.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img, "photos[]", OfferRes.offer.photos[0].original);
				}
				else if (OfferRes.offer.photos.Count == 2)
				{
					frame1.Content = new Image { Source = OfferRes.offer.photos[0].original, Aspect = Aspect.AspectFill };
					var streamContent = new StreamContent(File.OpenRead((frame1.Content as Image).Source.ToString()));
					await DisplayAlert("1", OfferRes.offer.photos[0].original, "q1");
					var profile_img = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
					profile_img.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img, "photos[]", OfferRes.offer.photos[0].original);

					frame2.Content = new Image { Source = OfferRes.offer.photos[1].original, Aspect = Aspect.AspectFill };

					var streamContent2 = new StreamContent(File.OpenRead(OfferRes.offer.photos[1].original));
					var profile_img2 = new ByteArrayContent(await streamContent2.ReadAsByteArrayAsync());
					profile_img2.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img2, "photos[]", OfferRes.offer.photos[1].original);
				}
				else if (OfferRes.offer.photos.Count == 3)
				{
					frame1.Content = new Image { Source = OfferRes.offer.photos[0].original, Aspect = Aspect.AspectFill };

					var streamContent = new StreamContent(File.OpenRead(OfferRes.offer.photos[0].original));
					var profile_img = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
					profile_img.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img, "photos[]", OfferRes.offer.photos[0].original);

					frame2.Content = new Image { Source = OfferRes.offer.photos[1].original, Aspect = Aspect.AspectFill };

					var streamContent2 = new StreamContent(File.OpenRead(OfferRes.offer.photos[1].original));
					var profile_img2 = new ByteArrayContent(await streamContent2.ReadAsByteArrayAsync());
					profile_img2.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img2, "photos[]", OfferRes.offer.photos[1].original);

					frame3.Content = new Image { Source = OfferRes.offer.photos[2].original, Aspect = Aspect.AspectFill };

					var streamContent3 = new StreamContent(File.OpenRead(OfferRes.offer.photos[2].original));
					var profile_img3 = new ByteArrayContent(await streamContent3.ReadAsByteArrayAsync());
					profile_img3.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img3, "photos[]", OfferRes.offer.photos[2].original);
				}
				else if (OfferRes.offer.photos.Count == 4)
				{
					frame1.Content = new Image { Source = OfferRes.offer.photos[0].original, Aspect = Aspect.AspectFill };

					var streamContent = new StreamContent(File.OpenRead(OfferRes.offer.photos[0].original));
					var profile_img = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
					profile_img.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img, "photos[]", OfferRes.offer.photos[0].original);

					frame2.Content = new Image { Source = OfferRes.offer.photos[1].original, Aspect = Aspect.AspectFill };

					var streamContent2 = new StreamContent(File.OpenRead(OfferRes.offer.photos[1].original));
					var profile_img2 = new ByteArrayContent(await streamContent2.ReadAsByteArrayAsync());
					profile_img2.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img2, "photos[]", OfferRes.offer.photos[1].original);

					frame3.Content = new Image { Source = OfferRes.offer.photos[2].original, Aspect = Aspect.AspectFill };

					var streamContent3 = new StreamContent(File.OpenRead(OfferRes.offer.photos[2].original));
					var profile_img3 = new ByteArrayContent(await streamContent3.ReadAsByteArrayAsync());
					profile_img3.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img3, "photos[]", OfferRes.offer.photos[2].original);

					frame4.Content = new Image { Source = OfferRes.offer.photos[3].original, Aspect = Aspect.AspectFill };

					var streamContent4 = new StreamContent(File.OpenRead(OfferRes.offer.photos[3].original));
					var profile_img4 = new ByteArrayContent(await streamContent4.ReadAsByteArrayAsync());
					profile_img4.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img4, "photos[]", OfferRes.offer.photos[3].original);
				}
				else if (OfferRes.offer.photos.Count == 5)
				{
					frame1.Content = new Image { Source = OfferRes.offer.photos[0].original, Aspect = Aspect.AspectFill };

					var streamContent = new StreamContent(File.OpenRead(OfferRes.offer.photos[0].original));
					var profile_img = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
					profile_img.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img, "photos[]", OfferRes.offer.photos[0].original);

					frame2.Content = new Image { Source = OfferRes.offer.photos[1].original, Aspect = Aspect.AspectFill };

					var streamContent2 = new StreamContent(File.OpenRead(OfferRes.offer.photos[1].original));
					var profile_img2 = new ByteArrayContent(await streamContent2.ReadAsByteArrayAsync());
					profile_img2.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img2, "photos[]", OfferRes.offer.photos[1].original);

					frame3.Content = new Image { Source = OfferRes.offer.photos[2].original, Aspect = Aspect.AspectFill };

					var streamContent3 = new StreamContent(File.OpenRead(OfferRes.offer.photos[2].original));
					var profile_img3 = new ByteArrayContent(await streamContent3.ReadAsByteArrayAsync());
					profile_img3.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img3, "photos[]", OfferRes.offer.photos[2].original);

					frame4.Content = new Image { Source = OfferRes.offer.photos[3].original, Aspect = Aspect.AspectFill };

					var streamContent4 = new StreamContent(File.OpenRead(OfferRes.offer.photos[3].original));
					var profile_img4 = new ByteArrayContent(await streamContent4.ReadAsByteArrayAsync());
					profile_img4.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img4, "photos[]", OfferRes.offer.photos[3].original);

					frame5.Content = new Image { Source = OfferRes.offer.photos[4].original, Aspect = Aspect.AspectFill };

					var streamContent5 = new StreamContent(File.OpenRead(OfferRes.offer.photos[4].original));
					var profile_img5 = new ByteArrayContent(await streamContent5.ReadAsByteArrayAsync());
					profile_img5.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img5, "photos[]", OfferRes.offer.photos[4].original);

				}
				else if (OfferRes.offer.photos.Count == 6)
				{
					frame1.Content = new Image { Source = OfferRes.offer.photos[0].original, Aspect = Aspect.AspectFill };

					var streamContent = new StreamContent(File.OpenRead(OfferRes.offer.photos[0].original));
					var profile_img = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
					profile_img.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img, "photos[]", OfferRes.offer.photos[0].original);

					frame2.Content = new Image { Source = OfferRes.offer.photos[1].original, Aspect = Aspect.AspectFill };

					var streamContent2 = new StreamContent(File.OpenRead(OfferRes.offer.photos[1].original));
					var profile_img2 = new ByteArrayContent(await streamContent2.ReadAsByteArrayAsync());
					profile_img2.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img2, "photos[]", OfferRes.offer.photos[1].original);

					frame3.Content = new Image { Source = OfferRes.offer.photos[2].original, Aspect = Aspect.AspectFill };

					var streamContent3 = new StreamContent(File.OpenRead(OfferRes.offer.photos[2].original));
					var profile_img3 = new ByteArrayContent(await streamContent3.ReadAsByteArrayAsync());
					profile_img3.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img3, "photos[]", OfferRes.offer.photos[2].original);

					frame4.Content = new Image { Source = OfferRes.offer.photos[3].original, Aspect = Aspect.AspectFill };

					var streamContent4 = new StreamContent(File.OpenRead(OfferRes.offer.photos[3].original));
					var profile_img4 = new ByteArrayContent(await streamContent4.ReadAsByteArrayAsync());
					profile_img4.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img4, "photos[]", OfferRes.offer.photos[3].original);

					frame5.Content = new Image { Source = OfferRes.offer.photos[4].original, Aspect = Aspect.AspectFill };

					var streamContent5 = new StreamContent(File.OpenRead(OfferRes.offer.photos[4].original));
					var profile_img5 = new ByteArrayContent(await streamContent5.ReadAsByteArrayAsync());
					profile_img5.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img5, "photos[]", OfferRes.offer.photos[4].original);

					frame6.Content = new Image { Source = OfferRes.offer.photos[5].original, Aspect = Aspect.AspectFill };

					var streamContent6 = new StreamContent(File.OpenRead(OfferRes.offer.photos[5].original));
					var profile_img6 = new ByteArrayContent(await streamContent6.ReadAsByteArrayAsync());
					profile_img6.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img6, "photos[]", OfferRes.offer.photos[5].original);

				}
				else if (OfferRes.offer.photos.Count == 7)
				{
					frame1.Content = new Image { Source = OfferRes.offer.photos[0].original, Aspect = Aspect.AspectFill };

					var streamContent = new StreamContent(File.OpenRead(OfferRes.offer.photos[0].original));
					var profile_img = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
					profile_img.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img, "photos[]", OfferRes.offer.photos[0].original);

					frame2.Content = new Image { Source = OfferRes.offer.photos[1].original, Aspect = Aspect.AspectFill };

					var streamContent2 = new StreamContent(File.OpenRead(OfferRes.offer.photos[1].original));
					var profile_img2 = new ByteArrayContent(await streamContent2.ReadAsByteArrayAsync());
					profile_img2.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img2, "photos[]", OfferRes.offer.photos[1].original);

					frame3.Content = new Image { Source = OfferRes.offer.photos[2].original, Aspect = Aspect.AspectFill };

					var streamContent3 = new StreamContent(File.OpenRead(OfferRes.offer.photos[2].original));
					var profile_img3 = new ByteArrayContent(await streamContent3.ReadAsByteArrayAsync());
					profile_img3.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img3, "photos[]", OfferRes.offer.photos[2].original);

					frame4.Content = new Image { Source = OfferRes.offer.photos[3].original, Aspect = Aspect.AspectFill };

					var streamContent4 = new StreamContent(File.OpenRead(OfferRes.offer.photos[3].original));
					var profile_img4 = new ByteArrayContent(await streamContent4.ReadAsByteArrayAsync());
					profile_img4.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img4, "photos[]", OfferRes.offer.photos[3].original);

					frame5.Content = new Image { Source = OfferRes.offer.photos[4].original, Aspect = Aspect.AspectFill };

					var streamContent5 = new StreamContent(File.OpenRead(OfferRes.offer.photos[4].original));
					var profile_img5 = new ByteArrayContent(await streamContent5.ReadAsByteArrayAsync());
					profile_img5.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img5, "photos[]", OfferRes.offer.photos[4].original);

					frame6.Content = new Image { Source = OfferRes.offer.photos[5].original, Aspect = Aspect.AspectFill };

					var streamContent6 = new StreamContent(File.OpenRead(OfferRes.offer.photos[5].original));
					var profile_img6 = new ByteArrayContent(await streamContent6.ReadAsByteArrayAsync());
					profile_img6.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img6, "photos[]", OfferRes.offer.photos[5].original);

					frame7.Content = new Image { Source = OfferRes.offer.photos[6].original, Aspect = Aspect.AspectFill };

					var streamContent7 = new StreamContent(File.OpenRead(OfferRes.offer.photos[6].original));
					var profile_img7 = new ByteArrayContent(await streamContent7.ReadAsByteArrayAsync());
					profile_img7.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img7, "photos[]", OfferRes.offer.photos[6].original);

				}
			}
			catch(Exception ex)
			{
				await DisplayAlert("Refresh photo error",ex.Message,"Ok");
			}
		
		}

		private async void TapGestureRecognizer_Tapped_8(object sender, EventArgs e)
		{
			string action = await DisplayActionSheet("", "Отменить", null, "Открыть", "Удалить");
			if(action == "Удалить")
			{
				if((sender as Frame) == ((sender as Frame).Parent as Grid).Children[1])
				{
					OfferRes.offer.photos.Remove(OfferRes.offer.photos[0]);

					var streamContent = new StreamContent(File.OpenRead(OfferRes.offer.photos[0].original));
					var profile_img = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
					profile_img.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img, "photos2delete[]", OfferRes.offer.photos[0].original);

					photosRefresh(OfferRes);
				}
				else if((sender as Frame) == ((sender as Frame).Parent as Grid).Children[2])
				{
					OfferRes.offer.photos.Remove(OfferRes.offer.photos[1]);

					var streamContent = new StreamContent(File.OpenRead(OfferRes.offer.photos[1].original));
					var profile_img = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
					profile_img.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img, "photos2delete[]", OfferRes.offer.photos[1].original);

					photosRefresh(OfferRes);
				}
				else if ((sender as Frame) == ((sender as Frame).Parent as Grid).Children[3])
				{
					OfferRes.offer.photos.Remove(OfferRes.offer.photos[2]);

					var streamContent = new StreamContent(File.OpenRead(OfferRes.offer.photos[2].original));
					var profile_img = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
					profile_img.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img, "photos2delete[]", OfferRes.offer.photos[2].original);

					photosRefresh(OfferRes);
				}
				else if ((sender as Frame) == ((sender as Frame).Parent as Grid).Children[4])
				{
					OfferRes.offer.photos.Remove(OfferRes.offer.photos[3]);

					var streamContent = new StreamContent(File.OpenRead(OfferRes.offer.photos[3].original));
					var profile_img = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
					profile_img.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img, "photos2delete[]", OfferRes.offer.photos[3].original);

					photosRefresh(OfferRes);
				}
				else if ((sender as Frame) == ((sender as Frame).Parent as Grid).Children[5])
				{
					OfferRes.offer.photos.Remove(OfferRes.offer.photos[4]);

					var streamContent = new StreamContent(File.OpenRead(OfferRes.offer.photos[4].original));
					var profile_img = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
					profile_img.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img, "photos2delete[]", OfferRes.offer.photos[4].original);

					photosRefresh(OfferRes);
				}
				else if ((sender as Frame) == ((sender as Frame).Parent as Grid).Children[6])
				{
					OfferRes.offer.photos.Remove(OfferRes.offer.photos[5]);

					var streamContent = new StreamContent(File.OpenRead(OfferRes.offer.photos[5].original));
					var profile_img = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
					profile_img.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img, "photos2delete[]", OfferRes.offer.photos[5].original);

					photosRefresh(OfferRes);
				}
				else if ((sender as Frame) == ((sender as Frame).Parent as Grid).Children[7])
				{
					OfferRes.offer.photos.Remove(OfferRes.offer.photos[6]);

					var streamContent = new StreamContent(File.OpenRead(OfferRes.offer.photos[6].original));
					var profile_img = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
					profile_img.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
					profile_imgTemp.Add(profile_img, "photos2delete[]", OfferRes.offer.photos[6].original);

					photosRefresh(OfferRes);
				}
			}
		}
	}
}