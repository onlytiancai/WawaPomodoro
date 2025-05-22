using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace WawaPomodoro
{
    public partial class ActivityForm : Form
    {
        private ActivityTracker activityTracker;
        private DateTime currentViewDate = DateTime.Today;
        private string currentViewMode = "日"; // "日", "周", "月"

        public ActivityForm(ActivityTracker tracker)
        {
            InitializeComponent();
            activityTracker = tracker;
            
            // 初始化日期选择器
            dateTimePicker.Value = DateTime.Today;
            
            // 加载活动记录
            LoadActivityData();
            
            // 加载白名单
            LoadWhitelistData();
        }

        private void LoadActivityData()
        {
            List<ActivityTracker.ActivityRecord> records;
            
            // 根据当前视图模式加载数据
            switch (currentViewMode)
            {
                case "周":
                    // 获取所选日期所在周的起始日期和结束日期
                    DateTime weekStart = GetWeekStartDate(currentViewDate);
                    DateTime weekEnd = weekStart.AddDays(6);
                    records = activityTracker.LoadActivitiesForDateRange(weekStart, weekEnd);
                    lblDateRange.Text = $"{weekStart:yyyy-MM-dd} 至 {weekEnd:yyyy-MM-dd}";
                    break;
                    
                case "月":
                    // 获取所选日期所在月的起始日期和结束日期
                    DateTime monthStart = new DateTime(currentViewDate.Year, currentViewDate.Month, 1);
                    DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);
                    records = activityTracker.LoadActivitiesForDateRange(monthStart, monthEnd);
                    lblDateRange.Text = $"{monthStart:yyyy-MM-dd} 至 {monthEnd:yyyy-MM-dd}";
                    break;
                    
                case "日":
                default:
                    // 获取所选日期的数据
                    records = activityTracker.LoadActivitiesForDate(currentViewDate);
                    lblDateRange.Text = currentViewDate.ToString("yyyy-MM-dd");
                    break;
            }

            // 创建数据表
            DataTable dt = new DataTable();
            dt.Columns.Add("进程名", typeof(string));
            dt.Columns.Add("窗口标题", typeof(string));
            dt.Columns.Add("域名", typeof(string));
            dt.Columns.Add("开始时间", typeof(DateTime));
            dt.Columns.Add("持续时间(秒)", typeof(int));
            dt.Columns.Add("日期", typeof(string));

            // 添加活动记录
            foreach (var activity in records)
            {
                dt.Rows.Add(
                    activity.ProcessName,
                    activity.WindowTitle,
                    activity.Domain,
                    activity.StartTime,
                    (int)activity.Duration.TotalSeconds,
                    activity.Date
                );
            }

            // 绑定到数据网格
            dataGridActivities.DataSource = dt;
            
            // 计算每个进程的总时间
            var processSummary = records
                .GroupBy(a => a.ProcessName)
                .Select(g => new { 
                    ProcessName = g.Key, 
                    TotalSeconds = g.Sum(a => a.Duration.TotalSeconds) 
                })
                .OrderByDescending(x => x.TotalSeconds)
                .ToList();

            // 创建进程汇总表
            DataTable summaryDt = new DataTable();
            summaryDt.Columns.Add("进程名", typeof(string));
            summaryDt.Columns.Add("总时间(秒)", typeof(int));
            summaryDt.Columns.Add("百分比", typeof(string));

            double totalTime = processSummary.Sum(p => p.TotalSeconds);
            foreach (var process in processSummary)
            {
                string percentage = totalTime > 0 
                    ? $"{(process.TotalSeconds / totalTime * 100):F1}%" 
                    : "0%";
                    
                summaryDt.Rows.Add(
                    process.ProcessName,
                    (int)process.TotalSeconds,
                    percentage
                );
            }

            // 绑定到汇总网格
            dataGridSummary.DataSource = summaryDt;
            
            // 计算每个域名的总时间
            var domainSummary = records
                .Where(a => !string.IsNullOrEmpty(a.Domain))
                .GroupBy(a => a.Domain)
                .Select(g => new { 
                    Domain = g.Key, 
                    TotalSeconds = g.Sum(a => a.Duration.TotalSeconds) 
                })
                .OrderByDescending(x => x.TotalSeconds)
                .ToList();

            // 创建域名汇总表
            DataTable domainDt = new DataTable();
            domainDt.Columns.Add("域名", typeof(string));
            domainDt.Columns.Add("总时间(秒)", typeof(int));
            domainDt.Columns.Add("百分比", typeof(string));

            double totalDomainTime = domainSummary.Sum(p => p.TotalSeconds);
            foreach (var domain in domainSummary)
            {
                string percentage = totalDomainTime > 0 
                    ? $"{(domain.TotalSeconds / totalDomainTime * 100):F1}%" 
                    : "0%";
                    
                domainDt.Rows.Add(
                    domain.Domain,
                    (int)domain.TotalSeconds,
                    percentage
                );
            }

            // 绑定到域名汇总网格
            dataGridDomains.DataSource = domainDt;
        }

        private DateTime GetWeekStartDate(DateTime date)
        {
            // 获取一周的第一天（星期一）
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        private void LoadWhitelistData()
        {
            // 加载进程白名单
            listBoxProcessWhitelist.Items.Clear();
            foreach (var process in activityTracker.AllowedProcesses)
            {
                listBoxProcessWhitelist.Items.Add(process);
            }
            
            // 加载域名白名单
            listBoxDomainWhitelist.Items.Clear();
            foreach (var domain in activityTracker.AllowedDomains)
            {
                listBoxDomainWhitelist.Items.Add(domain);
            }
        }

        private void btnAddToProcessWhitelist_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProcessName.Text))
                return;

            activityTracker.AddAllowedProcess(txtProcessName.Text);
            LoadWhitelistData();
            txtProcessName.Clear();
        }

        private void btnRemoveFromProcessWhitelist_Click(object sender, EventArgs e)
        {
            if (listBoxProcessWhitelist.SelectedItem == null)
                return;

            activityTracker.RemoveAllowedProcess(listBoxProcessWhitelist.SelectedItem.ToString());
            LoadWhitelistData();
        }
        
        private void btnAddToDomainWhitelist_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDomainName.Text))
                return;

            activityTracker.AddAllowedDomain(txtDomainName.Text);
            LoadWhitelistData();
            txtDomainName.Clear();
        }

        private void btnRemoveFromDomainWhitelist_Click(object sender, EventArgs e)
        {
            if (listBoxDomainWhitelist.SelectedItem == null)
                return;

            activityTracker.RemoveAllowedDomain(listBoxDomainWhitelist.SelectedItem.ToString());
            LoadWhitelistData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadActivityData();
        }

        private void btnClearHistory_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要清除所有活动记录吗？", "确认", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                activityTracker.ClearActivityHistory();
                LoadActivityData();
            }
        }
        
        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            currentViewDate = dateTimePicker.Value.Date;
            LoadActivityData();
        }
        
        private void rbDay_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDay.Checked)
            {
                currentViewMode = "日";
                LoadActivityData();
            }
        }
        
        private void rbWeek_CheckedChanged(object sender, EventArgs e)
        {
            if (rbWeek.Checked)
            {
                currentViewMode = "周";
                LoadActivityData();
            }
        }
        
        private void rbMonth_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMonth.Checked)
            {
                currentViewMode = "月";
                LoadActivityData();
            }
        }
    }
}