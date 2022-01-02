using Inventory.Common;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientAgent.Software
{
    public class WindowsInstalledSoftwareProvider : IInstalledSoftwareProvider
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "This implementation should only be used when ran on windows")]
        public IEnumerable<InstalledSoftware> GetInstalledSoftware()
        {
            Dictionary<string, InstalledSoftware> ret = new Dictionary<string, InstalledSoftware>();

            string displayName, version, publisher;


            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", false))
            {
                if (key != null)
                {
                    foreach (String keyName in key.GetSubKeyNames())
                    {
                        RegistryKey subkey = key.OpenSubKey(keyName);
                        displayName = subkey.GetValue("DisplayName") as string;
                        version = subkey.GetValue("DisplayVersion") as string;
                        publisher = subkey.GetValue("Publisher") as string;

                        if (string.IsNullOrEmpty(displayName))
                            continue;
                        if (ret.ContainsKey(displayName))
                            continue;

                        ret.Add(displayName, new InstalledSoftware
                        {
                            Name = displayName,
                            Version = version,
                            Publisher = publisher
                        });
                    }
                }
            }

            //using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", false))
            using (var localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                var key = localMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", false);

                if (key != null)
                {
                    foreach (String keyName in key.GetSubKeyNames())
                    {
                        RegistryKey subkey = key.OpenSubKey(keyName);
                        displayName = subkey.GetValue("DisplayName") as string;
                        version = subkey.GetValue("DisplayVersion") as string;
                        publisher = subkey.GetValue("Publisher") as string;

                        if (string.IsNullOrEmpty(displayName))
                            continue;
                        if (ret.ContainsKey(displayName))
                            continue;

                        ret.Add(displayName, new InstalledSoftware
                        {
                            Name = displayName,
                            Version = version,
                            Publisher = publisher
                        });
                    }
                }
            }

            using (var localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
            {
                var key = localMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", false);

                if (key != null)
                {
                    foreach (String keyName in key.GetSubKeyNames())
                    {
                        RegistryKey subkey = key.OpenSubKey(keyName);
                        displayName = subkey.GetValue("DisplayName") as string;
                        version = subkey.GetValue("DisplayVersion") as string;
                        publisher = subkey.GetValue("Publisher") as string;

                        if (string.IsNullOrEmpty(displayName))
                            continue;
                        if (ret.ContainsKey(displayName))
                            continue;

                        ret.Add(displayName, new InstalledSoftware
                        {
                            Name = displayName,
                            Version = version,
                            Publisher = publisher
                        });
                    }
                }
            }

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall", false))
            {
                if (key != null)
                {
                    foreach (String keyName in key.GetSubKeyNames())
                    {
                        RegistryKey subkey = key.OpenSubKey(keyName);
                        displayName = subkey.GetValue("DisplayName") as string;
                        version = subkey.GetValue("DisplayVersion") as string;
                        publisher = subkey.GetValue("Publisher") as string;

                        if (string.IsNullOrEmpty(displayName))
                            continue;
                        if (ret.ContainsKey(displayName))
                            continue;

                        ret.Add(displayName, new InstalledSoftware
                        {
                            Name = displayName,
                            Version = version,
                            Publisher = publisher
                        });
                    }
                }
            }

            return ret.Values;
        }

        public IEnumerable<RunningProgram> GetRunningPrograms()
        {
            var ret = new List<RunningProgram>();
            var processes = Process.GetProcesses();

            foreach(var process in processes)
            {
                ret.Add(new RunningProgram
                {
                    ProcessId = process.Id,
                    ProcessName = process.ProcessName,
                    WindowTitle = process.MainWindowTitle
                });
            }

            return ret;
        }
    }
}
