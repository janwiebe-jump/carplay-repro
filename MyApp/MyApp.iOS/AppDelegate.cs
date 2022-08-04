
using Foundation;
using UIKit;

namespace MyApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        // only used when creating a new app delegate in the scene delegate in order to be able to call 
        // LoadApplication in GetUI method
        public UIApplication application;
        public NSDictionary options;

        public override UISceneConfiguration GetConfiguration(UIApplication application, UISceneSession connectingSceneSession, UISceneConnectionOptions options)
        {
            if (connectingSceneSession.Role == UIWindowSceneSessionRole.CarTemplateApplication)
            {
                var scene = new UISceneConfiguration("CarPlay", connectingSceneSession.Role);

                //scene.DelegateClass = CarPlaySceneDelegate;
                return scene;
            }
            else
            {
                var scene = new UISceneConfiguration("Phone", connectingSceneSession.Role);
                return scene;
            }
        }

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
#if DEBUG && HOTRESTART
            ObjCRuntime.Class.ThrowOnInitFailure = false;
#endif
            
            global::Xamarin.Forms.Forms.Init();
            
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                // only used when creating a new app delegate in the scene delegate in order to be able to call 
                // LoadApplication in GetUI method
                this.application = app;
                this.options = options;

                return true;
            }



            LoadApplication(new App());

            var result = base.FinishedLaunching(app, options);

            return result;
        }


    }
}
