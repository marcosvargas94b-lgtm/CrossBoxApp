using CrossBoxApp.Models;
using CrossBoxApp.Models.Services;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using Plugin.Maui.Audio;
using ZXing.Net.Maui.Controls;
using Microsoft.Maui.LifecycleEvents;
using Plugin.Firebase.CloudMessaging;
using Plugin.Firebase.Core;
using System; // IMPORTANTE PARA EL ACTION

#if IOS
using Plugin.Firebase.Core.Platforms.iOS;
#elif ANDROID
using Plugin.Firebase.Core.Platforms.Android;
#endif

namespace CrossBoxApp
{
    public static class MauiProgram
    {
        // =======================================================
        // EL PUENTE MÁGICO BLAZOR <-> MAUI
        // =======================================================
        public static Action<string> InterceptSpotterAction { get; set; }
        public static string RutaPendienteSpotter { get; set; }

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseBarcodeReader()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

#if ANDROID || IOS
            builder.UseLocalNotification();
#endif

            builder.ConfigureLifecycleEvents(events =>
            {
#if IOS
                events.AddiOS(iOS => iOS.FinishedLaunching((app, launchOptions) => {
                    CrossFirebase.Initialize(); 
                    ConfigurarInterceptorPush(); 
                    return true; // <--- ¡CRÍTICO! DEBE SER TRUE, SI NO APPLE MATA LA NOTIFICACIÓN
                }));
#elif ANDROID
                events.AddAndroid(android => android.OnCreate((activity, state) =>
                {
                    CrossFirebase.Initialize(activity, () => activity);
                    ConfigurarInterceptorPush();
                }));
#endif
            });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<LiveSessionState>();
            builder.Services.AddSingleton<SesionService>();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://api-aftrack-mx-fphnazfmahdedtcj.canadacentral-01.azurewebsites.net/"), Timeout = TimeSpan.FromMinutes(3) });
            builder.Services.AddSingleton(AudioManager.Current);

            return builder.Build();
        }

        // =======================================================
        // EL CEREBRO DE LAS NOTIFICACIONES (Limpio y Extraído)
        // =======================================================
        private static void ConfigurarInterceptorPush()
        {
            CrossFirebaseCloudMessaging.Current.NotificationTapped += (sender, e) =>
            {
                var data = e.Notification.Data;
                if (data.ContainsKey("action") && data["action"] == "open_spotter")
                {
                    string nombre = data.ContainsKey("nombre") ? data["nombre"] : "Un Atleta";
                    string zona = data.ContainsKey("zona") ? data["zona"] : "El Gym";

                    // ARREGLO 2: En vez de un espacio " ", mandamos "NA" para que Blazor no se confunda con el %20 en la URL
                    string dist = data.ContainsKey("distintivo") && !string.IsNullOrWhiteSpace(data["distintivo"]) ? data["distintivo"] : "NA";
                    string min = data.ContainsKey("minutos") ? data["minutos"] : "2";

                    string urlDestino = $"/spotter-rescue/{Uri.EscapeDataString(nombre)}/{Uri.EscapeDataString(zona)}/{Uri.EscapeDataString(dist)}/{min}";

                    // ARREGLO 3: OBLIGATORIO usar MainThread para cambiar la UI de Blazor desde un evento en segundo plano
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        if (InterceptSpotterAction != null)
                        {
                            InterceptSpotterAction.Invoke(urlDestino);
                        }
                        else
                        {
                            RutaPendienteSpotter = urlDestino;
                        }
                    });
                }
            };
        }
    }
}