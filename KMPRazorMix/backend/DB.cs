using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using RestSharp;
namespace KMPRazorMix
{
    public class DB
    {
        static RestClient client;

        public static string GetDebug()
        {
            IRestResponse response = client.Execute(new RestRequest());
            return response.Content;
        }

        public static void Init()
        {
            client = new RestClient(WebConfigurationManager.AppSettings["CLOUDANT_URL"]);
        }
    }
}