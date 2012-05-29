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
            //Server.ServerBase server = new Server.ServerBase();
           // server.Listen();

            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();

            computer.Open();

            while (true)
            {
                computer.Accept(updateVisitor);

                foreach (IHardware hardware in computer.Hardware)
                {
                    VisitHardware(hardware, 0);
                }

                System.Threading.Thread.Sleep(1000);

                Console.Clear();
            }
            
        }

        private static void VisitHardware(IHardware hardware, int indentlevel)
        {
            Console.WriteLine(new String(' ', indentlevel * 2) + hardware.Name);

            foreach (ISensor sensor in hardware.Sensors)
            {
                Console.WriteLine("{3}{0} {1}: {2}", sensor.SensorType, sensor.Name, SensorUnits(sensor), new String(' ', (indentlevel + 1) * 2));
            }

            foreach (IHardware subhardware in hardware.SubHardware)
            {
                VisitHardware(subhardware, indentlevel + 1);
            }
        }

        private static string SensorUnits(ISensor sensor)
        {
            string format = "";
            switch (sensor.SensorType)
            {
                case SensorType.Voltage:
                    format = "{0:F3} V";
                    break;
                case SensorType.Clock:
                    format = "{0:F0} MHz";
                    break;
                case SensorType.Temperature:
                    format = "{0:F1} °C";
                    break;
                case SensorType.Fan:
                    format = "{0:F0} RPM";
                    break;
                case SensorType.Flow:
                    format = "{0:F0} L/h";
                    break;
                case SensorType.Power:
                    format = "{0:F1} W";
                    break;
                case SensorType.Data:
                    format = "{0:F1} GB";
                    break;
                case SensorType.Factor:
                    format = "{0:F3}";
                    break;
                case SensorType.Control:
                case SensorType.Load:
                    format = "{0:F1} %";
                    break;
                default:
                    format = "{0:F1}";
                    break;
            }

            return string.Format(format, sensor.Value);
        }
    }
}
