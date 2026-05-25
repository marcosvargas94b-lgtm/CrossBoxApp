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
using Microsoft.AspNetCore.Components.WebView.Maui;
#if IOS
using Plugin.Firebase.Core.Platforms.iOS;
using UIKit;       
using Foundation;  
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

            // ❌ ELIMINAMOS ConfigurarInterceptorPush() DE AQUÍ.
            // Tocaba Firebase antes de que iOS estuviera listo.

            builder.ConfigureLifecycleEvents(events =>
            {
#if IOS
                events.AddiOS(iOS => iOS.FinishedLaunching((app, launchOptions) => {
                    // 1. PRIMERO INICIALIZAMOS FIREBASE NATIVAMENTE
                    CrossFirebase.Initialize(); 
                    
                    // 2. AHORA SÍ, YA PODEMOS ENCENDER LOS RADARES SEGUROS
                    ConfigurarInterceptorPush();
                    
                    // --- SALVAVIDAS MANUAL PARA COLD START (App 100% cerrada) ---
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
                            ProcesarDatosPush(dict);
                        }
                    }
                    return true;
                }));
#elif ANDROID
                events.AddAndroid(android => android.OnCreate((activity, state) =>
                {
                    // 1. INICIALIZAMOS
                    CrossFirebase.Initialize(activity, () => activity); 
                    
                    // 2. ENCENDEMOS RADARES
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
            builder.Services.AddScoped(sp => new System.Net.Http.HttpClient { BaseAddress = new Uri("https://api-aftrack-linux-dkawb7h7f9dxeff7.canadacentral-01.azurewebsites.net/"), Timeout = TimeSpan.FromMinutes(3) });
            builder.Services.AddSingleton(AudioManager.Current);
#if IOS
            Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebViewHandler.BlazorWebViewMapper.AppendToMapping("EnableSwipeBack", (handler, view) =>
            {
                handler.PlatformView.AllowsBackForwardNavigationGestures = true;
            });
#endif

            return builder.Build();
        }

        private static void ConfigurarInterceptorPush()
        {
#if ANDROID || IOS
            CrossFirebaseCloudMessaging.Current.NotificationTapped += (sender, e) =>
            {
                if (e.Notification != null && e.Notification.Data != null)
                {
                    ProcesarDatosPush(e.Notification.Data);
                }
            };
#endif

            LocalNotificationCenter.Current.NotificationActionTapped += (NotificationActionEventArgs e) =>
            {
                if (e.IsTapped && !string.IsNullOrEmpty(e.Request.ReturningData) && e.Request.ReturningData.StartsWith("/spotter-rescue"))
                {
                    EnviarRutaABlazor(e.Request.ReturningData);
                }
            };
        }

        public static void ProcesarDatosPush(IDictionary<string, string> data)
        {
            if (data != null && data.TryGetValue("action", out var actionValue) && actionValue == "open_spotter")
            {
                string nombre = data.ContainsKey("nombre") ? data["nombre"] : "Atleta";
                string zona = data.ContainsKey("zona") ? data["zona"] : "Gym";
                string dist = data.ContainsKey("distintivo") ? data["distintivo"] : "NA";
                string min = data.ContainsKey("minutos") ? data["minutos"] : "2";
                string ts = data.ContainsKey("timestamp") ? data["timestamp"] : "0";

                string urlDestino = $"/spotter-rescue?Nombre={Uri.EscapeDataString(nombre)}&Zona={Uri.EscapeDataString(zona)}&Distintivo={Uri.EscapeDataString(dist)}&Minutos={min}&Ts={ts}";

                EnviarRutaABlazor(urlDestino);
            }
        }

        public static bool AcaboDePedirAyuda { get; set; } = false;

        private static void EnviarRutaABlazor(string urlDestino)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                RutaPendienteSpotter = urlDestino;
            });
        }
    }
}