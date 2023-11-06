using System.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Discord.Commands;

namespace discord_bot
{
    public class Post
    {
        HttpWebRequest _request;
        string _address;
        public Dictionary<string,string> Headers { get; set; }
        public string Response { get; set; }
        public string Data { get; set; }
        public WebProxy Proxy { get;  set; }

        public Post(string address)
        {
            _address = address;
            Headers = new Dictionary<string,string>();
        }

        public void Run(string _Encoding="UTF-8",CookieContainer cookieContainer = null)
        {
            // string url = "";
            //_address == url

            _request = (HttpWebRequest)WebRequest.Create(_address);
            _request.Method = "Post";
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoding = System.Text.Encoding.GetEncoding(_Encoding);

            if (cookieContainer != null)
            {
                _request.CookieContainer = cookieContainer;
                _request.Proxy = Proxy;
                foreach (var pair in Headers)
                {
                    _request.Headers.Add(pair.Key, pair.Value);
                }
            }


            byte[] sentData = Encoding.Default.GetBytes(Data);

            _request.ContentLength = sentData.Length;


            using (Stream requestStream = _request.GetRequestStream())
            {
                requestStream.Write(sentData, 0, sentData.Length);
                requestStream.Close();
            }



            var _response = (HttpWebResponse)_request.GetResponse();

            // string jsonString;
            using (StreamReader streamReader = new StreamReader(_response.GetResponseStream(),encoding))
            {
                Response = streamReader.ReadToEnd();
            }
        }
    }
}