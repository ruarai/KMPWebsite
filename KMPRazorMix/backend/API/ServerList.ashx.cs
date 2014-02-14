using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace KMPRazorMix
{
    /// <summary>
    /// Summary description for ServerList
    /// </summary>
    public class ServerList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            var serializer = new JavaScriptSerializer();

            var servers = serializer.Serialize(KMP.Servers);


            context.Response.Write(servers);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}