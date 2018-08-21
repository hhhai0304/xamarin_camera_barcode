using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace TestSwitchCamera
{
    public partial class MainPage : ContentPage
    {
        private ZXingScannerPage scanPage;
        public MainPage()
        {
            InitializeComponent();
            scanPage = new ZXingScannerPage();
            btnCamera.Clicked += async (sender, args) =>
            {
                await ScanNow();
            };

            scanPage.OnScanResult += (result) =>
            {
                // Stop scanning
                scanPage.IsScanning = false;

                // Pop the page and show the result
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    await DisplayAlert("Scanned Barcode", result.Text, "OK");
                });
            };
        }

        private async Task ScanNow()
        {
            MobileBarcodeScanner scanner = new MobileBarcodeScanner();
            Result result = null;
            bool isBack = false;

            TimeSpan timeSpan = new TimeSpan(0, 0, 0, 5, 0);
            Device.StartTimer(timeSpan, () =>
            {
                if (result == null && !isBack)
                {
                    scanner.AutoFocus();
                    return true;
                }
                return false;
            });

            result = await scanner.Scan();
            isBack = true;

            if (result != null)
            {
                await DisplayAlert("Scanned Barcode", result.Text, "OK");
            }

            //await Application.Current.MainPage.Navigation.PushAsync(new ScanPage());
        }

        private async Task TakePicture()
        {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }

            if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
            {
                var storage = DependencyService.Get<IStorage>();
                string path = storage.GetPath();
                StoreCameraMediaOptions haha = new StoreCameraMediaOptions();

                var file = await CrossMedia.Current.TakePhotoAsync(haha);

                if (file == null)
                {
                    return;
                }

                await DisplayAlert("File Location", file.Path, "OK");

                img.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
            else
            {
                await DisplayAlert("Permissions Denied", "Unable to take photos.", "OK");
                //On iOS you may want to send your user to the settings screen.
                //CrossPermissions.Current.OpenAppSettings();
            }
        }
    }
}