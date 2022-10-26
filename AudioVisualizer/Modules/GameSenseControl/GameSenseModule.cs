using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;

namespace AudioVisualizer.Modules.GameSenseControl
{
  public class GameSenseModule : IGameSenseModule
  {
    private string _sseAddress;

    public bool InitializeGameSenseConnection()
    {
      //Read the 'coreProps.json' inside %ProgramData%
      using (var reader = new StreamReader(@"C:\ProgramData\SteelSeries\SteelSeries Engine 3\coreProps.json"))
      {
        string json = reader.ReadToEnd();
        var item =  JsonSerializer.Deserialize<Item>(json); // JsonConvert.DeserializeObject<Item>(json);
        _sseAddress = item.Address;
      }

      return !string.IsNullOrEmpty(_sseAddress);
    }

    public async void SendInfoToGameSense(List<byte> data)
    {
      string test =
        $"{{\n\"game\": \"AUDIOVISUALIZER\", \n \"event\": \"AUDIO\",\n\"data\": {{\"values\": {JsonSerializer.Serialize(data)}}}\n}}";

      HttpWebRequest rq = (HttpWebRequest) WebRequest.Create("http://" + _sseAddress + "/game_event");
      rq.Method = "POST";
      rq.ContentType = "application/json";


      await using (var sWriter = new StreamWriter(rq.GetRequestStream()))
      {
        sWriter.Write(test);
        sWriter.Flush();
        sWriter.Close();
      }

      await rq.GetResponseAsync();
    }
  }

  public class Item
  {
    public string Address;
  }
}