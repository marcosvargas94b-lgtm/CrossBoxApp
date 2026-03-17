using CrossBoxApp.Models;
using CrossBoxApp.Models.Services;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;
using Plugin.Maui.Audio;
using ZXing.Net.Maui.Controls;
using Microsoft.Maui.LifecycleEvents;
using Plugin.Firebase.CloudMessaging;
using Plugin.Firebase.Core;
using System;
using System.Collections.Generic;

#if IOS
using Plugin.Firebase.Core.Platforms.iOS;
using UIKit;       // <--- NUEVO: Para el salvavidas manual de iOS
using Foundation;  // <--- NUEVO: Para el salvavidas manual de iOS
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

            // 1. ¡CRÍTICO! Encendemos los radares AQUÍ AFUERA, antes de que arranque cualquier plataforma
            // para no perder ningún toque en segundo plano.
            ConfigurarInterceptorPush();

            builder.ConfigureLifecycleEvents(events =>
            {
#if IOS
                events.AddiOS(iOS => iOS.FinishedLaunching((app, launchOptions) => {
                    CrossFirebase.Initialize(); 
                    
                    // --- 2. SALVAVIDAS MANUAL PARA COLD START (App 100% cerrada) ---
                    if (launchOptions != null && launchOptions.ContainsKey(UIApplication.LaunchOptionsRemoteNotificationKey))
                    {
                        var pushPayload = launchOptions[UIApplication.LaunchOptionsRemoteNotificationKey] as NSDictionary;
                        if (pushPayload != null)
                        {
                            var dict = new Dictionary<string, string>();
                            foreach (var key in pushPayload.Keys)
                            {
                                dict[key.ToString()] = pushPayload[key]?.ToString() ?? "";
                            }
                            // Inyectamos a la fuerza los datos a Blazor
                            ProcesarDatosPush(dict);
                        }
                    }
                    // ---------------------------------------------------------------
                    
                    return true;
                }));
#elif ANDROID
                events.AddAndroid(android => android.OnCreate((activity, state) =>
                {
                    CrossFirebase.Initialize(activity, () => activity); 
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
            // 1. ESCUCHAR FIREBASE PUSH (App Minimizada)
#if ANDROID || IOS
            // 1. ESCUCHAR FIREBASE PUSH (App Minimizada)
            // Esto solo compilará y se ejecutará en celulares, Windows lo ignorará.
            CrossFirebaseCloudMessaging.Current.NotificationTapped += (sender, e) =>
            {
                if (e.Notification != null && e.Notification.Data != null)
                {
                    ProcesarDatosPush(e.Notification.Data);
                }
            };
#endif

            // 2. ESCUCHAR NOTIFICACIONES LOCALES (Timer / App Abierta)
            LocalNotificationCenter.Current.NotificationActionTapped += (NotificationActionEventArgs e) =>
            {
                if (e.IsTapped && !string.IsNullOrEmpty(e.Request.ReturningData) && e.Request.ReturningData.StartsWith("/spotter-rescue"))
                {
                    EnviarRutaABlazor(e.Request.ReturningData);
                }
            };
        }

        // MÉTODO PÚBLICO PARA PROCESAR DICCIONARIOS
        public static void ProcesarDatosPush(IDictionary<string, string> data)
        {
            if (data != null && data.TryGetValue("action", out var actionValue) && actionValue == "open_spotter")
            {
                string nombre = data.ContainsKey("nombre") ? data["nombre"] : "Atleta";
                string zona = data.ContainsKey("zona") ? data["zona"] : "Gym";
                string dist = data.ContainsKey("distintivo") ? data["distintivo"] : "NA";
                string min = data.ContainsKey("minutos") ? data["minutos"] : "2";
                string ts = data.ContainsKey("timestamp") ? data["timestamp"] : "0"; // <--- Atrapamos el tiempo

                string urlDestino = $"/spotter-rescue?Nombre={Uri.EscapeDataString(nombre)}&Zona={Uri.EscapeDataString(zona)}&Distintivo={Uri.EscapeDataString(dist)}&Minutos={min}&Ts={ts}";

                EnviarRutaABlazor(urlDestino);
            }
        }

        private static void EnviarRutaABlazor(string urlDestino)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                // Aumentamos el respiro a 800ms para asegurar que Blazor en iOS termine de cargar su UI pesada
                await System.Threading.Tasks.Task.Delay(800);

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