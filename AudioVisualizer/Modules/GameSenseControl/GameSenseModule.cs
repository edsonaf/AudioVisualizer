using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Net;

namespace AudioVisualizer.Modules.GameSenseControl
{
  [PartCreationPolicy(CreationPolicy.Shared)] // Singleton
  [Export(typeof(IGameSenseModule))]
  public class GameSenseModule : IGameSenseModule
  {
    private string _sseAddress;

    public bool InitializeGameSenseConnection()
    {
      //Read the 'coreProps.json' inside %ProgramData%
      using (StreamReader reader = new StreamReader("C:\\ProgramData\\SteelSeries\\SteelSeries Engine 3\\coreProps.json"))
      {
        string json = reader.ReadToEnd();
        Item item = JsonConvert.DeserializeObject<Item>(json);
        _sseAddress = item.Address;
      };

      if (!String.IsNullOrEmpty(_sseAddress))
        return true;

      return false;
    }

    public async void SendInfoToGameSense(List<byte> data)
    {
      string test = "{\n\"game\": \"AUDIOVISUALIZER\", \n \"event\": \"AUDIO\",\n\"data\": {\"values\": " + JsonConvert.SerializeObject(data) + "}\n}";

      HttpWebRequest rq = (HttpWebRequest)WebRequest.Create("http://" + _sseAddress + "/game_event");
      rq.Method = "POST";
      rq.ContentType = "application/json";


      using (var sWriter = new StreamWriter(rq.GetRequestStream()))
      {
        var dataInArray = data.ToArray();
        sWriter.Write(test);
        sWriter.Flush();
        sWriter.Close();
      }

      var rp = await rq.GetResponseAsync();
    }
  }

  public class Item
  {
    public string Address;
  }
}
