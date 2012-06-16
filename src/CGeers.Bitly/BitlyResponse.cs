using Newtonsoft.Json;

namespace CGeers.Bitly
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BitlyResponse
    {
        [JsonProperty(PropertyName = "status_code")]
        public int StatusCode { get; set; }

        [JsonProperty(PropertyName = "status_txt")]
        public string Status { get; set; }

        public BitlyError Error
        {
            get
            {
                if (StatusCode == 200)
                    return BitlyError.None;

                switch (Status)
                {
                    case "NO_INTERNET_CONNECTION":
                        return BitlyError.NoInternetConnection;
                    case "INVALID_URI":
                        return BitlyError.InvalidUrl;
                    case "ALREADY_A_BITLY_LINK":
                        return BitlyError.AlreadyABitlyLink;
                    default:
                        return BitlyError.Unknown;
                }
            }
        }

        [JsonProperty(PropertyName = "data", NullValueHandling = NullValueHandling.Ignore, Required = Required.AllowNull)]
        public BitlyUrl Data { get; set; }
    }
}
