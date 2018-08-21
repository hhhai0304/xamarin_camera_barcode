using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace TestSwitchCamera
{
    public class ScanPage : ContentPage
    {
        private ZXingScannerPage scanPage;
        private Button buttonScanDefaultOverlay;
        private Button buttonScanCustomOverlay;
        private Button buttonScanContinuously;
        private readonly Button buttonScanCustomPage;
        private readonly Button buttonGenerateBarcode;

        public ScanPage() : base()
        {
            buttonScanDefaultOverlay = new Button
            {
                Text = "Scan with Default Overlay",
                AutomationId = "scanWithDefaultOverlay",
            };
            buttonScanDefaultOverlay.Clicked += async delegate
            {
                scanPage = new ZXingScannerPage();
                scanPage.OnScanResult += (result) =>
                {
                    scanPage.IsScanning = false;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Navigation.PopAsync();
                        DisplayAlert("Scanned Barcode", result.Text, "OK");
                    });
                };

                await Navigation.PushAsync(scanPage);
            };


            buttonScanCustomOverlay = new Button
            {
                Text = "Scan with Custom Overlay",
                AutomationId = "scanWithCustomOverlay",
            };
            buttonScanCustomOverlay.Clicked += async delegate
            {
                // Create our custom overlay
                var customOverlay = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };
                var torch = new Button
                {
                    Text = "Toggle Torch"
                };
                torch.Clicked += delegate
                {
                    scanPage.ToggleTorch();
                };
                customOverlay.Children.Add(torch);

                scanPage = new ZXingScannerPage(customOverlay: customOverlay);
                scanPage.OnScanResult += (result) =>
                {
                    scanPage.IsScanning = false;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Navigation.PopAsync();
                        DisplayAlert("Scanned Barcode", result.Text, "OK");
                    });
                };
                await Navigation.PushAsync(scanPage);
            };


            buttonScanContinuously = new Button
            {
                Text = "Scan Continuously",
                AutomationId = "scanContinuously",
            };
            buttonScanContinuously.Clicked += async delegate
            {
                scanPage = new ZXingScannerPage();
                scanPage.OnScanResult += (result) =>
                    Device.BeginInvokeOnMainThread(() =>
                       DisplayAlert("Scanned Barcode", result.Text, "OK"));

                await Navigation.PushAsync(scanPage);
            };

            buttonScanCustomPage = new Button
            {
                Text = "Scan with Custom Page",
                AutomationId = "scanWithCustomPage",
            };
            /*
			buttonScanCustomPage.Clicked += async delegate {
				var customScanPage = new CustomScanPage();
				await Navigation.PushAsync(customScanPage);
			};
*/

            buttonGenerateBarcode = new Button
            {
                Text = "Barcode Generator",
                AutomationId = "barcodeGenerator",
            };
            /*
            buttonGenerateBarcode.Clicked += async delegate {
				await Navigation.PushAsync(new BarcodePage());
			};
*/
            var stack = new StackLayout();
            stack.Children.Add(buttonScanDefaultOverlay);
            stack.Children.Add(buttonScanCustomOverlay);
            stack.Children.Add(buttonScanContinuously);
            stack.Children.Add(buttonScanCustomPage);
            stack.Children.Add(buttonGenerateBarcode);

            Content = stack;
        }
    }
}