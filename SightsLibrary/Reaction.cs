using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class Reaction
{
    private static readonly HttpClient client = new HttpClient();
    private const string FunctionUrl = "https://functions.yandexcloud.net/d4ep24ilo6379m49b44e";

    private int UserId { get; }
    private int PlaceId { get; }

    public Reaction(int userId, int placeId)
    {
        UserId = userId;
        PlaceId = placeId;
    }

    private async Task SendReactionAsync(int reaction)
    {
        var reactionData = new
        {
            user_id = UserId,
            place_id = PlaceId,
            reaction = reaction
        };

        string json = JsonConvert.SerializeObject(reactionData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Добавим вывод отправляемого JSON для отладки
        Console.WriteLine("Sending JSON:");
        Console.WriteLine(json);

        HttpResponseMessage response = await client.PostAsync(FunctionUrl, content);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Reaction recorded successfully.");
        }
        else
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {responseContent}");
        }
    }

    public Task Like()
    {
        int like = 1;
        return SendReactionAsync(like);
    }

    public Task Dislike()
    {
        int dislike = -1;
        return SendReactionAsync(dislike);
    }
}

