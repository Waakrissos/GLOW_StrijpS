namespace MauiLightController;

using Controller;
public partial class ToggleLights : ContentPage
{
    public ToggleLights()
    {
        Controller.Update(Controller.lights);
        InitializeComponent();
        CreateControl();
        foreach(Light light in Controller.Lights)
        {
            if(light.Height == 15)
            {
                light.ChangeColor(new int[] { 0, 0, 0 });
                light.Toggle(true);
            }
        }
    }
    private void CreateControl()
    {
        DataTemplate dataTemplate = new DataTemplate(() =>
        {
            Button buttonOn = new Button()
            {
                TextColor = Colors.Black,
                FontSize = 18,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 100,
                BackgroundColor = Colors.Yellow
            };
            buttonOn.SetBinding(Button.AutomationIdProperty, "Id");
            buttonOn.SetBinding(Button.TextProperty, "Name");
            buttonOn.Clicked += async (sender, args) => TurnLightOnClicked(sender, args, buttonOn.AutomationId);

            Button buttonOff = new Button()
            {
                TextColor = Colors.Black,
                FontSize = 18,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 100,
                BackgroundColor = Colors.Purple
            };
            buttonOff.SetBinding(Button.AutomationIdProperty, "Id");
            buttonOff.SetBinding(Button.TextProperty, "Name");
            buttonOff.Clicked += async (sender, args) => TurnLightOffClicked(sender, args, buttonOff.AutomationId);

            Button buttonFade = new Button()
            {
                TextColor = Colors.Black,
                FontSize = 18,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 100,
                BackgroundColor = Colors.Green
            };
            buttonFade.SetBinding(Button.AutomationIdProperty, "Id");
            buttonFade.SetBinding(Button.TextProperty, "Name");
            buttonFade.Clicked += async (sender, args) => FadeLightClicked(sender, args, buttonFade.AutomationId);


            StackLayout horizontalStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };
            horizontalStackLayout.Add(buttonOn);
            horizontalStackLayout.Add(buttonOff);
            horizontalStackLayout.Add(buttonFade);

            return horizontalStackLayout;
        });
        StackLayout stackLayout = new StackLayout();
        BindableLayout.SetItemsSource(stackLayout, Controller.Lights);
        BindableLayout.SetItemTemplate(stackLayout, dataTemplate);

        ScrollView scrollView = new ScrollView
        {
            Margin = new Thickness(20),
            Content = stackLayout
        };

        Title = "ScrollView demo";
        Content = scrollView;
    }

    private async void TurnLightOnClicked(object sender, EventArgs e, string assetid)
    {
        Controller.Lights.Find(L => L.Id == assetid).Toggle();
    }
    private async void TurnLightOffClicked(object sender, EventArgs e, string assetid)
    {
        Light light = Controller.Lights.Find(L => L.Id == assetid);
        if(light.Id != "3lGbluNj94x8A7b3NFieiy")
        {
            light.FadeLight();
        }
        
    }
    private async void FadeLightClicked(object sender, EventArgs e, string assetid)
    {
        Controller.Lights.Find(L => L.Id == assetid).Reset();
    }

    private void ToggleNight_Clicked(object sender, EventArgs e)
    {

    }

    private void ToggleDay_Clicked(object sender, EventArgs e)
    {

    }
}