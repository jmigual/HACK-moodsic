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
        private const String clientId = "1709588742";
        private const String clientKey = "6BCB9F365120D26BC4ED266D12BF5B2C";
        private const String userId = "261581584653824776";
        private const String userKey = "FCB959CBCC84B6A3CCB84BEEA92BC34C";
        String baseUrl = "https://c" + clientId + ".web.cddbp.net/webapi/xml/1.0/";

        public GetSong()
        {

        }

        public String registerUser()
        {
            Uri uri = new Uri(baseUrl + "register?client=" + clientId + "-" + clientKey);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();

            var reader = new StreamReader(resStream, Encoding.UTF8);
            string value = reader.ReadToEnd();

            return value;
        }

        public String[] getMusic(String mood, String artist = null, String era = null, String genre = null)
        {
            UriBuilder uriBuilder = new UriBuilder(baseUrl + "radio/create?client="
                + clientId + "-" + clientKey + "&user=" + userId + "-" + userKey);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            if (mood != null)   query["mood"]        = mood;
            if (artist != null) query["artist_name"] = artist;
            if (era != null)    query["era"]         = era;
            if (genre != null)  query["genre"]       = genre;
            
            uriBuilder.Query = query.ToString();
            String uriString = uriBuilder.ToString();
            Uri uri = new Uri(uriString);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();

            var reader = new StreamReader(resStream, Encoding.UTF8);
            string xmlRadio = reader.ReadToEnd();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlRadio);

            XmlNodeList tracks = doc.DocumentElement.SelectNodes("//TRACK");

            String[] songs = new String[tracks.Count];
            for (int i = 0; i < tracks.Count; ++i)
            {
                XmlNode track = tracks.Item(i);
                XmlNode titleNode, artistNode;
                titleNode = track.SelectSingleNode("TITLE");
                artistNode = track.SelectSingleNode("ARTIST");
                songs[i] = (titleNode != null ? titleNode.InnerText : "")
                    + " - " + (artistNode != null ? artistNode.InnerText : "");
            }

            return songs;

        }


    }
}


// https://c1709588742.web.cddbp.net/webapi/json/1.0/radio/create?filter_mood=65324&client=1709588742-6bcb9f365120d26bc4ed266d12bf5b2c&user=261581584653824776-FCB959CBCC84B6A3CCB84BEEA92BC34C
