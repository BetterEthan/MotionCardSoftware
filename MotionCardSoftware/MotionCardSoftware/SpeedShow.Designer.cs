namespace MotionCardSoftware
{
    partial class SpeedShow
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
            this.VelDisplay = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.显示比例 = new System.Windows.Forms.Label();
            this.ZoomLScrollBar = new System.Windows.Forms.HScrollBar();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.VelDisplayBox = new System.Windows.Forms.ComboBox();
            this.Vel = new System.Windows.Forms.Label();
            this.Length = new System.Windows.Forms.Label();
            this.显示选择 = new System.Windows.Forms.Label();
            this.单位 = new System.Windows.Forms.Label();
            this.速度更新 = new System.Windows.Forms.Button();
            this.VelDisplay.SuspendLayout();
            this.SuspendLayout();
            // 
            // VelDisplay
            // 
            this.VelDisplay.AllowDrop = true;
            this.VelDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VelDisplay.AutoScroll = true;
            this.VelDisplay.AutoScrollMargin = new System.Drawing.Size(10, 10);
            this.VelDisplay.AutoScrollMinSize = new System.Drawing.Size(10, 10);
            this.VelDisplay.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.VelDisplay.Controls.Add(this.label3);
            this.VelDisplay.Controls.Add(this.label2);
            this.VelDisplay.Controls.Add(this.label1);
            this.VelDisplay.Controls.Add(this.显示比例);
            this.VelDisplay.Controls.Add(this.ZoomLScrollBar);
            this.VelDisplay.Controls.Add(this.hScrollBar1);
            this.VelDisplay.Location = new System.Drawing.Point(0, 0);
            this.VelDisplay.Name = "VelDisplay";
            this.VelDisplay.Size = new System.Drawing.Size(748, 448);
            this.VelDisplay.TabIndex = 0;
            this.VelDisplay.Paint += new System.Windows.Forms.PaintEventHandler(this.VelDisplay_Paint);
            this.VelDisplay.MouseMove += new System.Windows.Forms.MouseEventHandler(this.VelDisplay_MouseMove);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(648, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "蓝色目标速度";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Green;
            this.label2.Location = new System.Drawing.Point(648, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "绿色电机速度";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(648, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "红色实际速度";
            // 
            // 显示比例
            // 
            this.显示比例.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.显示比例.AutoSize = true;
            this.显示比例.Location = new System.Drawing.Point(576, 16);
            this.显示比例.Name = "显示比例";
            this.显示比例.Size = new System.Drawing.Size(53, 12);
            this.显示比例.TabIndex = 2;
            this.显示比例.Text = "显示比例";
            // 
            // ZoomLScrollBar
            // 
            this.ZoomLScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ZoomLScrollBar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ZoomLScrollBar.LargeChange = 1;
            this.ZoomLScrollBar.Location = new System.Drawing.Point(648, 8);
            this.ZoomLScrollBar.Name = "ZoomLScrollBar";
            this.ZoomLScrollBar.Size = new System.Drawing.Size(88, 24);
            this.ZoomLScrollBar.TabIndex = 1;
            this.ZoomLScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ZoomLScrollBar_Scroll);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar1.Location = new System.Drawing.Point(0, 432);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(748, 16);
            this.hScrollBar1.TabIndex = 0;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // VelDisplayBox
            // 
            this.VelDisplayBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.VelDisplayBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VelDisplayBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.VelDisplayBox.FormattingEnabled = true;
            this.VelDisplayBox.Items.AddRange(new object[] {
            "1号轮速度",
            "2号轮速度",
            "3号轮速度",
            "位置误差"});
            this.VelDisplayBox.Location = new System.Drawing.Point(552, 456);
            this.VelDisplayBox.Name = "VelDisplayBox";
            this.VelDisplayBox.Size = new System.Drawing.Size(80, 20);
            this.VelDisplayBox.TabIndex = 4;
            this.VelDisplayBox.SelectedIndexChanged += new System.EventHandler(this.VelDisplayBox_SelectedIndexChanged);
            // 
            // Vel
            // 
            this.Vel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Vel.AutoSize = true;
            this.Vel.Location = new System.Drawing.Point(136, 464);
            this.Vel.Name = "Vel";
            this.Vel.Size = new System.Drawing.Size(23, 12);
            this.Vel.TabIndex = 1;
            this.Vel.Text = "Vel";
            // 
            // Length
            // 
            this.Length.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Length.AutoSize = true;
            this.Length.Location = new System.Drawing.Point(16, 464);
            this.Length.Name = "Length";
            this.Length.Size = new System.Drawing.Size(41, 12);
            this.Length.TabIndex = 3;
            this.Length.Text = "Length";
            // 
            // 显示选择
            // 
            this.显示选择.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.显示选择.AutoSize = true;
            this.显示选择.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.显示选择.Location = new System.Drawing.Point(456, 456);
            this.显示选择.Name = "显示选择";
            this.显示选择.Size = new System.Drawing.Size(85, 16);
            this.显示选择.TabIndex = 5;
            this.显示选择.Text = "显示选择:";
            // 
            // 单位
            // 
            this.单位.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.单位.AutoSize = true;
            this.单位.Location = new System.Drawing.Point(192, 464);
            this.单位.Name = "单位";
            this.单位.Size = new System.Drawing.Size(29, 12);
            this.单位.TabIndex = 6;
            this.单位.Text = "单位";
            // 
            // 速度更新
            // 
            this.速度更新.Location = new System.Drawing.Point(656, 456);
            this.速度更新.Name = "速度更新";
            this.速度更新.Size = new System.Drawing.Size(75, 23);
            this.速度更新.TabIndex = 7;
            this.速度更新.Text = "速度更新";
            this.速度更新.UseVisualStyleBackColor = true;
            this.速度更新.Click += new System.EventHandler(this.速度更新_Click);
            // 
            // SpeedShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(751, 480);
            this.Controls.Add(this.速度更新);
            this.Controls.Add(this.单位);
            this.Controls.Add(this.显示选择);
            this.Controls.Add(this.Length);
            this.Controls.Add(this.VelDisplayBox);
            this.Controls.Add(this.Vel);
            this.Controls.Add(this.VelDisplay);
            this.Name = "SpeedShow";
            this.Text = "SpeedShow";
            this.Load += new System.EventHandler(this.SpeedShow_Load);
            this.Resize += new System.EventHandler(this.SpeedShow_Resize);
            this.VelDisplay.ResumeLayout(false);
            this.VelDisplay.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel VelDisplay;
        private System.Windows.Forms.Label Vel;
        private System.Windows.Forms.Label Length;
        private System.Windows.Forms.ComboBox VelDisplayBox;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.HScrollBar ZoomLScrollBar;
        private System.Windows.Forms.Label 显示选择;
        private System.Windows.Forms.Label 显示比例;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label 单位;
        private System.Windows.Forms.Button 速度更新;
    }
}