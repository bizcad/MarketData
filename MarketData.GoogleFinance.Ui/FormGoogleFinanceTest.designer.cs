namespace QuantConnect.GoogleFinanceUI {
    partial class FormGoogleFinanceTest {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxData = new System.Windows.Forms.GroupBox();
            this.buttonMoveData = new System.Windows.Forms.Button();
            this.buttonCheckDataFormat = new System.Windows.Forms.Button();
            this.checkBoxSplitDays = new System.Windows.Forms.CheckBox();
            this.checkBoxDateTime = new System.Windows.Forms.CheckBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.checkBoxRawData = new System.Windows.Forms.CheckBox();
            this.richTextBoxData = new System.Windows.Forms.RichTextBox();
            this.groupBoxURL = new System.Windows.Forms.GroupBox();
            this.textBoxURL = new System.Windows.Forms.TextBox();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.panelURLData = new System.Windows.Forms.Panel();
            this.groupBoxDatesRange = new System.Windows.Forms.GroupBox();
            this.buttonList = new System.Windows.Forms.Button();
            this.radioButtonMinutes = new System.Windows.Forms.RadioButton();
            this.dateTimePickerSinceDate = new System.Windows.Forms.DateTimePicker();
            this.radioButtonSince = new System.Windows.Forms.RadioButton();
            this.radioButtonLastQuoute = new System.Windows.Forms.RadioButton();
            this.radioButtonAllData = new System.Windows.Forms.RadioButton();
            this.groupBoxTicker = new System.Windows.Forms.GroupBox();
            this.labelCode = new System.Windows.Forms.Label();
            this.labelExchange = new System.Windows.Forms.Label();
            this.textBoxExchange = new System.Windows.Forms.TextBox();
            this.textBoxTicker = new System.Windows.Forms.TextBox();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.checkBoxZipOutput = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel.SuspendLayout();
            this.groupBoxData.SuspendLayout();
            this.groupBoxURL.SuspendLayout();
            this.panelURLData.SuspendLayout();
            this.groupBoxDatesRange.SuspendLayout();
            this.groupBoxTicker.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.groupBoxData, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.groupBoxURL, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.panelURLData, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 119F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(519, 599);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // groupBoxData
            // 
            this.groupBoxData.Controls.Add(this.checkBoxZipOutput);
            this.groupBoxData.Controls.Add(this.buttonMoveData);
            this.groupBoxData.Controls.Add(this.buttonCheckDataFormat);
            this.groupBoxData.Controls.Add(this.checkBoxSplitDays);
            this.groupBoxData.Controls.Add(this.checkBoxDateTime);
            this.groupBoxData.Controls.Add(this.buttonSave);
            this.groupBoxData.Controls.Add(this.checkBoxRawData);
            this.groupBoxData.Controls.Add(this.richTextBoxData);
            this.groupBoxData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxData.Location = new System.Drawing.Point(3, 200);
            this.groupBoxData.Name = "groupBoxData";
            this.groupBoxData.Size = new System.Drawing.Size(513, 396);
            this.groupBoxData.TabIndex = 2;
            this.groupBoxData.TabStop = false;
            this.groupBoxData.Text = "Data";
            // 
            // buttonMoveData
            // 
            this.buttonMoveData.Enabled = false;
            this.buttonMoveData.Location = new System.Drawing.Point(353, 46);
            this.buttonMoveData.Name = "buttonMoveData";
            this.buttonMoveData.Size = new System.Drawing.Size(110, 23);
            this.buttonMoveData.TabIndex = 6;
            this.buttonMoveData.Text = "Move Data";
            this.buttonMoveData.UseVisualStyleBackColor = true;
            this.buttonMoveData.Click += new System.EventHandler(this.buttonMoveData_Click);
            // 
            // buttonCheckDataFormat
            // 
            this.buttonCheckDataFormat.Enabled = false;
            this.buttonCheckDataFormat.Location = new System.Drawing.Point(139, 46);
            this.buttonCheckDataFormat.Name = "buttonCheckDataFormat";
            this.buttonCheckDataFormat.Size = new System.Drawing.Size(110, 23);
            this.buttonCheckDataFormat.TabIndex = 5;
            this.buttonCheckDataFormat.Text = "Check Data Format";
            this.buttonCheckDataFormat.UseVisualStyleBackColor = true;
            this.buttonCheckDataFormat.Click += new System.EventHandler(this.buttonCheckDataFormat_Click);
            // 
            // checkBoxSplitDays
            // 
            this.checkBoxSplitDays.AutoSize = true;
            this.checkBoxSplitDays.Location = new System.Drawing.Point(431, 23);
            this.checkBoxSplitDays.Name = "checkBoxSplitDays";
            this.checkBoxSplitDays.Size = new System.Drawing.Size(73, 17);
            this.checkBoxSplitDays.TabIndex = 4;
            this.checkBoxSplitDays.Text = "Split Days";
            this.checkBoxSplitDays.UseVisualStyleBackColor = true;
            // 
            // checkBoxDateTime
            // 
            this.checkBoxDateTime.AutoSize = true;
            this.checkBoxDateTime.Location = new System.Drawing.Point(353, 23);
            this.checkBoxDateTime.Name = "checkBoxDateTime";
            this.checkBoxDateTime.Size = new System.Drawing.Size(72, 17);
            this.checkBoxDateTime.TabIndex = 3;
            this.checkBoxDateTime.Text = "DateTime";
            this.checkBoxDateTime.UseVisualStyleBackColor = true;
            this.checkBoxDateTime.CheckedChanged += new System.EventHandler(this.checkBoxDateTime_CheckedChanged);
            // 
            // buttonSave
            // 
            this.buttonSave.Enabled = false;
            this.buttonSave.Location = new System.Drawing.Point(139, 17);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(110, 23);
            this.buttonSave.TabIndex = 1;
            this.buttonSave.Text = "Save Data to File";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // checkBoxRawData
            // 
            this.checkBoxRawData.AutoSize = true;
            this.checkBoxRawData.Location = new System.Drawing.Point(10, 23);
            this.checkBoxRawData.Name = "checkBoxRawData";
            this.checkBoxRawData.Size = new System.Drawing.Size(72, 17);
            this.checkBoxRawData.TabIndex = 0;
            this.checkBoxRawData.Text = "Raw data";
            this.checkBoxRawData.UseVisualStyleBackColor = true;
            this.checkBoxRawData.CheckedChanged += new System.EventHandler(this.checkBoxRawData_CheckedChanged);
            // 
            // richTextBoxData
            // 
            this.richTextBoxData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxData.BackColor = System.Drawing.Color.White;
            this.richTextBoxData.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxData.Location = new System.Drawing.Point(5, 90);
            this.richTextBoxData.Name = "richTextBoxData";
            this.richTextBoxData.ReadOnly = true;
            this.richTextBoxData.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBoxData.Size = new System.Drawing.Size(503, 297);
            this.richTextBoxData.TabIndex = 2;
            this.richTextBoxData.Text = "";
            this.richTextBoxData.TextChanged += new System.EventHandler(this.textBoxData_TextChanged);
            // 
            // groupBoxURL
            // 
            this.groupBoxURL.Controls.Add(this.textBoxURL);
            this.groupBoxURL.Controls.Add(this.buttonDownload);
            this.groupBoxURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxURL.Location = new System.Drawing.Point(3, 122);
            this.groupBoxURL.Name = "groupBoxURL";
            this.groupBoxURL.Size = new System.Drawing.Size(513, 72);
            this.groupBoxURL.TabIndex = 1;
            this.groupBoxURL.TabStop = false;
            this.groupBoxURL.Text = "URL";
            // 
            // textBoxURL
            // 
            this.textBoxURL.Location = new System.Drawing.Point(9, 18);
            this.textBoxURL.Multiline = true;
            this.textBoxURL.Name = "textBoxURL";
            this.textBoxURL.ReadOnly = true;
            this.textBoxURL.Size = new System.Drawing.Size(339, 40);
            this.textBoxURL.TabIndex = 0;
            this.textBoxURL.TextChanged += new System.EventHandler(this.textBoxURL_TextChanged);
            // 
            // buttonDownload
            // 
            this.buttonDownload.Enabled = false;
            this.buttonDownload.Location = new System.Drawing.Point(377, 16);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(112, 23);
            this.buttonDownload.TabIndex = 1;
            this.buttonDownload.Text = "Download Data";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // panelURLData
            // 
            this.panelURLData.Controls.Add(this.groupBoxDatesRange);
            this.panelURLData.Controls.Add(this.groupBoxTicker);
            this.panelURLData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelURLData.Location = new System.Drawing.Point(3, 3);
            this.panelURLData.Name = "panelURLData";
            this.panelURLData.Size = new System.Drawing.Size(513, 113);
            this.panelURLData.TabIndex = 0;
            // 
            // groupBoxDatesRange
            // 
            this.groupBoxDatesRange.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDatesRange.Controls.Add(this.buttonList);
            this.groupBoxDatesRange.Controls.Add(this.radioButtonMinutes);
            this.groupBoxDatesRange.Controls.Add(this.dateTimePickerSinceDate);
            this.groupBoxDatesRange.Controls.Add(this.radioButtonSince);
            this.groupBoxDatesRange.Controls.Add(this.radioButtonLastQuoute);
            this.groupBoxDatesRange.Controls.Add(this.radioButtonAllData);
            this.groupBoxDatesRange.Location = new System.Drawing.Point(268, 4);
            this.groupBoxDatesRange.Name = "groupBoxDatesRange";
            this.groupBoxDatesRange.Size = new System.Drawing.Size(244, 109);
            this.groupBoxDatesRange.TabIndex = 1;
            this.groupBoxDatesRange.TabStop = false;
            this.groupBoxDatesRange.Text = "Range";
            // 
            // buttonList
            // 
            this.buttonList.Enabled = false;
            this.buttonList.Location = new System.Drawing.Point(109, 85);
            this.buttonList.Name = "buttonList";
            this.buttonList.Size = new System.Drawing.Size(112, 23);
            this.buttonList.TabIndex = 2;
            this.buttonList.Text = "Symbols From List";
            this.buttonList.UseVisualStyleBackColor = true;
            this.buttonList.Click += new System.EventHandler(this.buttonList_Click);
            // 
            // radioButtonMinutes
            // 
            this.radioButtonMinutes.AutoSize = true;
            this.radioButtonMinutes.Location = new System.Drawing.Point(19, 88);
            this.radioButtonMinutes.Name = "radioButtonMinutes";
            this.radioButtonMinutes.Size = new System.Drawing.Size(83, 17);
            this.radioButtonMinutes.TabIndex = 4;
            this.radioButtonMinutes.Text = "Minute Data";
            this.radioButtonMinutes.UseVisualStyleBackColor = true;
            this.radioButtonMinutes.CheckedChanged += new System.EventHandler(this.radioButtonMinutes_CheckedChanged);
            // 
            // dateTimePickerSinceDate
            // 
            this.dateTimePickerSinceDate.Enabled = false;
            this.dateTimePickerSinceDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerSinceDate.Location = new System.Drawing.Point(111, 63);
            this.dateTimePickerSinceDate.Name = "dateTimePickerSinceDate";
            this.dateTimePickerSinceDate.Size = new System.Drawing.Size(85, 20);
            this.dateTimePickerSinceDate.TabIndex = 3;
            this.dateTimePickerSinceDate.ValueChanged += new System.EventHandler(this.uriParameterControl_ValueChanged);
            // 
            // radioButtonSince
            // 
            this.radioButtonSince.AutoSize = true;
            this.radioButtonSince.Location = new System.Drawing.Point(19, 66);
            this.radioButtonSince.Name = "radioButtonSince";
            this.radioButtonSince.Size = new System.Drawing.Size(91, 17);
            this.radioButtonSince.TabIndex = 2;
            this.radioButtonSince.Text = "At least since:";
            this.radioButtonSince.UseVisualStyleBackColor = true;
            this.radioButtonSince.CheckedChanged += new System.EventHandler(this.uriParameterControl_ValueChanged);
            // 
            // radioButtonLastQuoute
            // 
            this.radioButtonLastQuoute.AutoSize = true;
            this.radioButtonLastQuoute.Location = new System.Drawing.Point(19, 43);
            this.radioButtonLastQuoute.Name = "radioButtonLastQuoute";
            this.radioButtonLastQuoute.Size = new System.Drawing.Size(75, 17);
            this.radioButtonLastQuoute.TabIndex = 1;
            this.radioButtonLastQuoute.Text = "Last quote";
            this.radioButtonLastQuoute.UseVisualStyleBackColor = true;
            // 
            // radioButtonAllData
            // 
            this.radioButtonAllData.AutoSize = true;
            this.radioButtonAllData.Checked = true;
            this.radioButtonAllData.Location = new System.Drawing.Point(19, 20);
            this.radioButtonAllData.Name = "radioButtonAllData";
            this.radioButtonAllData.Size = new System.Drawing.Size(60, 17);
            this.radioButtonAllData.TabIndex = 0;
            this.radioButtonAllData.TabStop = true;
            this.radioButtonAllData.Text = "All data";
            this.radioButtonAllData.UseVisualStyleBackColor = true;
            this.radioButtonAllData.CheckedChanged += new System.EventHandler(this.uriParameterControl_ValueChanged);
            // 
            // groupBoxTicker
            // 
            this.groupBoxTicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxTicker.Controls.Add(this.labelCode);
            this.groupBoxTicker.Controls.Add(this.labelExchange);
            this.groupBoxTicker.Controls.Add(this.textBoxExchange);
            this.groupBoxTicker.Controls.Add(this.textBoxTicker);
            this.groupBoxTicker.Location = new System.Drawing.Point(2, 4);
            this.groupBoxTicker.Name = "groupBoxTicker";
            this.groupBoxTicker.Size = new System.Drawing.Size(215, 105);
            this.groupBoxTicker.TabIndex = 0;
            this.groupBoxTicker.TabStop = false;
            this.groupBoxTicker.Text = "Ticker";
            // 
            // labelCode
            // 
            this.labelCode.AutoSize = true;
            this.labelCode.Location = new System.Drawing.Point(25, 66);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(44, 13);
            this.labelCode.TabIndex = 2;
            this.labelCode.Text = "Symbol:";
            // 
            // labelExchange
            // 
            this.labelExchange.AutoSize = true;
            this.labelExchange.Location = new System.Drawing.Point(25, 27);
            this.labelExchange.Name = "labelExchange";
            this.labelExchange.Size = new System.Drawing.Size(58, 13);
            this.labelExchange.TabIndex = 0;
            this.labelExchange.Text = "Exchange:";
            // 
            // textBoxExchange
            // 
            this.textBoxExchange.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxExchange.Location = new System.Drawing.Point(89, 23);
            this.textBoxExchange.Name = "textBoxExchange";
            this.textBoxExchange.Size = new System.Drawing.Size(100, 20);
            this.textBoxExchange.TabIndex = 1;
            this.textBoxExchange.Text = "NYSEARCA";
            this.textBoxExchange.TextChanged += new System.EventHandler(this.uriParameterControl_ValueChanged);
            // 
            // textBoxTicker
            // 
            this.textBoxTicker.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxTicker.Location = new System.Drawing.Point(89, 62);
            this.textBoxTicker.Name = "textBoxTicker";
            this.textBoxTicker.Size = new System.Drawing.Size(100, 20);
            this.textBoxTicker.TabIndex = 3;
            this.textBoxTicker.Text = "SPY";
            this.textBoxTicker.TextChanged += new System.EventHandler(this.uriParameterControl_ValueChanged);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.FileName = "GogleFinanceTest.txt";
            this.saveFileDialog.Filter = "All files (*.*)|*.*";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog";
            // 
            // checkBoxZipOutput
            // 
            this.checkBoxZipOutput.AutoSize = true;
            this.checkBoxZipOutput.Location = new System.Drawing.Point(268, 23);
            this.checkBoxZipOutput.Name = "checkBoxZipOutput";
            this.checkBoxZipOutput.Size = new System.Drawing.Size(76, 17);
            this.checkBoxZipOutput.TabIndex = 7;
            this.checkBoxZipOutput.Text = "Zip Output";
            this.checkBoxZipOutput.UseVisualStyleBackColor = true;
            // 
            // FormGoogleFinanceTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 599);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(16, 435);
            this.Name = "FormGoogleFinanceTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Google Finance Download";
            this.tableLayoutPanel.ResumeLayout(false);
            this.groupBoxData.ResumeLayout(false);
            this.groupBoxData.PerformLayout();
            this.groupBoxURL.ResumeLayout(false);
            this.groupBoxURL.PerformLayout();
            this.panelURLData.ResumeLayout(false);
            this.groupBoxDatesRange.ResumeLayout(false);
            this.groupBoxDatesRange.PerformLayout();
            this.groupBoxTicker.ResumeLayout(false);
            this.groupBoxTicker.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.GroupBox groupBoxTicker;
        private System.Windows.Forms.Label labelCode;
        private System.Windows.Forms.Label labelExchange;
        private System.Windows.Forms.TextBox textBoxExchange;
        private System.Windows.Forms.TextBox textBoxTicker;
        private System.Windows.Forms.GroupBox groupBoxDatesRange;
        private System.Windows.Forms.RadioButton radioButtonSince;
        private System.Windows.Forms.RadioButton radioButtonLastQuoute;
        private System.Windows.Forms.RadioButton radioButtonAllData;
        private System.Windows.Forms.DateTimePicker dateTimePickerSinceDate;
        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.TextBox textBoxURL;
        private System.Windows.Forms.GroupBox groupBoxData;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.CheckBox checkBoxRawData;
        private System.Windows.Forms.RichTextBox richTextBoxData;
        private System.Windows.Forms.GroupBox groupBoxURL;
        private System.Windows.Forms.Panel panelURLData;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.RadioButton radioButtonMinutes;
        private System.Windows.Forms.CheckBox checkBoxDateTime;
        private System.Windows.Forms.CheckBox checkBoxSplitDays;
        private System.Windows.Forms.Button buttonList;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button buttonCheckDataFormat;
        private System.Windows.Forms.Button buttonMoveData;
        private System.Windows.Forms.CheckBox checkBoxZipOutput;
        
    }
}

