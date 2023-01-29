using System;
using System.Diagnostics;
using System.Management;
using System.Security.Principal;
using System.Windows.Media;

namespace AudioVisualizer1.Utils;

public class SystemColorRetriever
{
    private static ManagementEventWatcher _watcher = null!;
    private const string ValueName = "ColorizationColor";
    private const string KeyPath = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM";

    private static Color _systemColor;

    public Color SystemColor
    {
        get => _systemColor;
        private set
        {
            _systemColor = value;
            // OnPropertyChanged(() => ThemeColor);
        }
    }

    public Brush ThemeColor
    {
        get
        {
            var color = SystemColor;
            return new SolidColorBrush(Color.FromArgb(255, color.R, color.G, color.B));
        }
    }


    /// <summary>
    /// http://stackoverflow.com/questions/4178049/preventing-registry-getvalue-overflow/4178122#4178122
    /// </summary>
    /// <returns></returns>
    public Color GetSystemColor()
    {
        var systemColor = Colors.DarkBlue; // default if failed
        try
        {
            var argbColor = (int)Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM",
                "ColorizationColor", null)!;
            var bytes = BitConverter.GetBytes(argbColor);
            systemColor = Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
        }
        catch (Exception)
        {
            // ignore
        }

        SystemColor = systemColor;
        return systemColor;
    }

    public void Start()
    {
        var currentUser = WindowsIdentity.GetCurrent();
        try
        {
            if (currentUser.User == null) return;

            var query = new WqlEventQuery(
                $@"SELECT * FROM RegistryValueChangeEvent WHERE Hive='HKEY_USERS' AND KeyPath='{currentUser.User.Value}\\Software\\Microsoft\\Windows\\DWM\\' AND ValueName='ColorizationColor'");
            _watcher = new ManagementEventWatcher(query);

            _watcher.EventArrived += OnKeyValueChanged;
            _watcher.Start();
        }
        catch (ManagementException err)
        {
            Debug.WriteLine($"Error: {err.Message}");
        }
    }

    public void Stop()
    {
        _watcher.Stop();
        _watcher.EventArrived -= OnKeyValueChanged;
    }

    private void OnKeyValueChanged(object sender, EventArrivedEventArgs e)
    {
        SystemColor = GetSystemColor();
    }
}