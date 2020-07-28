
using Android.Content;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Zegogo.Droid;
using Zegogo;

[assembly: ExportRenderer(typeof(NoUnderlineSearchBar), typeof(NoUnderlineSearchBarRenderer))]

namespace Zegogo.Droid
{
	[System.Obsolete]
	public class NoUnderlineSearchBarRenderer : SearchBarRenderer
	{

		protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
		{
			base.OnElementChanged(e);
			if (Control != null)
			{
				var plateId = Resources.GetIdentifier("android:id/search_plate", null, null);
				var plate = Control.FindViewById(plateId);
				plate.SetBackgroundColor(Android.Graphics.Color.Transparent);

				var searchView = Control;
				searchView.Iconified = true;
				searchView.SetIconifiedByDefault(false);
				// (Resource.Id.search_mag_icon); is wrong / Xammie bug


				int searchIconId = Context.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
				var icon = (ImageView)searchView.FindViewById(searchIconId);
				icon.LayoutParameters = new LinearLayout.LayoutParams(0, 0);
			}
		}
	}
}