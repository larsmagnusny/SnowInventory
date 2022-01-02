using Inventory.Common;
using Inventory.Common.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientAgent.Hardware
{
    public class LinuxHardwareProvider : IHardwareProvider
    {
        public HardwareProfile GetProfile()
        {
            var processStart = new ProcessStartInfo
            {
                FileName = "cat",
                Arguments = "/proc/cpuinfo",
                UseShellExecute = false,
                CreateNoWindow = false,
                RedirectStandardOutput = true,
                RedirectStandardError = false
            };

            var hostNameStart = new ProcessStartInfo
            {
                FileName = "hostname",
                UseShellExecute = false,
                CreateNoWindow = false,
                RedirectStandardOutput = true,
                RedirectStandardError = false
            };

            var hostnameProcess = new Process
            {
                StartInfo = hostNameStart
            };

            hostnameProcess.Start();
            var hostname = hostnameProcess.StandardOutput.ReadToEnd();
            hostnameProcess.WaitForExit();

            var process = new Process() { 
                StartInfo = processStart
            };

            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            var splitAttr = output.Split("\n\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            Dictionary<string, object> cpuAttributes = new();

            for(int i = 0; i < splitAttr.Length; i++)
            {
                var splitKey = splitAttr[i].Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                if (splitKey.Length == 0)
                    continue;

                if (splitKey[0].CompareTo("processor") == 0)
                {
                    Dictionary<string, string> processorAttributes = new();

                    for(int j = 0; j < splitKey.Length - 1; j += 2)
                    {
                        if (!processorAttributes.ContainsKey(splitKey[j]))
                        {
                            cpuAttributes[splitKey[j]] = splitKey[j + 1];
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < splitKey.Length - 1; j += 2)
                    {
                        if (!cpuAttributes.ContainsKey(splitKey[j]))
                        {
                            cpuAttributes[splitKey[j]] = splitKey[j + 1];
                        }
                    }
                }
            }

            return new()
            {
                Id = GuidUtility.Create(GuidUtility.UrlNamespace, (cpuAttributes["Serial"] as string).ToUpper()),
                ComputerName = hostname,
                BIOS = null,
                HDDSpace = 0,
                OSVersion = "Linux",
                Memory = 0,
                Motherboard = null
            };
        }
    }
}
