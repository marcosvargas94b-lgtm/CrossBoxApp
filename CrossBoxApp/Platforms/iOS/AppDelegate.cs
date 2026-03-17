using Foundation;
using UIKit;
using AVFoundation;
using Plugin.Firebase.CloudMessaging; // NECESARIO

namespace CrossBoxApp
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // --- BLOQUE MÁGICO PARA SPOTIFY ---
            try
            {
                var session = AVAudioSession.SharedInstance();
                AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Ambient);
                AVAudioSession.SharedInstance().SetActive(true);
                session.SetCategory(AVAudioSessionCategory.Playback, AVAudioSessionCategoryOptions.MixWithOthers);
                session.SetActive(true);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"Error Audio AppDelegate: {ex.Message}");
            }

            return base.FinishedLaunching(application, launchOptions);
        }

        // --- ESTOS MÉTODOS SON OBLIGATORIOS PARA QUE IOS DESPIERTE LA APP ---
        [Export("application:didRegisterForRemoteNotificationsWithDeviceToken:")]
        public void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            FirebaseCloudMessagingImplementation.RegisteredForRemoteNotifications(deviceToken);
        }

        [Export("application:didFailToRegisterForRemoteNotificationsWithError:")]
        public void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            FirebaseCloudMessagingImplementation.FailedToRegisterForRemoteNotifications(error);
        }
    }
}