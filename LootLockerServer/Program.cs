using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var client = new HttpClient();

// Lấy key server từ biến môi trường Railway
var lootKey = Environment.GetEnvironmentVariable("LOOTLOCKER_KEY");

// Endpoint cấp asset
app.MapPost("/giveAsset", async (HttpRequest request) =>
{
    var body = await new StreamReader(request.Body).ReadToEndAsync();
    dynamic data = JsonConvert.DeserializeObject(body);

    string playerId = data.playerId;
    string assetId = data.assetId;

    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {lootKey}");

    var payload = new { asset_id = int.Parse(assetId) };
    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

    var url = $"https://api.lootlocker.io/management/players/{playerId}/inventory/add";
    var response = await client.PostAsync(url, content);
    var resText = await response.Content.ReadAsStringAsync();

    return Results.Content(resText, "application/json");
});

// Endpoint remove asset
app.MapPost("/removeAsset", async (HttpRequest request) =>
{
    var body = await new StreamReader(request.Body).ReadToEndAsync();
    dynamic data = JsonConvert.DeserializeObject(body);

    string playerId = data.playerId;
    string assetId = data.assetId;

    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {lootKey}");

    var payload = new { asset_id = int.Parse(assetId) };
    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

    var url = $"https://api.lootlocker.io/management/players/{playerId}/inventory/remove";
    var response = await client.PostAsync(url, content);
    var resText = await response.Content.ReadAsStringAsync();

    return Results.Content(resText, "application/json");
});

app.Run();
