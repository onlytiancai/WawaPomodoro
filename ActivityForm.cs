using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
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
            
            // 从嵌入资源加载图标
            using (var stream = GetType().Assembly.GetManifestResourceStream("WawaPomodoro.app.ico"))
            {
                this.Icon = new Icon(stream);
            }
            
            // Initialize date picker
            dateTimePicker.Value = DateTime.Today;
            
            // Load activity data
            LoadActivityData();
            
            // Load whitelist data
            LoadWhitelistData();
        }

        private void LoadActivityData()
        {
            List<ActivityTracker.ActivityRecord> records;
            
            // Get records based on view mode
            switch (currentViewMode)
            {
                case "周":
                    // Get all activities for the current week
                    DateTime weekStart = GetWeekStartDate(currentViewDate);
                    DateTime weekEnd = weekStart.AddDays(6);
                    records = activityTracker.LoadActivitiesForDateRange(weekStart, weekEnd);
                    lblDateRange.Text = $"{weekStart:yyyy-MM-dd} 至 {weekEnd:yyyy-MM-dd}";
                    break;
                    
                case "月":
                    // Get all activities for the current month
                    DateTime monthStart = new DateTime(currentViewDate.Year, currentViewDate.Month, 1);
                    DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);
                    records = activityTracker.LoadActivitiesForDateRange(monthStart, monthEnd);
                    lblDateRange.Text = $"{monthStart:yyyy-MM-dd} 至 {monthEnd:yyyy-MM-dd}";
                    break;
                    
                case "日":
                default:
                    // Get activities for the selected day
                    records = activityTracker.LoadActivitiesForDate(currentViewDate);
                    lblDateRange.Text = currentViewDate.ToString("yyyy-MM-dd");
                    break;
            }

            // Create data table
            DataTable dt = new DataTable();
            dt.Columns.Add("进程", typeof(string));
            dt.Columns.Add("窗口标题", typeof(string));
            dt.Columns.Add("开始时间", typeof(DateTime));
            dt.Columns.Add("持续时间(秒)", typeof(int));
            dt.Columns.Add("日期", typeof(string));

            // Add activity records
            foreach (var activity in records)
            {
                dt.Rows.Add(
                    activity.ProcessName,
                    activity.WindowTitle,
                    activity.StartTime,
                    (int)activity.Duration.TotalSeconds,
                    activity.Date
                );
            }

            // Display activity data
            dataGridActivities.DataSource = dt;
            
            // Create process summary
            var processSummary = records
                .GroupBy(a => a.ProcessName)
                .Select(g => new { 
                    ProcessName = g.Key, 
                    TotalSeconds = g.Sum(a => a.Duration.TotalSeconds) 
                })
                .OrderByDescending(x => x.TotalSeconds)
                .ToList();

            // Create summary table
            DataTable summaryDt = new DataTable();
            summaryDt.Columns.Add("进程", typeof(string));
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

            // Display summary data
            dataGridSummary.DataSource = summaryDt;
            
            // 添加右键菜单到进程统计表格
            if (dataGridSummary.ContextMenuStrip == null)
            {
                ContextMenuStrip summaryContextMenu = new ContextMenuStrip();
                summaryContextMenu.Items.Add("添加到白名单", null, (s, e) => {
                    if (dataGridSummary.CurrentRow != null)
                    {
                        string processName = dataGridSummary.CurrentRow.Cells["进程"].Value.ToString();
                        if (!string.IsNullOrEmpty(processName))
                        {
                            activityTracker.AddAllowedProcess(processName);
                            LoadWhitelistData();
                            MessageBox.Show($"已将 {processName} 添加到白名单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                });
                dataGridSummary.ContextMenuStrip = summaryContextMenu;
            }
        }

        private DateTime GetWeekStartDate(DateTime date)
        {
            // Get Monday of the current week
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        private void LoadWhitelistData()
        {
            // Load process whitelist
            listBoxProcessWhitelist.Items.Clear();
            foreach (var process in activityTracker.AllowedProcesses)
            {
                listBoxProcessWhitelist.Items.Add(process);
            }
            
            // Load title keyword whitelist
            listBoxTitleWhitelist.Items.Clear();
            foreach (var keyword in activityTracker.AllowedTitleKeywords)
            {
                listBoxTitleWhitelist.Items.Add(keyword);
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
        
        private void btnAddToTitleWhitelist_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitleKeyword.Text))
                return;

            activityTracker.AddAllowedTitleKeyword(txtTitleKeyword.Text);
            LoadWhitelistData();
            txtTitleKeyword.Clear();
        }

        private void btnRemoveFromTitleWhitelist_Click(object sender, EventArgs e)
        {
            if (listBoxTitleWhitelist.SelectedItem == null)
                return;

            activityTracker.RemoveAllowedTitleKeyword(listBoxTitleWhitelist.SelectedItem.ToString());
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