using System.Diagnostics;

namespace MauiLightController;

public partial class GpsFollow : ContentPage
{
    private CancellationTokenSource _cancelTokenSource;
    private bool _isCheckingLocation = false;
    private double lon = 0;
    private double lat = 0;
    public GpsFollow()
	{
		InitializeComponent();
    }

    private async void Toggle(object sender, EventArgs e)
    {
        if (Switch.IsToggled)
        {
            _isCheckingLocation = true;
            Thread thread = new Thread(() => Track());
            thread.Start();
        }
        else
        {
            _isCheckingLocation= false;
        }
    }

    public async Task Track()
    {
        while (_isCheckingLocation)
        {
           await GetCurrentLocation();

        }
    }
    public async Task GetCurrentLocation()
    {
        try
        {
            
            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));

            _cancelTokenSource = new CancellationTokenSource();

            Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

            if (location != null)
            {
                string text =  location.Latitude.ToString() + location.Longitude.ToString();
                Label.Text = text;
                lon = location.Longitude;
                lat = location.Latitude;
            }

        }
        // Catch one of the following exceptions:
        //   FeatureNotSupportedException
        //   FeatureNotEnabledException
        //   PermissionException
        catch (Exception ex)
        {
            // Unable to get location
        }
        finally
        {
        }
    }

    private async void OnGoogleClicked(object sender, EventArgs e)
    {
        string url = "https://www.google.com/maps/place/" + lat.ToString().Replace(',', '.') + "," + lon.ToString().Replace(',', '.');
        Uri uri = new Uri(url);
        await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
    }
}