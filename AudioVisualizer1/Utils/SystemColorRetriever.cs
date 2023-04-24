using System;
using System.Diagnostics;
using System.Management;
using System.Security.Principal;
using System.Windows.Media;
using AudioVisualizer1.Core;

namespace AudioVisualizer1.Utils;

public class SystemColorRetriever : ObservableObject
{
    private static ManagementEventWatcher _watcher = null!;
    private const string ValueName = "ColorizationColor";
    private const string KeyPath = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM";

    private static Color _systemColor;
    private bool _isWatcherRunning;

    public bool IsWatcherRunning
    {
        get => _isWatcherRunning;
        set
        {
            if (value && !_isWatcherRunning)
            {
                Start();
                _isWatcherRunning = value;
            }
            else
            {
                _isWatcherRunning = value;
            }
        }
    }

    public Color SystemColor
    {
        get => _systemColor;
        private set
        {
            _systemColor = value;
            OnPropertyChanged();
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
        var systemColor = Colors.Black; // default if failed
        try
        {
            var argbColor = (int)Microsoft.Win32.Registry.GetValue(KeyPath,
                ValueName, null)!;
            var bytes = BitConverter.GetBytes(argbColor);
            systemColor = Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
        }
        catch (Exception e)
        {
            // ignore
            Debug.WriteLine($"Failed to get SystemColor from Registry. Error Message: {e.Message}");
        }

        SystemColor = systemColor;
        return systemColor;
    }

    public void Start()
    {
        if (_isWatcherRunning)
        {
            Debug.WriteLine("Watcher is already running. Abort");
            return;
        }

        Debug.WriteLine("Starting watcher...");
        var currentUser = WindowsIdentity.GetCurrent();
        try
        {
            if (currentUser.User == null) return;

            var query = new WqlEventQuery(
                @"SELECT * FROM RegistryValueChangeEvent " +
                @"WHERE Hive='HKEY_USERS' " +
                $@"AND KeyPath='{currentUser.User.Value}\\Software\\Microsoft\\Windows\\DWM\\' " +
                $@"AND ValueName='{ValueName}'");
            _watcher = new ManagementEventWatcher(query);

            _watcher.EventArrived += OnKeyValueChanged;
            _watcher.Start();
            _isWatcherRunning = true;
        }
        catch (ManagementException err)
        {
            Debug.WriteLine($"Error: {err.Message}");
            _isWatcherRunning = false;
            Stop();
        }
    }

    public void Stop()
    {
        Debug.WriteLine("Stopping watcher...");
        _watcher?.Stop();
        if (_watcher != null) _watcher.EventArrived -= OnKeyValueChanged;
        _isWatcherRunning = false;
    }

    private void OnKeyValueChanged(object sender, EventArrivedEventArgs e)
    {
        SystemColor = GetSystemColor();
        OnPropertyChanged(nameof(ThemeColor));
        Debug.WriteLine($"{nameof(SystemColor)} value changed: {SystemColor.ToString()}");
    }
}