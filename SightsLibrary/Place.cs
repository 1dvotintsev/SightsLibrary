using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class Place
{
    public int PlaceId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public string Location { get; set; }

    public Place(int id)
    {
        var task = FetchPlaceAsync(id);
        task.Wait();
    }

    private async Task FetchPlaceAsync(int id)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync($"https://functions.yandexcloud.net/d4efabacb9d3f3gupg8i?id={id}");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                JsonConvert.PopulateObject(json, this);
            }
            else
            {
                throw new Exception($"Failed to retrieve place with ID {id}: {response.ReasonPhrase}");
            }
        }
    }
}
