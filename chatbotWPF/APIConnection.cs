using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace chatbotWPF
{
    static class APIConnection
    {
        static public async Task Login(string email, string password)
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

                    string responseBody = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("Response from server: " + responseBody);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Request error: " + e.Message);
                }
            }
        }

        // TODO: Se till att vi skickar rätt JSON som API vill ha, just nu skickas modellen "Send", byt i API
        static public async Task SendPrompt(Send prompt)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Replace with your API URL
                    string apiUrl = $"https://localhost:7006/api/LLaMa/send/" + prompt.Target;

                    // Serialize the data to JSON
                    string jsonData = JsonConvert.SerializeObject(prompt);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    Debug.WriteLine(apiUrl);
                    // Send POST request
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    response.EnsureSuccessStatusCode(); // Throw if not a success code

                    string responseBody = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("Response from server: " + responseBody);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Request error: " + e.Message);
                }
            }
        }
    }
}
