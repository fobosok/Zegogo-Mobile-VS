using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Zegogo.pages;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Essentials;

namespace Zegogo
{
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class MainPage : CarouselPage
	{
		HomePage homePage;
		public MainPage()
		{
			InitializeComponent();
			SecureStorage.SetAsync("is start", "yes");
			homePage = new HomePage();
		}
		private async void Button_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(homePage, false);
			var existingPages = Navigation.NavigationStack.ToList();
			foreach (var page in existingPages)
			{
				Navigation.RemovePage(page);
			}
		}
	}
}
