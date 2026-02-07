using CrossBoxApp.Models;
using CrossBoxApp.Models.Services;
using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;
using Microsoft.Maui.LifecycleEvents;

#if IOS
using AVFoundation;
using UIKit;
#endif
namespace CrossBoxApp
{
    public static class MauiProgram
    {
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
            // --- AGREGA ESTE BLOQUE EXACTAMENTE AQUÍ ---
#if IOS
    builder.ConfigureLifecycleEvents(events =>
    {
        events.AddiOS(ios => ios.FinishedLaunching((app, launchOptions) =>
        {
            // Esto fuerza al iPhone a usar el volumen de "Media" y ignorar el switch de silencio
            var session = AVAudioSession.SharedInstance();
            session.SetCategory(AVAudioSessionCategory.Playback);
            session.SetActive(true);
            return true;
        }));
    });
#endif
            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<LiveSessionState>();
            builder.Services.AddSingleton<SesionService>();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://api-aftrack-mx-fphnazfmahdedtcj.canadacentral-01.azurewebsites.net/") });
            return builder.Build();
        }
    }
}
