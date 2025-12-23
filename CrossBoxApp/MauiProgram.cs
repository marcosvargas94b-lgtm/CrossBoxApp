using CrossBoxApp.Models;
using CrossBoxApp.Models.Services;
using Microsoft.Extensions.Logging;

namespace CrossBoxApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            // Configurar cliente HTTP para apuntar a tu API
            // IMPORTANTE: Si pruebas en Android Emulator, localhost es "10.0.2.2"
            // Si pruebas en Windows, es "localhost".
            // Lo ideal es que ya hayas publicado tu web a Azure y pongas esa URL aquí.

            builder.Services.AddSingleton<SesionService>();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://apitcb.pladse.dsistemas.educacionchiapas.gob.mx/") });
            return builder.Build();
        }
    }
}
