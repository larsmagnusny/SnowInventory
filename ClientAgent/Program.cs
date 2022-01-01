using ClientAgent.Hardware;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ClientAgent
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var hInfo = HardwareProvider.GetProfile();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44308/");

            // Are we already registered?
            var responseTask = await client.GetAsync($"clientagent/register/{hInfo.Id}");

            if (responseTask.IsSuccessStatusCode)
            {
                var isRegistered = await responseTask.Content.ReadFromJsonAsync<bool>();
                if (!isRegistered)
                {
                    var postResult = await client.PostAsJsonAsync($"clientagent/register", hInfo);

                    var postTaskResult = await postResult.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
