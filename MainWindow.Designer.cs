
namespace FOMSSubmarine
{
	partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.MainMenu1 = new System.Windows.Forms.MenuStrip();
            this.m_File = new System.Windows.Forms.ToolStripMenuItem();
            this.m_CloseProgram = new System.Windows.Forms.ToolStripMenuItem();
            this.m_Setting = new System.Windows.Forms.ToolStripMenuItem();
            this.m_CableInfoSet = new System.Windows.Forms.ToolStripMenuItem();
            this.m_OTDRParamSet = new System.Windows.Forms.ToolStripMenuItem();
            this.m_EquipmentsSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.m_EmailSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.m_Tool = new System.Windows.Forms.ToolStripMenuItem();
            this.m_SingleCtrl = new System.Windows.Forms.ToolStripMenuItem();
            this.Frame1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.WebBrowser = new System.Windows.Forms.WebBrowser();
            this.VNCViewer = new VncSharp.RemoteDesktop();
            this.Frame5 = new System.Windows.Forms.GroupBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.Frame3 = new System.Windows.Forms.GroupBox();
            this.txt_SectionLoss = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.Frame2 = new System.Windows.Forms.GroupBox();
            this.txt_ChNo = new System.Windows.Forms.TextBox();
            this.Frame7 = new System.Windows.Forms.GroupBox();
            this.txt_Messages = new System.Windows.Forms.TextBox();
            this.Frame4 = new System.Windows.Forms.GroupBox();
            this.cb_Close = new System.Windows.Forms.Button();
            this.cb_MeasureStop = new System.Windows.Forms.Button();
            this.cb_MeasureStart = new System.Windows.Forms.Button();
            this.fr_LANCon = new System.Windows.Forms.GroupBox();
            this.cb_OSWConnect = new System.Windows.Forms.Button();
            this.cbox_PortName = new System.Windows.Forms.ComboBox();
            this.cb_OTDRConnect = new System.Windows.Forms.Button();
            this.txt_IPaddress = new System.Windows.Forms.TextBox();
            this.LblIO = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.MainMenu1.SuspendLayout();
            this.Frame1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.Frame5.SuspendLayout();
            this.Frame3.SuspendLayout();
            this.Frame2.SuspendLayout();
            this.Frame7.SuspendLayout();
            this.Frame4.SuspendLayout();
            this.fr_LANCon.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu1
            // 
            this.MainMenu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_File,
            this.m_Setting,
            this.m_Tool});
            this.MainMenu1.Location = new System.Drawing.Point(0, 0);
            this.MainMenu1.Name = "MainMenu1";
            this.MainMenu1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.MainMenu1.Size = new System.Drawing.Size(1124, 24);
            this.MainMenu1.TabIndex = 1;
            this.MainMenu1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.MainMenu1_ItemClicked);
            // 
            // m_File
            // 
            this.m_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_CloseProgram});
            this.m_File.Name = "m_File";
            this.m_File.Size = new System.Drawing.Size(69, 20);
            this.m_File.Text = "파일(&File)";
            // 
            // m_CloseProgram
            // 
            this.m_CloseProgram.Name = "m_CloseProgram";
            this.m_CloseProgram.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.m_CloseProgram.Size = new System.Drawing.Size(182, 22);
            this.m_CloseProgram.Text = "끝내기 (&Exit)";
            this.m_CloseProgram.Click += new System.EventHandler(this.m_CloseProgram_Click);
            // 
            // m_Setting
            // 
            this.m_Setting.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_CableInfoSet,
            this.m_OTDRParamSet,
            this.m_EquipmentsSettings,
            this.m_EmailSettings});
            this.m_Setting.Name = "m_Setting";
            this.m_Setting.Size = new System.Drawing.Size(89, 20);
            this.m_Setting.Text = "세팅(&Setting)";
            // 
            // m_CableInfoSet
            // 
            this.m_CableInfoSet.Name = "m_CableInfoSet";
            this.m_CableInfoSet.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.m_CableInfoSet.Size = new System.Drawing.Size(381, 22);
            this.m_CableInfoSet.Text = "케이블 정보 세팅 (&Cable Information Settings)";
            this.m_CableInfoSet.Click += new System.EventHandler(this.m_CableInfoSet_Click);
            // 
            // m_OTDRParamSet
            // 
            this.m_OTDRParamSet.Name = "m_OTDRParamSet";
            this.m_OTDRParamSet.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.m_OTDRParamSet.Size = new System.Drawing.Size(381, 22);
            this.m_OTDRParamSet.Text = "OTDR 파라미터 세팅 (&OTDR Parameters Settings)";
            this.m_OTDRParamSet.Click += new System.EventHandler(this.m_OTDRParamSet_Click);
            // 
            // m_EquipmentsSettings
            // 
            this.m_EquipmentsSettings.Name = "m_EquipmentsSettings";
            this.m_EquipmentsSettings.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.m_EquipmentsSettings.Size = new System.Drawing.Size(381, 22);
            this.m_EquipmentsSettings.Text = "장비 설정 (&Equipments Settings)";
            this.m_EquipmentsSettings.Click += new System.EventHandler(this.m_EquipmentsSettings_Click);
            // 
            // m_EmailSettings
            // 
            this.m_EmailSettings.Name = "m_EmailSettings";
            this.m_EmailSettings.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.m_EmailSettings.Size = new System.Drawing.Size(381, 22);
            this.m_EmailSettings.Text = "Email 설정 (&Email Settings)";
            this.m_EmailSettings.Click += new System.EventHandler(this.m_EmailSettings_Click);
            // 
            // m_Tool
            // 
            this.m_Tool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_SingleCtrl});
            this.m_Tool.Name = "m_Tool";
            this.m_Tool.Size = new System.Drawing.Size(74, 20);
            this.m_Tool.Text = "도구(&Tool)";
            // 
            // m_SingleCtrl
            // 
            this.m_SingleCtrl.Name = "m_SingleCtrl";
            this.m_SingleCtrl.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.m_SingleCtrl.Size = new System.Drawing.Size(281, 22);
            this.m_SingleCtrl.Text = "개별 측정기 (&Single Controller)";
            this.m_SingleCtrl.Click += new System.EventHandler(this.m_SingleCtrl_Click);
            // 
            // Frame1
            // 
            this.Frame1.AllowDrop = true;
            this.Frame1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Frame1.BackColor = System.Drawing.SystemColors.Control;
            this.Frame1.Controls.Add(this.panel1);
            this.Frame1.Controls.Add(this.Frame5);
            this.Frame1.Controls.Add(this.Frame3);
            this.Frame1.Controls.Add(this.Frame2);
            this.Frame1.Controls.Add(this.Frame7);
            this.Frame1.Controls.Add(this.Frame4);
            this.Frame1.Controls.Add(this.fr_LANCon);
            this.Frame1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Frame1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame1.Location = new System.Drawing.Point(7, 26);
            this.Frame1.Name = "Frame1";
            this.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame1.Size = new System.Drawing.Size(1110, 838);
            this.Frame1.TabIndex = 0;
            this.Frame1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.WebBrowser);
            this.panel1.Controls.Add(this.VNCViewer);
            this.panel1.Location = new System.Drawing.Point(10, 190);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1100, 648);
            this.panel1.TabIndex = 28;
            // 
            // WebBrowser
            // 
            this.WebBrowser.AllowWebBrowserDrop = false;
            this.WebBrowser.Location = new System.Drawing.Point(450, 16);
            this.WebBrowser.Name = "WebBrowser";
            this.WebBrowser.Size = new System.Drawing.Size(374, 361);
            this.WebBrowser.TabIndex = 16;
            this.WebBrowser.Url = new System.Uri("http://daum.net", System.UriKind.Absolute);
            // 
            // VNCViewer
            // 
            this.VNCViewer.AutoScroll = true;
            this.VNCViewer.AutoScrollMinSize = new System.Drawing.Size(608, 427);
            this.VNCViewer.BackColor = System.Drawing.Color.Black;
            this.VNCViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VNCViewer.Location = new System.Drawing.Point(0, 0);
            this.VNCViewer.Name = "VNCViewer";
            this.VNCViewer.Size = new System.Drawing.Size(1100, 648);
            this.VNCViewer.TabIndex = 28;
            this.VNCViewer.Visible = false;
            // 
            // Frame5
            // 
            this.Frame5.AllowDrop = true;
            this.Frame5.BackColor = System.Drawing.SystemColors.Control;
            this.Frame5.Controls.Add(this.Label1);
            this.Frame5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Frame5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame5.Location = new System.Drawing.Point(480, 17);
            this.Frame5.Name = "Frame5";
            this.Frame5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame5.Size = new System.Drawing.Size(117, 88);
            this.Frame5.TabIndex = 22;
            this.Frame5.TabStop = false;
            // 
            // Label1
            // 
            this.Label1.AllowDrop = true;
            this.Label1.BackColor = System.Drawing.SystemColors.Control;
            this.Label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label1.Location = new System.Drawing.Point(55, 47);
            this.Label1.Name = "Label1";
            this.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label1.Size = new System.Drawing.Size(48, 18);
            this.Label1.TabIndex = 24;
            // 
            // Frame3
            // 
            this.Frame3.AllowDrop = true;
            this.Frame3.BackColor = System.Drawing.SystemColors.Control;
            this.Frame3.Controls.Add(this.txt_SectionLoss);
            this.Frame3.Controls.Add(this.Label3);
            this.Frame3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Frame3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame3.Location = new System.Drawing.Point(335, 104);
            this.Frame3.Name = "Frame3";
            this.Frame3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame3.Size = new System.Drawing.Size(173, 81);
            this.Frame3.TabIndex = 19;
            this.Frame3.TabStop = false;
            this.Frame3.Text = "Attenuation";
            // 
            // txt_SectionLoss
            // 
            this.txt_SectionLoss.AcceptsReturn = true;
            this.txt_SectionLoss.AllowDrop = true;
            this.txt_SectionLoss.BackColor = System.Drawing.SystemColors.Window;
            this.txt_SectionLoss.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_SectionLoss.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_SectionLoss.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txt_SectionLoss.Location = new System.Drawing.Point(7, 33);
            this.txt_SectionLoss.MaxLength = 0;
            this.txt_SectionLoss.Name = "txt_SectionLoss";
            this.txt_SectionLoss.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txt_SectionLoss.Size = new System.Drawing.Size(97, 27);
            this.txt_SectionLoss.TabIndex = 20;
            this.txt_SectionLoss.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Label3
            // 
            this.Label3.AllowDrop = true;
            this.Label3.BackColor = System.Drawing.SystemColors.Control;
            this.Label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label3.Location = new System.Drawing.Point(108, 38);
            this.Label3.Name = "Label3";
            this.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label3.Size = new System.Drawing.Size(59, 18);
            this.Label3.TabIndex = 21;
            this.Label3.Text = "dB/km";
            // 
            // Frame2
            // 
            this.Frame2.AllowDrop = true;
            this.Frame2.BackColor = System.Drawing.SystemColors.Control;
            this.Frame2.Controls.Add(this.txt_ChNo);
            this.Frame2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Frame2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame2.Location = new System.Drawing.Point(514, 104);
            this.Frame2.Name = "Frame2";
            this.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame2.Size = new System.Drawing.Size(83, 81);
            this.Frame2.TabIndex = 17;
            this.Frame2.TabStop = false;
            this.Frame2.Text = "CH No.";
            // 
            // txt_ChNo
            // 
            this.txt_ChNo.AcceptsReturn = true;
            this.txt_ChNo.AllowDrop = true;
            this.txt_ChNo.BackColor = System.Drawing.SystemColors.Window;
            this.txt_ChNo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_ChNo.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_ChNo.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txt_ChNo.Location = new System.Drawing.Point(6, 33);
            this.txt_ChNo.MaxLength = 0;
            this.txt_ChNo.Name = "txt_ChNo";
            this.txt_ChNo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txt_ChNo.Size = new System.Drawing.Size(63, 27);
            this.txt_ChNo.TabIndex = 18;
            this.txt_ChNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Frame7
            // 
            this.Frame7.AllowDrop = true;
            this.Frame7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Frame7.BackColor = System.Drawing.SystemColors.Control;
            this.Frame7.Controls.Add(this.txt_Messages);
            this.Frame7.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Frame7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame7.Location = new System.Drawing.Point(602, 17);
            this.Frame7.Name = "Frame7";
            this.Frame7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame7.Size = new System.Drawing.Size(500, 168);
            this.Frame7.TabIndex = 14;
            this.Frame7.TabStop = false;
            this.Frame7.Text = "Messages";
            // 
            // txt_Messages
            // 
            this.txt_Messages.AcceptsReturn = true;
            this.txt_Messages.AllowDrop = true;
            this.txt_Messages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Messages.BackColor = System.Drawing.SystemColors.Window;
            this.txt_Messages.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_Messages.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Messages.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txt_Messages.Location = new System.Drawing.Point(8, 23);
            this.txt_Messages.MaxLength = 0;
            this.txt_Messages.Multiline = true;
            this.txt_Messages.Name = "txt_Messages";
            this.txt_Messages.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txt_Messages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_Messages.Size = new System.Drawing.Size(484, 136);
            this.txt_Messages.TabIndex = 15;
            this.txt_Messages.TextChanged += new System.EventHandler(this.txt_Messages_TextChanged);
            // 
            // Frame4
            // 
            this.Frame4.AllowDrop = true;
            this.Frame4.BackColor = System.Drawing.SystemColors.Control;
            this.Frame4.Controls.Add(this.cb_Close);
            this.Frame4.Controls.Add(this.cb_MeasureStop);
            this.Frame4.Controls.Add(this.cb_MeasureStart);
            this.Frame4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Frame4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame4.Location = new System.Drawing.Point(7, 104);
            this.Frame4.Name = "Frame4";
            this.Frame4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame4.Size = new System.Drawing.Size(321, 81);
            this.Frame4.TabIndex = 7;
            this.Frame4.TabStop = false;
            this.Frame4.Text = "Measurement";
            // 
            // cb_Close
            // 
            this.cb_Close.AllowDrop = true;
            this.cb_Close.BackColor = System.Drawing.SystemColors.Control;
            this.cb_Close.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Close.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_Close.Location = new System.Drawing.Point(225, 26);
            this.cb_Close.Name = "cb_Close";
            this.cb_Close.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cb_Close.Size = new System.Drawing.Size(84, 36);
            this.cb_Close.TabIndex = 13;
            this.cb_Close.Text = "CLOSE";
            this.cb_Close.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.cb_Close.UseVisualStyleBackColor = false;
            this.cb_Close.Click += new System.EventHandler(this.cb_Close_Click);
            // 
            // cb_MeasureStop
            // 
            this.cb_MeasureStop.AllowDrop = true;
            this.cb_MeasureStop.BackColor = System.Drawing.SystemColors.Control;
            this.cb_MeasureStop.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_MeasureStop.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_MeasureStop.Location = new System.Drawing.Point(123, 26);
            this.cb_MeasureStop.Name = "cb_MeasureStop";
            this.cb_MeasureStop.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cb_MeasureStop.Size = new System.Drawing.Size(96, 36);
            this.cb_MeasureStop.TabIndex = 12;
            this.cb_MeasureStop.Text = "STOP";
            this.cb_MeasureStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.cb_MeasureStop.UseVisualStyleBackColor = false;
            this.cb_MeasureStop.Click += new System.EventHandler(this.cb_MeasureStop_Click);
            // 
            // cb_MeasureStart
            // 
            this.cb_MeasureStart.AllowDrop = true;
            this.cb_MeasureStart.BackColor = System.Drawing.SystemColors.Control;
            this.cb_MeasureStart.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_MeasureStart.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_MeasureStart.Location = new System.Drawing.Point(14, 26);
            this.cb_MeasureStart.Name = "cb_MeasureStart";
            this.cb_MeasureStart.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cb_MeasureStart.Size = new System.Drawing.Size(104, 36);
            this.cb_MeasureStart.TabIndex = 11;
            this.cb_MeasureStart.Text = "START";
            this.cb_MeasureStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.cb_MeasureStart.UseVisualStyleBackColor = false;
            this.cb_MeasureStart.Click += new System.EventHandler(this.cb_MeasureStart_Click);
            // 
            // fr_LANCon
            // 
            this.fr_LANCon.AllowDrop = true;
            this.fr_LANCon.BackColor = System.Drawing.SystemColors.Control;
            this.fr_LANCon.Controls.Add(this.cb_OSWConnect);
            this.fr_LANCon.Controls.Add(this.cbox_PortName);
            this.fr_LANCon.Controls.Add(this.cb_OTDRConnect);
            this.fr_LANCon.Controls.Add(this.txt_IPaddress);
            this.fr_LANCon.Controls.Add(this.LblIO);
            this.fr_LANCon.Controls.Add(this.Label2);
            this.fr_LANCon.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fr_LANCon.ForeColor = System.Drawing.SystemColors.ControlText;
            this.fr_LANCon.Location = new System.Drawing.Point(7, 17);
            this.fr_LANCon.Name = "fr_LANCon";
            this.fr_LANCon.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.fr_LANCon.Size = new System.Drawing.Size(467, 88);
            this.fr_LANCon.TabIndex = 1;
            this.fr_LANCon.TabStop = false;
            this.fr_LANCon.Text = "Instruments Connection";
            // 
            // cb_OSWConnect
            // 
            this.cb_OSWConnect.AllowDrop = true;
            this.cb_OSWConnect.BackColor = System.Drawing.SystemColors.Control;
            this.cb_OSWConnect.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_OSWConnect.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_OSWConnect.Location = new System.Drawing.Point(338, 41);
            this.cb_OSWConnect.Name = "cb_OSWConnect";
            this.cb_OSWConnect.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cb_OSWConnect.Size = new System.Drawing.Size(96, 32);
            this.cb_OSWConnect.TabIndex = 9;
            this.cb_OSWConnect.Text = "Connect";
            this.cb_OSWConnect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.cb_OSWConnect.UseVisualStyleBackColor = false;
            this.cb_OSWConnect.Click += new System.EventHandler(this.cb_OSWConnect_Click);
            // 
            // cbox_PortName
            // 
            this.cbox_PortName.AllowDrop = true;
            this.cbox_PortName.BackColor = System.Drawing.SystemColors.Window;
            this.cbox_PortName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbox_PortName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cbox_PortName.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9"});
            this.cbox_PortName.Location = new System.Drawing.Point(252, 43);
            this.cbox_PortName.Name = "cbox_PortName";
            this.cbox_PortName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cbox_PortName.Size = new System.Drawing.Size(79, 24);
            this.cbox_PortName.TabIndex = 8;
            this.cbox_PortName.Text = "COM3";
            this.cbox_PortName.SelectionChangeCommitted += new System.EventHandler(this.cbox_PortName_SelectionChangeCommitted);
            // 
            // cb_OTDRConnect
            // 
            this.cb_OTDRConnect.AllowDrop = true;
            this.cb_OTDRConnect.BackColor = System.Drawing.SystemColors.Control;
            this.cb_OTDRConnect.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_OTDRConnect.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_OTDRConnect.Location = new System.Drawing.Point(126, 41);
            this.cb_OTDRConnect.Name = "cb_OTDRConnect";
            this.cb_OTDRConnect.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cb_OTDRConnect.Size = new System.Drawing.Size(96, 32);
            this.cb_OTDRConnect.TabIndex = 3;
            this.cb_OTDRConnect.Text = "Connect";
            this.cb_OTDRConnect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.cb_OTDRConnect.UseVisualStyleBackColor = false;
            this.cb_OTDRConnect.Click += new System.EventHandler(this.cb_OTDRConnect_Click);
            // 
            // txt_IPaddress
            // 
            this.txt_IPaddress.AcceptsReturn = true;
            this.txt_IPaddress.AllowDrop = true;
            this.txt_IPaddress.BackColor = System.Drawing.SystemColors.Window;
            this.txt_IPaddress.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_IPaddress.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_IPaddress.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txt_IPaddress.Location = new System.Drawing.Point(14, 43);
            this.txt_IPaddress.MaxLength = 0;
            this.txt_IPaddress.Name = "txt_IPaddress";
            this.txt_IPaddress.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txt_IPaddress.Size = new System.Drawing.Size(104, 22);
            this.txt_IPaddress.TabIndex = 2;
            this.txt_IPaddress.Text = "192.168.1.2";
            // 
            // LblIO
            // 
            this.LblIO.AllowDrop = true;
            this.LblIO.BackColor = System.Drawing.SystemColors.Control;
            this.LblIO.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblIO.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LblIO.Location = new System.Drawing.Point(14, 24);
            this.LblIO.Name = "LblIO";
            this.LblIO.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LblIO.Size = new System.Drawing.Size(181, 18);
            this.LblIO.TabIndex = 6;
            this.LblIO.Text = "OTDR Connect";
            // 
            // Label2
            // 
            this.Label2.AllowDrop = true;
            this.Label2.BackColor = System.Drawing.SystemColors.Control;
            this.Label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label2.Location = new System.Drawing.Point(252, 24);
            this.Label2.Name = "Label2";
            this.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label2.Size = new System.Drawing.Size(179, 18);
            this.Label2.TabIndex = 5;
            this.Label2.Text = "OSW Connect";
            // 
            // MainWindow
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1124, 872);
            this.Controls.Add(this.Frame1);
            this.Controls.Add(this.MainMenu1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(402, 61);
            this.MinimumSize = new System.Drawing.Size(1140, 880);
            this.Name = "MainWindow";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "FOMS Submarine";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_Closed);
            this.Load += new System.EventHandler(this.Form_Load);
            this.MainMenu1.ResumeLayout(false);
            this.MainMenu1.PerformLayout();
            this.Frame1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.Frame5.ResumeLayout(false);
            this.Frame3.ResumeLayout(false);
            this.Frame3.PerformLayout();
            this.Frame2.ResumeLayout(false);
            this.Frame2.PerformLayout();
            this.Frame7.ResumeLayout(false);
            this.Frame7.PerformLayout();
            this.Frame4.ResumeLayout(false);
            this.fr_LANCon.ResumeLayout(false);
            this.fr_LANCon.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.ToolStripMenuItem m_CloseProgram;
		public System.Windows.Forms.ToolStripMenuItem m_File;
		public System.Windows.Forms.ToolStripMenuItem m_CableInfoSet;
		public System.Windows.Forms.ToolStripMenuItem m_OTDRParamSet;
		public System.Windows.Forms.ToolStripMenuItem m_EquipmentsSettings;
		public System.Windows.Forms.ToolStripMenuItem m_EmailSettings;
		public System.Windows.Forms.ToolStripMenuItem m_Setting;
		public System.Windows.Forms.ToolStripMenuItem m_SingleCtrl;
		public System.Windows.Forms.ToolStripMenuItem m_Tool;
		public System.Windows.Forms.MenuStrip MainMenu1;
		// public System.Windows.Forms.TextBox txt_Threshold;
		public System.Windows.Forms.Label Label1;
		public System.Windows.Forms.GroupBox Frame5;
		public System.Windows.Forms.TextBox txt_SectionLoss;
		public System.Windows.Forms.Label Label3;
		public System.Windows.Forms.GroupBox Frame3;
		public System.Windows.Forms.TextBox txt_ChNo;
		public System.Windows.Forms.GroupBox Frame2;
		public System.Windows.Forms.TextBox txt_Messages;
		public System.Windows.Forms.GroupBox Frame7;
		public System.Windows.Forms.Button cb_Close;
		public System.Windows.Forms.Button cb_MeasureStop;
		public System.Windows.Forms.Button cb_MeasureStart;
		public System.Windows.Forms.GroupBox Frame4;
		public System.Windows.Forms.Button cb_OSWConnect;
		public System.Windows.Forms.ComboBox cbox_PortName;
		public System.Windows.Forms.Button cb_OTDRConnect;
		public System.Windows.Forms.TextBox txt_IPaddress;
		public System.Windows.Forms.Label LblIO;
		public System.Windows.Forms.Label Label2;
		public System.Windows.Forms.GroupBox fr_LANCon;
		public System.Windows.Forms.GroupBox Frame1;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.WebBrowser WebBrowser;
        public VncSharp.RemoteDesktop VNCViewer;
    }
}