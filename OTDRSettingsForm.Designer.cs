
namespace FOMSSubmarine
{
	partial class OTDRSettingsForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OTDRSettingsForm));
			this.fr_OTDRSet = new System.Windows.Forms.GroupBox();
			this.cb_WaveLength = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.Label2 = new System.Windows.Forms.Label();
			this.cb_AcquisitionMode = new System.Windows.Forms.ComboBox();
			this.Label5 = new System.Windows.Forms.Label();
			this.gb_ManualParameters = new System.Windows.Forms.GroupBox();
			this.cb_Resolution = new System.Windows.Forms.ComboBox();
			this.cb_Range = new System.Windows.Forms.ComboBox();
			this.cb_PulseWidth = new System.Windows.Forms.ComboBox();
			this.Label3 = new System.Windows.Forms.Label();
			this.Label4 = new System.Windows.Forms.Label();
			this.Label6 = new System.Windows.Forms.Label();
			this.Label7 = new System.Windows.Forms.Label();
			this.Label11 = new System.Windows.Forms.Label();
			this.txt_IOR = new System.Windows.Forms.TextBox();
			this.txt_BSC = new System.Windows.Forms.TextBox();
			this.cb_AverageTime = new System.Windows.Forms.ComboBox();
			this.cb_OTDRSetSave = new System.Windows.Forms.Button();
			this.cb_OTDRSetClose = new System.Windows.Forms.Button();
			this.cb_OTDRSetInit = new System.Windows.Forms.Button();
			this.Label13 = new System.Windows.Forms.Label();
			this.Label14 = new System.Windows.Forms.Label();
			this.Label15 = new System.Windows.Forms.Label();
			this.fr_OTDRSet.SuspendLayout();
			this.gb_ManualParameters.SuspendLayout();
			this.SuspendLayout();
			// 
			// fr_OTDRSet
			// 
			this.fr_OTDRSet.AllowDrop = true;
			this.fr_OTDRSet.BackColor = System.Drawing.SystemColors.Control;
			this.fr_OTDRSet.Controls.Add(this.cb_WaveLength);
			this.fr_OTDRSet.Controls.Add(this.label1);
			this.fr_OTDRSet.Controls.Add(this.Label2);
			this.fr_OTDRSet.Controls.Add(this.cb_AcquisitionMode);
			this.fr_OTDRSet.Controls.Add(this.Label5);
			this.fr_OTDRSet.Controls.Add(this.gb_ManualParameters);
			this.fr_OTDRSet.Controls.Add(this.txt_IOR);
			this.fr_OTDRSet.Controls.Add(this.txt_BSC);
			this.fr_OTDRSet.Controls.Add(this.cb_AverageTime);
			this.fr_OTDRSet.Controls.Add(this.cb_OTDRSetSave);
			this.fr_OTDRSet.Controls.Add(this.cb_OTDRSetClose);
			this.fr_OTDRSet.Controls.Add(this.cb_OTDRSetInit);
			this.fr_OTDRSet.Controls.Add(this.Label13);
			this.fr_OTDRSet.Controls.Add(this.Label14);
			this.fr_OTDRSet.Controls.Add(this.Label15);
			this.fr_OTDRSet.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.fr_OTDRSet.ForeColor = System.Drawing.SystemColors.ControlText;
			this.fr_OTDRSet.Location = new System.Drawing.Point(7, 9);
			this.fr_OTDRSet.Name = "fr_OTDRSet";
			this.fr_OTDRSet.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.fr_OTDRSet.Size = new System.Drawing.Size(438, 278);
			this.fr_OTDRSet.TabIndex = 0;
			this.fr_OTDRSet.TabStop = false;
			this.fr_OTDRSet.Text = "OTDR Setting:";
			// 
			// cb_WaveLength
			// 
			this.cb_WaveLength.AllowDrop = true;
			this.cb_WaveLength.BackColor = System.Drawing.SystemColors.Window;
			this.cb_WaveLength.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cb_WaveLength.ForeColor = System.Drawing.SystemColors.WindowText;
			this.cb_WaveLength.Items.AddRange(new object[] {
            "1310",
            "1550",
            "1625"});
			this.cb_WaveLength.Location = new System.Drawing.Point(126, 28);
			this.cb_WaveLength.Name = "cb_WaveLength";
			this.cb_WaveLength.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cb_WaveLength.Size = new System.Drawing.Size(84, 24);
			this.cb_WaveLength.TabIndex = 9;
			this.cb_WaveLength.Text = "1550";
			this.cb_WaveLength.SelectionChangeCommitted += new System.EventHandler(this.cb_WaveLength_SelectionChangeCommitted);
			// 
			// label1
			// 
			this.label1.AllowDrop = true;
			this.label1.BackColor = System.Drawing.SystemColors.Control;
			this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label1.Location = new System.Drawing.Point(14, 68);
			this.label1.Name = "label1";
			this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label1.Size = new System.Drawing.Size(112, 22);
			this.label1.TabIndex = 32;
			this.label1.Text = "Aquisition Mode";
			// 
			// Label2
			// 
			this.Label2.AllowDrop = true;
			this.Label2.BackColor = System.Drawing.SystemColors.Control;
			this.Label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label2.Location = new System.Drawing.Point(14, 29);
			this.Label2.Name = "Label2";
			this.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label2.Size = new System.Drawing.Size(88, 27);
			this.Label2.TabIndex = 34;
			this.Label2.Text = "WaveLength";
			// 
			// cb_AcquisitionMode
			// 
			this.cb_AcquisitionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cb_AcquisitionMode.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cb_AcquisitionMode.FormattingEnabled = true;
			this.cb_AcquisitionMode.Items.AddRange(new object[] {
            "Auto",
            "Manual"});
			this.cb_AcquisitionMode.Location = new System.Drawing.Point(151, 66);
			this.cb_AcquisitionMode.Name = "cb_AcquisitionMode";
			this.cb_AcquisitionMode.Size = new System.Drawing.Size(92, 24);
			this.cb_AcquisitionMode.TabIndex = 14;
			this.cb_AcquisitionMode.SelectedIndexChanged += new System.EventHandler(this.cb_AcquisitionMode_SelectedIndexChanged);
			// 
			// Label5
			// 
			this.Label5.AllowDrop = true;
			this.Label5.BackColor = System.Drawing.SystemColors.Control;
			this.Label5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label5.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label5.Location = new System.Drawing.Point(213, 28);
			this.Label5.Name = "Label5";
			this.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label5.Size = new System.Drawing.Size(31, 27);
			this.Label5.TabIndex = 33;
			this.Label5.Text = "nm";
			// 
			// gb_ManualParameters
			// 
			this.gb_ManualParameters.Controls.Add(this.cb_Resolution);
			this.gb_ManualParameters.Controls.Add(this.cb_Range);
			this.gb_ManualParameters.Controls.Add(this.cb_PulseWidth);
			this.gb_ManualParameters.Controls.Add(this.Label3);
			this.gb_ManualParameters.Controls.Add(this.Label4);
			this.gb_ManualParameters.Controls.Add(this.Label6);
			this.gb_ManualParameters.Controls.Add(this.Label7);
			this.gb_ManualParameters.Controls.Add(this.Label11);
			this.gb_ManualParameters.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gb_ManualParameters.Location = new System.Drawing.Point(12, 93);
			this.gb_ManualParameters.Name = "gb_ManualParameters";
			this.gb_ManualParameters.Size = new System.Drawing.Size(232, 133);
			this.gb_ManualParameters.TabIndex = 1;
			this.gb_ManualParameters.TabStop = false;
			this.gb_ManualParameters.Text = "Manual Parameters";
			// 
			// cb_Resolution
			// 
			this.cb_Resolution.AllowDrop = true;
			this.cb_Resolution.BackColor = System.Drawing.SystemColors.Window;
			this.cb_Resolution.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cb_Resolution.ForeColor = System.Drawing.SystemColors.WindowText;
			this.cb_Resolution.Location = new System.Drawing.Point(114, 57);
			this.cb_Resolution.Name = "cb_Resolution";
			this.cb_Resolution.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cb_Resolution.Size = new System.Drawing.Size(84, 24);
			this.cb_Resolution.TabIndex = 10;
			this.cb_Resolution.Text = "High";
			this.cb_Resolution.SelectionChangeCommitted += new System.EventHandler(this.cb_Resolution_SelectionChangeCommitted);
			// 
			// cb_Range
			// 
			this.cb_Range.AllowDrop = true;
			this.cb_Range.BackColor = System.Drawing.SystemColors.Window;
			this.cb_Range.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cb_Range.ForeColor = System.Drawing.SystemColors.WindowText;
			this.cb_Range.Items.AddRange(new object[] {
            "0.5",
            "1.0",
            "2.5",
            "5.0",
            "10.0",
            "25.0",
            "50.0",
            "100.0",
            "200.0",
            "300.0"});
			this.cb_Range.Location = new System.Drawing.Point(114, 22);
			this.cb_Range.Name = "cb_Range";
			this.cb_Range.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cb_Range.Size = new System.Drawing.Size(84, 24);
			this.cb_Range.TabIndex = 8;
			this.cb_Range.Text = "100.0";
			this.cb_Range.SelectionChangeCommitted += new System.EventHandler(this.cb_Range_SelectionChangeCommitted);
			// 
			// cb_PulseWidth
			// 
			this.cb_PulseWidth.AllowDrop = true;
			this.cb_PulseWidth.BackColor = System.Drawing.SystemColors.Window;
			this.cb_PulseWidth.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cb_PulseWidth.ForeColor = System.Drawing.SystemColors.WindowText;
			this.cb_PulseWidth.Items.AddRange(new object[] {
            "10",
            "20",
            "50",
            "100",
            "4000"});
			this.cb_PulseWidth.Location = new System.Drawing.Point(114, 93);
			this.cb_PulseWidth.Name = "cb_PulseWidth";
			this.cb_PulseWidth.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cb_PulseWidth.Size = new System.Drawing.Size(84, 24);
			this.cb_PulseWidth.TabIndex = 7;
			this.cb_PulseWidth.Text = "4000";
			this.cb_PulseWidth.SelectionChangeCommitted += new System.EventHandler(this.cb_PulseWidth_SelectionChangeCommitted);
			// 
			// Label3
			// 
			this.Label3.AllowDrop = true;
			this.Label3.BackColor = System.Drawing.SystemColors.Control;
			this.Label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label3.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label3.Location = new System.Drawing.Point(14, 23);
			this.Label3.Name = "Label3";
			this.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label3.Size = new System.Drawing.Size(79, 27);
			this.Label3.TabIndex = 30;
			this.Label3.Text = "Range";
			// 
			// Label4
			// 
			this.Label4.AllowDrop = true;
			this.Label4.BackColor = System.Drawing.SystemColors.Control;
			this.Label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label4.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label4.Location = new System.Drawing.Point(14, 96);
			this.Label4.Name = "Label4";
			this.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label4.Size = new System.Drawing.Size(87, 27);
			this.Label4.TabIndex = 29;
			this.Label4.Text = "PulseWidth";
			// 
			// Label6
			// 
			this.Label6.AllowDrop = true;
			this.Label6.BackColor = System.Drawing.SystemColors.Control;
			this.Label6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label6.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label6.Location = new System.Drawing.Point(201, 23);
			this.Label6.Name = "Label6";
			this.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label6.Size = new System.Drawing.Size(27, 27);
			this.Label6.TabIndex = 27;
			this.Label6.Text = "km";
			// 
			// Label7
			// 
			this.Label7.AllowDrop = true;
			this.Label7.BackColor = System.Drawing.SystemColors.Control;
			this.Label7.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label7.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label7.Location = new System.Drawing.Point(201, 93);
			this.Label7.Name = "Label7";
			this.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label7.Size = new System.Drawing.Size(27, 27);
			this.Label7.TabIndex = 26;
			this.Label7.Text = "ns";
			// 
			// Label11
			// 
			this.Label11.AllowDrop = true;
			this.Label11.BackColor = System.Drawing.SystemColors.Control;
			this.Label11.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label11.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label11.Location = new System.Drawing.Point(14, 59);
			this.Label11.Name = "Label11";
			this.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label11.Size = new System.Drawing.Size(87, 27);
			this.Label11.TabIndex = 25;
			this.Label11.Text = "Resolution";
			// 
			// txt_IOR
			// 
			this.txt_IOR.AcceptsReturn = true;
			this.txt_IOR.AllowDrop = true;
			this.txt_IOR.BackColor = System.Drawing.SystemColors.Window;
			this.txt_IOR.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txt_IOR.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txt_IOR.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txt_IOR.Location = new System.Drawing.Point(265, 52);
			this.txt_IOR.MaxLength = 0;
			this.txt_IOR.Name = "txt_IOR";
			this.txt_IOR.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txt_IOR.Size = new System.Drawing.Size(160, 22);
			this.txt_IOR.TabIndex = 6;
			this.txt_IOR.Text = "1.466700";
			this.txt_IOR.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txt_IOR.TextChanged += new System.EventHandler(this.txt_IOR_TextChanged);
			// 
			// txt_BSC
			// 
			this.txt_BSC.AcceptsReturn = true;
			this.txt_BSC.AllowDrop = true;
			this.txt_BSC.BackColor = System.Drawing.SystemColors.Window;
			this.txt_BSC.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txt_BSC.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txt_BSC.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txt_BSC.Location = new System.Drawing.Point(265, 104);
			this.txt_BSC.MaxLength = 0;
			this.txt_BSC.Name = "txt_BSC";
			this.txt_BSC.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txt_BSC.Size = new System.Drawing.Size(160, 22);
			this.txt_BSC.TabIndex = 5;
			this.txt_BSC.Text = "-81.50";
			this.txt_BSC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txt_BSC.TextChanged += new System.EventHandler(this.txt_BSC_TextChanged);
			// 
			// cb_AverageTime
			// 
			this.cb_AverageTime.AllowDrop = true;
			this.cb_AverageTime.BackColor = System.Drawing.SystemColors.Window;
			this.cb_AverageTime.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cb_AverageTime.ForeColor = System.Drawing.SystemColors.WindowText;
			this.cb_AverageTime.Items.AddRange(new object[] {
            "15 sec.",
            "30 sec.",
            "45 sec.",
            "1 min.",
            "3 min."});
			this.cb_AverageTime.Location = new System.Drawing.Point(126, 238);
			this.cb_AverageTime.Name = "cb_AverageTime";
			this.cb_AverageTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cb_AverageTime.Size = new System.Drawing.Size(84, 24);
			this.cb_AverageTime.TabIndex = 4;
			this.cb_AverageTime.Text = "60 sec.";
			this.cb_AverageTime.SelectionChangeCommitted += new System.EventHandler(this.cb_AverageTime_SelectionChangeCommitted);
			// 
			// cb_OTDRSetSave
			// 
			this.cb_OTDRSetSave.AllowDrop = true;
			this.cb_OTDRSetSave.BackColor = System.Drawing.SystemColors.Control;
			this.cb_OTDRSetSave.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cb_OTDRSetSave.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cb_OTDRSetSave.Location = new System.Drawing.Point(265, 167);
			this.cb_OTDRSetSave.Name = "cb_OTDRSetSave";
			this.cb_OTDRSetSave.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cb_OTDRSetSave.Size = new System.Drawing.Size(160, 27);
			this.cb_OTDRSetSave.TabIndex = 3;
			this.cb_OTDRSetSave.Text = "Save";
			this.cb_OTDRSetSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.cb_OTDRSetSave.UseVisualStyleBackColor = false;
			this.cb_OTDRSetSave.Click += new System.EventHandler(this.cb_OTDRSetSave_Click);
			// 
			// cb_OTDRSetClose
			// 
			this.cb_OTDRSetClose.AllowDrop = true;
			this.cb_OTDRSetClose.BackColor = System.Drawing.SystemColors.Control;
			this.cb_OTDRSetClose.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cb_OTDRSetClose.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cb_OTDRSetClose.Location = new System.Drawing.Point(265, 201);
			this.cb_OTDRSetClose.Name = "cb_OTDRSetClose";
			this.cb_OTDRSetClose.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cb_OTDRSetClose.Size = new System.Drawing.Size(160, 27);
			this.cb_OTDRSetClose.TabIndex = 2;
			this.cb_OTDRSetClose.Text = "Close";
			this.cb_OTDRSetClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.cb_OTDRSetClose.UseVisualStyleBackColor = false;
			this.cb_OTDRSetClose.Click += new System.EventHandler(this.cb_OTDRSetClose_Click);
			// 
			// cb_OTDRSetInit
			// 
			this.cb_OTDRSetInit.AllowDrop = true;
			this.cb_OTDRSetInit.BackColor = System.Drawing.SystemColors.Control;
			this.cb_OTDRSetInit.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cb_OTDRSetInit.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cb_OTDRSetInit.Location = new System.Drawing.Point(265, 236);
			this.cb_OTDRSetInit.Name = "cb_OTDRSetInit";
			this.cb_OTDRSetInit.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cb_OTDRSetInit.Size = new System.Drawing.Size(160, 27);
			this.cb_OTDRSetInit.TabIndex = 1;
			this.cb_OTDRSetInit.Text = "Initialization";
			this.cb_OTDRSetInit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.cb_OTDRSetInit.UseVisualStyleBackColor = false;
			this.cb_OTDRSetInit.Click += new System.EventHandler(this.cb_OTDRSetInit_Click);
			// 
			// Label13
			// 
			this.Label13.AllowDrop = true;
			this.Label13.BackColor = System.Drawing.SystemColors.Control;
			this.Label13.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label13.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label13.Location = new System.Drawing.Point(265, 35);
			this.Label13.Name = "Label13";
			this.Label13.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label13.Size = new System.Drawing.Size(165, 18);
			this.Label13.TabIndex = 13;
			this.Label13.Text = "IOR (1.400000 ~ 1.699999)";
			// 
			// Label14
			// 
			this.Label14.AllowDrop = true;
			this.Label14.BackColor = System.Drawing.SystemColors.Control;
			this.Label14.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label14.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label14.Location = new System.Drawing.Point(265, 87);
			this.Label14.Name = "Label14";
			this.Label14.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label14.Size = new System.Drawing.Size(138, 18);
			this.Label14.TabIndex = 12;
			this.Label14.Text = "BSC (-90.00 ~ -40.00)";
			// 
			// Label15
			// 
			this.Label15.AllowDrop = true;
			this.Label15.BackColor = System.Drawing.SystemColors.Control;
			this.Label15.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label15.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label15.Location = new System.Drawing.Point(14, 240);
			this.Label15.Name = "Label15";
			this.Label15.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label15.Size = new System.Drawing.Size(94, 18);
			this.Label15.TabIndex = 11;
			this.Label15.Text = "AverageTime";
			// 
			// OTDRSettingsForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(457, 297);
			this.Controls.Add(this.fr_OTDRSet);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Location = new System.Drawing.Point(4, 30);
			this.Name = "OTDRSettingsForm";
			this.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Text = "OTDR Parameters Settings";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_Closed);
			this.Load += new System.EventHandler(this.Form_Load);
			this.fr_OTDRSet.ResumeLayout(false);
			this.fr_OTDRSet.PerformLayout();
			this.gb_ManualParameters.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		public System.Windows.Forms.TextBox txt_IOR;
		public System.Windows.Forms.TextBox txt_BSC;
		public System.Windows.Forms.ComboBox cb_AverageTime;
		public System.Windows.Forms.Button cb_OTDRSetSave;
		public System.Windows.Forms.Button cb_OTDRSetClose;
		public System.Windows.Forms.Button cb_OTDRSetInit;
		public System.Windows.Forms.Label Label13;
		public System.Windows.Forms.Label Label14;
		public System.Windows.Forms.Label Label15;
		public System.Windows.Forms.GroupBox fr_OTDRSet;
		public System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cb_AcquisitionMode;
		private System.Windows.Forms.GroupBox gb_ManualParameters;
		public System.Windows.Forms.ComboBox cb_Resolution;
		public System.Windows.Forms.ComboBox cb_Range;
		public System.Windows.Forms.ComboBox cb_PulseWidth;
		public System.Windows.Forms.Label Label3;
		public System.Windows.Forms.Label Label4;
		public System.Windows.Forms.Label Label6;
		public System.Windows.Forms.Label Label7;
		public System.Windows.Forms.Label Label11;
		public System.Windows.Forms.ComboBox cb_WaveLength;
		public System.Windows.Forms.Label Label2;
		public System.Windows.Forms.Label Label5;
	}
}
