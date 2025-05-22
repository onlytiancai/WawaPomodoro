using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Timers;

namespace WawaPomodoro
{
    public class ActivityTracker
    {
        // Win32 API 调用
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        // 活动记录
        public class ActivityRecord
        {
            public string ProcessName { get; set; }
            public string WindowTitle { get; set; }
            public string Domain { get; set; }
            public DateTime StartTime { get; set; }
            public TimeSpan Duration { get; set; }
            public string Date => StartTime.ToString("yyyy-MM-dd");
        }

        private System.Timers.Timer trackingTimer;
        private List<ActivityRecord> activityHistory = new List<ActivityRecord>();
        private ActivityRecord currentActivity;
        private HashSet<string> allowedProcesses = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private HashSet<string> allowedDomains = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private Action<string, string, string> onDisallowedActivityActivated;
        private string logFilePath;

        public List<ActivityRecord> ActivityHistory => activityHistory;
        public HashSet<string> AllowedProcesses => allowedProcesses;
        public HashSet<string> AllowedDomains => allowedDomains;

        public ActivityTracker(Action<string, string, string> disallowedActivityCallback)
        {
            onDisallowedActivityActivated = disallowedActivityCallback;
            trackingTimer = new System.Timers.Timer(1000); // 每秒检查一次
            trackingTimer.Elapsed += TrackingTimer_Elapsed;
            
            // 设置日志文件路径
            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WawaPomodoro");
                
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }
            
            logFilePath = Path.Combine(appDataPath, $"activity_{DateTime.Now:yyyy-MM-dd}.jsonl");
            
            // 加载配置
            LoadSettings();
            
            // 加载当天的活动记录
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
                // 获取当前活动窗口
                IntPtr hWnd = GetForegroundWindow();
                if (hWnd == IntPtr.Zero) return;

                // 获取进程ID和窗口标题
                uint processId;
                GetWindowThreadProcessId(hWnd, out processId);

                Process process = Process.GetProcessById((int)processId);
                string processName = process.ProcessName;

                StringBuilder titleBuilder = new StringBuilder(256);
                GetWindowText(hWnd, titleBuilder, titleBuilder.Capacity);
                string windowTitle = titleBuilder.ToString();
                
                // 提取域名（如果是浏览器）
                string domain = ExtractDomainFromTitle(processName, windowTitle);

