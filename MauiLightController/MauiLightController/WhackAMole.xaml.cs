using Microsoft.Maui;
using System.Diagnostics;

namespace MauiLightController;

using Controller;
using RestSharp;

public partial class WhackAMole : ContentPage
{
	private bool Inprogress;
    private int conesize = 20;
    private double lat = 51.445794;
    private double lon = 5.458397;
    private int score;
    private int target;
    private Stopwatch TargetTime;
    private Light Light;
    private List<Light> LightList;
    public WhackAMole()
	{
		InitializeComponent();
		Inprogress = false;
        TargetTime = new Stopwatch();
        target = -100;
        LightList = new List<Light>();
        foreach(Light light in Controller.Lights)
        {
            if(light.Height == 15)
            {
                LightList.Add(light);
            }
        }
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
        if (Inprogress)
        {
            int rotation = (int)e.Reading.HeadingMagneticNorth;
            arrow.RotateTo( target - rotation,5,Easing.Linear);
            RulesLabel.Text = $"rotation: {rotation}";
            ScoreLabel.Text = $"Target: {target} \nScore: {score}";
            if (rotation - conesize < target && rotation + conesize > target)
            {
                if(TargetTime.ElapsedMilliseconds > 1000) 
                {
                    Vibration.Default.Vibrate(100);
                    score++;
                    TargetTime.Reset();
                    Light.ChangeColor(new int[] { 0, 0, 0 });
                    target = -100;
                }else if(TargetTime.ElapsedMilliseconds == 0)
                {
                    TargetTime.Restart();
                }
            }
        }
    }
    

    private void StartButton_Clicked(object sender, EventArgs e)
	{
        score = 0;
        target = -100;
        ScoreLabel.Text = $"Target: {target} \nScore: {score}";
        ScoreLabel.IsVisible = true;
        Inprogress = true;
        TimeProgressBar.Progress = 1;
        Game();
	}

	private async Task Game()
	{
        Stopwatch stopwatch = Stopwatch.StartNew();
		TimeProgressBar.ProgressTo(0, 20000, Easing.Linear);
        Thread Thread = new Thread(() =>
        {
            foreach (Light light in LightList)
            {
                light.Toggle(true);
                light.ChangeColor(new int[] { 0, 0, 0 });
                light.SetWarmBrightness(255);
                light.SetColdBrightness(255);
            }
            int index = 0;
            while (stopwatch.ElapsedMilliseconds < 20000)
            {
                if (target == -100)
                {
                    Random random = new Random();
                    
                    while (true)
                    {
                        int i = random.Next(1, LightList.Count - 1);
                        if (index != i)
                        {
                            index = i;
                            break;
                        }
                    }
                    Light = LightList[random.Next(1, LightList.Count - 1)];
                    Light.ChangeColor(new int[] { 0, 255, 0 });
                    target = Light.CalculateAngle(lon, lat);
                    RulesLabel.IsVisible = true;
                }
            }
            foreach (Light light in LightList)
            {
                light.Toggle(false);
                light.ChangeColor(new int[] { 0, 0, 0 });
                light.SetWarmBrightness(30);
                light.SetColdBrightness(30);
            }
            Inprogress = false;
            TargetTime.Reset();
        });
        Thread.Start();
        
    }
}