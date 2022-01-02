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

            Console.WriteLine(output);

            var splitAttr = output.Split("\n\n", StringSplitOptions.TrimEntries);

            Dictionary<string, object> cpuAttributes = new();

            for(int i = 0; i < splitAttr.Length; i++)
            {
                Console.WriteLine(splitAttr[i]);

                var categoryAttrs = splitAttr[i].Split('\n');

                if (categoryAttrs.Length == 0)
                    continue;

                if (categoryAttrs[0].IndexOf("processor") != -1)
                {
                    Dictionary<string, string> processorAttributes = new();

                    for(int j = 0; j < categoryAttrs.Length; j++)
                    {
                        var splitKey = categoryAttrs[j].Split(':', StringSplitOptions.TrimEntries);
                        Console.WriteLine($"{splitKey[0]}: {splitKey[1]}");
                        if (!processorAttributes.ContainsKey(splitKey[0]))
                        {
                            processorAttributes[splitKey[0]] = splitKey[1];
                        }
                    }

                    cpuAttributes.Add($"cpu{processorAttributes["processor"]}", processorAttributes);
                }
                else
                {
                    for (int j = 0; j < categoryAttrs.Length; j++)
                    {
                        var splitKey = categoryAttrs[j].Split(':', StringSplitOptions.TrimEntries);
                        Console.WriteLine($"{splitKey[0]}: {splitKey[1]}");
                        if (!cpuAttributes.ContainsKey(splitKey[0]))
                        {
                            cpuAttributes[splitKey[0]] = splitKey[1];
                        }
                    }
                }
            }

            Console.WriteLine(hostname);
            Console.WriteLine(cpuAttributes.ToString());

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
