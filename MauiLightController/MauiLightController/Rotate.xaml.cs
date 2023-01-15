namespace MauiLightController;

using Controller;
using System.Diagnostics;

public partial class Rotate : ContentPage
{
    Stopwatch stopwatch;
    Stopwatch LocationTimer;
    public bool Active = false;
    private CancellationTokenSource _cancelTokenSource;
    private bool _isCheckingLocation;
    private double lat = 51.445794;
    private double lon = 5.458397;
    private int lastRotation;
    private int conesize = 20;
    Dictionary<string, int> Lightpositions = new Dictionary<string, int>();
    public Rotate()
    {
        foreach(Light light in Controller.Lights)
        {
            if (light.Name != "Broeinest")
            {
                Controller.ChangeColor(light.Id, new int[] { 255, 255, 255 });
                light.CalculateAngle(lon, lat);
            }
        }
        stopwatch = Stopwatch.StartNew();
        LocationTimer = Stopwatch.StartNew();
        InitializeComponent();
        if (Compass.Default.IsSupported)
        {
            if (!Compass.Default.IsMonitoring)
            {
                // Turn on compass
                Compass.Default.ReadingChanged += Compass_ReadingChanged;
                Compass.Default.Start(SensorSpeed.Fastest);
            }
            else
            {
                // Turn off compass
                Compass.Default.Stop();
                Compass.Default.ReadingChanged -= Compass_ReadingChanged;
            }
        }
    }
    private async void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
    {
        // Update UI Label with compass state
        if (stopwatch.ElapsedMilliseconds > 100 && Active)
        {

            int rotation = (int)e.Reading.HeadingMagneticNorth;
            if (!_isCheckingLocation && LocationTimer.ElapsedMilliseconds > 10000 && Switch.IsToggled)
            {
                Thread thread = new Thread(() => GetCurrentLocation()); 
                thread.Start();
            }
            if (rotation > lastRotation + 1 || rotation < lastRotation - 1)
            {
                if (lon != 0 && lat != 0)
                {
                    foreach (Light light in Controller.Lights)
                    {
                        if(light.Name != "Broeinest")
                        {
                            if (light.Id == "3v4kD62VpTAHAmMFW536hQ")
                            {
                                Img1.Rotation = light.Angle - rotation;
                            }

                            if (light.Angle < rotation + conesize && light.Angle > rotation - conesize)
                            {
                                if (!light.OnOff)
                                {
                                    light.Toggle();
                                }
                            }
                            else if(light.OnOff)
                            {
                                if (light.OnOff)
                                {
                                    light.Toggle();
                                }
                            }
                        }
                    }
                    CompassLabel.Text = "Angle: " + rotation + "\n" + lon + "  " + lat;
                }
                lastRotation = rotation;
            }
            stopwatch.Restart();
        }
    }

    private int CalculateAngle(double lon1, double lat1, double lon2, double lat2)
    {
        double angle = -400;
        double aanliggend = lat2 - lat1;
        double overstaand = lon2 - lon1;
        angle = Math.Atan(overstaand / aanliggend) * 180 / Math.PI;
        if (lat1 <= lat2)
        {
            if (angle < 0) angle += 360;
        }
        else
        {
            angle += 180;
        }

        return (int)angle;
    }

    public async Task GetCurrentLocation()
    {
        try
        {
            _isCheckingLocation = true;

            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));

            _cancelTokenSource = new CancellationTokenSource();

            Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

            if (location != null)
            {
                Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                Location location2 = new Location(lat, lon);
                if(Location.CalculateDistance(location,location2,DistanceUnits.Kilometers) > 0.005)
                {

                }
                lon =location.Longitude;
                lat =location.Latitude;
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
            _isCheckingLocation = false;
        }
    }

    public void CancelRequest()
    {
        if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
            _cancelTokenSource.Cancel();
    }
    private void OnActivateClicked(object sender, EventArgs e)
    {
        Active = !Active;
        if (Active)
        {
            ActivateBtn.Text = "Deactivate";
        }
        else
        {
            ActivateBtn.Text = "Activate";
        }
    }

    private void ConeSizeSliderChanged(object sender, EventArgs e)
    {
        ConeSizeLabel.Text = "Cone Size: " + (int)SliderConeSize.Value;
        conesize = (int)SliderConeSize.Value;
    }

    private void Toggle(object sender, EventArgs e)
    {
        if (!Switch.IsToggled)
        {
            lat = 51.4458;
            lon = 5.4584;
        }
    }

    private async void OnGoogleClicked(object sender, EventArgs e)
    {
        string url = "https://www.google.com/maps/place/" + lat.ToString().Replace(',', '.') + "," + lon.ToString().Replace(',', '.');
        Uri uri = new Uri(url);
        await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
    }
}