using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenHardwareMonitor.GUI;
using OpenHardwareMonitor.Hardware;

namespace Beastmon2.Server
{
    class ComputerInfo
    {

        public static string GetInfos() 
        {
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();

            computer.Open();

            computer.Accept(updateVisitor);

            StringBuilder builder = new StringBuilder();

            foreach (IHardware hardware in computer.Hardware)
            {
                VisitHardware(hardware, 0, builder);
            }

            return builder.ToString().Replace(" ", "&nbsp;").Replace("\n", "<br />");
        }

        private static void VisitHardware(IHardware hardware, int indentlevel, StringBuilder builder)
        {
            builder.AppendLine(new String(' ', indentlevel * 2) + hardware.Name);

            foreach (ISensor sensor in hardware.Sensors)
            {
                builder.AppendFormat("{3}{0} {1}: {2}\n", sensor.SensorType, sensor.Name, SensorUnits(sensor), new String(' ', (indentlevel + 1) * 2));
            }

            foreach (IHardware subhardware in hardware.SubHardware)
            {
                VisitHardware(subhardware, indentlevel + 1, builder);
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
