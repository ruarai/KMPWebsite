using System.Web;

namespace KMPRazorMix
{
    /// <summary>
    /// Summary description for Updater
    /// </summary>
    public class Updater : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            KMP.Update();
            context.Response.StatusCode = 404;
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