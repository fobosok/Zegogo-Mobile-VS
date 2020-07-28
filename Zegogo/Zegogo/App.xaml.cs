using FormsControls.Base;
using System;
using System.Globalization;
using System.Threading;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Zegogo.pages;
using Zegogo.Resources;

namespace Zegogo
{
	public partial class App : Application
	{
		public App()
		{
			Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjcwOTUwQDMxMzgyZTMxMmUzMG5ScGhlQmZXVVNVWWVpWmtRL2tKVC9acXVyckR1b3R4RXVpM2x4ano4Mm89;MTk5ODczQDMxMzcyZTM0MmUzMENaOFQvOXdZeCtYdGF0VzVOdHRsdmN3eHBUMGY3dDhYQWxnNm5kVzJJckU9");

			if (SecureStorage.GetAsync("lang").Result == "en" || SecureStorage.GetAsync("lang").Result == "ru" || SecureStorage.GetAsync("lang").Result == "et" || SecureStorage.GetAsync("lang").Result == "fi" || SecureStorage.GetAsync("lang").Result == "lt" || SecureStorage.GetAsync("lang").Result == "lv")
			{

			}
			else
			{
				SecureStorage.SetAsync("lang", "en");
			}

			Thread.CurrentThread.CurrentUICulture = new CultureInfo(SecureStorage.GetAsync("lang").Result);
			AppResources.Culture = new CultureInfo(SecureStorage.GetAsync("lang").Result);

			InitializeComponent();
			
			if(SecureStorage.GetAsync("is start").Result == "yes")
			{
				MainPage = new AnimationNavigationPage(new HomePage());
			}
			else
			{
				MainPage = new AnimationNavigationPage(new MainPage());
			}
		}
		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
