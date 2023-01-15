namespace MauiLightController;
using Controller;
public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
        Console.WriteLine(Controller.Lights[0].CalculateDistance(Controller.Lights[2].Latitude, Controller.Lights[2].Longitude));
		InitializeComponent();
	}

	private async void OnToggleLightsClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("ToggleLights");
	}
    private async void OnLightShowClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("LightShow");
    }
    private async void OnRotateClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Rotate");
    }
    private async void OnGpsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("GpsFollow");
    }
    private async void OnFadeControlClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("FadeControl");
    }
	  private async void OnWaveClicked(object sender, EventArgs e)
	  {
        await Shell.Current.GoToAsync("Wave");
    }
	  private async void OnFollowClicked(object sender, EventArgs e)
	  {
		  await Shell.Current.GoToAsync("LightFollow");
	  }
    private async void OnWhackAMoleClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("WhackAMole");
    }

    private async void ToggleDay_Clicked(object sender, EventArgs e)
    {
        foreach (Light light in Controller.Lights)
        {
            light.toggle(false);
            light.SetWarmBrightness(0);
            light.SetColdBrightness(0);
            light.changeColor(new int[] { 0, 0, 0 });
        }
    }
    private async void ToggleNight_Clicked(object sender, EventArgs e)
    {
        foreach(Light light in Controller.Lights)
        {
            light.toggle(true);
            light.SetWarmBrightness(30);
            light.SetColdBrightness(30);
            light.changeColor(new int[] { 0, 0, 0 });
        }
    }
}
