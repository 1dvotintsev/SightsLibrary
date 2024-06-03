using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class Place
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public string Location { get; set; }

    private static readonly HttpClient client = new HttpClient();
    private const string functionUrl = "https://functions.yandexcloud.net/d4efabacb9d3f3gupg8i";

    public Place(int id)
    {
        Task.Run(async () => await InitializePlaceAsync(id)).Wait();
    }

    private async Task InitializePlaceAsync(int id)
    {
        string requestUrl = $"{functionUrl}?id={id}";

        try
        {
            HttpResponseMessage response = await client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                var place = JsonConvert.DeserializeObject<Place>(responseBody);
                this.Id = place.Id;
                this.Name = place.Name;
                this.Description = place.Description;
                this.Type = place.Type;
                this.Location = place.Location;
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();;
                throw new Exception($"Failed to retrieve place with ID {id}: {response.ReasonPhrase}");
            }
        }
        catch (HttpRequestException e)
        {
            throw new Exception($"Failed to retrieve place with ID {id}: {e.Message}");
        }
    }
}
