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

        public ActivityForm(ActivityTracker tracker)
        {
            InitializeComponent();
            activityTracker = tracker;
            
            // 加载活动记录
            LoadActivityData();
            
            // 加载白名单
            LoadWhitelistData();
        }

        private void LoadActivityData()
        {
            // 创建数据表
            DataTable dt = new DataTable();
            dt.Columns.Add("进程名", typeof(string));
            dt.Columns.Add("窗口标题", typeof(string));
            dt.Columns.Add("开始时间", typeof(DateTime));
            dt.Columns.Add("持续时间(秒)", typeof(int));

            // 添加活动记录
            foreach (var activity in activityTracker.ActivityHistory)
            {
                dt.Rows.Add(
                    activity.ProcessName,
                    activity.WindowTitle,
                    activity.StartTime,
                    (int)activity.Duration.TotalSeconds
                );
            }

            // 绑定到数据网格
            dataGridActivities.DataSource = dt;
            
            // 计算每个进程的总时间
            var processSummary = activityTracker.ActivityHistory
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
        }

        private void LoadWhitelistData()
        {
            listBoxWhitelist.Items.Clear();
            foreach (var process in activityTracker.AllowedProcesses)
            {
                listBoxWhitelist.Items.Add(process);
            }
        }

        private void btnAddToWhitelist_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProcessName.Text))
                return;

            activityTracker.AddAllowedProcess(txtProcessName.Text);
            LoadWhitelistData();
            txtProcessName.Clear();
        }

        private void btnRemoveFromWhitelist_Click(object sender, EventArgs e)
        {
            if (listBoxWhitelist.SelectedItem == null)
                return;

            activityTracker.RemoveAllowedProcess(listBoxWhitelist.SelectedItem.ToString());
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
    }
}