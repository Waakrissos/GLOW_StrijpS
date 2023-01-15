using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class Light
    {
        public string Id { get;private set; }
        public string Name { get; private set; }
        public string Mast { get; private set; }
        public double Longitude { get; private set; }
        public double Latitude { get; private set; }
        public int Height { get; private set; }
        public int Angle { get; private set; }

        public int[] Color { get; private set; }
        public int WarmBrightness { get; private set; }
        public int ColdBrightness { get; private set; }
        public bool OnOff { get; private set; }

        public Thread Thread { get; private set; }
        private bool Working { get; set; }
        private bool Stop { get; set; }


        private string realm = "strijp";
        private string url = "https://staging.strijp.openremote.app";

        public Light(LightModel model)
        {
            Id = model.id;
            Name = model.name;
            Mast = model.attributes.mast.value;
            if(model.attributes.location != null)
            {
                Longitude = model.attributes.location.value.coordinates[0];
                Latitude = model.attributes.location.value.coordinates[1];
            }
            Color = new int[] { 0, 0, 0 };
            WarmBrightness = model.attributes.brightnessWhiteWarmLed.value;
            ColdBrightness = model.attributes.brightnessWhiteColdLed.value;
            OnOff = model.attributes.onOff.value;
            Height = Convert.ToInt32(model.attributes.mastHeight.value);
            Working = false;
            Stop = false;
        }

        public int CalculateAngle(double lon, double lat)
        {
            double angle = -400;
            double aanliggend = Latitude - lat;
            double overstaand = Longitude - lon;
            angle = Math.Atan(overstaand / aanliggend) * 180 / Math.PI;
            if (lat <= Latitude)
            {
                if (angle < 0) angle += 360;
            }
            else
            {
                angle += 180;
            }

            Angle = (int)angle;
            return Angle;
        }

        public double CalculateDistance(double latitude, double longitude)
        {
            double x = Latitude * Latitude - latitude * latitude;
            if (x < 0) x = x * -1;

            double y = Longitude * Longitude - longitude * longitude;
            if (y < 0) y = y * -1;

            double dist = Math.Sqrt(x + y);
            return dist * 111.139;
        }

        public void StopTasks()
        {
            if (Working)
            {
                Stop = true;
            }
            while (Stop);
        }
        public async Task ChangeColor(int[] color)
        {
            StopTasks();
            Thread = new Thread(async() => changeColor(color));
            Thread.Start();
        }
        public async Task changeColor(int[] color)
        {
            var client = new RestClient(url + "/api/" + realm + "/asset/" + Id + "/attribute/colourRgbLed");
            var request = new RestRequest();
            request.Method = Method.Put;
            request.AddBody(color);
            request.AddHeader("authorization", "Bearer " + Token.GetToken());
            await client.ExecuteAsync(request);
            Color = color;
            return;
        }

        public void Toggle()
        {
            StopTasks();
            OnOff = !OnOff;
            Thread = new Thread(() => toggle(OnOff));
            Thread.Start();
        }
        public void Toggle(bool onOff)
        {
            StopTasks();
            OnOff = onOff;
            Thread = new Thread(() => toggle(OnOff));
            Thread.Start();
        }
        public async void toggle(bool onOff)
        {
            OnOff = onOff;
            var client = new RestClient(url + "/api/" + realm + "/asset/" + Id + "/attribute/onOff");
            var request = new RestRequest();
            request.Method = Method.Put;
            request.AddBody(onOff);
            request.AddHeader("authorization", "Bearer " + Token.GetToken());
            Console.WriteLine(client.Execute(request).Content);
            return;
        }

        public void Reset()
        {
            StopTasks();
            var client = new RestClient(url + "/api/" + realm + "/asset/" + Id + "/attribute/onOff");
            var request = new RestRequest();
            request.Method = Method.Put;
            request.AddBody(true);
            request.AddHeader("authorization", "Bearer " + Token.GetToken());
            Console.WriteLine(client.Execute(request).Content);
            changeColor(new int[] { 0, 0, 0 });
            return;
        }

        public void SetWarmBrightness(int brightness)
        {
            if(brightness < 0) brightness = 0;
            if(brightness > 255) brightness = 255;
            WarmBrightness = brightness;
            Thread = new Thread(() =>
            {
                var client = new RestClient(url + "/api/" + realm + "/asset/" + Id + "/attribute/brightnessWhiteWarmLed");
                var request = new RestRequest();
                request.Method = Method.Put;
                request.AddBody(brightness);
                request.AddHeader("authorization", "Bearer " + Token.GetToken());
                Console.WriteLine(client.Execute(request).Content);
                return;
            });
            Thread.Start();
        }

        public void SetColdBrightness(int brightness)
        {
            if (brightness < 0) brightness = 0;
            if (brightness > 255) brightness = 255;
            ColdBrightness = brightness;
            Thread = new Thread(() =>
            {
                var client = new RestClient(url + "/api/" + realm + "/asset/" + Id + "/attribute/brightnessWhiteColdLed");
                var request = new RestRequest();
                request.Method = Method.Put;
                request.AddBody(brightness);
                request.AddHeader("authorization", "Bearer " + Token.GetToken());
                Console.WriteLine(client.Execute(request).Content);
                return;
            });
            Thread.Start();
        }

        public void FadeLight()
        {
            StopTasks();
            Thread = new Thread(() =>
            {
                Working = true;
                int[] Color = new int[] { 255, 0, 0 };
                while (Working)
                {
                    changeColor(Color);
                    Color = offset(Color, 50);
                    Thread.Sleep(100);
                    if (Stop)
                    {
                        Working = false;
                    }
                }
                Stop = false;
                return;
            });
            Thread.Start();
        }

        int[] offset(int[] color, int offset)
        {
            for (int i = 0; i < color.Length; i++)
            {
                if (color[i] == 255)
                {
                    int after = i + 1;
                    int before = i - 1;
                    if (after == color.Length) after = 0;
                    if (before == -1) before = color.Length - 1;

                    if (color[before] == 0 && color[after] < color[i])
                    {
                        color[after] += offset;
                        if (color[after] > 255)
                        {
                            color[i] -= color[after] - 255;
                            color[after] = 255;
                        }
                    }
                    else
                    {
                        color[before] -= offset;
                        if (color[before] < 0)
                        {
                            color[after] -= color[before];
                            color[before] = 0;
                        }
                    }
                    break;
                }
            }
            return color;
        }

    }
}
