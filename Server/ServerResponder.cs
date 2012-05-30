using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Reflection;
using System.IO;
using System.Threading;

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
            if (this.context.Request.RawUrl == "/Info")
            {
                while (ComputerInfo.Info == null) 
                    Thread.Sleep(500);

                string info = ComputerInfo.Info.SerializeToJSON();
                byte[] infoBytes = Encoding.UTF8.GetBytes(info);

                context.Response.ContentType = "application/json; charset=utf8";
                context.Response.ContentLength64 = infoBytes.Length;

                try
                {
                    context.Response.OutputStream.Write(infoBytes, 0, infoBytes.Length);
                }
                catch (Exception)
                {
                    // don't even care
                    // Exception was most likely from the user closing their browser in the middle of Writing to the stream.
                }
                finally
                {
                    context.Response.OutputStream.Close();
                }
            }
            else
            {
                string resourceName = "Beastmon2.Content.html";

                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);

                    context.Response.ContentType = "text/html; charset=utf8";
                    context.Response.ContentLength64 = buffer.Length;

                    try
                    {
                        context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                    }
                    catch (Exception)
                    {
                        // don't even care
                        // Exception was most likely from the user closing their browser in the middle of Writing to the stream.
                    }
                    finally
                    {
                        context.Response.OutputStream.Close();
                    }
                }
            }
        }
    }
}
