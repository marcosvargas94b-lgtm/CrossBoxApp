using ZXing.Net.Maui;

namespace CrossBoxApp;

public partial class EscanerPage : ContentPage
{
    public event Action<string> OnScanResult;
    private bool procesando = false; // Candado para evitar lecturas múltiples

    public EscanerPage()
    {
        InitializeComponent();

        cameraView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormat.QrCode,
            AutoRotate = true,
            Multiple = false
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        procesando = false; // Reiniciamos el candado al abrir
        await CheckCameraPermission();
    }

    // --- NUEVO: Limpieza de memoria al cerrar ---
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        cameraView.IsDetecting = false; // Apagar cámara
    }

    private async Task CheckCameraPermission()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.Camera>();
        }

        if (status == PermissionStatus.Granted)
        {
            cameraView.IsDetecting = true;
        }
        else
        {
            await DisplayAlert("Permiso requerido", "Necesitamos acceso a la cámara.", "OK");
            await Navigation.PopModalAsync();
        }
    }

    private void Camera_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        // 1. Si ya leímos uno, ignoramos el resto para no saturar
        if (procesando) return;

        var resultado = e.Results.FirstOrDefault();

        if (resultado != null && !string.IsNullOrEmpty(resultado.Value))
        {
            procesando = true; // Bloqueamos lecturas futuras

            MainThread.BeginInvokeOnMainThread(() =>
            {
                // 2. Apagamos detección visual
                cameraView.IsDetecting = false;

                // 3. ¡SOLO INVOCAMOS EL EVENTO! 
                // NO hacemos PopModalAsync aquí. Dejamos que Home.razor cierre la ventana.
                // Esto evita el conflicto de doble cierre.
                OnScanResult?.Invoke(resultado.Value);
            });
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        // Este botón "Cancelar" sí puede cerrar la ventana porque no envía resultados
        await Navigation.PopModalAsync();
    }
}