                // 检查是否是新的活动
                if (currentActivity == null || 
                    currentActivity.ProcessName != processName || 
                    currentActivity.WindowTitle != windowTitle)
                {
                    // 保存之前的活动记录
                    if (currentActivity != null)
                    {
                        currentActivity.Duration = DateTime.Now - currentActivity.StartTime;
                        activityHistory.Add(currentActivity);
                        SaveActivity(currentActivity);
                    }

                    // 创建新的活动记录
                    currentActivity = new ActivityRecord
                    {
                        ProcessName = processName,
                        WindowTitle = windowTitle,
                        Domain = domain,
                        StartTime = DateTime.Now,
                        Duration = TimeSpan.Zero
                    };

                    // 检查是否在白名单中
                    bool isAllowed = allowedProcesses.Contains(processName);
                    
                    // 如果是浏览器且有域名，检查域名是否在白名单中
                    if (!isAllowed && !string.IsNullOrEmpty(domain))
                    {
                        isAllowed = allowedDomains.Contains(domain);
                    }
                    
                    if (!isAllowed && onDisallowedActivityActivated != null)
                    {
                        onDisallowedActivityActivated(processName, windowTitle, domain);
                    }
                }
                else
                {
                    // 更新当前活动的持续时间
                    currentActivity.Duration = DateTime.Now - currentActivity.StartTime;
                }
            }
            catch (Exception)
            {
                // 忽略可能的异常，如进程已结束等
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        private string ExtractDomainFromTitle(string processName, string windowTitle)
        {
            // 检查是否是常见浏览器
            if (IsBrowser(processName))
            {
                // 尝试从浏览器地址栏获取URL
                string url = GetBrowserUrl(processName);
                if (!string.IsNullOrEmpty(url))
                {
                    try
                    {
                        // 从URL中提取域名
                        Uri uri = new Uri(url);
                        return uri.Host.ToLower();
                    }
                    catch
                    {
                        // 如果URL解析失败，尝试正则表达式提取
                        string pattern = @"(?:https?:\/\/)?(?:www\.)?([a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)+)";
                        Match match = Regex.Match(url, pattern);
                        if (match.Success)
                        {
                            return match.Groups[1].Value.ToLower();
                        }
                    }
                }

                // 如果无法从地址栏获取，回退到从标题提取
                string titlePattern = @"(?:https?:\/\/)?(?:www\.)?([a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)+)";
                Match titleMatch = Regex.Match(windowTitle, titlePattern);
                if (titleMatch.Success)
                {
                    return titleMatch.Groups[1].Value.ToLower();
                }
                
                // 尝试其他常见格式
                titlePattern = @"(?:.+)(?:\s[-–—]\s)(?:.*?)([a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+)";
                titleMatch = Regex.Match(windowTitle, titlePattern);
                if (titleMatch.Success)
                {
                    return titleMatch.Groups[1].Value.ToLower();
                }
            }
            return string.Empty;
        }

        private string GetBrowserUrl(string browserName)
        {
            try
            {
                // 获取所有该浏览器的进程
                Process[] processes = Process.GetProcessesByName(browserName);
                
                foreach (Process process in processes)
                {
                    if (process.MainWindowHandle != IntPtr.Zero)
                    {
                        // 尝试查找地址栏
                        string url = string.Empty;
                        
                        // 不同浏览器有不同的地址栏查找方法
                        switch (browserName.ToLower())
                        {
                            case "chrome":
                            case "msedge":
                                url = GetChromeUrl(process.MainWindowHandle);
                                break;
                            case "firefox":
                                url = GetFirefoxUrl(process.MainWindowHandle);
                                break;
                        }
                        
                        if (!string.IsNullOrEmpty(url))
                        {
                            return url;
                        }
                    }
                }
            }
            catch
            {
                // 忽略可能的异常
            }
            
            return string.Empty;
        }

        private string GetChromeUrl(IntPtr mainWindowHandle)
        {
            // Chrome和Edge的地址栏通常是一个Edit控件
            StringBuilder className = new StringBuilder(100);
            IntPtr editHandle = IntPtr.Zero;
            
            // 查找所有子窗口，寻找地址栏
            EnumChildWindows(mainWindowHandle, (hwnd, lParam) =>
            {
                GetClassName(hwnd, className, className.Capacity);
                if (className.ToString() == "Chrome_OmniboxView")
                {
                    editHandle = hwnd;
                    return false; // 停止枚举
                }
                return true; // 继续枚举
            }, IntPtr.Zero);
            
            if (editHandle != IntPtr.Zero)
            {
                // 获取地址栏文本
                StringBuilder urlText = new StringBuilder(2048);
                GetWindowText(editHandle, urlText, urlText.Capacity);
                return urlText.ToString();
            }
            
            return string.Empty;
        }

        private string GetFirefoxUrl(IntPtr mainWindowHandle)
        {
            // Firefox的地址栏查找方法
            StringBuilder className = new StringBuilder(100);
            IntPtr editHandle = IntPtr.Zero;
            
            // 查找所有子窗口，寻找地址栏
            EnumChildWindows(mainWindowHandle, (hwnd, lParam) =>
            {
                GetClassName(hwnd, className, className.Capacity);
                if (className.ToString() == "MozillaWindowClass")
                {
                    // 在Firefox中继续查找地址栏
                    IntPtr urlBarHandle = FindWindowEx(hwnd, IntPtr.Zero, "MozillaWindowClass", null);
                    if (urlBarHandle != IntPtr.Zero)
                    {
                        editHandle = urlBarHandle;
                        return false; // 停止枚举
                    }
                }
                return true; // 继续枚举
            }, IntPtr.Zero);
            
            if (editHandle != IntPtr.Zero)
            {
                // 获取地址栏文本
                StringBuilder urlText = new StringBuilder(2048);
                GetWindowText(editHandle, urlText, urlText.Capacity);
                return urlText.ToString();
            }
            
            return string.Empty;
        }

        private bool IsBrowser(string processName)
        {
            string[] browsers = { "chrome", "msedge", "firefox", "opera", "iexplore", "safari", "brave" };
            return browsers.Contains(processName.ToLower());
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
        
        public void AddAllowedDomain(string domain)
        {
            if (!string.IsNullOrWhiteSpace(domain))
            {
                allowedDomains.Add(domain.ToLower());
                SaveSettings();
            }
        }

        public void RemoveAllowedDomain(string domain)
        {
            if (allowedDomains.Remove(domain.ToLower()))
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
                // 忽略保存失败的错误
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
                                // 忽略单行解析错误
                            }
                        }
                    }
                }
            }
            catch
            {
                // 忽略加载失败的错误
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
                                // 忽略单行解析错误
                            }
                        }
                    }
                }
            }
            catch
            {
                // 忽略加载失败的错误
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
                    AllowedDomains = allowedDomains.ToArray()
                };
                
                string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(settingsPath, json);
            }
            catch
            {
                // 忽略保存失败的错误
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
                        
                        if (settings.AllowedDomains != null)
                        {
                            foreach (var domain in settings.AllowedDomains)
                            {
                                allowedDomains.Add(domain.ToLower());
                            }
                        }
                    }
                }
                else
                {
                    // 添加默认白名单
                    allowedProcesses.Add("devenv");     // Visual Studio
                    allowedProcesses.Add("Code");       // VS Code
                    allowedProcesses.Add("notepad");    // 记事本
                    allowedProcesses.Add("chrome");     // Chrome浏览器
                    allowedProcesses.Add("msedge");     // Edge浏览器
                    allowedProcesses.Add("firefox");    // Firefox浏览器
                    allowedProcesses.Add("WawaPomodoro"); // 自身程序
                    
                    // 添加默认域名白名单
                    allowedDomains.Add("github.com");
                    allowedDomains.Add("stackoverflow.com");
                    allowedDomains.Add("docs.microsoft.com");
                    
                    // 保存默认设置
                    SaveSettings();
                }
            }
            catch
            {
                // 忽略加载失败的错误，使用空白名单
            }
        }
        
        private class SettingsData
        {
            public string[] AllowedProcesses { get; set; }
            public string[] AllowedDomains { get; set; }
        }
    }
}