using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Npgsql;

public class User
{
    private static readonly HttpClient client = new HttpClient();
    private const string FunctionUrl = "https://functions.yandexcloud.net/d4eph01k1qn0fvbpdmef";
    private string connectionString = "Host=79.174.88.41;Port=17429;Username=admin_1;Password=12345bB!;Database=db_reactions";

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

    public static async Task<List<int>> GetFavourite1Async( int id)
    {
        List<int> placeIds = new List<int>();
        string connectionString = "Host=79.174.88.41;Port=17429;Username=admin_1;Password=12345bB!;Database=db_reactions";

        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();

        // Пример SQL-запроса. Замените на свой запрос в соответствии с вашей схемой базы данных.
        var sql = "SELECT place_id FROM reactions WHERE user_id = @userId AND reaction = 1";

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("userId", id);

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            placeIds.Add(reader.GetInt32(1)); // Предполагается, что place_id находится в первом столбце результата
        }

        return placeIds;
    }
}

