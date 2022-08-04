using CarPlay;
using Foundation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;

namespace MyApp.iOS.CarPlay
{
    [Register("CarPlaySceneDelegate")]
    public class CarPlaySceneDelegate : CPTemplateApplicationSceneDelegate
    {
        private CPInterfaceController interfaceController;


        [Export("scene:willConnectToSession:options:")]
        public override async void WillConnect(UIScene scene, UISceneSession session, UISceneConnectionOptions connectionOptions)
        {

        }

        [Export("templateApplicationScene:didConnectInterfaceController:toWindow:")]
        public override void DidConnect(CPTemplateApplicationScene templateApplicationScene, CPInterfaceController interfaceController, CPWindow window)
        {
            Boot(interfaceController).Forget();
        }

        [Export("templateApplicationScene:didConnectInterfaceController:")]
        public override void DidConnect(CPTemplateApplicationScene templateApplicationScene, CPInterfaceController interfaceController)
        {
            Boot(interfaceController).Forget();
        }

        [Export("templateApplicationScene:didDisconnectInterfaceController:")]
        public override void DidDisconnect(CPTemplateApplicationScene templateApplicationScene, CPInterfaceController interfaceController)
        {
            DisconectEvents();
        }

        [Export("templateApplicationScene:didDisconnectInterfaceController:fromWindow:")]
        public override void DidDisconnect(CPTemplateApplicationScene templateApplicationScene, CPInterfaceController interfaceController, CPWindow window)
        {
            DisconectEvents();
        }

        private void DisconectEvents()
        {

            this.interfaceController = null;
        }

        private async Task Boot(CPInterfaceController interfaceController)
        {

            this.interfaceController = interfaceController;

                
            await SetRootTemplate();
        }


        private async Task SetRootTemplate()
        {
            var templates = await GetTabTemplates();
            var tabScreen = CreateTemplate(templates);

            interfaceController.SetRootTemplate(tabScreen, animated: true, (res, error) =>
            {
            });
        }

        private CPTabBarTemplate CreateTemplate(CPTemplate[] templates)
        {
            var tabScreen = new CPTabBarTemplate(templates);
            return tabScreen;
        }


        private async Task<CPTemplate[]> GetTabTemplates()
        {
            var homeSection = await GetHomeSections();

            var firstTemplate = new CPListTemplate("Home", homeSection);


            return new CPTemplate[] { firstTemplate };
        }

        private async Task<CPListSection[]> GetHomeSections()
        {
            // Simulate our network calls to retrieve actual data.
            await Task.Delay(200);

            var section = new CPListSection[]
            {
                CreateImageRowSection(),
            };

            return section;
        }

        private CPListSection CreateImageRowSection()
        {
            var data1 = NSData.FromUrl(new NSUrl("https://unsplash.com/photos/F4afVdyrlz4/download?ixid=MnwxMjA3fDB8MXxhbGx8fHx8fHx8fHwxNjU5NjE5NDAz&force=true&w=640"));
            var data2= NSData.FromUrl(new NSUrl("https://unsplash.com/photos/6W-MS6havNk/download?ixid=MnwxMjA3fDB8MXxhbGx8fHx8fHx8fHwxNjU5NjE5NDA0&force=true&w=640"));


            var images = Enumerable.Range(1, 4).Take((int)CPListImageRowItem.MaximumNumberOfGridImages).Select((x) =>
            {
                return UIImage.LoadFromData(x % 2 == 0 ? data1 : data2);
            }).ToArray();


            var item = new CPListImageRowItem("Images", images);

            item.ListImageRowHandler = new CPListImageRowItemHandler(ImageListRowItemHandler);
           
            // We need to hack a little: https://github.com/xamarin/xamarin-macios/issues/9996#issuecomment-1182188805
            var rowItem = ObjCRuntime.Runtime.GetINativeObject<CPListItem>(item.Handle, true, false);

            var section = new CPListSection(new CPListItem[] { rowItem });

            return section;
        }

        private void ImageListRowItemHandler(CPListImageRowItem item, nint index, Action completionHandler)
        {
            this.interfaceController.PushTemplate(CPNowPlayingTemplate.SharedTemplate, true);
                
            completionHandler();
        }
    }

    public static class TaskExt
    {
        public static void Forget(this Task task)
        {
            // note: this code is inspired by a tweet from Ben Adams: https://twitter.com/ben_a_adams/status/1045060828700037125
            // Only care about tasks that may fault (not completed) or are faulted,
            // so fast-path for SuccessfullyCompleted and Canceled tasks.
            if (!task.IsCompleted || task.IsFaulted)
            {
                // use "_" (Discard operation) to remove the warning IDE0058: Because this call is not awaited, execution of the current method continues before the call is completed
                // https://docs.microsoft.com/en-us/dotnet/csharp/discards#a-standalone-discard
                _ = ForgetAwaited(task);
            }


        }

        // Allocate the async/await state machine only when needed for performance reason.
        // More info about the state machine: https://blogs.msdn.microsoft.com/seteplia/2017/11/30/dissecting-the-async-methods-in-c/?WT.mc_id=DT-MVP-5003978
        async static Task ForgetAwaited(Task task)
        {
            try
            {
                // No need to resume on the original SynchronizationContext, so use ConfigureAwait(false)
                await task.ConfigureAwait(false);
            }
            catch
            {
                // Nothing to do here
            }
        }
    }
}