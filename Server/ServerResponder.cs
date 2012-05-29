using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Reflection;
using System.IO;

namespace Beastmon2.Server
{
    class ServerResponder
    {
        HttpListenerContext context;

        public ServerResponder(HttpListenerContext context)
        {
            this.context = context;
        }

        public void ProcessRequest()
        {
            //string resourceName = "Beastmon2.Content.html";

            //using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            //{
            //    byte[] buffer = new byte[stream.Length];
            //    stream.Read(buffer, 0, buffer.Length);

            //    context.Response.ContentType = "text/html; charset=utf8";
            //    context.Response.ContentLength64 = buffer.Length;
            //    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            //    context.Response.OutputStream.Close();
            //}

            byte[] buffer = Encoding.UTF8.GetBytes(ComputerInfo.GetInfos() + "<meta http-equiv=\"refresh\" content=\"1\">");
            context.Response.ContentType = "text/html; charset=utf8";
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
        }
    }
}
