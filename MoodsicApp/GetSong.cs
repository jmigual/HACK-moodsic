using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.IO;

namespace MoodsicApp
{
    class GetSong
    {
        String clientId = "1709588742";
        String clientKey = "6BCB9F365120D26BC4ED266D12BF5B2C";
        String url;

        public GetSong()
        {
            


        }

        public String registerUser()
        {
            url = "https://c" + clientId + ".web.cddbp.net/webapi/xml/1.0/register?client=" + clientId + "-" + clientKey;
            Uri uri = new Uri(url);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();

            var reader = new StreamReader(resStream, Encoding.UTF8);
            string value = reader.ReadToEnd();

            return value;
        }

    }
}
