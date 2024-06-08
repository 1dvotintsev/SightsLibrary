using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class User
{
    private static readonly HttpClient client = new HttpClient();
    private const string FunctionUrl = "https://functions.yandexcloud.net/d4eph01k1qn0fvbpdmef";

    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Reactions { get; set; }

    public User() { }

    public static async Task<User> GetUserByEmailAsync(string email)
    {
        var queryParams = new { email = email };
        string json = JsonConvert.SerializeObject(queryParams);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync(FunctionUrl, content);

        if (response.IsSuccessStatusCode)
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Received JSON: " + responseContent);
            User user = JsonConvert.DeserializeObject<User>(responseContent);
            return user;
        }
        else
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {responseContent}");
            return null;
        }
    }
}

