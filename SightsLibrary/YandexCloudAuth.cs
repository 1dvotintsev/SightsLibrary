using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class YandexCloudAuth
{
    private readonly string _signInUrl;
    private readonly string _registerUrl;

    public YandexCloudAuth()
    {
        _signInUrl = "https://functions.yandexcloud.net/d4ego1f5aatp0o7nnsgi";
        _registerUrl = "https://functions.yandexcloud.net/d4espqbmedvr2cic98lk";
    }

    public YandexCloudAuth(string signInUrl, string registerUrl)
    {
        _signInUrl = signInUrl;
        _registerUrl = registerUrl;
    }

    public async Task<bool> SignInAsync(string email, string password)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                var credentials = new
                {
                    email = email,
                    password = password
                };

                var json = JsonConvert.SerializeObject(credentials);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(_signInUrl, content);
                string result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var responseObj = JsonConvert.DeserializeObject<SignInResponse>(result);
                    return responseObj.Authenticated;
                }
                else
                {
                    Console.WriteLine($"SignIn failed with status code {response.StatusCode}: {result}");
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SignIn encountered an error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RegisterAsync(string login, string password, string name, string email, string dateOfBirth, string reactions)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                var user = new
                {
                    login = login,
                    password = password,
                    name = name,
                    email = email,
                    date_of_birth = dateOfBirth,
                    reactions = reactions
                };

                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(_registerUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    string result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Register failed with status code {response.StatusCode}: {result}");
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Register encountered an error: {ex.Message}");
            return false;
        }
    }

    private class SignInResponse
    {
        [JsonProperty("authenticated")]
        public bool Authenticated { get; set; }
    }
}
