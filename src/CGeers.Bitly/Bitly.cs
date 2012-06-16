using System;
using System.Net;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CGeers.Bitly
{
    public class Bitly
    {
        public delegate void BitlyCallback<T>(T data) where T : class, new();

        public void Shorten(string longUrl, BitlyCallback<BitlyResponse> callback)
        {
            var scheme = DetermineScheme(longUrl);
            if (scheme != null && longUrl.IndexOf(scheme, StringComparison.CurrentCultureIgnoreCase) == -1)
            {
                longUrl = string.Format("{0}://{1}", scheme, longUrl);
            }
            
            if (!IsValidUrl(longUrl))
            {
                callback(new BitlyResponse { Status = "INVALID_URI", StatusCode = 500 });
                return;
            }

            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                callback(new BitlyResponse { StatusCode = 500, Status = "NO_INTERNET_CONNECTION" });
                return;
            }
         
            var urlToShorten = UrlEncode(InsertForwardSlashBeforeQueryPath(longUrl));

            var requestUri = new StringBuilder(BitlyApi.EndPoint);
            requestUri.AppendFormat("?apiKey={0}&", BitlyApi.ApiKey);
            requestUri.AppendFormat("login={0}&", BitlyApi.Login);
            requestUri.AppendFormat("longUrl={0}", urlToShorten);

            var request = (HttpWebRequest) WebRequest.Create(requestUri.ToString());
            request.Method = "GET";

            request.BeginGetResponse(
                ar =>
                {
                    var response = (HttpWebResponse)request.EndGetResponse(ar);
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        var json = JObject.Parse(reader.ReadToEnd());

                        var bitlyResponse = new BitlyResponse
                                                {
                                                    StatusCode = json["status_code"].ToObject<int>(),
                                                    Status = json["status_txt"].ToString()
                                                };

                        if (bitlyResponse.StatusCode == 200)
                        {
                            bitlyResponse.Data = JsonConvert.DeserializeObject<BitlyUrl>(json["data"].ToString());
                        }

                        callback(bitlyResponse);
                    }
                },
                null);
        }

        private static string DetermineScheme(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;

            try
            {
                var uri = new UriBuilder(url);
                return uri.Scheme;
            }
            catch (UriFormatException)
            {
                return null;
            }
        }

        private static bool IsValidUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            try
            {
                Uri test;
                return Uri.TryCreate(url, UriKind.Absolute, out test);
            }
            catch
            {
                return false;
            }
        }

        private static string InsertForwardSlashBeforeQueryPath(string longUrl)
        {
            var tempUrl = longUrl;
            var indexOfQuestionMark = tempUrl.IndexOf("?", StringComparison.InvariantCultureIgnoreCase);
            if (indexOfQuestionMark != -1 && tempUrl[indexOfQuestionMark - 1] != '/')
            {
                tempUrl = tempUrl.Insert(indexOfQuestionMark, "/");
            }
            return tempUrl;
        }

        private static string UrlEncode(string s)
        {
            char[] temp = HttpUtility.UrlEncode(s).ToCharArray();
            for (int i = 0; i < temp.Length - 2; i++)
            {
                if (temp[i] == '%')
                {
                    temp[i + 1] = char.ToUpper(temp[i + 1]);
                    temp[i + 2] = char.ToUpper(temp[i + 2]);
                }
            }

            var values = new Dictionary<string, string>
            {
                { "!", "%21" },
                { "#", "%23" },
                { "$", "%24" },
                { "&", "%26" },
                { "'", "%27" },
                { "(", "%28" },
                { ")", "%29" },
                { "*", "%2A" },
                { "+", "%2B" },
                { ",", "%2C" },
                { "/", "%2F" },
                { ":", "%3A" },
                { ";", "%3B" },
                { "=", "%3D" },
                { "?", "%3F" },
                { "@", "%40" },
                { "[", "%5B" },
                { "]", "%5D" }
            };

            var data = new StringBuilder(new string(temp));
            foreach (string character in values.Keys)
            {
                data.Replace(character, values[character]);
            }

            return data.ToString();
        }
    }
}
