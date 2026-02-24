using Foundation;
using UIKit;
using AVFoundation; // <--- AGREGA ESTO IMPORTANTE

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

                // 1. Configuramos la sesión ANTES de que arranque la UI
                // Category: Playback (para sonar en silencio)
                // Options: MixWithOthers (CLAVE: No pausar Spotify)
                AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Ambient);
                AVAudioSession.SharedInstance().SetActive(true);
                session.SetCategory(AVAudioSessionCategory.Playback, AVAudioSessionCategoryOptions.MixWithOthers);

                // 2. Activamos la sesión
                session.SetActive(true);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"Error Audio AppDelegate: {ex.Message}");
            }
            // ----------------------------------

            return base.FinishedLaunching(application, launchOptions);
        }
    }
}