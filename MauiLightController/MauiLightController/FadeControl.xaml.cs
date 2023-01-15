namespace MauiLightController;

using Controller;
using RestSharp;
using System.Diagnostics;

public partial class FadeControl : ContentPage
{ 
    private int interval;
    private int increment;
    private int changeSpeed;
    private int[] color;
    private Stopwatch Stopwatch;
    Light Light = Controller.Lights[3];
	public FadeControl()
	{
		InitializeComponent();
        interval = (int)Timer.Value;
        increment = (int)Increment.Value;
        changeSpeed = (int)(1000d / (double)interval * increment);
        color = new int[] { 255, 0, 0 };
	}

    private void Toggle(object sender, EventArgs e)
    {
        if (Switch.IsToggled)
        {
            Light.toggle(true);
            Thread Thread = new Thread(async () =>
            {
                Stopwatch = Stopwatch.StartNew();
                while (Switch.IsToggled)
                {
                    if(Stopwatch.ElapsedMilliseconds > interval)
                    {
                        await Light.changeColor(color);
                        color = Controller.offset(color, increment);
                        Stopwatch.Restart();
                    }
                }
            });
            Thread.Start();
        }
        else
        {
            Light.toggle(false);
        }
    }

    private void TimerChanged(object sender, EventArgs e)
    {
        interval = (int)Timer.Value;
        TimerLbl.Text = String.Format("Timer: {0}", interval);
        if (SwitchStaticSpeed.IsToggled)
        {
            increment = (int)(changeSpeed / (1000d / (double)interval));
            IncrementLbl.Text = String.Format("Increment: {0}", increment);
            Increment.Value = increment;
        }
        else
        {
            changeSpeed = (int)(1000d / (double)interval * increment);
            ChangeSpeedLbl.Text = "Static Change Speed: " + changeSpeed;
        }
    }

    private void IncrementChanged(object sender, EventArgs e)
    {
        increment = (int)Increment.Value;
        
        IncrementLbl.Text = String.Format("Increment: {0}", increment);
        if (SwitchStaticSpeed.IsToggled)
        {
            interval = (int)(1000 / ((double)changeSpeed / (double)increment));
            TimerLbl.Text = String.Format("Timer: {0}", interval);
            Timer.Value = interval;
        }
        else
        {
            changeSpeed = (int)(1000d / (double)interval * increment);
            ChangeSpeedLbl.Text = "Static Change Speed: " + changeSpeed;
        }
    }

    private void ColdSliderChanged(object sender, EventArgs e)
    {
        ColdLbl.Text = "Cold Brightness: " + (int)ColdSlider.Value;
        Light.SetColdBrightness((int)ColdSlider.Value);
    }

    private void WarmSliderChanged(object sender, EventArgs e)
    {
        WarmLbl.Text ="Warm Brightness: " + (int)WarmSlider.Value;
        Light.SetWarmBrightness((int)WarmSlider.Value);
    }
}