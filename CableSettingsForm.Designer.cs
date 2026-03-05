
namespace FOMSSubmarine
{
	partial class CableSettingsForm
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

		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CableSettingsForm));
            this.Text2 = new System.Windows.Forms.TextBox();
            this.Frame3 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.txt_BendLimit = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cb_CableSetClose = new System.Windows.Forms.Button();
            this.cb_Save = new System.Windows.Forms.Button();
            this.Frame1 = new System.Windows.Forms.GroupBox();
            this.Text12 = new System.Windows.Forms.TextBox();
            this.txt_MaxLimit = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.Frame4 = new System.Windows.Forms.GroupBox();
            this.txt_ACursorDelta = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_BCursorDelta = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_DummyCable = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_FiberNumMM = new System.Windows.Forms.TextBox();
            this.txt_CableLen = new System.Windows.Forms.TextBox();
            this.Text10 = new System.Windows.Forms.TextBox();
            this.Text3 = new System.Windows.Forms.TextBox();
            this.txt_FiberNumSM = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Frame3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.Frame1.SuspendLayout();
            this.Frame4.SuspendLayout();
            this.SuspendLayout();
            // 
            // Text2
            // 
            this.Text2.AcceptsReturn = true;
            this.Text2.AllowDrop = true;
            this.Text2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Text2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Text2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Text2.Location = new System.Drawing.Point(21, 147);
            this.Text2.MaxLength = 0;
            this.Text2.Name = "Text2";
            this.Text2.ReadOnly = true;
            this.Text2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text2.Size = new System.Drawing.Size(123, 26);
            this.Text2.TabIndex = 15;
            this.Text2.Text = "Fiber # (MM)";
            // 
            // Frame3
            // 
            this.Frame3.AllowDrop = true;
            this.Frame3.BackColor = System.Drawing.SystemColors.Control;
            this.Frame3.Controls.Add(this.groupBox1);
            this.Frame3.Controls.Add(this.cb_CableSetClose);
            this.Frame3.Controls.Add(this.cb_Save);
            this.Frame3.Controls.Add(this.Frame1);
            this.Frame3.Controls.Add(this.Frame4);
            this.Frame3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame3.Location = new System.Drawing.Point(7, 9);
            this.Frame3.Name = "Frame3";
            this.Frame3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame3.Size = new System.Drawing.Size(306, 531);
            this.Frame3.TabIndex = 0;
            this.Frame3.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.AllowDrop = true;
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.txt_BendLimit);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox1.Location = new System.Drawing.Point(8, 398);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox1.Size = new System.Drawing.Size(291, 70);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Bending Limit Threshold";
            // 
            // textBox1
            // 
            this.textBox1.AcceptsReturn = true;
            this.textBox1.AllowDrop = true;
            this.textBox1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBox1.Location = new System.Drawing.Point(7, 26);
            this.textBox1.MaxLength = 0;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox1.Size = new System.Drawing.Size(123, 26);
            this.textBox1.TabIndex = 10;
            this.textBox1.Text = "Limit";
            // 
            // txt_BendLimit
            // 
            this.txt_BendLimit.AcceptsReturn = true;
            this.txt_BendLimit.AllowDrop = true;
            this.txt_BendLimit.BackColor = System.Drawing.SystemColors.Window;
            this.txt_BendLimit.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_BendLimit.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_BendLimit.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txt_BendLimit.Location = new System.Drawing.Point(136, 26);
            this.txt_BendLimit.MaxLength = 0;
            this.txt_BendLimit.Name = "txt_BendLimit";
            this.txt_BendLimit.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txt_BendLimit.Size = new System.Drawing.Size(77, 26);
            this.txt_BendLimit.TabIndex = 9;
            this.txt_BendLimit.Text = "0.25";
            this.txt_BendLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txt_BendLimit.TextChanged += new System.EventHandler(this.txt_BendLimit_TextChanged);
            // 
            // label8
            // 
            this.label8.AllowDrop = true;
            this.label8.BackColor = System.Drawing.SystemColors.Control;
            this.label8.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(218, 26);
            this.label8.Name = "label8";
            this.label8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label8.Size = new System.Drawing.Size(69, 27);
            this.label8.TabIndex = 11;
            this.label8.Text = "[dB/km]";
            // 
            // cb_CableSetClose
            // 
            this.cb_CableSetClose.AllowDrop = true;
            this.cb_CableSetClose.BackColor = System.Drawing.SystemColors.Control;
            this.cb_CableSetClose.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_CableSetClose.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_CableSetClose.Location = new System.Drawing.Point(165, 484);
            this.cb_CableSetClose.Name = "cb_CableSetClose";
            this.cb_CableSetClose.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cb_CableSetClose.Size = new System.Drawing.Size(130, 36);
            this.cb_CableSetClose.TabIndex = 13;
            this.cb_CableSetClose.Text = "CLOSE";
            this.cb_CableSetClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.cb_CableSetClose.UseVisualStyleBackColor = false;
            this.cb_CableSetClose.Click += new System.EventHandler(this.cb_CableSetClose_Click);
            // 
            // cb_Save
            // 
            this.cb_Save.AllowDrop = true;
            this.cb_Save.BackColor = System.Drawing.SystemColors.Control;
            this.cb_Save.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Save.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_Save.Location = new System.Drawing.Point(11, 484);
            this.cb_Save.Name = "cb_Save";
            this.cb_Save.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cb_Save.Size = new System.Drawing.Size(130, 36);
            this.cb_Save.TabIndex = 12;
            this.cb_Save.Text = "SAVE";
            this.cb_Save.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.cb_Save.UseVisualStyleBackColor = false;
            this.cb_Save.Click += new System.EventHandler(this.cb_Save_Click);
            // 
            // Frame1
            // 
            this.Frame1.AllowDrop = true;
            this.Frame1.BackColor = System.Drawing.SystemColors.Control;
            this.Frame1.Controls.Add(this.Text12);
            this.Frame1.Controls.Add(this.txt_MaxLimit);
            this.Frame1.Controls.Add(this.Label5);
            this.Frame1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Frame1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame1.Location = new System.Drawing.Point(7, 315);
            this.Frame1.Name = "Frame1";
            this.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame1.Size = new System.Drawing.Size(291, 70);
            this.Frame1.TabIndex = 8;
            this.Frame1.TabStop = false;
            this.Frame1.Text = "Section Loss Alarm Threshold";
            // 
            // Text12
            // 
            this.Text12.AcceptsReturn = true;
            this.Text12.AllowDrop = true;
            this.Text12.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Text12.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Text12.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text12.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Text12.Location = new System.Drawing.Point(7, 26);
            this.Text12.MaxLength = 0;
            this.Text12.Name = "Text12";
            this.Text12.ReadOnly = true;
            this.Text12.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text12.Size = new System.Drawing.Size(123, 26);
            this.Text12.TabIndex = 10;
            this.Text12.Text = "Maximum Limit";
            // 
            // txt_MaxLimit
            // 
            this.txt_MaxLimit.AcceptsReturn = true;
            this.txt_MaxLimit.AllowDrop = true;
            this.txt_MaxLimit.BackColor = System.Drawing.SystemColors.Window;
            this.txt_MaxLimit.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_MaxLimit.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_MaxLimit.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txt_MaxLimit.Location = new System.Drawing.Point(136, 26);
            this.txt_MaxLimit.MaxLength = 0;
            this.txt_MaxLimit.Name = "txt_MaxLimit";
            this.txt_MaxLimit.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txt_MaxLimit.Size = new System.Drawing.Size(77, 26);
            this.txt_MaxLimit.TabIndex = 9;
            this.txt_MaxLimit.Text = "0.25";
            this.txt_MaxLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txt_MaxLimit.TextChanged += new System.EventHandler(this.txt_MaxLimit_TextChanged);
            // 
            // Label5
            // 
            this.Label5.AllowDrop = true;
            this.Label5.BackColor = System.Drawing.SystemColors.Control;
            this.Label5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label5.Location = new System.Drawing.Point(218, 26);
            this.Label5.Name = "Label5";
            this.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label5.Size = new System.Drawing.Size(69, 27);
            this.Label5.TabIndex = 11;
            this.Label5.Text = "[dB/km]";
            // 
            // Frame4
            // 
            this.Frame4.AllowDrop = true;
            this.Frame4.BackColor = System.Drawing.SystemColors.Control;
            this.Frame4.Controls.Add(this.txt_ACursorDelta);
            this.Frame4.Controls.Add(this.textBox5);
            this.Frame4.Controls.Add(this.label7);
            this.Frame4.Controls.Add(this.txt_BCursorDelta);
            this.Frame4.Controls.Add(this.textBox3);
            this.Frame4.Controls.Add(this.label3);
            this.Frame4.Controls.Add(this.txt_DummyCable);
            this.Frame4.Controls.Add(this.textBox2);
            this.Frame4.Controls.Add(this.label1);
            this.Frame4.Controls.Add(this.txt_FiberNumMM);
            this.Frame4.Controls.Add(this.txt_CableLen);
            this.Frame4.Controls.Add(this.Text10);
            this.Frame4.Controls.Add(this.Text3);
            this.Frame4.Controls.Add(this.txt_FiberNumSM);
            this.Frame4.Controls.Add(this.Label2);
            this.Frame4.Controls.Add(this.Label6);
            this.Frame4.Controls.Add(this.Label4);
            this.Frame4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Frame4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame4.Location = new System.Drawing.Point(7, 17);
            this.Frame4.Name = "Frame4";
            this.Frame4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame4.Size = new System.Drawing.Size(291, 291);
            this.Frame4.TabIndex = 1;
            this.Frame4.TabStop = false;
            this.Frame4.Text = "Cable Information";
            // 
            // txt_ACursorDelta
            // 
            this.txt_ACursorDelta.AcceptsReturn = true;
            this.txt_ACursorDelta.AllowDrop = true;
            this.txt_ACursorDelta.BackColor = System.Drawing.SystemColors.Window;
            this.txt_ACursorDelta.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_ACursorDelta.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_ACursorDelta.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txt_ACursorDelta.Location = new System.Drawing.Point(136, 203);
            this.txt_ACursorDelta.MaxLength = 0;
            this.txt_ACursorDelta.Name = "txt_ACursorDelta";
            this.txt_ACursorDelta.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txt_ACursorDelta.Size = new System.Drawing.Size(77, 26);
            this.txt_ACursorDelta.TabIndex = 24;
            this.txt_ACursorDelta.Text = "0.5";
            this.txt_ACursorDelta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txt_ACursorDelta.TextChanged += new System.EventHandler(this.txt_ACursorDelta_TextChanged);
            // 
            // textBox5
            // 
            this.textBox5.AcceptsReturn = true;
            this.textBox5.AllowDrop = true;
            this.textBox5.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.textBox5.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox5.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBox5.Location = new System.Drawing.Point(7, 203);
            this.textBox5.MaxLength = 0;
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox5.Size = new System.Drawing.Size(123, 26);
            this.textBox5.TabIndex = 23;
            this.textBox5.Text = "Cursor A";
            // 
            // label7
            // 
            this.label7.AllowDrop = true;
            this.label7.BackColor = System.Drawing.SystemColors.Control;
            this.label7.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(218, 203);
            this.label7.Name = "label7";
            this.label7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label7.Size = new System.Drawing.Size(56, 27);
            this.label7.TabIndex = 25;
            this.label7.Text = "[km]";
            // 
            // txt_BCursorDelta
            // 
            this.txt_BCursorDelta.AcceptsReturn = true;
            this.txt_BCursorDelta.AllowDrop = true;
            this.txt_BCursorDelta.BackColor = System.Drawing.SystemColors.Window;
            this.txt_BCursorDelta.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_BCursorDelta.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_BCursorDelta.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txt_BCursorDelta.Location = new System.Drawing.Point(136, 246);
            this.txt_BCursorDelta.MaxLength = 0;
            this.txt_BCursorDelta.Name = "txt_BCursorDelta";
            this.txt_BCursorDelta.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txt_BCursorDelta.Size = new System.Drawing.Size(77, 26);
            this.txt_BCursorDelta.TabIndex = 21;
            this.txt_BCursorDelta.Text = "0.5";
            this.txt_BCursorDelta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txt_BCursorDelta.TextChanged += new System.EventHandler(this.txt_BCursorDelta_TextChanged);
            // 
            // textBox3
            // 
            this.textBox3.AcceptsReturn = true;
            this.textBox3.AllowDrop = true;
            this.textBox3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.textBox3.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBox3.Location = new System.Drawing.Point(7, 246);
            this.textBox3.MaxLength = 0;
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox3.Size = new System.Drawing.Size(123, 26);
            this.textBox3.TabIndex = 20;
            this.textBox3.Text = "Cursor B";
            // 
            // label3
            // 
            this.label3.AllowDrop = true;
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(218, 246);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label3.Size = new System.Drawing.Size(56, 27);
            this.label3.TabIndex = 22;
            this.label3.Text = "[km]";
            // 
            // txt_DummyCable
            // 
            this.txt_DummyCable.AcceptsReturn = true;
            this.txt_DummyCable.AllowDrop = true;
            this.txt_DummyCable.BackColor = System.Drawing.SystemColors.Window;
            this.txt_DummyCable.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_DummyCable.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_DummyCable.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txt_DummyCable.Location = new System.Drawing.Point(136, 160);
            this.txt_DummyCable.MaxLength = 0;
            this.txt_DummyCable.Name = "txt_DummyCable";
            this.txt_DummyCable.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txt_DummyCable.Size = new System.Drawing.Size(77, 26);
            this.txt_DummyCable.TabIndex = 18;
            this.txt_DummyCable.Text = "2600";
            this.txt_DummyCable.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txt_DummyCable.TextChanged += new System.EventHandler(this.txt_DummyCable_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.AcceptsReturn = true;
            this.textBox2.AllowDrop = true;
            this.textBox2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.textBox2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBox2.Location = new System.Drawing.Point(7, 160);
            this.textBox2.MaxLength = 0;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox2.Size = new System.Drawing.Size(123, 26);
            this.textBox2.TabIndex = 17;
            this.textBox2.Text = "Dummy Cable";
            // 
            // label1
            // 
            this.label1.AllowDrop = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(218, 160);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(56, 27);
            this.label1.TabIndex = 19;
            this.label1.Text = "[m]";
            // 
            // txt_FiberNumMM
            // 
            this.txt_FiberNumMM.AcceptsReturn = true;
            this.txt_FiberNumMM.AllowDrop = true;
            this.txt_FiberNumMM.BackColor = System.Drawing.SystemColors.Window;
            this.txt_FiberNumMM.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_FiberNumMM.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_FiberNumMM.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txt_FiberNumMM.Location = new System.Drawing.Point(136, 117);
            this.txt_FiberNumMM.MaxLength = 0;
            this.txt_FiberNumMM.Name = "txt_FiberNumMM";
            this.txt_FiberNumMM.ReadOnly = true;
            this.txt_FiberNumMM.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txt_FiberNumMM.Size = new System.Drawing.Size(77, 26);
            this.txt_FiberNumMM.TabIndex = 14;
            this.txt_FiberNumMM.Text = "0";
            this.txt_FiberNumMM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txt_FiberNumMM.TextChanged += new System.EventHandler(this.txt_FiberNumMM_TextChanged);
            // 
            // txt_CableLen
            // 
            this.txt_CableLen.AcceptsReturn = true;
            this.txt_CableLen.AllowDrop = true;
            this.txt_CableLen.BackColor = System.Drawing.SystemColors.Window;
            this.txt_CableLen.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_CableLen.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_CableLen.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txt_CableLen.Location = new System.Drawing.Point(136, 31);
            this.txt_CableLen.MaxLength = 0;
            this.txt_CableLen.Name = "txt_CableLen";
            this.txt_CableLen.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txt_CableLen.Size = new System.Drawing.Size(77, 26);
            this.txt_CableLen.TabIndex = 5;
            this.txt_CableLen.Text = "52";
            this.txt_CableLen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txt_CableLen.TextChanged += new System.EventHandler(this.txt_CableLen_TextChanged);
            // 
            // Text10
            // 
            this.Text10.AcceptsReturn = true;
            this.Text10.AllowDrop = true;
            this.Text10.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Text10.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Text10.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text10.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Text10.Location = new System.Drawing.Point(7, 31);
            this.Text10.MaxLength = 0;
            this.Text10.Name = "Text10";
            this.Text10.ReadOnly = true;
            this.Text10.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text10.Size = new System.Drawing.Size(123, 26);
            this.Text10.TabIndex = 4;
            this.Text10.Text = "Cable Length";
            // 
            // Text3
            // 
            this.Text3.AcceptsReturn = true;
            this.Text3.AllowDrop = true;
            this.Text3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Text3.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Text3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Text3.Location = new System.Drawing.Point(7, 74);
            this.Text3.MaxLength = 0;
            this.Text3.Name = "Text3";
            this.Text3.ReadOnly = true;
            this.Text3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text3.Size = new System.Drawing.Size(123, 26);
            this.Text3.TabIndex = 3;
            this.Text3.Text = "Fiber # (SM)";
            // 
            // txt_FiberNumSM
            // 
            this.txt_FiberNumSM.AcceptsReturn = true;
            this.txt_FiberNumSM.AllowDrop = true;
            this.txt_FiberNumSM.BackColor = System.Drawing.SystemColors.Window;
            this.txt_FiberNumSM.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_FiberNumSM.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_FiberNumSM.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txt_FiberNumSM.Location = new System.Drawing.Point(136, 74);
            this.txt_FiberNumSM.MaxLength = 0;
            this.txt_FiberNumSM.Name = "txt_FiberNumSM";
            this.txt_FiberNumSM.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txt_FiberNumSM.Size = new System.Drawing.Size(77, 26);
            this.txt_FiberNumSM.TabIndex = 2;
            this.txt_FiberNumSM.Text = "12";
            this.txt_FiberNumSM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txt_FiberNumSM.TextChanged += new System.EventHandler(this.txt_FiberNumSM_TextChanged);
            // 
            // Label2
            // 
            this.Label2.AllowDrop = true;
            this.Label2.BackColor = System.Drawing.SystemColors.Control;
            this.Label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label2.Location = new System.Drawing.Point(218, 117);
            this.Label2.Name = "Label2";
            this.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label2.Size = new System.Drawing.Size(56, 27);
            this.Label2.TabIndex = 16;
            this.Label2.Text = "[EA]";
            // 
            // Label6
            // 
            this.Label6.AllowDrop = true;
            this.Label6.BackColor = System.Drawing.SystemColors.Control;
            this.Label6.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label6.Location = new System.Drawing.Point(218, 74);
            this.Label6.Name = "Label6";
            this.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label6.Size = new System.Drawing.Size(56, 27);
            this.Label6.TabIndex = 7;
            this.Label6.Text = "[EA]";
            // 
            // Label4
            // 
            this.Label4.AllowDrop = true;
            this.Label4.BackColor = System.Drawing.SystemColors.Control;
            this.Label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label4.Location = new System.Drawing.Point(218, 31);
            this.Label4.Name = "Label4";
            this.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label4.Size = new System.Drawing.Size(56, 27);
            this.Label4.TabIndex = 6;
            this.Label4.Text = "[km]";
            // 
            // CableSettingsForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(321, 552);
            this.Controls.Add(this.Text2);
            this.Controls.Add(this.Frame3);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(4, 30);
            this.Name = "CableSettingsForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Cable Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_Closed);
            this.Load += new System.EventHandler(this.Form_Load);
            this.Frame3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.Frame1.ResumeLayout(false);
            this.Frame1.PerformLayout();
            this.Frame4.ResumeLayout(false);
            this.Frame4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.TextBox Text2;
		public System.Windows.Forms.Button cb_CableSetClose;
		public System.Windows.Forms.Button cb_Save;
		public System.Windows.Forms.TextBox Text12;
		public System.Windows.Forms.TextBox txt_MaxLimit;
		public System.Windows.Forms.Label Label5;
		public System.Windows.Forms.GroupBox Frame1;
		public System.Windows.Forms.TextBox txt_FiberNumMM;
		public System.Windows.Forms.TextBox txt_CableLen;
		public System.Windows.Forms.TextBox Text10;
		public System.Windows.Forms.TextBox Text3;
		public System.Windows.Forms.TextBox txt_FiberNumSM;
		public System.Windows.Forms.Label Label2;
		public System.Windows.Forms.Label Label6;
		public System.Windows.Forms.Label Label4;
		public System.Windows.Forms.GroupBox Frame4;
		public System.Windows.Forms.GroupBox Frame3;
		public System.Windows.Forms.TextBox txt_DummyCable;
		public System.Windows.Forms.TextBox textBox2;
		public System.Windows.Forms.Label label1;
		public System.Windows.Forms.TextBox txt_ACursorDelta;
		public System.Windows.Forms.TextBox textBox5;
		public System.Windows.Forms.Label label7;
		public System.Windows.Forms.TextBox txt_BCursorDelta;
		public System.Windows.Forms.TextBox textBox3;
		public System.Windows.Forms.Label label3;
        public System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.TextBox txt_BendLimit;
        public System.Windows.Forms.Label label8;
    }
}
