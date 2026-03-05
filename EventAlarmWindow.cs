using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace FOMSSubmarine
{
	internal partial class EventAlarmWindow : Form
	{
		public EventAlarmWindow()
		{
			InitializeComponent();

			DataGridViewCellStyle columnHeadersCellStyle = new DataGridViewCellStyle()
			{
				Alignment = DataGridViewContentAlignment.MiddleLeft,
				BackColor = Color.FromArgb(192, 192, 255),
				Font = new Font("Verdana", 11.25f, FontStyle.Bold, GraphicsUnit.Point, 0),
				ForeColor = SystemColors.WindowText,
				SelectionBackColor = Color.FromArgb(192, 192, 255),
				SelectionForeColor = SystemColors.WindowText,
				WrapMode = DataGridViewTriState.True
			};
			gridView_EventAlarm.ColumnHeadersDefaultCellStyle = columnHeadersCellStyle;

			var dataGridViewColumns = new DataGridViewColumn[]
			{
				new DataGridViewTextBoxColumn() { HeaderText = "Event Time",
					AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells },
				new DataGridViewTextBoxColumn() { HeaderText = "Fiber#" },
				new DataGridViewTextBoxColumn() { HeaderText = "Event Type" },
				new DataGridViewTextBoxColumn() { HeaderText = "Distance[m]" },
				new DataGridViewTextBoxColumn() { HeaderText = "Loss[dB]" }
			};
			gridView_EventAlarm.Columns.AddRange(dataGridViewColumns);
			gridView_EventAlarm.RowHeadersVisible = false;
		}

		private void Form_Load(object sender, EventArgs e)
		{
			// testre ssde
			//string eventTime = DateTime.Now.ToString("yyyy-MM-dd") + "-" + DateTime.Now.ToString("HH:mm:ss");
			//AddItem(eventTime + "\t" + 1 + "\t" + "Bending" + "\t" + string.Format("{0:#0#}", 50215) + "\t" + string.Format("{0:#0.00#}", 0.615));
		}

		private void Form_Closed(object sender, FormClosedEventArgs e)
		{
		}

		public void AddItem(string item)
		{
			string[] strs = item.Split('\t');
			int index = gridView_EventAlarm.Rows.Add(strs);
			gridView_EventAlarm.Rows[index].Cells[0].Style.BackColor = Color.FromArgb(192, 192, 255);
			g.Log(item);
		}
	}
}