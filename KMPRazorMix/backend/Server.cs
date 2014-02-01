using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Web;

namespace KMPRazorMix
{
    public class Server
    {
        public Server(string ip,int httpPort)
        {
            IP = ip;
            HTTPPort = httpPort;
        }

        public string IP;
        public int Port;
        public int HTTPPort;

        public int Players;
        public int MaxPlayers;
        public List<string> PlayerNames = new List<string>();

        public string Version;

        public string Information;

        public int UpdatesPerSecond;

        public int InactiveShipLimit;

        public int ScreenshotHeight;
        public bool ScreenshotSave;

        public bool Whitelisted;


        public bool IsOnline;
        public string FailedWith;

        public string Address
        {
            get
            {
                return string.Format("{0}:{1}",IP,Port);
            }
            set
            { 
                var split = value.Split(':');
                IP = split[0];
                Port = int.Parse(split[1]);
            }
        }

        public void UpdateServer()
        {
            try
            {
                var retriever = new WebClient();
                var infoPage = retriever.DownloadString("http://" + IP + ":" + HTTPPort).Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

                Version = HttpUtility.HtmlEncode(infoPage[0].Replace("Version: ", ""));

                Port = int.Parse(infoPage[1].Replace("Port: ", ""));

                var players = infoPage[2].Replace("Num Players: ", "").Split('/');
                Players = int.Parse(players[0]);
                MaxPlayers = int.Parse(players[1]);

                PlayerNames.Clear();
                var playerNames = infoPage[3].Replace("Players: ", "").Split(',');
                foreach (var playerName in playerNames)
                {
                    PlayerNames.Add(HttpUtility.HtmlEncode(playerName.Trim()));
                }

                PlayerNames = PlayerNames.Distinct().ToList();

                Information = HttpUtility.HtmlEncode(infoPage[4].Replace("Information: ", ""));

                UpdatesPerSecond = int.Parse(infoPage[5].Replace("Updates per Second: ", ""));

                InactiveShipLimit = int.Parse(infoPage[6].Replace("Inactive Ship Limit: ", ""));

                ScreenshotHeight = int.Parse(infoPage[7].Replace("Screenshot Height: ", ""));

                ScreenshotSave = bool.Parse(infoPage[8].Replace("Screenshot Save: ", ""));

                Whitelisted = bool.Parse(infoPage[9].Replace("Whitelisted: ", ""));

                IsOnline = true;
            }
            catch(Exception e)
            {
                FailedWith = e.ToString();
                IsOnline = false;
            }
        }
    }
}
