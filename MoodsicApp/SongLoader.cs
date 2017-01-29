using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Web;
using System.Xml;
using Newtonsoft.Json;
using HtmlAgilityPack;

namespace MoodsicApp
{
    static class SongLoader
    {
        private const String clientId = "1709588742";
        private const String clientKey = "6BCB9F365120D26BC4ED266D12BF5B2C";
        private const String userId = "261581584653824776";
        private const String userKey = "FCB959CBCC84B6A3CCB84BEEA92BC34C";
        const String baseUrl = "https://c" + clientId + ".web.cddbp.net/webapi/xml/1.0/";

        private const String GuguelApiKey = "AIzaSyBZwH8D6zc8ze3s6UUK4C307QjsubkVvbA";
        private const String GuguelBaseUrl = "https://www.googleapis.com/youtube/v3/search?part=snippet&maxResults=1";

        private const String Utube2mp3Domain = "https://www.youtubeinmp3.com";
        private const String baseUrlUtube2mp3 = Utube2mp3Domain + "/widget/button/?video=https://www.youtube.com/watch?v=";
        private const String videoPath = "My Songs\\";

        public static String[] GetMusic(String mood, String artist = null, String era = null, String genre = null)
        {
            UriBuilder uriBuilder = new UriBuilder(baseUrl + "radio/create?client="
                + clientId + "-" + clientKey + "&user=" + userId + "-" + userKey);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            if (mood != null) query["mood"] = mood;
            if (artist != null) query["artist_name"] = artist;
            if (era != null) query["era"] = era;
            if (genre != null) query["genre"] = genre;

            uriBuilder.Query = query.ToString();
            String uriString = uriBuilder.ToString();
            Uri uri = new Uri(uriString);
            Console.WriteLine("Getting track from: " + uri.ToString());

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

        public static String GetYoutubeId(String searchString)
        {
            UriBuilder uriBuilder = new UriBuilder(GuguelBaseUrl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["q"] = searchString;
            query["key"] = GuguelApiKey;

            uriBuilder.Query = query.ToString();
            String uriString = uriBuilder.ToString();
            Console.WriteLine("Searching in YouTube: " + uriString);
            Uri uri = new Uri(uriString);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();

            var reader = new StreamReader(resStream, Encoding.UTF8);
            string jsonString = reader.ReadToEnd();
            //Console.WriteLine(jsonString);

            dynamic jsonObj = JsonConvert.DeserializeObject(jsonString);

            if (jsonObj.items.Count == 0) return null;
            return jsonObj.items[0].id.videoId;
        }

        public static void DownloadVideo(String videoId)
        {
            if (!Directory.Exists(videoPath))
                Directory.CreateDirectory(videoPath);

            // Create a new WebClient instance.
            WebClient client = new WebClient();

            String buttonUri = baseUrlUtube2mp3 + videoId;
            string htmlCode = client.DownloadString(buttonUri);
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlCode);

            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            {
                Console.WriteLine("Parse errors :(");
                return;
            }

            String downloadVideoUri = Utube2mp3Domain + htmlDoc.DocumentNode.SelectSingleNode("//a").Attributes["href"].Value;
            
            Console.WriteLine("Downloading video from \"{0}\"", downloadVideoUri);
            // Download the Web resource and save it into the current filesystem folder.
            //client.DownloadFile(downloadVideoUri, Environment.SpecialFolder.MyMusic + "\\" + videoId);
            Byte[] bytes = client.DownloadData(downloadVideoUri);
            File.WriteAllBytes(videoPath + videoId + ".mp3", bytes);
            Console.WriteLine("Successfully Downloaded File \"{0}\" from \"{1}\" to \"{2}\"", videoId, downloadVideoUri, videoPath + videoId + ".mp3");
            Console.WriteLine("\nDownloaded file saved in the following file system folder:\n\t" + Environment.SpecialFolder.MyMusic + "\\" + videoId);
        }
    }
}
