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
      using (var reader = new StreamReader(@"C:\ProgramData\SteelSeries\SteelSeries Engine 3\coreProps.json"))
      {
        var json = reader.ReadToEnd();
        var item = JsonConvert.DeserializeObject<Item>(json);
        _sseAddress = item.Address;
      }

      return !String.IsNullOrEmpty(_sseAddress);
    }

    public async void SendInfoToGameSense(List<byte> data)
    {
      string test =
        $"{{\n\"game\": \"AUDIOVISUALIZER\", \n \"event\": \"AUDIO\",\n\"data\": {{\"values\": {JsonConvert.SerializeObject(data)}}}\n}}";

      HttpWebRequest rq = (HttpWebRequest) WebRequest.Create("http://" + _sseAddress + "/game_event");
      rq.Method = "POST";
      rq.ContentType = "application/json";


      using (var sWriter = new StreamWriter(rq.GetRequestStream()))
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