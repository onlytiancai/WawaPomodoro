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
            btnClearHistory = new Button();
            btnRefresh = new Button();
            dataGridActivities = new DataGridView();
            tabPageSummary = new TabPage();
            dataGridSummary = new DataGridView();
            tabPageWhitelist = new TabPage();
            btnRemoveFromWhitelist = new Button();
            btnAddToWhitelist = new Button();
            txtProcessName = new TextBox();
            label1 = new Label();
            listBoxWhitelist = new ListBox();
            
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
            tabControl1.Size = new Size(660, 437);
            tabControl1.TabIndex = 0;
            
            // 
            // tabPageActivities
            // 
            tabPageActivities.Controls.Add(btnClearHistory);
            tabPageActivities.Controls.Add(btnRefresh);
            tabPageActivities.Controls.Add(dataGridActivities);
            tabPageActivities.Location = new Point(4, 24);
            tabPageActivities.Name = "tabPageActivities";
            tabPageActivities.Padding = new Padding(3);
            tabPageActivities.Size = new Size(652, 409);
            tabPageActivities.TabIndex = 0;
            tabPageActivities.Text = "活动记录";
            tabPageActivities.UseVisualStyleBackColor = true;
            
            // 
            // btnClearHistory
            // 
            btnClearHistory.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClearHistory.Location = new Point(571, 380);
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
            btnRefresh.Location = new Point(490, 380);
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
            dataGridActivities.Location = new Point(6, 6);
            dataGridActivities.Name = "dataGridActivities";
            dataGridActivities.ReadOnly = true;
            dataGridActivities.RowTemplate.Height = 25;
            dataGridActivities.Size = new Size(640, 368);
            dataGridActivities.TabIndex = 0;
            
            // 
            // tabPageSummary
            // 
            tabPageSummary.Controls.Add(dataGridSummary);
            tabPageSummary.Location = new Point(4, 24);
            tabPageSummary.Name = "tabPageSummary";
            tabPageSummary.Padding = new Padding(3);
            tabPageSummary.Size = new Size(652, 409);
            tabPageSummary.TabIndex = 1;
            tabPageSummary.Text = "时间汇总";
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
            dataGridSummary.Size = new Size(640, 397);
            dataGridSummary.TabIndex = 0;
            
            // 
            // tabPageWhitelist
            // 
            tabPageWhitelist.Controls.Add(btnRemoveFromWhitelist);
            tabPageWhitelist.Controls.Add(btnAddToWhitelist);
            tabPageWhitelist.Controls.Add(txtProcessName);
            tabPageWhitelist.Controls.Add(label1);
            tabPageWhitelist.Controls.Add(listBoxWhitelist);
            tabPageWhitelist.Location = new Point(4, 24);
            tabPageWhitelist.Name = "tabPageWhitelist";
            tabPageWhitelist.Size = new Size(652, 409);
            tabPageWhitelist.TabIndex = 2;
            tabPageWhitelist.Text = "白名单设置";
            tabPageWhitelist.UseVisualStyleBackColor = true;
            
            // 
            // btnRemoveFromWhitelist
            // 
            btnRemoveFromWhitelist.Location = new Point(336, 93);
            btnRemoveFromWhitelist.Name = "btnRemoveFromWhitelist";
            btnRemoveFromWhitelist.Size = new Size(75, 23);
            btnRemoveFromWhitelist.TabIndex = 4;
            btnRemoveFromWhitelist.Text = "移除";
            btnRemoveFromWhitelist.UseVisualStyleBackColor = true;
            btnRemoveFromWhitelist.Click += btnRemoveFromWhitelist_Click;
            
            // 
            // btnAddToWhitelist
            // 
            btnAddToWhitelist.Location = new Point(336, 64);
            btnAddToWhitelist.Name = "btnAddToWhitelist";
            btnAddToWhitelist.Size = new Size(75, 23);
            btnAddToWhitelist.TabIndex = 3;
            btnAddToWhitelist.Text = "添加";
            btnAddToWhitelist.UseVisualStyleBackColor = true;
            btnAddToWhitelist.Click += btnAddToWhitelist_Click;
            
            // 
            // txtProcessName
            // 
            txtProcessName.Location = new Point(336, 35);
            txtProcessName.Name = "txtProcessName";
            txtProcessName.Size = new Size(200, 23);
            txtProcessName.TabIndex = 2;
            
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(336, 17);
            label1.Name = "label1";
            label1.Size = new Size(56, 15);
            label1.TabIndex = 1;
            label1.Text = "进程名：";
            
            // 
            // listBoxWhitelist
            // 
            listBoxWhitelist.FormattingEnabled = true;
            listBoxWhitelist.ItemHeight = 15;
            listBoxWhitelist.Location = new Point(6, 6);
            listBoxWhitelist.Name = "listBoxWhitelist";
            listBoxWhitelist.Size = new Size(324, 394);
            listBoxWhitelist.TabIndex = 0;
            
            // 
            // ActivityForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(684, 461);
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
        private ListBox listBoxWhitelist;
        private Label label1;
        private TextBox txtProcessName;
        private Button btnAddToWhitelist;
        private Button btnRemoveFromWhitelist;
    }
}