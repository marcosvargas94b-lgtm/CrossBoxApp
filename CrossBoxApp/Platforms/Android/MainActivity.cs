using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Plugin.Firebase.CloudMessaging;
using Plugin.LocalNotification;
using System.Collections.Generic;

namespace CrossBoxApp
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, Exported = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    // CRÍTICO: Agregamos el IntentFilter para que Android sepa que esta Activity maneja los clics de Firebase
    [IntentFilter(new[] { "FCM_PLUGIN_ACTIVITY" }, Categories = new[] { Intent.CategoryDefault })]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            

            // 2. Manejar Cold Start
            ManejarNotificacion(Intent);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            // Manejar Background
            ManejarNotificacion(intent);
        }

        private void ManejarNotificacion(Intent intent)
        {
            if (intent == null) return;

            // --- A. INTENTAMOS CON EL PLUGIN DE FIREBASE ---
            FirebaseCloudMessagingImplementation.OnNewIntent(intent);

            // --- B. SALVAVIDAS MANUAL PARA COLD START (Si el plugin falló) ---
            if (intent.Extras != null && intent.HasExtra("action") && intent.GetStringExtra("action") == "open_spotter")
            {
                var dict = new Dictionary<string, string>();
                foreach (var key in intent.Extras.KeySet())
                {
                    dict.Add(key, intent.Extras.GetString(key) ?? "");
                }

                // Forzamos el evento al cerebro central
                MauiProgram.ProcesarDatosPush(dict);
            }

            // --- C. NOTIFICACIONES LOCALES (Tu Timer) ---
            LocalNotificationCenter.NotifyNotificationTapped(intent);
        }
    }
}