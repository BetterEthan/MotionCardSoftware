namespace MotionCardSoftware
{
    partial class MotionControlPlatform
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.路径绘制 = new System.Windows.Forms.Button();
            this.路径恢复 = new System.Windows.Forms.Button();
            this.KeyPointNumLabel = new System.Windows.Forms.Label();
            this.路径清空 = new System.Windows.Forms.Button();
            this.UsartBox = new System.Windows.Forms.ComboBox();
            this.串口开关 = new System.Windows.Forms.Button();
            this.TimeReadCom = new System.Windows.Forms.Timer(this.components);
            this.示教路径 = new System.Windows.Forms.Button();
            this.实时路径 = new System.Windows.Forms.Button();
            this.示教路径恢复 = new System.Windows.Forms.Button();
            this.实时路径恢复 = new System.Windows.Forms.Button();
            this.NoticeInformation = new System.Windows.Forms.Label();
            this.发送路径 = new System.Windows.Forms.Button();
            this.速度图像 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.InputBox = new System.Windows.Forms.TextBox();
            this.确定输入 = new System.Windows.Forms.Button();
            this.SendLabel = new System.Windows.Forms.Label();
            this.开始运行 = new System.Windows.Forms.Button();
            this.停止运行 = new System.Windows.Forms.Button();
            this.关键点信息 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.打开文件 = new System.Windows.Forms.Button();
            this.viewControl = new DxfViewExample.ViewControl();
            this.SuspendLayout();
            // 
            // 路径绘制
            // 
            this.路径绘制.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.路径绘制.Location = new System.Drawing.Point(690, 32);
            this.路径绘制.Name = "路径绘制";
            this.路径绘制.Size = new System.Drawing.Size(75, 23);
            this.路径绘制.TabIndex = 1;
            this.路径绘制.Text = "路径绘制";
            this.路径绘制.UseVisualStyleBackColor = true;
            this.路径绘制.Click += new System.EventHandler(this.路径绘制_Click);
            // 
            // 路径恢复
            // 
            this.路径恢复.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.路径恢复.Location = new System.Drawing.Point(778, 32);
            this.路径恢复.Name = "路径恢复";
            this.路径恢复.Size = new System.Drawing.Size(75, 23);
            this.路径恢复.TabIndex = 2;
            this.路径恢复.Text = "路径恢复";
            this.路径恢复.UseVisualStyleBackColor = true;
            this.路径恢复.Click += new System.EventHandler(this.路径恢复_Click);
            // 
            // KeyPointNumLabel
            // 
            this.KeyPointNumLabel.AutoSize = true;
            this.KeyPointNumLabel.Location = new System.Drawing.Point(616, 16);
            this.KeyPointNumLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.KeyPointNumLabel.Name = "KeyPointNumLabel";
            this.KeyPointNumLabel.Size = new System.Drawing.Size(101, 12);
            this.KeyPointNumLabel.TabIndex = 5;
            this.KeyPointNumLabel.Text = "KeyPointNumLabel";
            // 
            // 路径清空
            // 
            this.路径清空.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.路径清空.Location = new System.Drawing.Point(778, 128);
            this.路径清空.Name = "路径清空";
            this.路径清空.Size = new System.Drawing.Size(75, 23);
            this.路径清空.TabIndex = 9;
            this.路径清空.Text = "路径清空";
            this.路径清空.UseVisualStyleBackColor = true;
            this.路径清空.Click += new System.EventHandler(this.路径清空_Click);
            // 
            // UsartBox
            // 
            this.UsartBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UsartBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.UsartBox.FormattingEnabled = true;
            this.UsartBox.Location = new System.Drawing.Point(690, 168);
            this.UsartBox.Name = "UsartBox";
            this.UsartBox.Size = new System.Drawing.Size(72, 20);
            this.UsartBox.TabIndex = 12;
            this.UsartBox.DropDown += new System.EventHandler(this.UsartBox_DropDown);
            // 
            // 串口开关
            // 
            this.串口开关.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.串口开关.Location = new System.Drawing.Point(778, 168);
            this.串口开关.Name = "串口开关";
            this.串口开关.Size = new System.Drawing.Size(75, 23);
            this.串口开关.TabIndex = 13;
            this.串口开关.Text = "打开串口";
            this.串口开关.UseVisualStyleBackColor = true;
            this.串口开关.Click += new System.EventHandler(this.串口开关_Click);
            // 
            // TimeReadCom
            // 
            this.TimeReadCom.Tick += new System.EventHandler(this.TimeReadCom_Tick);
            // 
            // 示教路径
            // 
            this.示教路径.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.示教路径.Location = new System.Drawing.Point(690, 64);
            this.示教路径.Name = "示教路径";
            this.示教路径.Size = new System.Drawing.Size(75, 23);
            this.示教路径.TabIndex = 15;
            this.示教路径.Text = "示教路径";
            this.示教路径.UseVisualStyleBackColor = true;
            this.示教路径.Click += new System.EventHandler(this.示教路径_Click);
            // 
            // 实时路径
            // 
            this.实时路径.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.实时路径.Location = new System.Drawing.Point(690, 96);
            this.实时路径.Name = "实时路径";
            this.实时路径.Size = new System.Drawing.Size(75, 23);
            this.实时路径.TabIndex = 16;
            this.实时路径.Text = "实时路径";
            this.实时路径.UseVisualStyleBackColor = true;
            this.实时路径.Click += new System.EventHandler(this.实时路径_Click);
            // 
            // 示教路径恢复
            // 
            this.示教路径恢复.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.示教路径恢复.Location = new System.Drawing.Point(778, 64);
            this.示教路径恢复.Name = "示教路径恢复";
            this.示教路径恢复.Size = new System.Drawing.Size(75, 23);
            this.示教路径恢复.TabIndex = 17;
            this.示教路径恢复.Text = "路径恢复";
            this.示教路径恢复.UseVisualStyleBackColor = true;
            this.示教路径恢复.Click += new System.EventHandler(this.示教路径恢复_Click);
            // 
            // 实时路径恢复
            // 
            this.实时路径恢复.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.实时路径恢复.Location = new System.Drawing.Point(778, 96);
            this.实时路径恢复.Name = "实时路径恢复";
            this.实时路径恢复.Size = new System.Drawing.Size(75, 23);
            this.实时路径恢复.TabIndex = 18;
            this.实时路径恢复.Text = "路径恢复";
            this.实时路径恢复.UseVisualStyleBackColor = true;
            this.实时路径恢复.Click += new System.EventHandler(this.实时路径恢复_Click);
            // 
            // NoticeInformation
            // 
            this.NoticeInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NoticeInformation.AutoSize = true;
            this.NoticeInformation.Location = new System.Drawing.Point(448, 520);
            this.NoticeInformation.Name = "NoticeInformation";
            this.NoticeInformation.Size = new System.Drawing.Size(107, 12);
            this.NoticeInformation.TabIndex = 19;
            this.NoticeInformation.Text = "NoticeInformation";
            // 
            // 发送路径
            // 
            this.发送路径.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.发送路径.Location = new System.Drawing.Point(690, 128);
            this.发送路径.Name = "发送路径";
            this.发送路径.Size = new System.Drawing.Size(75, 23);
            this.发送路径.TabIndex = 20;
            this.发送路径.Text = "发送路径";
            this.发送路径.UseVisualStyleBackColor = true;
            this.发送路径.Click += new System.EventHandler(this.发送路径_Click);
            // 
            // 速度图像
            // 
            this.速度图像.Location = new System.Drawing.Point(80, 8);
            this.速度图像.Name = "速度图像";
            this.速度图像.Size = new System.Drawing.Size(64, 24);
            this.速度图像.TabIndex = 0;
            this.速度图像.Text = "速度图像";
            this.速度图像.UseVisualStyleBackColor = true;
            this.速度图像.Click += new System.EventHandler(this.速度图像_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(776, 344);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 21;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // InputBox
            // 
            this.InputBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.InputBox.Location = new System.Drawing.Point(698, 208);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(152, 21);
            this.InputBox.TabIndex = 22;
            // 
            // 确定输入
            // 
            this.确定输入.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.确定输入.Location = new System.Drawing.Point(778, 240);
            this.确定输入.Name = "确定输入";
            this.确定输入.Size = new System.Drawing.Size(72, 23);
            this.确定输入.TabIndex = 23;
            this.确定输入.Text = "确定";
            this.确定输入.UseVisualStyleBackColor = true;
            this.确定输入.Click += new System.EventHandler(this.确定输入_Click);
            // 
            // SendLabel
            // 
            this.SendLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SendLabel.AutoSize = true;
            this.SendLabel.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.SendLabel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SendLabel.Location = new System.Drawing.Point(584, 520);
            this.SendLabel.Name = "SendLabel";
            this.SendLabel.Size = new System.Drawing.Size(59, 12);
            this.SendLabel.TabIndex = 26;
            this.SendLabel.Text = "SendLabel";
            // 
            // 开始运行
            // 
            this.开始运行.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.开始运行.Location = new System.Drawing.Point(690, 288);
            this.开始运行.Name = "开始运行";
            this.开始运行.Size = new System.Drawing.Size(75, 23);
            this.开始运行.TabIndex = 27;
            this.开始运行.Text = "开始运行";
            this.开始运行.UseVisualStyleBackColor = true;
            this.开始运行.Click += new System.EventHandler(this.开始运行_Click);
            // 
            // 停止运行
            // 
            this.停止运行.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.停止运行.Location = new System.Drawing.Point(778, 288);
            this.停止运行.Name = "停止运行";
            this.停止运行.Size = new System.Drawing.Size(75, 23);
            this.停止运行.TabIndex = 28;
            this.停止运行.Text = "停止运行";
            this.停止运行.UseVisualStyleBackColor = true;
            this.停止运行.Click += new System.EventHandler(this.停止运行_Click);
            // 
            // 关键点信息
            // 
            this.关键点信息.Location = new System.Drawing.Point(152, 8);
            this.关键点信息.Name = "关键点信息";
            this.关键点信息.Size = new System.Drawing.Size(75, 23);
            this.关键点信息.TabIndex = 29;
            this.关键点信息.Text = "关键点信息";
            this.关键点信息.UseVisualStyleBackColor = true;
            this.关键点信息.Click += new System.EventHandler(this.关键点信息_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 520);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 30;
            this.label1.Text = "label1";
            // 
            // 打开文件
            // 
            this.打开文件.Location = new System.Drawing.Point(0, 8);
            this.打开文件.Name = "打开文件";
            this.打开文件.Size = new System.Drawing.Size(75, 23);
            this.打开文件.TabIndex = 31;
            this.打开文件.Text = "打开文件";
            this.打开文件.UseVisualStyleBackColor = true;
            this.打开文件.Click += new System.EventHandler(this.打开文件_Click);
            // 
            // viewControl
            // 
            this.viewControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.viewControl.AutoSize = true;
            this.viewControl.BackColor = System.Drawing.Color.Black;
            this.viewControl.Location = new System.Drawing.Point(0, 32);
            this.viewControl.Model = null;
            this.viewControl.Name = "viewControl";
            this.viewControl.Size = new System.Drawing.Size(680, 480);
            this.viewControl.TabIndex = 0;
            this.viewControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.viewControl_MouseDown);
            this.viewControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.viewControl_MouseMove);
            this.viewControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.viewControl_MouseUp);
            // 
            // MotionControlPlatform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(862, 536);
            this.Controls.Add(this.打开文件);
            this.Controls.Add(this.viewControl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.关键点信息);
            this.Controls.Add(this.停止运行);
            this.Controls.Add(this.开始运行);
            this.Controls.Add(this.SendLabel);
            this.Controls.Add(this.确定输入);
            this.Controls.Add(this.InputBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.速度图像);
            this.Controls.Add(this.发送路径);
            this.Controls.Add(this.NoticeInformation);
            this.Controls.Add(this.实时路径恢复);
            this.Controls.Add(this.示教路径恢复);
            this.Controls.Add(this.实时路径);
            this.Controls.Add(this.示教路径);
            this.Controls.Add(this.串口开关);
            this.Controls.Add(this.UsartBox);
            this.Controls.Add(this.路径清空);
            this.Controls.Add(this.KeyPointNumLabel);
            this.Controls.Add(this.路径恢复);
            this.Controls.Add(this.路径绘制);
            this.MinimumSize = new System.Drawing.Size(775, 449);
            this.Name = "MotionControlPlatform";
            this.Text = "MotionCardPlatform";
            this.Load += new System.EventHandler(this.MotionControlPlatform_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button 路径绘制;
        private System.Windows.Forms.Button 路径恢复;
        private System.Windows.Forms.Label KeyPointNumLabel;
        private System.Windows.Forms.Button 路径清空;
        private System.Windows.Forms.ComboBox UsartBox;
        private System.Windows.Forms.Button 串口开关;
        private System.Windows.Forms.Button 示教路径;
        private System.Windows.Forms.Button 实时路径;
        private System.Windows.Forms.Button 示教路径恢复;
        private System.Windows.Forms.Button 实时路径恢复;
        private System.Windows.Forms.Label NoticeInformation;
        private System.Windows.Forms.Button 发送路径;
        private System.Windows.Forms.Button 速度图像;
        public System.Windows.Forms.Timer TimeReadCom;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox InputBox;
        private System.Windows.Forms.Button 确定输入;
        private System.Windows.Forms.Label SendLabel;
        private System.Windows.Forms.Button 开始运行;
        private System.Windows.Forms.Button 停止运行;
        private System.Windows.Forms.Button 关键点信息;
        private System.Windows.Forms.Label label1;
        private DxfViewExample.ViewControl viewControl;
        private System.Windows.Forms.Button 打开文件;
    }
}

