using System;
using System.Drawing;
using System.Windows.Forms;

namespace WawaPomodoro;

public partial class PomodoroForm : Form
{
    // 番茄时钟状态
    private enum TimerState { Work, ShortBreak, LongBreak, Stopped }
    
    // 计时器相关变量
    private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
    private TimerState currentState = TimerState.Stopped;
    private int remainingSeconds;
    private int completedPomodoros = 0;
    
    // 默认时间设置（以秒为单位）
    private readonly int workTime = 25 * 60;      // 25分钟
    private readonly int shortBreakTime = 5 * 60; // 5分钟
    private readonly int longBreakTime = 15 * 60; // 15分钟
    
    // 托盘图标相关
    private NotifyIcon trayIcon;
    private ContextMenuStrip trayMenu;
    private Icon tomatoIcon;
    
    public PomodoroForm()
    {
        InitializeComponent();
        
        // 创建番茄图标
        tomatoIcon = TomatoIcon.CreateTomatoIcon();
        
        // 设置窗体图标和标题
        this.Icon = tomatoIcon;
        this.Text = "番茄时钟";
        this.StartPosition = FormStartPosition.CenterScreen;
        
        // 初始化计时器
        timer.Interval = 1000; // 1秒
        timer.Tick += Timer_Tick;
        
        // 初始化托盘图标
        InitializeTrayIcon();
        
        // 初始化显示
        UpdateDisplay();
    }
    
    private void InitializeTrayIcon()
    {
        // 创建托盘菜单
        trayMenu = new ContextMenuStrip();
        trayMenu.Items.Add("打开主窗口", null, OnTrayOpenClick);
        trayMenu.Items.Add("-"); // 分隔线
        trayMenu.Items.Add("退出", null, OnTrayExitClick);
        
        // 创建托盘图标
        trayIcon = new NotifyIcon
        {
            Icon = tomatoIcon, // 使用番茄图标
            Text = "番茄时钟",
            ContextMenuStrip = trayMenu,
            Visible = true
        };
        
        // 双击托盘图标显示主窗口
        trayIcon.DoubleClick += OnTrayOpenClick;
    }
    
    private void OnTrayOpenClick(object sender, EventArgs e)
    {
        this.Show();
        this.WindowState = FormWindowState.Normal;
        this.Activate();
    }
    
    private void OnTrayExitClick(object sender, EventArgs e)
    {
        // 确保在退出前清理托盘图标
        trayIcon.Visible = false;
        Application.Exit();
    }
    
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        // 如果是用户关闭窗口，则最小化到托盘而不是真正关闭
        if (e.CloseReason == CloseReason.UserClosing)
        {
            e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
            
            // 显示提示信息
            trayIcon.ShowBalloonTip(3000, "番茄时钟", "应用程序已最小化到托盘", ToolTipIcon.Info);
        }
        else
        {
            // 其他关闭原因（如系统关闭）则正常关闭
            base.OnFormClosing(e);
        }
    }
    
    private void Timer_Tick(object sender, EventArgs e)
    {
        if (remainingSeconds > 0)
        {
            remainingSeconds--;
            UpdateDisplay();
        }
        else
        {
            // 时间到
            timer.Stop();
            PlayAlarm();
            
            // 根据当前状态决定下一步
            switch (currentState)
            {
                case TimerState.Work:
                    completedPomodoros++;
                    if (completedPomodoros % 4 == 0)
                    {
                        // 每完成4个番茄钟，进入长休息
                        MessageBox.Show("工作时间结束！开始长休息。", "番茄时钟");
                        StartTimer(TimerState.LongBreak);
                    }
                    else
                    {
                        // 否则进入短休息
                        MessageBox.Show("工作时间结束！开始短休息。", "番茄时钟");
                        StartTimer(TimerState.ShortBreak);
                    }
                    break;
                    
                case TimerState.ShortBreak:
                case TimerState.LongBreak:
                    MessageBox.Show("休息时间结束！开始新的工作时段。", "番茄时钟");
                    StartTimer(TimerState.Work);
                    break;
            }
            
            // 时间到时，如果窗口最小化，显示托盘通知
            if (this.WindowState == FormWindowState.Minimized || !this.Visible)
            {
                string message = currentState == TimerState.Work ? 
                    "工作时间结束！" : "休息时间结束！";
                trayIcon.ShowBalloonTip(3000, "番茄时钟", message, ToolTipIcon.Info);
            }
        }
    }
    
    private void StartTimer(TimerState state)
    {
        currentState = state;
        
        // 根据状态设置时间
        switch (state)
        {
            case TimerState.Work:
                remainingSeconds = workTime;
                break;
            case TimerState.ShortBreak:
                remainingSeconds = shortBreakTime;
                break;
            case TimerState.LongBreak:
                remainingSeconds = longBreakTime;
                break;
        }
        
        UpdateDisplay();
        timer.Start();
        
        // 更新托盘图标提示文本
        string stateText = currentState switch
        {
            TimerState.Work => "工作中",
            TimerState.ShortBreak => "短休息",
            TimerState.LongBreak => "长休息",
            _ => "就绪"
        };
        trayIcon.Text = $"番茄时钟 - {stateText}";
    }
    
    private void StopTimer()
    {
        timer.Stop();
        currentState = TimerState.Stopped;
        UpdateDisplay();
        trayIcon.Text = "番茄时钟 - 已暂停";
    }
    
    private void UpdateDisplay()
    {
        // 更新时间显示
        int minutes = remainingSeconds / 60;
        int seconds = remainingSeconds % 60;
        lblTimeDisplay.Text = $"{minutes:D2}:{seconds:D2}";
        
        // 更新状态显示
        string stateText = currentState switch
        {
            TimerState.Work => "工作中",
            TimerState.ShortBreak => "短休息",
            TimerState.LongBreak => "长休息",
            _ => "就绪"
        };
        
        lblStatus.Text = stateText;
        lblPomodoros.Text = $"已完成: {completedPomodoros} 个番茄";
        
        // 更新按钮状态
        btnStart.Enabled = currentState == TimerState.Stopped;
        btnStop.Enabled = currentState != TimerState.Stopped;
    }
    
    private void PlayAlarm()
    {
        try
        {
            System.Media.SystemSounds.Exclamation.Play();
        }
        catch
        {
            // 忽略播放声音失败的错误
        }
    }
    
    private void btnStart_Click(object sender, EventArgs e)
    {
        StartTimer(TimerState.Work);
    }
    
    private void btnStop_Click(object sender, EventArgs e)
    {
        StopTimer();
    }
    
    private void btnReset_Click(object sender, EventArgs e)
    {
        StopTimer();
        completedPomodoros = 0;
        remainingSeconds = workTime;
        UpdateDisplay();
    }
    
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            // 确保在窗体销毁时清理托盘图标和图标资源
            if (trayIcon != null)
            {
                trayIcon.Dispose();
            }
            
            if (tomatoIcon != null)
            {
                tomatoIcon.Dispose();
            }
            
            if (components != null)
            {
                components.Dispose();
            }
        }
        base.Dispose(disposing);
    }
}