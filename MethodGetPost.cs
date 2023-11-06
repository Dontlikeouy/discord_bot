using System.Diagnostics;
using System.Text;
using System.IO;
using System.Net;
using System;

namespace discord_bot
{
    public class MethodGetPost
    {


        static public string GetPost(HttpWebRequest _request, string Response, string Data, string _Encoding)
        {


            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoding = System.Text.Encoding.GetEncoding(_Encoding);

        

            if (Data != null)
            {
                byte[] sentData = Encoding.Default.GetBytes(Data);

                _request.ContentLength = sentData.Length;


                using (Stream requestStream = _request.GetRequestStream())
                {
                    requestStream.Write(sentData, 0, sentData.Length);
                    requestStream.Close();
                }
            }

            var sad = (HttpWebResponse)_request.GetResponse();

            using (StreamReader streamReader = new StreamReader(sad.GetResponseStream(), encoding))
            {
                Response = streamReader.ReadToEnd();
            }



            return Response;
        }
    }
}