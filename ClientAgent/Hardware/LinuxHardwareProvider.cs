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

            var osInfoStart = new ProcessStartInfo
            {
                FileName = "cat",
                Arguments = "/etc/os-release",
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

            if (!string.IsNullOrEmpty(hostname))
                hostname = hostname.Trim(' ', '\n');

            hostnameProcess.WaitForExit();

            var osInfoProcess = new Process
            {
                StartInfo = osInfoStart
            };

            osInfoProcess.Start();
            var osInfoOutput = osInfoProcess.StandardOutput.ReadToEnd();
            osInfoProcess.WaitForExit();

            var process = new Process() { 
                StartInfo = processStart
            };

            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            var splitAttr = output.Split("\n\n", StringSplitOptions.TrimEntries);
            var splitOsInfo = osInfoOutput.Split("\n", StringSplitOptions.RemoveEmptyEntries);

            Dictionary<string, object> cpuAttributes = new();
            Dictionary<string, string> osInfo = new();

            for(int i = 0; i < splitAttr.Length; i++)
            {
                var categoryAttrs = splitAttr[i].Split('\n');

                if (categoryAttrs.Length == 0)
                    continue;

                if (categoryAttrs[0].IndexOf("processor") != -1)
                {
                    Dictionary<string, string> processorAttributes = new();

                    for(int j = 0; j < categoryAttrs.Length; j++)
                    {
                        var splitKey = categoryAttrs[j].Split(':', StringSplitOptions.TrimEntries);
                        if (!processorAttributes.ContainsKey(splitKey[0]))
                        {
                            processorAttributes[splitKey[0]] = splitKey[1]?.Trim('\n');
                        }
                    }

                    cpuAttributes.Add($"cpu{processorAttributes["processor"]}", processorAttributes);
                }
                else
                {
                    for (int j = 0; j < categoryAttrs.Length; j++)
                    {
                        var splitKey = categoryAttrs[j].Split(':', StringSplitOptions.TrimEntries);
                        if (!cpuAttributes.ContainsKey(splitKey[0]))
                        {
                            cpuAttributes[splitKey[0]] = splitKey[1]?.Trim('\n');
                        }
                    }
                }
            }

            for(int i = 0; i < splitOsInfo.Length; i++)
            {
                if (string.IsNullOrEmpty(splitOsInfo[i]))
                    continue;

                var splitKey = splitOsInfo[i].Split("=");

                osInfo.Add(splitKey[0], splitKey[1].Trim('"', '\n'));
            }

            return new()
            {
                Id = GuidUtility.Create(GuidUtility.UrlNamespace, (cpuAttributes["Serial"] as string).ToUpper()),
                ComputerName = hostname,
                BIOS = null,
                HDDSpace = 0,
                OSVersion = $"{osInfo["NAME"]} {osInfo["VERSION"]}",
                Memory = 0,
                Motherboard = null
            };
        }
    }
}
