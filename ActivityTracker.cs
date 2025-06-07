using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Timers;

namespace WawaPomodoro
{
    public class ActivityTracker
    {
        // Win32 API calls
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        // Activity record
        public class ActivityRecord
        {
            public string ProcessName { get; set; }
            public string WindowTitle { get; set; }
            public DateTime StartTime { get; set; }
            public TimeSpan Duration { get; set; }
            public string Date => StartTime.ToString("yyyy-MM-dd");
        }

        private System.Timers.Timer trackingTimer;
        private List<ActivityRecord> activityHistory = new List<ActivityRecord>();
        private ActivityRecord currentActivity;
        private HashSet<string> allowedProcesses = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private HashSet<string> allowedTitleKeywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private Action<string, string, string> onDisallowedActivityActivated;
        private string logFilePath;

        public List<ActivityRecord> ActivityHistory => activityHistory;
        public HashSet<string> AllowedProcesses => allowedProcesses;
        public HashSet<string> AllowedTitleKeywords => allowedTitleKeywords;

        public ActivityTracker(Action<string, string, string> disallowedActivityCallback)
        {
            onDisallowedActivityActivated = disallowedActivityCallback;
            trackingTimer = new System.Timers.Timer(1000); // Check every second
            trackingTimer.Elapsed += TrackingTimer_Elapsed;
            
            // Set up log path
            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WawaPomodoro");
                
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }
            
            logFilePath = Path.Combine(appDataPath, $"activity_{DateTime.Now:yyyy-MM-dd}.jsonl");
            
            // Load settings
            LoadSettings();
            
            // Load today's activities
            LoadTodayActivities();
        }

        public void Start()
        {
            trackingTimer.Start();
            UpdateCurrentActivity();
        }

        public void Stop()
        {
            trackingTimer.Stop();
            if (currentActivity != null)
            {
                currentActivity.Duration = DateTime.Now - currentActivity.StartTime;
                activityHistory.Add(currentActivity);
                SaveActivity(currentActivity);
                currentActivity = null;
            }
        }

