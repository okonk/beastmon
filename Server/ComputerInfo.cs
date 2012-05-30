using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenHardwareMonitor.GUI;
using OpenHardwareMonitor.Hardware;
using System.Threading;
using System.Runtime.InteropServices;

namespace Beastmon2.Server
{
    class ComputerInfo
    {
        public static MonitoringInfo Info { get; set; }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX buffer);

        public static void Monitor() 
        {
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();

            computer.Open();

            while (true)
            {
                computer.Accept(updateVisitor);
                MonitoringInfo currentInfo = new MonitoringInfo();

                foreach (IHardware hardware in computer.Hardware)
                {
                    if (hardware.HardwareType == HardwareType.CPU)
                    {
                        CPU cpu = new CPU();
                        AddCPUInfo(hardware.Sensors, cpu);

                        currentInfo.CPUs.Add(cpu);
                    }
                    else if (hardware.HardwareType == HardwareType.GpuAti)
                    {
                        GPU gpu = new GPU();
                        gpu.GPUTempMax = 100; // assume max gpu temp is 100 *C
                        AddGPUInfo(hardware.Sensors, gpu);

                        currentInfo.GPUs.Add(gpu);
                    }
                    else if (hardware.HardwareType == HardwareType.GpuNvidia)
                    {
                        GPU gpu = new GPU();
                        gpu.GPUTempMax = 100; // assume max gpu temp is 100 *C
                        AddGPUInfo(hardware.Sensors, gpu);

                        currentInfo.GPUs.Add(gpu);
                    }
                }

                RAM ram = new RAM();
                MEMORYSTATUSEX memoryUsage = new MEMORYSTATUSEX();
                if (GlobalMemoryStatusEx(memoryUsage))
                {
                    ram.FreeMemory = (int)(memoryUsage.ullAvailPhys / (1024 * 1024)); // in MB
                    ram.TotalMemory = (int)(memoryUsage.ullTotalPhys / (1024 * 1024)); // in MB
                }

                currentInfo.RAM = ram;

                Info = currentInfo;

                Thread.Sleep(1000);
            }
        }

        private static void AddGPUInfo(ISensor[] sensors, GPU gpu)
        {
            foreach (ISensor sensor in sensors)
            {
                if (sensor.SensorType == SensorType.Load)
                {
                    gpu.GPULoad = sensor.Value.Value;
                }
                else if (sensor.SensorType == SensorType.Temperature)
                {
                    gpu.GPUTemp = sensor.Value.Value;
                }
            }
        }

        private static void AddCPUInfo(ISensor[] sensors, CPU cpu)
        {
            float tjMax = 100;
            float[] loads = new float[64]; // 64 cores ought to be enough for anybody
            float[] temps = new float[64];
            float[] highTemps = new float[64];
            int numberOfLoads = 0;
            int numberOfTemps = 0;
            ISensor lastTempSensor = null; // used for finding tjMax

            foreach (ISensor sensor in sensors)
            {
                if (sensor.SensorType == SensorType.Load 
                    && sensor.Name.StartsWith("CPU Core"))
                {
                    loads[sensor.Index - 1] = sensor.Value.Value;
                    numberOfLoads++;
                }
                else if (sensor.SensorType == SensorType.Temperature 
                    && sensor.Name.StartsWith("CPU Core") // CPU Core for Intel
                    || sensor.Name.StartsWith("Core")) // Core for AMD Phenom
                {
                    temps[sensor.Index] = sensor.Value.Value;
                    highTemps[sensor.Index] = sensor.Max.Value;
                    numberOfTemps++;

                    lastTempSensor = sensor;
                }
            }

            if (lastTempSensor != null)
            {
                foreach (IParameter parameter in lastTempSensor.Parameters)
                {
                    if (parameter.Name.StartsWith("TjMax"))
                    {
                        tjMax = parameter.Value;
                    }
                }
            }

            cpu.TjMax = tjMax;
            cpu.CPUTempHighest = highTemps.Take(numberOfTemps).Max();
            cpu.CoreLoads = loads.Take(numberOfLoads).ToArray();
            cpu.CoreTemps = temps.Take(numberOfTemps).ToArray();
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    class MEMORYSTATUSEX
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;
        public MEMORYSTATUSEX()
        {
            this.dwLength = (uint)Marshal.SizeOf(this);
        }
    }
}
