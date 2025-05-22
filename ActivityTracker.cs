using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
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
            public string? ProcessName { get; set; }
            public string? WindowTitle { get; set; }
            public DateTime StartTime { get; set; }
            public TimeSpan Duration { get; set; }
        }

        private System.Timers.Timer trackingTimer;
        private List<ActivityRecord> activityHistory = new List<ActivityRecord>();
        private ActivityRecord? currentActivity;
        private HashSet<string> allowedProcesses = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private Action<string, string> onDisallowedProcessActivated;

        public List<ActivityRecord> ActivityHistory => activityHistory;
        public HashSet<string> AllowedProcesses => allowedProcesses;

        public ActivityTracker(Action<string, string> disallowedProcessCallback)
        {
            onDisallowedProcessActivated = disallowedProcessCallback;
            trackingTimer = new System.Timers.Timer(1000); // 每秒检查一次
            trackingTimer.Elapsed += TrackingTimer_Elapsed;
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
                currentActivity = null;
            }
        }

        private void TrackingTimer_Elapsed(object? sender, ElapsedEventArgs e)
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
                    }

                    // 创建新的活动记录
                    currentActivity = new ActivityRecord
                    {
                        ProcessName = processName,
                        WindowTitle = windowTitle,
                        StartTime = DateTime.Now,
                        Duration = TimeSpan.Zero
                    };

                    // 检查是否在白名单中
                    if (!allowedProcesses.Contains(processName) && onDisallowedProcessActivated != null)
                    {
                        onDisallowedProcessActivated(processName, windowTitle);
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

        public void AddAllowedProcess(string processName)
        {
            allowedProcesses.Add(processName);
        }

        public void RemoveAllowedProcess(string processName)
        {
            allowedProcesses.Remove(processName);
        }

        public void ClearActivityHistory()
        {
            activityHistory.Clear();
        }
    }
}