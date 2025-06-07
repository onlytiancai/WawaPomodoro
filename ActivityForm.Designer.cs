namespace WawaPomodoro
{
    partial class ActivityForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            tabPageActivities = new TabPage();
            panel1 = new Panel();
            lblDateRange = new Label();
            rbMonth = new RadioButton();
            rbWeek = new RadioButton();
            rbDay = new RadioButton();
            dateTimePicker = new DateTimePicker();
            btnClearHistory = new Button();
            btnRefresh = new Button();
            dataGridActivities = new DataGridView();
            tabPageSummary = new TabPage();
            dataGridSummary = new DataGridView();
            tabPageWhitelist = new TabPage();
            groupBox2 = new GroupBox();
            btnRemoveFromTitleWhitelist = new Button();
            btnAddToTitleWhitelist = new Button();
            txtTitleKeyword = new TextBox();
            label2 = new Label();
            listBoxTitleWhitelist = new ListBox();
            groupBox1 = new GroupBox();
            btnRemoveFromProcessWhitelist = new Button();
            btnAddToProcessWhitelist = new Button();
            txtProcessName = new TextBox();
            label1 = new Label();
            listBoxProcessWhitelist = new ListBox();

            
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPageActivities);
            tabControl1.Controls.Add(tabPageSummary);
            tabControl1.Controls.Add(tabPageWhitelist);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(760, 537);
            tabControl1.TabIndex = 0;
            
            // 
            // tabPageActivities
            // 
            tabPageActivities.Controls.Add(panel1);
            tabPageActivities.Controls.Add(lblDateRange);
            tabPageActivities.Controls.Add(rbMonth);
            tabPageActivities.Controls.Add(rbWeek);
            tabPageActivities.Controls.Add(rbDay);
            tabPageActivities.Controls.Add(dateTimePicker);
            tabPageActivities.Controls.Add(btnClearHistory);
            tabPageActivities.Controls.Add(btnRefresh);
            tabPageActivities.Controls.Add(dataGridActivities);
            tabPageActivities.Location = new Point(4, 24);
            tabPageActivities.Name = "tabPageActivities";
            tabPageActivities.Padding = new Padding(3);
            tabPageActivities.Size = new Size(752, 509);
            tabPageActivities.TabIndex = 0;
            tabPageActivities.Text = "活动记录";
            tabPageActivities.UseVisualStyleBackColor = true;
            
            // 
            // panel1
            // 
            panel1.BackColor = Color.LightGray;
            panel1.Location = new Point(6, 40);
            panel1.Name = "panel1";
            panel1.Size = new Size(740, 1);
            panel1.TabIndex = 8;
            
            // 
            // lblDateRange
            // 
            lblDateRange.AutoSize = true;
            lblDateRange.Location = new Point(500, 15);
            lblDateRange.Name = "lblDateRange";
            lblDateRange.Size = new Size(79, 15);
            lblDateRange.TabIndex = 7;
            lblDateRange.Text = "2023-01-01";
            
            // 
            // rbMonth
            // 
            rbMonth.AutoSize = true;
            rbMonth.Location = new Point(450, 13);
            rbMonth.Name = "rbMonth";
            rbMonth.Size = new Size(38, 19);
            rbMonth.TabIndex = 6;
            rbMonth.Text = "月";
            rbMonth.UseVisualStyleBackColor = true;
            rbMonth.CheckedChanged += rbMonth_CheckedChanged;
            
            // 
            // rbWeek
            // 
            rbWeek.AutoSize = true;
            rbWeek.Location = new Point(400, 13);
            rbWeek.Name = "rbWeek";
            rbWeek.Size = new Size(38, 19);
            rbWeek.TabIndex = 5;
            rbWeek.Text = "周";
            rbWeek.UseVisualStyleBackColor = true;
            rbWeek.CheckedChanged += rbWeek_CheckedChanged;
            
            // 
            // rbDay
            // 
            rbDay.AutoSize = true;
            rbDay.Checked = true;
            rbDay.Location = new Point(350, 13);
            rbDay.Name = "rbDay";
            rbDay.Size = new Size(38, 19);
            rbDay.TabIndex = 4;
            rbDay.TabStop = true;
            rbDay.Text = "日";
            rbDay.UseVisualStyleBackColor = true;
            rbDay.CheckedChanged += rbDay_CheckedChanged;
            
            // 
            // dateTimePicker
            // 
            dateTimePicker.Location = new Point(6, 11);
            dateTimePicker.Name = "dateTimePicker";
            dateTimePicker.Size = new Size(200, 23);
            dateTimePicker.TabIndex = 3;
            dateTimePicker.ValueChanged += dateTimePicker_ValueChanged;
            
            // 
            // btnClearHistory
            // 
            btnClearHistory.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClearHistory.Location = new Point(671, 480);
            btnClearHistory.Name = "btnClearHistory";
            btnClearHistory.Size = new Size(75, 23);
            btnClearHistory.TabIndex = 2;
            btnClearHistory.Text = "清除记录";
            btnClearHistory.UseVisualStyleBackColor = true;
            btnClearHistory.Click += btnClearHistory_Click;
            
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRefresh.Location = new Point(590, 480);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(75, 23);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "刷新";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            
            // 
            // dataGridActivities
            // 
            dataGridActivities.AllowUserToAddRows = false;
            dataGridActivities.AllowUserToDeleteRows = false;
            dataGridActivities.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridActivities.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridActivities.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridActivities.Location = new Point(6, 47);
            dataGridActivities.Name = "dataGridActivities";
            dataGridActivities.ReadOnly = true;
            dataGridActivities.RowTemplate.Height = 25;
            dataGridActivities.Size = new Size(740, 427);
            dataGridActivities.TabIndex = 0;
            
            // 
            // tabPageSummary
            // 
            tabPageSummary.Controls.Add(dataGridSummary);
            tabPageSummary.Location = new Point(4, 24);
            tabPageSummary.Name = "tabPageSummary";
            tabPageSummary.Padding = new Padding(3);
            tabPageSummary.Size = new Size(752, 509);
            tabPageSummary.TabIndex = 1;
            tabPageSummary.Text = "进程统计";
            tabPageSummary.UseVisualStyleBackColor = true;
            
            // 
            // dataGridSummary
            // 
            dataGridSummary.AllowUserToAddRows = false;
            dataGridSummary.AllowUserToDeleteRows = false;
            dataGridSummary.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridSummary.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridSummary.Location = new Point(6, 6);
            dataGridSummary.Name = "dataGridSummary";
            dataGridSummary.ReadOnly = true;
            dataGridSummary.RowTemplate.Height = 25;
            dataGridSummary.Size = new Size(740, 497);
            dataGridSummary.TabIndex = 0;
            
            // 
            // tabPageWhitelist
            // 
            tabPageWhitelist.Controls.Add(groupBox2);
            tabPageWhitelist.Controls.Add(groupBox1);
            tabPageWhitelist.Location = new Point(4, 24);
            tabPageWhitelist.Name = "tabPageWhitelist";
            tabPageWhitelist.Size = new Size(752, 509);
            tabPageWhitelist.TabIndex = 2;
            tabPageWhitelist.Text = "白名单设置";
            tabPageWhitelist.UseVisualStyleBackColor = true;
            
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnRemoveFromTitleWhitelist);
            groupBox2.Controls.Add(btnAddToTitleWhitelist);
            groupBox2.Controls.Add(txtTitleKeyword);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(listBoxTitleWhitelist);
            groupBox2.Location = new Point(376, 6);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(370, 497);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "标题关键词白名单";
            
            // 
            // btnRemoveFromTitleWhitelist
            // 
            btnRemoveFromTitleWhitelist.Location = new Point(289, 93);
            btnRemoveFromTitleWhitelist.Name = "btnRemoveFromTitleWhitelist";
            btnRemoveFromTitleWhitelist.Size = new Size(75, 23);
            btnRemoveFromTitleWhitelist.TabIndex = 4;
            btnRemoveFromTitleWhitelist.Text = "删除";
            btnRemoveFromTitleWhitelist.UseVisualStyleBackColor = true;
            btnRemoveFromTitleWhitelist.Click += btnRemoveFromTitleWhitelist_Click;
            
            // 
            // btnAddToTitleWhitelist
            // 
            btnAddToTitleWhitelist.Location = new Point(289, 64);
            btnAddToTitleWhitelist.Name = "btnAddToTitleWhitelist";
            btnAddToTitleWhitelist.Size = new Size(75, 23);
            btnAddToTitleWhitelist.TabIndex = 3;
            btnAddToTitleWhitelist.Text = "添加";
            btnAddToTitleWhitelist.UseVisualStyleBackColor = true;
            btnAddToTitleWhitelist.Click += btnAddToTitleWhitelist_Click;
            
            // 
            // txtTitleKeyword
            // 
            txtTitleKeyword.Location = new Point(289, 35);
            txtTitleKeyword.Name = "txtTitleKeyword";
            txtTitleKeyword.Size = new Size(75, 23);
            txtTitleKeyword.TabIndex = 2;
            
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(289, 17);
            label2.Name = "label2";
            label2.Size = new Size(56, 15);
            label2.TabIndex = 1;
            label2.Text = "关键词：";
            
            // 
            // listBoxTitleWhitelist
            // 
            listBoxTitleWhitelist.FormattingEnabled = true;
            listBoxTitleWhitelist.ItemHeight = 15;
            listBoxTitleWhitelist.Location = new Point(6, 22);
            listBoxTitleWhitelist.Name = "listBoxTitleWhitelist";
            listBoxTitleWhitelist.Size = new Size(277, 469);
            listBoxTitleWhitelist.TabIndex = 0;
            
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnRemoveFromProcessWhitelist);
            groupBox1.Controls.Add(btnAddToProcessWhitelist);
            groupBox1.Controls.Add(txtProcessName);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(listBoxProcessWhitelist);
            groupBox1.Location = new Point(6, 6);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(364, 497);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "进程白名单";
            
            // 
            // btnRemoveFromProcessWhitelist
            // 
            btnRemoveFromProcessWhitelist.Location = new Point(283, 93);
            btnRemoveFromProcessWhitelist.Name = "btnRemoveFromProcessWhitelist";
            btnRemoveFromProcessWhitelist.Size = new Size(75, 23);
            btnRemoveFromProcessWhitelist.TabIndex = 4;
            btnRemoveFromProcessWhitelist.Text = "删除";
            btnRemoveFromProcessWhitelist.UseVisualStyleBackColor = true;
            btnRemoveFromProcessWhitelist.Click += btnRemoveFromProcessWhitelist_Click;
            
            // 
            // btnAddToProcessWhitelist
            // 
            btnAddToProcessWhitelist.Location = new Point(283, 64);
            btnAddToProcessWhitelist.Name = "btnAddToProcessWhitelist";
            btnAddToProcessWhitelist.Size = new Size(75, 23);
            btnAddToProcessWhitelist.TabIndex = 3;
            btnAddToProcessWhitelist.Text = "添加";
            btnAddToProcessWhitelist.UseVisualStyleBackColor = true;
            btnAddToProcessWhitelist.Click += btnAddToProcessWhitelist_Click;
            
            // 
            // txtProcessName
            // 
            txtProcessName.Location = new Point(283, 35);
            txtProcessName.Name = "txtProcessName";
            txtProcessName.Size = new Size(75, 23);
            txtProcessName.TabIndex = 2;
            
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(283, 17);
            label1.Name = "label1";
            label1.Size = new Size(56, 15);
            label1.TabIndex = 1;
            label1.Text = "进程名：";
            
            // 
            // listBoxProcessWhitelist
            // 
            listBoxProcessWhitelist.FormattingEnabled = true;
            listBoxProcessWhitelist.ItemHeight = 15;
            listBoxProcessWhitelist.Location = new Point(6, 22);
            listBoxProcessWhitelist.Name = "listBoxProcessWhitelist";
            listBoxProcessWhitelist.Size = new Size(271, 469);
            listBoxProcessWhitelist.TabIndex = 0;
            
            // 
            // ActivityForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 561);
            Controls.Add(tabControl1);
            Name = "ActivityForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "活动记录与白名单设置";
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPageActivities;
        private TabPage tabPageSummary;
        private TabPage tabPageWhitelist;
        private DataGridView dataGridActivities;
        private DataGridView dataGridSummary;
        private Button btnRefresh;
        private Button btnClearHistory;
        private GroupBox groupBox1;
        private Label label1;
        private ListBox listBoxProcessWhitelist;
        private Button btnRemoveFromProcessWhitelist;
        private Button btnAddToProcessWhitelist;
        private TextBox txtProcessName;
        private DateTimePicker dateTimePicker;
        private RadioButton rbMonth;
        private RadioButton rbWeek;
        private RadioButton rbDay;
        private Label lblDateRange;
        private Panel panel1;
        private GroupBox groupBox2;
        private Button btnRemoveFromTitleWhitelist;
        private Button btnAddToTitleWhitelist;
        private TextBox txtTitleKeyword;
        private Label label2;
        private ListBox listBoxTitleWhitelist;
    }
}