        private void TrackingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            UpdateCurrentActivity();
        }

        private void UpdateCurrentActivity()
        {
            try
            {
                // Get current active window
                IntPtr hWnd = GetForegroundWindow();
                if (hWnd == IntPtr.Zero) return;

                // Get process ID and name
                uint processId;
                GetWindowThreadProcessId(hWnd, out processId);

                Process process = Process.GetProcessById((int)processId);
                string processName = process.ProcessName;

                StringBuilder titleBuilder = new StringBuilder(256);
                GetWindowText(hWnd, titleBuilder, titleBuilder.Capacity);
                string windowTitle = titleBuilder.ToString();
                
                // Check if activity has changed
                if (currentActivity == null || 
                    currentActivity.ProcessName != processName || 
                    currentActivity.WindowTitle != windowTitle)
                {
                    // Save previous activity
                    if (currentActivity != null)
                    {
                        currentActivity.Duration = DateTime.Now - currentActivity.StartTime;
                        activityHistory.Add(currentActivity);
                        SaveActivity(currentActivity);
                    }

                    // Create new activity record
                    currentActivity = new ActivityRecord
                    {
                        ProcessName = processName,
                        WindowTitle = windowTitle,
                        StartTime = DateTime.Now,
                        Duration = TimeSpan.Zero
                    };

                    // Check if process is allowed or title contains allowed keywords
                    bool isAllowed = allowedProcesses.Contains(processName);
                    
                    // If process is not allowed, check if title contains any allowed keywords
                    if (!isAllowed && !string.IsNullOrEmpty(windowTitle))
                    {
                        foreach (var keyword in allowedTitleKeywords)
                        {
                            if (windowTitle.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                            {
                                isAllowed = true;
                                break;
                            }
                        }
                    }
                    
                    if (!isAllowed && onDisallowedActivityActivated != null)
                    {
                        onDisallowedActivityActivated(processName, windowTitle, string.Empty);
                    }
                }
                else
                {
                    // Update current activity duration
                    currentActivity.Duration = DateTime.Now - currentActivity.StartTime;
                }
            }
            catch (Exception)
            {
                // Ignore errors to prevent crashes
            }
        }

        public void AddAllowedProcess(string processName)
        {
            if (!string.IsNullOrWhiteSpace(processName))
            {
                allowedProcesses.Add(processName);
                SaveSettings();
            }
        }

        public void RemoveAllowedProcess(string processName)
        {
            if (allowedProcesses.Remove(processName))
            {
                SaveSettings();
            }
        }
        
        public void AddAllowedTitleKeyword(string keyword)
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                allowedTitleKeywords.Add(keyword);
                SaveSettings();
            }
        }

        public void RemoveAllowedTitleKeyword(string keyword)
        {
            if (allowedTitleKeywords.Remove(keyword))
            {
                SaveSettings();
            }
        }

        public void ClearActivityHistory()
        {
            activityHistory.Clear();
        }
        
        private void SaveActivity(ActivityRecord activity)
        {
            try
            {
                string json = JsonSerializer.Serialize(activity);
                File.AppendAllText(logFilePath, json + Environment.NewLine);
            }
            catch
            {
                // Ignore file errors
            }
        }
        
        private void LoadTodayActivities()
        {
            try
            {
                if (File.Exists(logFilePath))
                {
                    string[] lines = File.ReadAllLines(logFilePath);
                    foreach (string line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            try
                            {
                                var activity = JsonSerializer.Deserialize<ActivityRecord>(line);
                                if (activity != null)
                                {
                                    activityHistory.Add(activity);
                                }
                            }
                            catch
                            {
                                // Ignore parsing errors
                            }
                        }
                    }
                }
            }
            catch
            {
                // Ignore file errors
            }
        }
        
        public List<ActivityRecord> LoadActivitiesForDate(DateTime date)
        {
            string dateStr = date.ToString("yyyy-MM-dd");
            string filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WawaPomodoro",
                $"activity_{dateStr}.jsonl");
                
            List<ActivityRecord> records = new List<ActivityRecord>();
            
            try
            {
                if (File.Exists(filePath))
                {
                    string[] lines = File.ReadAllLines(filePath);
                    foreach (string line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            try
                            {
                                var activity = JsonSerializer.Deserialize<ActivityRecord>(line);
                                if (activity != null)
                                {
                                    records.Add(activity);
                                }
                            }
                            catch
                            {
                                // Ignore parsing errors
                            }
                        }
                    }
                }
            }
            catch
            {
                // Ignore file errors
            }
            
            return records;
        }
        
        public List<ActivityRecord> LoadActivitiesForDateRange(DateTime startDate, DateTime endDate)
        {
            List<ActivityRecord> records = new List<ActivityRecord>();
            
            for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                records.AddRange(LoadActivitiesForDate(date));
            }
            
            return records;
        }
        
        private void SaveSettings()
        {
            try
            {
                string appDataPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "WawaPomodoro");
                    
                if (!Directory.Exists(appDataPath))
                {
                    Directory.CreateDirectory(appDataPath);
                }
                
                string settingsPath = Path.Combine(appDataPath, "settings.json");
                
                var settings = new
                {
                    AllowedProcesses = allowedProcesses.ToArray(),
                    AllowedTitleKeywords = allowedTitleKeywords.ToArray()
                };
                
                string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(settingsPath, json);
            }
            catch
            {
                // Ignore file errors
            }
        }
        
        private void LoadSettings()
        {
            try
            {
                string appDataPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "WawaPomodoro");
                string settingsPath = Path.Combine(appDataPath, "settings.json");
                
                if (File.Exists(settingsPath))
                {
                    string json = File.ReadAllText(settingsPath);
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var settings = JsonSerializer.Deserialize<SettingsData>(json, options);
                    
                    if (settings != null)
                    {
                        if (settings.AllowedProcesses != null)
                        {
                            foreach (var process in settings.AllowedProcesses)
                            {
                                allowedProcesses.Add(process);
                            }
                        }
                        
                        if (settings.AllowedTitleKeywords != null)
                        {
                            foreach (var keyword in settings.AllowedTitleKeywords)
                            {
                                allowedTitleKeywords.Add(keyword);
                            }
                        }
                    }
                }
                else
                {
                    // Default allowed processes
                    allowedProcesses.Add("devenv");     // Visual Studio
                    allowedProcesses.Add("Code");       // VS Code
                    allowedProcesses.Add("notepad");    // Notepad
                    allowedProcesses.Add("chrome");     // Chrome browser
                    allowedProcesses.Add("msedge");     // Edge browser
                    allowedProcesses.Add("firefox");    // Firefox browser
                    allowedProcesses.Add("WawaPomodoro"); // This app
                    
                    // Save default settings
                    SaveSettings();
                }
            }
            catch
            {
                // Ignore file errors, use default allowed processes
            }
        }
        
        private class SettingsData
        {
            public string[] AllowedProcesses { get; set; }
            public string[] AllowedTitleKeywords { get; set; }
        }
    }
}