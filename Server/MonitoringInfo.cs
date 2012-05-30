using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beastmon2.Server
{
    public class MonitoringInfo
    {
        public List<CPU> CPUs { get; set; }
        public List<GPU> GPUs { get; set; }
        public RAM RAM { get; set; }

        public MonitoringInfo()
        {
            this.CPUs = new List<CPU>();
            this.GPUs = new List<GPU>();
        }

        public string SerializeToJSON()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("{'CPUs': [");
            builder.Append(string.Join(",", this.CPUs.Select(cpu => cpu.SerializeToJSON())));
            builder.Append("],");
            builder.Append("'GPUs': [");
            builder.Append(string.Join(",", this.GPUs.Select(gpu => gpu.SerializeToJSON())));
            builder.Append("],");
            builder.AppendFormat("'RAM': {0}}}", this.RAM.SerializeToJSON());

            return builder.Replace("'", "\"").ToString();
        }
    }

    public class CPU
    {
        public float[] CoreLoads { get; set; }
        public float[] CoreTemps { get; set; }
        public float TjMax { get; set; }
        public float CPUTempHighest { get; set; }

        public string SerializeToJSON()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("{'CoreLoads': [");
            builder.Append(string.Join(",", this.CoreLoads));
            builder.Append("],");
            builder.Append("'CoreTemps': [");
            builder.Append(string.Join(",", this.CoreTemps));
            builder.Append("],");
            builder.AppendFormat("'TjMax': {0}, 'CPUTempHighest': {1}}}", this.TjMax, this.CPUTempHighest);

            return builder.ToString();
        }
    }

    public class GPU
    {
        public float GPULoad { get; set; }
        public float GPUTemp { get; set; }
        public float GPUTempMax { get; set; }

        public string SerializeToJSON()
        {
            return string.Format("{{'GPULoad': {0},'GPUTemp': {1},'GPUTempMax': {2}}}", this.GPULoad, this.GPUTemp, this.GPUTempMax);
        }
    }

    public class RAM
    {
        public int FreeMemory { get; set; }
        public int TotalMemory { get; set; }

        public string SerializeToJSON()
        {
            return string.Format("{{'FreeMemory': {0},'TotalMemory': {1}}}", this.FreeMemory, this.TotalMemory);
        }
    }
}
