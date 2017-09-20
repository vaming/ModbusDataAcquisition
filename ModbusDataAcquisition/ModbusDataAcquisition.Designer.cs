namespace ModbusDataAcquisition
{
    partial class ModbusDataAcquisition
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.communicationSetting = new System.Windows.Forms.GroupBox();
            this.btnOpenCloseSCom = new System.Windows.Forms.Button();
            this.cbSerial = new System.Windows.Forms.ComboBox();
            this.cbBaudRate = new System.Windows.Forms.ComboBox();
            this.cbStop = new System.Windows.Forms.ComboBox();
            this.cbDataBits = new System.Windows.Forms.ComboBox();
            this.cbParity = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbAddr = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.communicationSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // communicationSetting
            // 
            this.communicationSetting.Controls.Add(this.btnSave);
            this.communicationSetting.Controls.Add(this.tbAddr);
            this.communicationSetting.Controls.Add(this.label6);
            this.communicationSetting.Controls.Add(this.label5);
            this.communicationSetting.Controls.Add(this.label4);
            this.communicationSetting.Controls.Add(this.label3);
            this.communicationSetting.Controls.Add(this.label2);
            this.communicationSetting.Controls.Add(this.label1);
            this.communicationSetting.Controls.Add(this.cbParity);
            this.communicationSetting.Controls.Add(this.cbDataBits);
            this.communicationSetting.Controls.Add(this.cbStop);
            this.communicationSetting.Controls.Add(this.cbBaudRate);
            this.communicationSetting.Controls.Add(this.cbSerial);
            this.communicationSetting.Controls.Add(this.btnOpenCloseSCom);
            this.communicationSetting.Location = new System.Drawing.Point(13, 4);
            this.communicationSetting.Name = "communicationSetting";
            this.communicationSetting.Size = new System.Drawing.Size(180, 311);
            this.communicationSetting.TabIndex = 0;
            this.communicationSetting.TabStop = false;
            this.communicationSetting.Text = "通信设置";
            // 
            // btnOpenCloseSCom
            // 
            this.btnOpenCloseSCom.Location = new System.Drawing.Point(7, 273);
            this.btnOpenCloseSCom.Name = "btnOpenCloseSCom";
            this.btnOpenCloseSCom.Size = new System.Drawing.Size(75, 23);
            this.btnOpenCloseSCom.TabIndex = 0;
            this.btnOpenCloseSCom.Text = "打开通信";
            this.btnOpenCloseSCom.UseVisualStyleBackColor = true;
            // 
            // cbSerial
            // 
            this.cbSerial.FormattingEnabled = true;
            this.cbSerial.Location = new System.Drawing.Point(74, 29);
            this.cbSerial.Name = "cbSerial";
            this.cbSerial.Size = new System.Drawing.Size(81, 20);
            this.cbSerial.TabIndex = 1;
            // 
            // cbBaudRate
            // 
            this.cbBaudRate.AutoCompleteCustomSource.AddRange(new string[] {
            "4800",
            "9600",
            "19200",
            "38400",
            "115200"});
            this.cbBaudRate.FormattingEnabled = true;
            this.cbBaudRate.Location = new System.Drawing.Point(74, 69);
            this.cbBaudRate.Name = "cbBaudRate";
            this.cbBaudRate.Size = new System.Drawing.Size(81, 20);
            this.cbBaudRate.TabIndex = 2;
            // 
            // cbStop
            // 
            this.cbStop.AutoCompleteCustomSource.AddRange(new string[] {
            "1",
            "1.5",
            "2"});
            this.cbStop.FormattingEnabled = true;
            this.cbStop.Location = new System.Drawing.Point(74, 107);
            this.cbStop.Name = "cbStop";
            this.cbStop.Size = new System.Drawing.Size(81, 20);
            this.cbStop.TabIndex = 3;
            // 
            // cbDataBits
            // 
            this.cbDataBits.AutoCompleteCustomSource.AddRange(new string[] {
            "5",
            "6",
            "7",
            "8"});
            this.cbDataBits.FormattingEnabled = true;
            this.cbDataBits.Location = new System.Drawing.Point(74, 150);
            this.cbDataBits.Name = "cbDataBits";
            this.cbDataBits.Size = new System.Drawing.Size(81, 20);
            this.cbDataBits.TabIndex = 4;
            // 
            // cbParity
            // 
            this.cbParity.AutoCompleteCustomSource.AddRange(new string[] {
            "无",
            "奇校验",
            "偶校验"});
            this.cbParity.FormattingEnabled = true;
            this.cbParity.Location = new System.Drawing.Point(74, 191);
            this.cbParity.Name = "cbParity";
            this.cbParity.Size = new System.Drawing.Size(81, 20);
            this.cbParity.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "通信口";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "波特率";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "停止位";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "数据位";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 199);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "校验位";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 236);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "从站地址";
            // 
            // tbAddr
            // 
            this.tbAddr.Location = new System.Drawing.Point(74, 233);
            this.tbAddr.Name = "tbAddr";
            this.tbAddr.Size = new System.Drawing.Size(81, 21);
            this.tbAddr.TabIndex = 12;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(88, 273);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "保存参数";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // ModbusDataAcquisition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1440, 724);
            this.Controls.Add(this.communicationSetting);
            this.Name = "ModbusDataAcquisition";
            this.Text = "数据采集器";
            this.communicationSetting.ResumeLayout(false);
            this.communicationSetting.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox communicationSetting;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox tbAddr;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbParity;
        private System.Windows.Forms.ComboBox cbDataBits;
        private System.Windows.Forms.ComboBox cbStop;
        private System.Windows.Forms.ComboBox cbBaudRate;
        private System.Windows.Forms.ComboBox cbSerial;
        private System.Windows.Forms.Button btnOpenCloseSCom;
    }
}

