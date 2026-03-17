using Foundation;
using UIKit;
using AVFoundation;
using UserNotifications; // <--- VITAL PARA PEDIR PERMISO A APPLE

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

            // --- ¡LA LLAVE DE APPLE! ---
            // Esto le dice al iPhone que autorice a la app a recibir alertas REMOTAS de Firebase
            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound, (granted, error) =>
            {
                if (granted)
                {
                    // Una vez autorizado, INSCRIBIMOS el hardware en los servidores de Apple
                    InvokeOnMainThread(() => UIApplication.SharedApplication.RegisterForRemoteNotifications());
                }
            });

            return base.FinishedLaunching(application, launchOptions);
        }
    }
}