using System;
using System.Windows.Media;

namespace AudioVisualizer.Utils.SystemColorRetriever
{
  public static class SystemColorRetriever
  {
    // http://stackoverflow.com/questions/4178049/preventing-registry-getvalue-overflow/4178122#4178122
    public static Color GetSystemColor()
    {
      Color systemColor = Colors.DarkBlue; // default if failed
      try
      {
        int argbColor = (int)Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "ColorizationColor", null);
        byte[] bytes = BitConverter.GetBytes(argbColor);
        systemColor = Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
      }
      catch (Exception)
      {
        // ignore
      }

      return systemColor;
    }
  }
}