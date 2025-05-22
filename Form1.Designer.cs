namespace WawaPomodoro;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        lblTimeDisplay = new Label();
        lblStatus = new Label();
        lblPomodoros = new Label();
        btnStart = new Button();
        btnStop = new Button();
        btnReset = new Button();
        panel1 = new Panel();
        
        // 
        // lblTimeDisplay
        // 
        lblTimeDisplay.AutoSize = false;
        lblTimeDisplay.Font = new Font("Segoe UI", 48F, FontStyle.Bold, GraphicsUnit.Point);
        lblTimeDisplay.Location = new Point(12, 20);
        lblTimeDisplay.Name = "lblTimeDisplay";
        lblTimeDisplay.Size = new Size(360, 100);
        lblTimeDisplay.TabIndex = 0;
        lblTimeDisplay.Text = "25:00";
        lblTimeDisplay.TextAlign = ContentAlignment.MiddleCenter;
        
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = false;
        lblStatus.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
        lblStatus.Location = new Point(12, 130);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(360, 30);
        lblStatus.TabIndex = 1;
        lblStatus.Text = "就绪";
        lblStatus.TextAlign = ContentAlignment.MiddleCenter;
        
        // 
        // lblPomodoros
        // 
        lblPomodoros.AutoSize = false;
        lblPomodoros.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
        lblPomodoros.Location = new Point(12, 170);
        lblPomodoros.Name = "lblPomodoros";
        lblPomodoros.Size = new Size(360, 30);
        lblPomodoros.TabIndex = 2;
        lblPomodoros.Text = "已完成: 0 个番茄";
        lblPomodoros.TextAlign = ContentAlignment.MiddleCenter;
        
        // 
        // panel1
        // 
        panel1.BackColor = System.Drawing.Color.WhiteSmoke;
        panel1.Location = new Point(12, 210);
        panel1.Name = "panel1";
        panel1.Size = new Size(360, 2);
        panel1.TabIndex = 3;
        
        // 
        // btnStart
        // 
        btnStart.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
        btnStart.Location = new Point(12, 230);
        btnStart.Name = "btnStart";
        btnStart.Size = new Size(110, 40);
        btnStart.TabIndex = 4;
        btnStart.Text = "开始";
        btnStart.UseVisualStyleBackColor = true;
        btnStart.Click += btnStart_Click;
        
        // 
        // btnStop
        // 
        btnStop.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
        btnStop.Location = new Point(137, 230);
        btnStop.Name = "btnStop";
        btnStop.Size = new Size(110, 40);
        btnStop.TabIndex = 5;
        btnStop.Text = "暂停";
        btnStop.UseVisualStyleBackColor = true;
        btnStop.Click += btnStop_Click;
        
        // 
        // btnReset
        // 
        btnReset.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
        btnReset.Location = new Point(262, 230);
        btnReset.Name = "btnReset";
        btnReset.Size = new Size(110, 40);
        btnReset.TabIndex = 6;
        btnReset.Text = "重置";
        btnReset.UseVisualStyleBackColor = true;
        btnReset.Click += btnReset_Click;
        
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(384, 291);
        Controls.Add(btnReset);
        Controls.Add(btnStop);
        Controls.Add(btnStart);
        Controls.Add(panel1);
        Controls.Add(lblPomodoros);
        Controls.Add(lblStatus);
        Controls.Add(lblTimeDisplay);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "番茄时钟";
        ResumeLayout(false);
    }

    #endregion

    private Label lblTimeDisplay;
    private Label lblStatus;
    private Label lblPomodoros;
    private Panel panel1;
    private Button btnStart;
    private Button btnStop;
    private Button btnReset;
}