using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;

namespace AudioVisualizer1.MVVM.Model;

public interface IGameSense
{
    /// <summary>
    /// Looks for the coreProps.json of SteelSeries containing the local address of the keyboard and saves it.
    /// </summary>
    /// <returns>Returns true if address found</returns>
    bool InitializeGameSenseConnection();

    void SendInfoToGameSense(List<byte> data);
}

public class GameSense : IGameSense
{
    private readonly HttpClient _client = new();
    private string _sseAddress;

    public bool InitializeGameSenseConnection()
    {
        //Read the 'coreProps.json' inside %ProgramData%
        using (var reader = new StreamReader(@"C:\ProgramData\SteelSeries\SteelSeries Engine 3\coreProps.json"))
        {
            var json = reader.ReadToEnd();
            var item = JsonSerializer.Deserialize<Item>(json);
            _sseAddress = item!.Address;
        }

        return !string.IsNullOrEmpty(_sseAddress);
    }

    public async void SendInfoToGameSense(List<byte> data)
    {
        var test =
            $"{{\n\"game\": \"AUDIOVISUALIZER\", \n \"event\": \"AUDIO\",\n\"data\": {{\"values\": {JsonSerializer.Serialize(data)}}}\n}}";

        try
        {
            using var response =
                await _client.PostAsync("http://" + _sseAddress + "/game_event", new StringContent(test));
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }
    }
}

public struct Item
{
    public string Address;
}