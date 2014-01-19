using System;
using System.Net;
using MarkdownSharp;
using System.Collections.Generic;

namespace KMPRazorMix
{
    public class KMP
    {
        private const string DataSource = "http://raw.github.com/ruarai/kmpinfo/master/kmpinfo";
        private const string ChangelogSource = "http://raw.github.com/ruarai/kmpinfo/master/changelog";
        private const string ReadmeSource = "http://raw.github.com/ruarai/kmpinfo/master/kmpreadme.md";
        private const string ServersSource = "http://raw.github.com/ruarai/kmpinfo/master/kmpservers";

        public static string KMPVersion;
        public static string KSPCompatibleVersion;

        public static string ClientDownload;
        public static string ServerDownload;

        public static string Changelog;
        public static string ChangelogSummary;
        public static string Readme;


        public static List<Server> Servers = new List<Server>(); 

        public static DateTime LastUpdate;
        public static bool CurrentlyUpdating = false;

        public static void Update()
        {
            CurrentlyUpdating = true;
            LastUpdate = DateTime.Now;

            var retriever = new WebClient();

            var page = retriever.DownloadString(DataSource).Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            KMPVersion = page[0];
            ChangelogSummary = page[1];
            KSPCompatibleVersion = page[2];

            ClientDownload = page[3];
            ServerDownload = page[4];

            var markDown = new Markdown();
            Changelog = markDown.Transform(retriever.DownloadString(ChangelogSource));
            Readme = markDown.Transform(retriever.DownloadString(ReadmeSource));


            string serverDownload = retriever.DownloadString(ServersSource);
            Servers.Clear();
            var servers = serverDownload.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            foreach (var server in servers)
            {
                try
                {
                    var split = server.Split('$');
                    var serverK = new Server(split[0], int.Parse(split[1]));
                    serverK.UpdateServer();
                    Servers.Add(serverK);
                }
                catch { }

            }
            CurrentlyUpdating = false;
        }
    }
}