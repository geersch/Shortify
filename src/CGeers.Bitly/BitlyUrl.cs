using Newtonsoft.Json;

namespace CGeers.Bitly
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BitlyUrl
    {
        [JsonProperty(PropertyName = "long_url")]
        public string LongUrl { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }
}