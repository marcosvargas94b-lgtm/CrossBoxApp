using CrossBoxApp.Models;
using CrossBoxApp.Models.Services;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs; // Necesario para e.Request
using Plugin.Maui.Audio;
using ZXing.Net.Maui.Controls;
using Microsoft.Maui.LifecycleEvents;
using Plugin.Firebase.CloudMessaging;
using Plugin.Firebase.Core;
using System;
using System.Collections.Generic;

#if IOS
using Plugin.Firebase.Core.Platforms.iOS;
#elif ANDROID
using Plugin.Firebase.Core.Platforms.Android;
#endif

namespace CrossBoxApp
{
    public static class MauiProgram
    {
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
                    return true;
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
            builder.Services.AddLogging(configure => configure.AddDebug());
#endif
            builder.Services.AddSingleton<LiveSessionState>();
            builder.Services.AddSingleton<SesionService>();
            builder.Services.AddScoped(sp => new System.Net.Http.HttpClient { BaseAddress = new Uri("https://api-aftrack-mx-fphnazfmahdedtcj.canadacentral-01.azurewebsites.net/"), Timeout = TimeSpan.FromMinutes(3) });
            builder.Services.AddSingleton(AudioManager.Current);

            return builder.Build();
        }

        private static void ConfigurarInterceptorPush()
        {
            // 1. ESCUCHAR FIREBASE PUSH
            CrossFirebaseCloudMessaging.Current.NotificationTapped += (sender, e) =>
            {
                ProcesarDatosPush(e.Notification.Data);
            };

            // 2. ESCUCHAR NOTIFICACIONES LOCALES (Timer de Descanso)
            LocalNotificationCenter.Current.NotificationActionTapped += (NotificationActionEventArgs e) =>
            {
                if (e.IsTapped && !string.IsNullOrEmpty(e.Request.ReturningData) && e.Request.ReturningData.StartsWith("/spotter-rescue"))
                {
                    EnviarRutaABlazor(e.Request.ReturningData);
                }
            };
        }

        // MÉTODO PÚBLICO PARA PROCESAR DICCIONARIOS (Usado por MainActivity también)
        public static void ProcesarDatosPush(IDictionary<string, string> data)
        {
            if (data != null && data.TryGetValue("action", out var actionValue) && actionValue == "open_spotter")
            {
                string nombre = data.ContainsKey("nombre") ? data["nombre"] : "Un Atleta";
                string zona = data.ContainsKey("zona") ? data["zona"] : "El Gym";
                string dist = data.ContainsKey("distintivo") && !string.IsNullOrWhiteSpace(data["distintivo"]) ? data["distintivo"] : "NA";
                string min = data.ContainsKey("minutos") ? data["minutos"] : "2";

                string urlDestino = $"/spotter-rescue/{Uri.EscapeDataString(nombre)}/{Uri.EscapeDataString(zona)}/{Uri.EscapeDataString(dist)}/{min}";

                EnviarRutaABlazor(urlDestino);
            }
        }

        private static void EnviarRutaABlazor(string urlDestino)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                // Respiro crítico para Blazor
                await System.Threading.Tasks.Task.Delay(300);

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
    }
}