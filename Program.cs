using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenHardwareMonitor;
using OpenHardwareMonitor.Hardware;
using OpenHardwareMonitor.GUI;

namespace Beastmon2
{
    class Program
    {
        static void Main(string[] args)
        {
            Server.ServerBase server = new Server.ServerBase();
            server.Listen();
        }
    }
}
