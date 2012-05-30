using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;

namespace Beastmon2.Server
{
    class ServerBase
    {
        Thread monitoringThread;

        public ServerBase()
        {
           this.monitoringThread = new Thread(ComputerInfo.Monitor);
           this.monitoringThread.Start();
        }

        public void Listen()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://*:5300/");
            listener.Start();

            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                new Thread(new ServerResponder(context).ProcessRequest).Start();
            }
        }
    }
}
