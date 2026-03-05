using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace FOMSSubmarine
{
	internal partial class SingleControllerForm : Form
	{
		private bool completedMeasurement = false;
		private OTDRControl otdr;
		private OSWControl osw;

		public SingleControllerForm()
		{
			InitializeComponent();
		}

		public void SetDeviceControls(OTDRControl otdr, OSWControl osw)
		{
			this.otdr = otdr;
			this.osw = osw;
		}

		private void Form_Load(object sender, EventArgs e)
		{
			txtChannel.Text = Program.MainWindow.txt_ChNo.Text;

			completedMeasurement = false;
		}

		private void Form_Closed(object sender, FormClosedEventArgs e)
		{
		}

		//================================================================================================
		// OTDR Measurement
		//================================================================================================

		private void cb_MeasureStart_Click(object sender, EventArgs eventArgs)
		{
			Program.MainWindow.SendOTDRCursorSettings();

			Program.MainWindow.txt_Messages.SelectedText = "Measuring..." + Environment.NewLine;
			completedMeasurement = false;
			otdr.IsReceivedDataCallbackEnabled = false;
			bool succeed = false;

			try
			{
				if (otdr.Model == OTDRControl.OTDRModel.Anritsu_MT9083)
				{
					otdr.SendCommand("sens:loss:mode 4; *WAI"); // Loss_Mode (4:dB/km LSA Loss Mode)
					otdr.SendCommand("init; *WAI; sens:trac:ready?");
					string Complete_Msg = "";
					otdr.WaitResponse();
					Complete_Msg = otdr.Response;

					if (Complete_Msg == "1")
					{
						succeed = true;
					}
				}
				else if (otdr.Model == OTDRControl.OTDRModel.VIAVI_SmartOTDR)
				{
					Program.MainWindow.m_OTDRParamSet.Enabled = false;
					otdr.SendCommand("KEY STAR");
					var sw = new Stopwatch();
					while (sw.ElapsedMilliseconds < 120000)
					{
						otdr.SendCommand("STAT:ACQ?");
						otdr.WaitResponse();
						if (otdr.Response == "STOPPED")
						{
							succeed = true;
							break;
						}
					}
					Program.MainWindow.m_OTDRParamSet.Enabled = true;
				}
			}
			catch (Exception ex)
			{
				DebuggingHelper.Trace(ex);
			}

			if (succeed)
			{
				// 측정 완료 후 trace data 준비 되었다면..
				Program.MainWindow.txt_Messages.SelectedText = "Measurement Complete!" + Environment.NewLine + Environment.NewLine;
				MessageBox.Show("Measurement Complete!", Constants.InternalAppName);
				completedMeasurement = true;
			}
		}

		//================================================================================================
		// SOR Save
		//================================================================================================

		private void cb_Save_Click(object sender, EventArgs eventArgs)
		{
			if (completedMeasurement)
			{
				GlobalVariables.Save_Button_On = 1;
				GlobalVariables.Data_Size = 0;
				if (otdr.Model == OTDRControl.OTDRModel.Anritsu_MT9083)
				{
					otdr.SendCommand("*CLS;trac:load:sor?");
				}
				else if (otdr.Model == OTDRControl.OTDRModel.VIAVI_SmartOTDR)
				{
					otdr.SendCommand("*CLS;OTDR:SSOR?");
				}
				otdr.IsReceivedDataCallbackEnabled = true;
			}
		}

		//================================================================================================
		// Key Pad 동작
		//================================================================================================

		///Key Pad Number
		private void cbPadNum_Click(object sender, EventArgs eventArgs)
		{
			if (sender is Button button)
			{
				txtChannel.Text = txtChannel.Text + button.Text.ToString();
			}
		}

		/// Key Pad C
		private void cbPadClear_Click(object sender, EventArgs eventArgs)
		{
			txtChannel.Text = "";
		}

		/// Key Pad OK
		private void cbPadEnter_Click(object sender, EventArgs eventArgs)
		{
			int.TryParse(txtChannel.Text, out int channel);
			if (ValidateChannel(channel))
			{
				SetChannel(channel);
			}
		}

		//================================================================================================
		// Up/Down Button Control
		//================================================================================================

		/// [▲] Button Click!
		private void cbUp_Click(object sender, EventArgs eventArgs)
		{
			int.TryParse(txtChannel.Text, out int channel);
			if (ValidateChannel(++channel))
			{
				SetChannel(channel);
			}
		}

		/// [▼] Button Click!
		private void cbDown_Click(object sender, EventArgs eventArgs)
		{ 
			int.TryParse(txtChannel.Text, out int channel);
			if (ValidateChannel(--channel))
			{
				SetChannel(channel);
			}
		}

		//================================================================================================
		// Channel Moving Control using Text Editor
		//================================================================================================

		private void txtChannel_KeyPress(object sender, KeyPressEventArgs eventArgs)
		{
			if (eventArgs.KeyChar == '\r')	// enter key pressed
			{
				int.TryParse(txtChannel.Text, out int channel);
				if (ValidateChannel(channel))
				{
					SetChannel(channel);
				}
			}
			else if (eventArgs.KeyChar < '0' || eventArgs.KeyChar > '9')
			{
				eventArgs.Handled = true;
			}
		}

		private void cb_OSWClose_Click(object sender, EventArgs eventArgs)
		{
			Close();
		}

		private bool ValidateChannel(int channel)
		{
			if (channel < 1 || channel > 144)
			{
				MessageBox.Show("Out of Channel Range!", Constants.InternalAppName);
				txtChannel.Text = "";
				return false;
			}
			else
			{
				return true;
			}
		}

		private void SetChannel(int channel)
		{
			try
			{
				osw.SwitchChannel(channel);

				txtChannel.Text = string.Format("{0:D3}", channel);
				Program.MainWindow.txt_ChNo.Text = txtChannel.Text;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}
	}
}
