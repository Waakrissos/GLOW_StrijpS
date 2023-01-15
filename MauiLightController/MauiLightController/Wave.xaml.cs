namespace MauiLightController;

using Controller;
using Microsoft.Maui.Controls;
using System.Diagnostics;

public partial class Wave : ContentPage
{
    //Controller Controller { get; set; }
    public Wave()
	{
        InitializeComponent();
        //Controller = new Controller();
    }

    public class Light
    {
        public string Id { get; set; }
    }

    //private void CreateControl()
    //{
    //    DataTemplate dataTemplate = new DataTemplate(() =>
    //    {
    //        Button buttonOn = new Button()
    //        {
    //            FontSize = 24,
    //            VerticalOptions = LayoutOptions.Center,
    //            WidthRequest = 100,
    //            BackgroundColor = Colors.White
    //        };
    //        buttonOn.SetBinding(Button.TextProperty, "Id");
    //        buttonOn.Clicked += async (sender, args) => TurnWaveOn();

    //        Button buttonOff = new Button()
    //        {
    //            FontSize = 24,
    //            VerticalOptions = LayoutOptions.Center,
    //            WidthRequest = 100,
    //            BackgroundColor = Colors.Purple
    //        };
    //        buttonOff.SetBinding(Button.TextProperty, "Id");
    //        buttonOff.Clicked += async (sender, args) => TurnWaveOff();

    //        StackLayout Layout = new StackLayout
    //        {
    //            Orientation = StackOrientation.Horizontal
    //        };
    //        Layout.Add(buttonOn);
    //        Layout.Add(buttonOff);

    //        return Layout;
    //    });
    //}


    //private async void TurnWaveOn(object sender, EventArgs e)
    //{
    //    Controller.WaveOn();
    //}
    private async void TurnWaveOff(object sender, EventArgs e)
    {
        Controller.Reset();
    }

    private void WaveBtn_Clicked(object sender, EventArgs e)
    {

    }
}