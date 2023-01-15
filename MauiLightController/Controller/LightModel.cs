using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class LightModel
    {
        public string id { get; set; }
        public int version { get; set; }
        public long createdOn { get; set; }
        public string name { get; set; }
        public bool accessPublicRead { get; set; }
        public string parentId { get; set; }
        public string realm { get; set; }
        public string type { get; set; }
        public List<string> path { get; set; }
        public Attributes attributes { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AgentLink
    {
        public string id { get; set; }
        public string type { get; set; }
    }

    public class Attributes
    {
        public ColourRgbLed colourRgbLed { get; set; }
        public Mast mast { get; set; }
        public MastHeight mastHeight { get; set; }
        public IpAddress ipAddress { get; set; }
        public location location { get; set; }
        public BrightnessWhiteWarmLed brightnessWhiteWarmLed { get; set; }
        public BrightnessWhiteColdLed brightnessWhiteColdLed { get; set; }
        public OnOff onOff { get; set; }
        public Direction direction { get; set; }
    }

    public class BrightnessWhiteColdLed
    {
        public string type { get; set; }
        public int value { get; set; }
        public string name { get; set; }
        public Meta meta { get; set; }
        public long timestamp { get; set; }
    }

    public class BrightnessWhiteWarmLed
    {
        public string type { get; set; }
        public int value { get; set; }
        public string name { get; set; }
        public Meta meta { get; set; }
        public long timestamp { get; set; }
    }

    public class ColourRgbLed
    {
        public string type { get; set; }
        public string value { get; set; }
        public string name { get; set; }
        public Meta meta { get; set; }
        public long timestamp { get; set; }
    }

    public class Direction
    {
        public string type { get; set; }
        public int value { get; set; }
        public string name { get; set; }
        public Meta meta { get; set; }
        public long timestamp { get; set; }
    }

    public class IpAddress
    {
        public string type { get; set; }
        public string value { get; set; }
        public string name { get; set; }
        public Meta meta { get; set; }
        public long timestamp { get; set; }
    }

    public class location
    {
        public string type { get; set; }
        public Value value { get; set; }
        public string name { get; set; }
        public Meta meta { get; set; }
        public long timestamp { get; set; }
    }

    public class Mast
    {
        public string type { get; set; }
        public string value { get; set; }
        public string name { get; set; }
        public Meta meta { get; set; }
        public long timestamp { get; set; }
    }

    public class MastHeight
    {
        public string type { get; set; }
        public string value { get; set; }
        public string name { get; set; }
        public Meta meta { get; set; }
        public long timestamp { get; set; }
    }

    public class Meta
    {
        public bool ruleState { get; set; }
        public bool accessRestrictedRead { get; set; }
        public bool storeDataPoints { get; set; }
        public bool accessRestrictedWrite { get; set; }
        public AgentLink agentLink { get; set; }
        public string label { get; set; }
        public bool readOnly { get; set; }
    }

    public class OnOff
    {
        public string type { get; set; }
        public bool value { get; set; }
        public string name { get; set; }
        public Meta meta { get; set; }
        public long timestamp { get; set; }
    }

    public class Value
    {
        public List<double> coordinates { get; set; }
        public string type { get; set; }
    }
}
