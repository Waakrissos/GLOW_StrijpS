namespace MauiLightController;

using Controller;
//using Java.Util;
//using Java.Net;
using System.Diagnostics;
using System.Windows.Input;

public partial class LightFollow : ContentPage
{
    private CancellationTokenSource _cancelTokenSource;
    private bool _isCheckingLocation;
    Light closestLight;
    Light currentLight;
    private bool follow = true;
    public LightFollow()
    {
        InitializeComponent();
    }

    //public async void GetCurrentLocation(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        _isCheckingLocation = true;

    //        GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

    //        _cancelTokenSource = new CancellationTokenSource();

    //        Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

    //        if (location != null) { }
    //            Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");


    //        //return location
    //    }
    //    // Catch one of the following exceptions:
    //    //   FeatureNotSupportedException
    //    //   FeatureNotEnabledException
    //    //   PermissionException
    //    catch (Exception ex)
    //    {
    //        // Unable to get location
    //    }
    //    finally
    //    {
    //        _isCheckingLocation = false;
    //    }
    //}
    public async void GetCurrentLocation(object sender, EventArgs e)
    {
        try
        {
            _isCheckingLocation = true;

            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

            _cancelTokenSource = new CancellationTokenSource();

            Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

            if (location != null) { }
            Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");


            //return location
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

    public void CancelRequest(object sender, EventArgs e)
    {
        if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
            _cancelTokenSource.Cancel();
    }

    public async Task<string> GetCachedLocation(object sender, EventArgs e)
    {
        try
        {
            Location location = await Geolocation.Default.GetLastKnownLocationAsync();

            if (location != null)
                return $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";
            //return location;
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            // Handle not supported on device exception
        }
        catch (FeatureNotEnabledException fneEx)
        {
            // Handle not enabled on device exception
        }
        catch (PermissionException pEx)
        {
            // Handle permission exception
        }
        catch (Exception ex)
        {
            // Unable to get location
        }

        return "None";
    }



    // -----------------------------------------------------------------
    // FOLLOW
    private async void OnFollowClicked(object sender, EventArgs e)
    {

        while (follow == true)
        {
            var myLocation = await Geolocation.GetLocationAsync();
            double minDistance = 1000;

            foreach (Light light in Controller.Lights)
            {
                var lightLocation = new Location(light.Latitude, light.Longitude);
                var distance = myLocation.CalculateDistance(lightLocation, DistanceUnits.Kilometers);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestLight = light;
                }

            }
            if (closestLight != null && closestLight != currentLight)
            {
                if (currentLight != null)
                {
                    Controller.TurnOff(currentLight.Id);
                    Controller.ChangeColor(currentLight.Id, new int[] { 0, 0, 0 });
                }
                Console.WriteLine($"ID: {closestLight.Id}, Name: {closestLight.Name}");
                Controller.ChangeColor(closestLight.Id, new int[] { 255, 0, 0 });

                Controller.TurnOn(closestLight.Id);
                currentLight = closestLight;
            }

        }
    }

    private async void OnDeacticateFollowClicked(object sender, EventArgs e)
    {

        follow = false;
        Controller.Reset();
    }


    // -----------------------------------------------------------------
    // BELOW WORK FOR ONE LIGHT AT A TIME
    private async void OnActivateClicked(object sender, EventArgs e)
    {
        var myLocation = await Geolocation.GetLocationAsync();
        double minDistance = 1000;

        foreach (Light light in Controller.Lights)
        {
            var lightLocation = new Location(light.Latitude, light.Longitude);
            var distance = myLocation.CalculateDistance(lightLocation, DistanceUnits.Kilometers);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestLight = light;
            }

        }
        if (closestLight != null)
        {
            Console.WriteLine($"ID: {closestLight.Id}, Name: {closestLight.Name}");
            Controller.ChangeColor(closestLight.Id, new int[] { 255, 0, 0 });
            Controller.TurnOn(closestLight.Id);

        }
    }

    private async void OnDeacticateClicked(object sender, EventArgs e)
    {
        Controller.Reset();
    }



}