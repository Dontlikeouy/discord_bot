using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace discord_bot
{
    public class Get
    {
        HttpWebRequest _request;
        string _address;
        public Dictionary<string, string> Headers { get; set; }
        public string Response { get; set; }
        public WebProxy Proxy { get; internal set; }

        public Get(string address)
        {
            _address = address;
            Headers = new Dictionary<string, string>();
        }

        public void Run(string _Encoding = "UTF-8", CookieContainer cookieContainer = null)
        {

            _request = (HttpWebRequest)WebRequest.Create(_address);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoding = System.Text.Encoding.GetEncoding(_Encoding);


            _request.Method = "GET";
            if (cookieContainer != null)
            {
                _request.CookieContainer = cookieContainer;
                _request.Proxy = Proxy;
                foreach (var pair in Headers)
                {
                    _request.Headers.Add(pair.Key, pair.Value);
                }
            }

            var _response = (HttpWebResponse)_request.GetResponse();
            using (StreamReader streamReader = new StreamReader(_response.GetResponseStream(), encoding))
            {
                Response = streamReader.ReadToEnd();
            }
        }
    }
}