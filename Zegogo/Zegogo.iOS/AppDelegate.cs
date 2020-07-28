using Syncfusion.XForms.iOS.Chat;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.XForms.iOS.TabView;
using Syncfusion.XForms.Pickers.iOS;
using Syncfusion.SfAutoComplete.XForms.iOS;
using Syncfusion.SfNumericTextBox.XForms.iOS;
using Syncfusion.XForms.iOS.ComboBox;
using Syncfusion.XForms.iOS.Border;
using Syncfusion.SfCarousel.XForms.iOS;
using Syncfusion.XForms.iOS.Shimmer;
using Syncfusion.SfBusyIndicator.XForms.iOS;
using Syncfusion.XForms.iOS.Buttons;
using Syncfusion.XForms.iOS.TextInputLayout;
using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace Zegogo.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
global::Xamarin.Forms.Forms.Init();
SfChatRenderer.Init();
SfListViewRenderer.Init();
SfTabViewRenderer.Init();
SfDatePickerRenderer.Init();
SfAutoCompleteRenderer.Init();
SfNumericTextBoxRenderer.Init();
SfComboBoxRenderer.Init();
SfBorderRenderer.Init();
SfCarouselRenderer.Init();
SfShimmerRenderer.Init();
SfBusyIndicatorRenderer.Init();
SfCheckBoxRenderer.Init();
SfButtonRenderer.Init();
SfTextInputLayoutRenderer.Init();
			FormsControls.Touch.Main.Init();

			LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
