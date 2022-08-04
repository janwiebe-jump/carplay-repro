
using Foundation;
using System;
using System.Linq;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;

namespace MyApp.iOS
{
    [Register("SceneDelegate")]
    public class SceneDelegate : UIResponder, IUIWindowSceneDelegate
    {
        private App app;

        [Export("window")]
        public UIWindow Window { get; set; }

        [Export("scene:willConnectToSession:options:")]
        public async void WillConnect(UIScene scene, UISceneSession session, UISceneConnectionOptions connectionOptions)
        {
            // Use this method to optionally configure and attach the UIWindow `window` to the provided UIWindowScene `scene`.
            // If using a storyboard, the `window` property will automatically be initialized and attached to the scene.
            // This delegate does not imply the connecting scene or session are new (see UIApplicationDelegate `GetConfiguration` instead).


            var windowScene = scene as UIWindowScene;
            if (windowScene != null)
            {
                Window = new UIWindow(windowScene.CoordinateSpace.Bounds);
                if (Window != null)
                {
                    // This just creates a new App instance without creating a new AppDelegate
                    // In iether case I still see issues with navigation events going to the wrong window,
                    // i.e. I tap Add Item in one window, but the new item page launches in another window
                    // Or use the menu to go to the ABout page, and the About page opens in the other window
                    // This is in keeping with what I have seen in reagrds to embedding forms pages into native 
                    // projects. In this case, you need to use the platforms navigation APIs, not Xamarin.Forms
                    // navigation APIs
                    app = new App();
                    // Set the scene's RootViewController from the one X.Forms created
                    Window.RootViewController = app.MainPage.CreateViewController();



                    app.PropertyChanged += App_PropertyChanged;
                    //// This creates the Xamarin.Forms UI with a new app delegate
                    //var ad = new AppDelegate();
                    //ad.GetUI();

                    //// Set the scene's RootViewController from the one X.Forms created
                    //Window.RootViewController = ad.Window.RootViewController;


                    // Set the WindowScene
                    Window.WindowScene = windowScene;
                }
            }

            // Try to handle the deep link url in 2 cases: Via UrlContexts and UserActivities.
            // This only called when app is closed.

            
            var urlContext = connectionOptions?.UrlContexts?.ToArray().FirstOrDefault();
            if (urlContext != null && urlContext.Url != null)
            {
                var url = urlContext.Url.ToString();
                await TryHandleUrl(url);
            }


            var userActivity = connectionOptions?.UserActivities?.ToArray().FirstOrDefault(x => x.ActivityType == NSUserActivityType.BrowsingWeb);
            if (userActivity != null && userActivity.WebPageUrl != null)
            {
              
                var url = userActivity.WebPageUrl.ToString();
                await TryHandleUrl(url);
            }
        }


        [Export("scene:openURLContexts:")]
        public async void OpenUrlContexts(UIScene scene, NSSet<UIOpenUrlContext> urlContexts)
        {
            var url = urlContexts.AnyObject.Url?.ToString();
            await TryHandleUrl(url);
        }

        [Export("scene:continueUserActivity:")]
        public void ContinueUserActivity(UIScene scene, NSUserActivity userActivity)
        {
            
        }

        private void App_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(App.MainPage))
            {
                Window.RootViewController = app.MainPage.CreateViewController();
            }
        }

        [Export("sceneDidDisconnect:")]
        public void DidDisconnect(UIScene scene)
        {
            // Called as the scene is being released by the system.
            // This occurs shortly after the scene enters the background, or when its session is discarded.
            // Release any resources associated with this scene that can be re-created the next time the scene connects.
            // The scene may re-connect later, as its session was not neccessarily discarded (see UIApplicationDelegate `DidDiscardSceneSessions` instead).
        }

        [Export("sceneDidBecomeActive:")]
        public void DidBecomeActive(UIScene scene)
        {
            // Called when the scene has moved from an inactive state to an active state.
            // Use this method to restart any tasks that were paused (or not yet started) when the scene was inactive.
        }

        [Export("sceneWillResignActive:")]
        public void WillResignActive(UIScene scene)
        {
            // Called when the scene will move from an active state to an inactive state.
            // This may occur due to temporary interruptions (ex. an incoming phone call).
        }

        [Export("sceneWillEnterForeground:")]
        public void WillEnterForeground(UIScene scene)
        {
            // Called as the scene transitions from the background to the foreground.
            // Use this method to undo the changes made on entering the background.
        }

        [Export("sceneDidEnterBackground:")]
        public void DidEnterBackground(UIScene scene)
        {
            // Called as the scene transitions from the foreground to the background.
            // Use this method to save data, release shared resources, and store enough scene-specific state information
            // to restore the scene back to its current state.
        }


        private async Task TryHandleUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
               
            }
        }
    }
}