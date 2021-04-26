
using Foundation;
using UIKit;

namespace HelloXFPrism.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            this.LoadApplication(new App(new iOSPlatformInitializer()));

            return base.FinishedLaunching(app, options);
        }
    }
}