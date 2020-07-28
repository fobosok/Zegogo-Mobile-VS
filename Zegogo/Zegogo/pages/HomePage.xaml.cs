using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Zegogo.pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : TabbedPage
	{
		public HomePage()
		{
			InitializeComponent();
			Children.Add(new advertisement());
			if (SecureStorage.GetAsync("is login").Result == "yes")
			{
				Children.Add(new favorites());
				Children.Add(new add());
				Children.Add(new messages());
				Children.Add(new profile());
			}
			else
			{
				Children.Add(new loginPage { IconImageSource = "tab2"});
				Children.Add(new loginPage { IconImageSource = "tab3"});
				Children.Add(new loginPage { IconImageSource = "tab4"});
				Children.Add(new loginPage { IconImageSource = "tab5"});
			}
		}
	}
}