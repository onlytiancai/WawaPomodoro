using System.Threading;

namespace WawaPomodoro;

static class Program
{
    // 创建互斥体以防止多实例运行
    private static Mutex? _mutex = null;
    
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // 检查是否已有实例在运行
        const string appName = "WawaPomodoroApp";
        bool createdNew;
        _mutex = new Mutex(true, appName, out createdNew);
        
        if (!createdNew)
        {
            MessageBox.Show("番茄时钟已经在运行中！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new PomodoroForm());
    }    
}