using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Script.Serialization;

namespace KMPRazorMix
{
    public class Server
    {
        public Server(string ip, int httpPort)
        {
            Host = ip;
            HTTPPort = httpPort;
        }

        public string Host;
        public int Port;
        public int HTTPPort;

        public int Players;
        public int MaxPlayers;
        public List<string> PlayerNames = new List<string>();

        public string Version;

        public string Information;
        public string Name;

        public int UpdatesPerSecond;

        public int InactiveShipLimit;

        public int ScreenshotHeight;
        public bool ScreenshotSave;

        public bool Whitelisted;


        public bool IsOnline;


        public string Country;
        public double Latitude;
        public double Longitude;

        public string Address
        {
            get
            {
                return string.Format("{0}:{1}", Host, Port);
            }
            set
            {
                var split = value.Split(':');
                Host = split[0];
                Port = int.Parse(split[1]);
            }
        }
        public void FirstUpdate()
        {
            GetLocation();
            Update();
        }

        public void Update()
        {
            try
            {

                var retriever = new WebClient();
                var infoPage = retriever.DownloadString("http://" + Host + ":" + HTTPPort).Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

                Version = infoPage[0].Replace("Version: ", "");

                Port = int.Parse(infoPage[1].Replace("Port: ", ""));

                var players = infoPage[2].Replace("Num Players: ", "").Split('/');
                Players = int.Parse(players[0]);
                MaxPlayers = int.Parse(players[1]);

                PlayerNames.Clear();
                var playerNames = infoPage[3].Replace("Players: ", "").Split(',');
                foreach (var playerName in playerNames)
                {
                    PlayerNames.Add(playerName.Trim());
                }

                PlayerNames = PlayerNames.Distinct().ToList();

                Information = infoPage[4].Replace("Information: ", "");
                //Name is a easier to display version of information incase they decide to put a paragraph of information
                Name = Information.Length > 100 ? Information.Substring(0, 30) : Information;


                UpdatesPerSecond = int.Parse(infoPage[5].Replace("Updates per Second: ", ""));

                InactiveShipLimit = int.Parse(infoPage[6].Replace("Inactive Ship Limit: ", ""));

                ScreenshotHeight = int.Parse(infoPage[7].Replace("Screenshot Height: ", ""));

                ScreenshotSave = bool.Parse(infoPage[8].Replace("Screenshot Save: ", ""));

                Whitelisted = bool.Parse(infoPage[9].Replace("Whitelisted: ", ""));

                IsOnline = true;
            }
            catch (Exception e)
            {
                IsOnline = false;
            }
        }

        private void GetLocation()
        {
            try
            {
                var retriever = new WebClient();

                string json = retriever.DownloadString("http://freegeoip.net/json/" + Host);

                var serializer = new JavaScriptSerializer();

                var deserialized = serializer.Deserialize<GeoData>(json);

                Country = deserialized.country_code;
                Latitude = deserialized.latitude;
                Longitude = deserialized.longitude;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to geolocate server at {0}",Host);
            }
        }
    }
}
