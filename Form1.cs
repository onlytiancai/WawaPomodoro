using System;
using System.Drawing;
using System.Windows.Forms;

namespace WawaPomodoro;

public partial class Form1 : Form
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
    
    public Form1()
    {
        InitializeComponent();
        
        // 设置窗体标题和大小
        this.Text = "番茄时钟";
        this.StartPosition = FormStartPosition.CenterScreen;
        
        // 初始化计时器
        timer.Interval = 1000; // 1秒
        timer.Tick += Timer_Tick;
        
        // 初始化显示
        UpdateDisplay();
    }
    
    private void Timer_Tick(object? sender, EventArgs? e)
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
    }
    
    private void StopTimer()
    {
        timer.Stop();
        currentState = TimerState.Stopped;
        UpdateDisplay();
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
}