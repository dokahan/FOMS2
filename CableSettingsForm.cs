using System;
using System.Windows.Forms;

namespace FOMSSubmarine
{
	internal partial class CableSettingsForm : Form
	{
		private OTDRControl otdr;

		public CableSettingsForm()
		{
			InitializeComponent();
		}

		public void SetDeviceControls(OTDRControl otdr)
		{
			this.otdr = otdr;
		}

		private void Form_Load(object sender, EventArgs e)
		{
			cb_Save.Enabled = false;
			Text2.Enabled = false;
			txt_FiberNumMM.Enabled = false;

			txt_CableLen.Text = GlobalVariables.Cable_Len.ToString();
			txt_FiberNumSM.Text = GlobalVariables.Fiber_NumSM.ToString();
			txt_FiberNumMM.Text = GlobalVariables.Fiber_NumMM.ToString();
			txt_MaxLimit.Text = GlobalVariables.Maximum_Limit.ToString();
			txt_BendLimit.Text = GlobalVariables.Bend_Limit.ToString();
			txt_DummyCable.Text = GlobalVariables.DummyCable_Len.ToString();
			txt_ACursorDelta.Text = GlobalVariables.ACursorDelta.ToString();
			txt_BCursorDelta.Text = GlobalVariables.BCursorDelta.ToString();
		}

		private void Form_Closed(object sender, FormClosedEventArgs e)
		{
		}

		private void txt_CableLen_TextChanged(object sender, EventArgs eventArgs)
		{
			cb_Save.Enabled = true;
		}

		private void txt_FiberNumSM_TextChanged(object sender, EventArgs eventArgs)
		{
			cb_Save.Enabled = true;
		}

		private void txt_FiberNumMM_TextChanged(object sender, EventArgs eventArgs)
		{
			cb_Save.Enabled = true;
		}

		private void txt_MaxLimit_TextChanged(object sender, EventArgs eventArgs)
		{
			cb_Save.Enabled = true;
		}

		private void txt_BendLimit_TextChanged(object sender, EventArgs e)
		{
			cb_Save.Enabled = true; //  romee
		}

		private void txt_DummyCable_TextChanged(object sender, EventArgs eventArgs)
		{
			cb_Save.Enabled = true;
		}

		private void txt_ACursorDelta_TextChanged(object sender, EventArgs e)
		{
			cb_Save.Enabled = true;
		}

		private void txt_BCursorDelta_TextChanged(object sender, EventArgs e)
		{
			cb_Save.Enabled = true;
		}

		private void cb_Save_Click(object sender, EventArgs eventArgs)
		{
			try
			{
				DialogResult RetMsg = MessageBox.Show("설정을 저장하시겠습니까?", "저장 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				if (RetMsg == DialogResult.Yes)
				{
					GlobalVariables.Cable_Len = Utilities.ConvertFromString<float>(txt_CableLen.Text);
					GlobalVariables.Fiber_NumSM = Utilities.ConvertFromString<float>(txt_FiberNumSM.Text);
					GlobalVariables.Fiber_NumMM = Utilities.ConvertFromString<float>(txt_FiberNumMM.Text);
					GlobalVariables.Fiber_Number = GlobalVariables.Fiber_NumSM + GlobalVariables.Fiber_NumMM;
					GlobalVariables.Maximum_Limit = Utilities.ConvertFromString<float>(txt_MaxLimit.Text);
					GlobalVariables.Bend_Limit = Utilities.ConvertFromString<float>(txt_BendLimit.Text);

					if (otdr.Model == OTDRControl.OTDRModel.Anritsu_MT9083)
					{
						otdr.SendCommand("sour:ran " + GlobalVariables.Cable_Len.ToString() + ";*WAI");
					}
					else if (otdr.Model == OTDRControl.OTDRModel.VIAVI_SmartOTDR)
					{
						otdr.SendCommand("OTDS:KMR " + GlobalVariables.Cable_Len.ToString());
					}

					GlobalVariables.BCursor = GlobalVariables.Cable_Len;
					GlobalVariables.RLSACursorStart = GlobalVariables.Cable_Len - 1;
					GlobalVariables.RLSACursorEnd = GlobalVariables.Cable_Len;
					//if (otdr.Model == OTDRControl.OTDRModel.Anritsu_MT9083)
					//{
					//	otdr.SendCommand(
					//		"sens:acur " + GlobalVariables.ACursor.ToString() + ";*WAI;" +
					//		"sens:bcur " + GlobalVariables.BCursor.ToString() + ";*WAI;" +
					//		"sens:lsal " + GlobalVariables.LLSACursorStart.ToString() + "," + GlobalVariables.LLSACursorEnd.ToString() + ";*WAI;" +
					//		"sens:lsar " + GlobalVariables.RLSACursorStart.ToString() + "," + GlobalVariables.RLSACursorEnd.ToString());
					//}
					//else if (otdr.Model == OTDRControl.OTDRModel.VIAVI_SmartOTDR)
					//{
					//	int ac = (int)(GlobalVariables.ACursor * 1000);
					//	int bc = (int)(GlobalVariables.BCursor * 1000);

					//	otdr.SendCommand(
					//		"CUR:CUR:POS A," + ac.ToString() + ";" +
					//		"CUR:CUR:POS B," + bc.ToString());
					//}

					GlobalVariables.DummyCable_Len = Utilities.ConvertFromString<float>(txt_DummyCable.Text);
					GlobalVariables.ACursorDelta = Utilities.ConvertFromString<float>(txt_ACursorDelta.Text);
					GlobalVariables.BCursorDelta = Utilities.ConvertFromString<float>(txt_BCursorDelta.Text);

					Program.MainWindow.txt_Messages.SelectedText =
						DateTime.Now.ToString("HH:mm:ss") + "Setting Change is Saved." + Environment.NewLine;

					GlobalVariables.SaveCableSettings();
					Program.MainWindow.SendOTDRCursorSettings();
				}

				Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void cb_CableSetClose_Click(object sender, EventArgs eventArgs)
		{
			Close();
		}

    }
}
