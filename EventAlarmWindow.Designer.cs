
namespace FOMSSubmarine
{
	partial class EventAlarmWindow
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EventAlarmWindow));
			this.gridView_EventAlarm = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)(this.gridView_EventAlarm)).BeginInit();
			this.SuspendLayout();
			// 
			// gridView_EventAlarm
			// 
			this.gridView_EventAlarm.AllowDrop = true;
			this.gridView_EventAlarm.AllowUserToAddRows = false;
			this.gridView_EventAlarm.AllowUserToDeleteRows = false;
			this.gridView_EventAlarm.AllowUserToResizeColumns = this.gridView_EventAlarm.ColumnHeadersVisible;
			this.gridView_EventAlarm.AllowUserToResizeRows = this.gridView_EventAlarm.RowHeadersVisible;
			this.gridView_EventAlarm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridView_EventAlarm.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.gridView_EventAlarm.ColumnHeadersHeight = 24;
			this.gridView_EventAlarm.EnableHeadersVisualStyles = false;
			this.gridView_EventAlarm.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gridView_EventAlarm.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
			this.gridView_EventAlarm.Location = new System.Drawing.Point(0, 0);
			this.gridView_EventAlarm.Name = "gridView_EventAlarm";
			this.gridView_EventAlarm.ReadOnly = true;
			this.gridView_EventAlarm.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.gridView_EventAlarm.ShowCellToolTips = false;
			this.gridView_EventAlarm.Size = new System.Drawing.Size(673, 350);
			this.gridView_EventAlarm.StandardTab = true;
			this.gridView_EventAlarm.TabIndex = 0;
			// 
			// EventAlarmWindow
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(672, 348);
			this.Controls.Add(this.gridView_EventAlarm);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Location = new System.Drawing.Point(8, 30);
			this.MinimumSize = new System.Drawing.Size(688, 387);
			this.Name = "EventAlarmWindow";
			this.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Text = "Event Alarm";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_Closed);
			this.Load += new System.EventHandler(this.Form_Load);
			((System.ComponentModel.ISupportInitialize)(this.gridView_EventAlarm)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.DataGridView gridView_EventAlarm;
	}
}