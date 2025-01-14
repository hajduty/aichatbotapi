using chatbotWPF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace chatbotWPF
{
    static class APIConnection
    {
        static public async Task<bool> Login(string email, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Replace with your API URL
                    string apiUrl = "https://localhost:7006/api/Auth/login";

                    // Data to send in the POST request
                    var data = new
                    {
                        email = email,
                        password = password
                    };

                    // Serialize the data to JSON
                    string jsonData = JsonConvert.SerializeObject(data);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Send POST request
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    response.EnsureSuccessStatusCode(); // Throw if not a success code

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }

                    string responseBody = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("Response from server: " + responseBody);
                    return false;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Request error: " + e.Message);
                    return false;
                }
            }
        }

        // TODO: Se till att vi skickar rätt JSON som API vill ha, just nu skickas modellen "Send", byt i API

        static public async Task<AiResponse> SendPrompt(Send prompt, string presidentId)
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = $"https://localhost:7006/LLama/send/" + presidentId;

                string jsonData = JsonConvert.SerializeObject(prompt);
                client.Timeout = TimeSpan.FromMinutes(5);

                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AiResponse>(responseBody);
                Debug.WriteLine(responseBody);
                return result;

            }
        }
    }
}
