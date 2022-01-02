using ClientAgent.Hardware;
using ClientAgent.Software;
using Inventory.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace ClientAgent
{
    class Program
    {
        private static bool InDocker { 
            get {
                return Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
            }
        }

        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                services.AddSingleton<IInstalledSoftwareProvider, WindowsInstalledSoftwareProvider>();

                if (InDocker)
                    services.AddSingleton<IHardwareProvider, DockerHardwareProvider>();
                else
                    services.AddSingleton<IHardwareProvider, WindowsHardwareProvider>();
            }
            else
                throw new NotImplementedException("Platform not supported!");

            var serviceProvider = services.BuildServiceProvider();

            IHardwareProvider hardwareProvider = serviceProvider.GetService<IHardwareProvider>();

            var hInfo = hardwareProvider.GetProfile();

            var clientHandler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; },
                SslProtocols = System.Security.Authentication.SslProtocols.Tls
            };

            HttpClient client = new HttpClient(clientHandler);
            client.BaseAddress = new Uri("https://DESKTOP-2TOHRIH.local:44308/");

            // Are we already registered?
            var postResult = await client.PostAsJsonAsync($"clientagent/register", hInfo);
            var postTaskResult = await postResult.Content.ReadAsStringAsync();

            IInstalledSoftwareProvider softwareProvider = serviceProvider.GetService<IInstalledSoftwareProvider>();

            var heartBeat = 1;

            while (true)
            {
                var installedSoftware = softwareProvider.GetInstalledSoftware();
                var runningPrograms = softwareProvider.GetRunningPrograms();

                postResult = await client.PostAsJsonAsync("clientagent/heartbeat", new HeartBeat {
                    ClientAgentId = hInfo.Id,
                    InstalledSoftware = installedSoftware,
                    RunningPrograms = runningPrograms
                });

                if (!postResult.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Heartbeat failed, trying again in {heartBeat} minutes");
                }
                else
                {
                    Console.WriteLine("Updated installed programs and running programs...");
                }

                Thread.Sleep(heartBeat * 1000 * 60);
            }
        }
    }
}
