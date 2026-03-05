
namespace FOMSSubmarine
{
	partial class EquipmentsSettingsForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EquipmentsSettingsForm));
			this.comboBoxOTDR = new System.Windows.Forms.ComboBox();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOk = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.comboBoxOSW = new System.Windows.Forms.ComboBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// comboBoxOTDR
			// 
			this.comboBoxOTDR.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxOTDR.FormattingEnabled = true;
			this.comboBoxOTDR.Location = new System.Drawing.Point(12, 27);
			this.comboBoxOTDR.Name = "comboBoxOTDR";
			this.comboBoxOTDR.Size = new System.Drawing.Size(236, 27);
			this.comboBoxOTDR.TabIndex = 0;
			// 
			// buttonCancel
			// 
			this.buttonCancel.AllowDrop = true;
			this.buttonCancel.BackColor = System.Drawing.SystemColors.Control;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonCancel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.buttonCancel.Location = new System.Drawing.Point(148, 196);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.buttonCancel.Size = new System.Drawing.Size(120, 39);
			this.buttonCancel.TabIndex = 15;
			this.buttonCancel.Text = "CANCEL";
			this.buttonCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.buttonCancel.UseVisualStyleBackColor = false;
			// 
			// buttonOk
			// 
			this.buttonOk.AllowDrop = true;
			this.buttonOk.BackColor = System.Drawing.SystemColors.Control;
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOk.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonOk.ForeColor = System.Drawing.SystemColors.ControlText;
			this.buttonOk.Location = new System.Drawing.Point(10, 196);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.buttonOk.Size = new System.Drawing.Size(120, 39);
			this.buttonOk.TabIndex = 14;
			this.buttonOk.Text = "OK";
			this.buttonOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.buttonOk.UseVisualStyleBackColor = false;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.comboBoxOTDR);
			this.groupBox1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
			this.groupBox1.Location = new System.Drawing.Point(10, 13);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(260, 78);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "OTDR";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.comboBoxOSW);
			this.groupBox2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
			this.groupBox2.Location = new System.Drawing.Point(10, 98);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(260, 78);
			this.groupBox2.TabIndex = 17;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "OSW";
			// 
			// comboBoxOSW
			// 
			this.comboBoxOSW.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxOSW.FormattingEnabled = true;
			this.comboBoxOSW.Location = new System.Drawing.Point(12, 27);
			this.comboBoxOSW.Name = "comboBoxOSW";
			this.comboBoxOSW.Size = new System.Drawing.Size(236, 27);
			this.comboBoxOSW.TabIndex = 0;
			// 
			// EquipmentsSettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(280, 250);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EquipmentsSettingsForm";
			this.ShowInTaskbar = false;
			this.Text = "Equipments Settings";
			this.Load += new System.EventHandler(this.Form_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox comboBoxOTDR;
		public System.Windows.Forms.Button buttonCancel;
		public System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ComboBox comboBoxOSW;
	}
}