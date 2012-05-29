using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beastmon2.Server
{
    public class MonitoringInfo
    {
        public string CPUName { get; set; }
        public int[] CPULoads { get; set; }
        public int[] CPUTemps { get; set; }
        public string CPUTempUnits { get; set; }
        public float CPUFreq { get; set; }
        public float FSBFreq { get; set; }
        public float CPUMultiplier { get; set; }
        public float CPUVoltage { get; set; }
        
        public int CPUTempMax { get; set; }

        
        public int TotalMemory { get; set; }
        
        public int FreeMemory { get; set; }

        
        public int GPUTemp { get; set; }
        
        public int GPUTempMax { get; set; }
        
        public string GPUTempUnits { get; set; }
        
        public int GPULoad { get; set; }
        
        public string GPULoadUnits { get; set; }
        
        public float GPUMemUsage { get; set; }
        
        public float GPUMemTotal { get; set; }
        
        public string GPUMemUnits { get; set; }
        
        public float GPUCoreClock { get; set; }
        
        public string GPUCoreClockUnits { get; set; }
        
        public float GPUMemClock { get; set; }
        
        public string GPUMemClockUnits { get; set; }
        
        public int GPUFanUsage { get; set; }
        
        public string GPUFanUsageUnits { get; set; }
        
        public int GPUFanSpeed { get; set; }
        
        public string GPUFanSpeedUnits { get; set; }
        
        public string GPUDevice { get; set; }
        
        public float GPUCoreVoltage { get; set; }
        
        public string GPUCoreVoltageUnits { get; set; }
        
        public float GPUMemVoltage { get; set; }
        
        public string GPUMemVoltageUnits { get; set; }

        
        public float FPS { get; set; }
    }
}
