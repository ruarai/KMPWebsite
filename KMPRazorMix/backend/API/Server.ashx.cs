using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace KMPRazorMix.backend.API
{
    /// <summary>
    /// Summary description for Server
    /// </summary>
    public class Server : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            var host = context.Request["host"];

            if (host != null && KMP.Servers.Exists(s => s.Address == host))
            {
                var serializer = new JavaScriptSerializer();

                var server = serializer.Serialize(KMP.Servers.First(s => s.Address == host));

                context.Response.Write(server);
            }
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