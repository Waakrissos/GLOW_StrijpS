
namespace MauiLightController;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute("ToggleLights", typeof(ToggleLights));
        Routing.RegisterRoute("LightShow", typeof(LightShow));
        Routing.RegisterRoute("Rotate", typeof(Rotate));
        Routing.RegisterRoute("GpsFollow", typeof(GpsFollow));
        Routing.RegisterRoute("FadeControl", typeof(FadeControl));
        Routing.RegisterRoute("Wave", typeof(Wave));
        Routing.RegisterRoute("LightFollow", typeof(LightFollow));
        Routing.RegisterRoute("WhackAMole", typeof(WhackAMole));
    }
}
