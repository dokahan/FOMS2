using System;
using System.Windows.Forms;

namespace FOMSSubmarine
{
	public partial class EquipmentsSettingsForm : Form
	{
		public EquipmentsSettingsForm()
		{
			InitializeComponent();
		}

		private void Form_Load(object sender, EventArgs e)
		{
			foreach (string modelName in OTDRControl.ModelNames)
			{
				comboBoxOTDR.Items.Add(modelName);
			}
			foreach (string modelName in OSWControl.ModelNames)
			{
				comboBoxOSW.Items.Add(modelName);
			}
			comboBoxOTDR.SelectedIndex = SelectedOTDRIndex;
			comboBoxOSW.SelectedIndex = SelectedOSWIndex;
		}

		public int SelectedOTDRIndex { get; set; }
		public int SelectedOSWIndex { get; set; }

		private void buttonOk_Click(object sender, EventArgs e)
		{
			SelectedOTDRIndex = comboBoxOTDR.SelectedIndex;
			SelectedOSWIndex = comboBoxOSW.SelectedIndex;
		}
	}
}
