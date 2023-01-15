using System.Diagnostics;
using Newtonsoft.Json;
using RestSharp;
namespace Controller
{
    // All the code in this file is included in all platforms.
    public static class Controller
    {
        static string realm = "strijp";
        static string url = "https://staging.strijp.openremote.app";

        static string broeinestid = "3lGbluNj94x8A7b3NFieiy";
        static string O6021 = "3v4kD62VpTAHAmMFW536hQ";
        static string W6021 = "4ErkQXvRN0b1aFS1z5Mi8t";
        static string O6022 = "50WxcFY2bcWYuX9BCJaPdN";
        static string W6022 = "2zxSJTBK2HyCJRTVP25HER";
        static string O6023 = "5WSH1baydm37mEQaVfpRtt";
        static string W6023 = "5QOQbbZbIcMrneDToD7Wgu";
        static string N6024 = "7S324dnACVjlg2TO53C4zj";
        static string Z6024 = "3XXJACAqHZCkhgn8PWZKaQ";
        static string N6025 = "4k8ThtXzLEc50LHowdGYan";
        static string Z6025 = "777yWSXa64OdBpQLzWUv5R";
        static string O6026 = "6e5JpYJ0g17nwS9vLFzgfp";
        static string W6026 = "2mezxE1lOSngRyXubh0wOf";
        static string O6027 = "5kYW7teouE8TFqmWwdsMz0";
        static string W6027 = "5UMschrkm2g3hsmEXuYXbq";
        static string O6028 = "6F1eDFmy5CQW9NFiM7JDSJ";
        static string W6028 = "76oDMkuQEqM90lYy2eskyv";
        static string N6019 = "6p0MitVkYBqbGsufmodR5L";
        static string O6019 = "2WAxHSELyVOomNMCqPDUDN";
        static string W6019 = "4VKssN6iP29z47CWvozw1j";
        static string Z6019 = "3xtIVlCt0bi4EqfbPhyEv1";
        static string N6020 = "4H3KMCTc7Mq6kjEKTH4LZv";
        static string O6020 = "4HdGR7gj8aZDaQKice56qf";
        static string W6020 = "77B2EdOongaQqLOPZy2Pbf";
        static string Z6020 = "2hhQbFLgzKoY8EuBPjXmzM";

        static public List<string> lights;
        public static List<Light> Lights { get; set; }

        static Controller()
        {
            lights = new List<string>() { O6021, W6021, O6022, W6022, O6023, W6023, N6024, Z6024, N6025, Z6025, O6026, W6026, O6027, W6027, O6028, W6028, N6019, O6019, W6019, Z6019, N6020, O6020, W6020, Z6020};
            Lights = new List<Light>();
            Update(lights);
        }

        public static void FadeAllLights(string[] lights, int[] color)
        {
            while (true)
            {
                color = offset(color, 10);
                foreach (string light in lights)
                {
                    Console.WriteLine(DateTime.Now);
                    color = offset(color, 1536 / lights.Length);
                    ChangeColor(light, color);
                }
            }
        }

        public static void Update(List<string> ids)
        {
            foreach(string id in ids)
            {
                Lights.Remove(Lights.Find(l => l.Id == id));
                GetAsset(id);
            }
        }

        //add light to list of lights
        public static string GetAsset(string assetid)
        {
            var client = new RestClient(url + "/api/" + realm + "/asset/" + assetid);
            var request = new RestRequest();
            request.Method = Method.Get;
            request.AddHeader("authorization", "Bearer " + Token.GetToken());
            RestResponse response = client.Execute(request);
            try
            {
                LightModel lightModel = JsonConvert.DeserializeObject<LightModel>(response.Content);
                Lights.Add(new Light(lightModel));
            }
            catch
            {

            }
            return response.Content.ToString();
        }

        public static async void ChangeColor(string assetid, int[] color)
        {
            var client = new RestClient(url + "/api/" + realm + "/asset/" + assetid + "/attribute/colourRgbLed");
            var request = new RestRequest();
            request.Method = Method.Put;
            request.AddBody(color);
            request.AddHeader("authorization", "Bearer " + Token.GetToken());
            client.ExecuteAsync(request);
            return;
        }

        public static void TurnOn(string assetid)
        {
            var client = new RestClient(url + "/api/" + realm + "/asset/" + assetid + "/attribute/onOff");
            var request = new RestRequest();
            request.Method = Method.Put;
            request.AddBody(true);
            request.AddHeader("authorization", "Bearer " + Token.GetToken());
            Console.WriteLine(client.Execute(request).Content);
            return;
        }

        public static void TurnOff(string assetid)
        {
            var client = new RestClient(url + "/api/" + realm + "/asset/" + assetid + "/attribute/onOff");
            var request = new RestRequest();
            request.Method = Method.Put;
            request.AddBody(false);
            request.AddHeader("authorization", "Bearer " + Token.GetToken());
            Console.WriteLine(client.Execute(request).Content);
            return;
        }

        public static void SetWarmBrightness(string assetid, int brightness)
        {
            var client = new RestClient(url + "/api/" + realm + "/asset/" + assetid + "/attribute/brightnessWhiteWarmLed");
            var request = new RestRequest();
            request.Method = Method.Put;
            request.AddBody(brightness);
            request.AddHeader("authorization", "Bearer " + Token.GetToken());
            Console.WriteLine(client.Execute(request).Content);
            return;
        }

        public static void SetColdBrightness(string assetid, int brightness)
        {
            var client = new RestClient(url + "/api/" + realm + "/asset/" + assetid + "/attribute/brightnessWhiteColdLed");
            var request = new RestRequest();
            request.Method = Method.Put;
            request.AddBody(brightness);
            request.AddHeader("authorization", "Bearer " + Token.GetToken());
            Console.WriteLine(client.Execute(request).Content);
            return;
        }

        public static async Task FadeLight(string assetid)
        {
            int[] Color = new int[] { 255, 0, 0 };
            for (int i = 0; i < 3; i++)
            {
                int x = i + 1;
                if (x == 3) x = 0;

                for (int j = 0; j <= 255; j = j + 5)
                {
                    Thread.Sleep(500);
                    Color[x] = j;
                    ChangeColor(assetid, Color);
                }
                for (int j = 255; j >= 0; j = j - 5)
                {
                    Thread.Sleep(500);
                    Color[i] = j;
                    ChangeColor(assetid, Color);
                }
            }
            return;
        }

        public static int[] offset(int[] color, int offset)
        {
            for (int i = 0; i < color.Length; i++)
            {
                if (color[i] == 255)
                {
                    int after = i + 1;
                    int before = i - 1;
                    if (after == color.Length) after = 0;
                    if (before == -1) before = color.Length - 1;

                    if (color[before] == 0 && color[after] <= color[i])
                    {
                        color[after] += offset;
                        if (color[after] >= 255)
                        {
                            color[i] -= color[after] - 255;
                            color[after] = 255;
                        }
                    }
                    else
                    {
                        color[before] -= offset;
                        if (color[before] <= 0)
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
        public static void Reset()
        {
            TurnOff(broeinestid);
            ChangeColor(broeinestid, new int[] { 0, 0, 0 });
        }
    }
}