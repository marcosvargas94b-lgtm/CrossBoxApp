using ZXing.Net.Maui;

namespace CrossBoxApp;

public partial class EscanerPage : ContentPage
{
   
    public event Action<string> OnScanResult;

    public EscanerPage()
    {
        InitializeComponent();

     
        cameraView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormat.QrCode,
            AutoRotate = true,
            Multiple = false
        };

     
        cameraView.IsDetecting = false;
    }

  
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CheckCameraPermission();
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
   
            await DisplayAlert("Permiso requerido", "Necesitamos acceso a la cámara para escanear el código QR.", "OK");
            await Navigation.PopModalAsync();
        }
    }

    private void Camera_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
  
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            var resultado = e.Results.FirstOrDefault();
            if (resultado != null && !string.IsNullOrEmpty(resultado.Value))
            {
            
                cameraView.IsDetecting = false;

             
                OnScanResult?.Invoke(resultado.Value);

            
                await Navigation.PopModalAsync();
            }
        });
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}