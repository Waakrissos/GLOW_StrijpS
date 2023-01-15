namespace MauiLightController;
using Controller;
using System.Diagnostics;

public partial class LightShow : ContentPage
{
	private bool Working = false;
	private bool stop = false;
	private List<Light> ShowLights;
	public LightShow()
	{
		InitializeComponent();
		ShowLights = new List<Light>();
		foreach(Light light in Controller.Lights)
		{
			if(light.Height == 15)
			{
                ShowLights.Add(light);
            }
		}
	}

	private void LightShow1Btn_Clicked(object sender, EventArgs e)
	{
        Thread thread = new Thread(() => FadeAllLights());
        thread.Start();
	}

	private void Stop()
	{
        stop = true;
        while (Working)
        {

        }
        foreach (Light light in ShowLights)
        {
            light.Toggle(false);
        }
        stop = false;
    }

	private async void FadeAllLights()
	{
        Stop();
        int mastcount = 1;
        Thread.Sleep(1000);
        for(int i = 0; i < ShowLights.Count(); i++)
        {
            ShowLights[i].Toggle(true);
            if (i != 0)
            {
                if (ShowLights[i].Mast != ShowLights[i - 1].Mast)
                {
                    mastcount++;
                }
            }
        }
        Working = true;
        int interval = 500;
        int increment = 50;
        int offset = 255 * 6 / mastcount;
        int[] color = { 255, 0, 0 };
        
        while (!stop)
        {
            for(int i =0; i< ShowLights.Count(); i++)
            {
                if(i != 0)
                {
                    if (ShowLights[i].Mast != ShowLights[i-1].Mast)
                    {
                        color = Controller.offset(color, offset);
                    }
                }
                ShowLights[i].changeColor(color);
            }
            Thread.Sleep(interval);
            color = Controller.offset(color, increment + offset);
        }
        Working = false;
    }

    private void Stop1Btn_Clicked(object sender, EventArgs e)
    {
        Stop();
    }
}