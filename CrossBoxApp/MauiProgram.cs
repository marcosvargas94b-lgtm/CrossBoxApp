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
                    ConfigurarInterceptorPush(); // Llamamos al cerebro
                    return false;
                }));
#elif ANDROID
                events.AddAndroid(android => android.OnCreate((activity, state) =>
                {
                    CrossFirebase.Initialize(activity, () => activity);
                    ConfigurarInterceptorPush(); // Llamamos al cerebro
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
                    string dist = data.ContainsKey("distintivo") && !string.IsNullOrWhiteSpace(data["distintivo"]) ? data["distintivo"] : " ";
                    string min = data.ContainsKey("minutos") ? data["minutos"] : "2";

                    // Armamos la ruta exacta a la que Blazor debe ir
                    string urlDestino = $"/spotter-rescue/{Uri.EscapeDataString(nombre)}/{Uri.EscapeDataString(zona)}/{Uri.EscapeDataString(dist)}/{min}";

                    // CASO 1: Si la app ya estaba abierta en segundo plano, le avisamos a Blazor que navegue
                    if (InterceptSpotterAction != null)
                    {
                        InterceptSpotterAction.Invoke(urlDestino);
                    }
                    // CASO 2: Si la app estaba totalmente CERRADA, guardamos la ruta en memoria para cuando Blazor termine de arrancar
                    else
                    {
                        RutaPendienteSpotter = urlDestino;
                    }
                }
            };
        }
    }
}