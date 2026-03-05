using System;
using System.Windows.Forms;

namespace FOMSSubmarine
{
	internal partial class OTDRSettingsForm : Form
	{
		private OTDRControl otdr;

		private bool AutoAcquisitionMode;
		private int WaveLength;
		private float Range;
		private int Resolution;
		private int PulseWidth;
		private int AverageTime;
		private double IOR;
		private double BSC;

		private float[][] smartOTDRPulseWidthTable = new float[5][] {
			new float[4] { 10, 30, 100, 300 },
			new float[4] { 30, 100, 300, 500 },
			new float[4] { 100, 300, 500, 1000 },
			new float[4] { 300, 500, 1000, 3000 },
			new float[4] { 500, 1000, 3000, 10000 }
		};

		public OTDRSettingsForm()
		{
			InitializeComponent();
		}

		public void SetDeviceControls(OTDRControl otdr)
		{
			this.otdr = otdr;
		}

		private void Form_Load(object sender, EventArgs e)
		{
			// 실행시 초기 상태 세팅
			AutoAcquisitionMode = GlobalVariables.AutoAcquisitionMode;
			WaveLength = GlobalVariables.WaveLength;
			Range = GlobalVariables.Range;
			Resolution = GlobalVariables.Resolution;
			PulseWidth = GlobalVariables.PulseWidth;
			AverageTime = GlobalVariables.AverageTime;
			IOR = GlobalVariables.IOR;
			BSC = GlobalVariables.BSC;

			if (otdr.Model == OTDRControl.OTDRModel.Anritsu_MT9083)
			{
				cb_WaveLength.Items.Add("ALL");

				cb_AcquisitionMode.SelectedIndex = 1;
				cb_AcquisitionMode.Enabled = false;
				AutoAcquisitionMode = false;

				cb_Resolution.Items.Add("Low");
				cb_Resolution.Items.Add("High");
				cb_Resolution.Items.Add("Very High");

				cb_Resolution.Enabled = false;
				cb_PulseWidth.Enabled = false;
			}
			else if (otdr.Model == OTDRControl.OTDRModel.VIAVI_SmartOTDR)
			{
				cb_WaveLength.Items.Add("1310_1550");

				string[] ranges = { "10.0", "20.0", "40.0", "80.0", "160.0" };
				cb_Range.Items.Clear();
				foreach (var range in ranges)
				{
					cb_Range.Items.Add(range);
				}
				int selectedIndex = AdjustRangeForComboBoxItems();
				cb_PulseWidth.Items.Clear();
				foreach (var pw in smartOTDRPulseWidthTable[selectedIndex])
				{
					cb_PulseWidth.Items.Add(pw.ToString());
				}
				AdjustPulseWidthForComboBoxItems();

				cb_Resolution.Items.Add("Auto");
				cb_Resolution.Text = "Auto";

				cb_AverageTime.Items.Clear();
				cb_AverageTime.Items.Add("30 sec.");
				cb_AverageTime.Items.Add("60 sec.");
				cb_AverageTime.Items.Add("180 sec.");

				if (AverageTime != 30 && AverageTime != 60 && AverageTime != 180)
				{
					AverageTime = 60;
				}

				cb_Resolution.Enabled = true;
				cb_PulseWidth.Enabled = true;
			}

			// OTDR Setting Parameters 활성화 설정
			cb_WaveLength.Enabled = true;
			cb_Range.Enabled = true;
			cb_AverageTime.Enabled = true;
			txt_IOR.Enabled = true;
			txt_BSC.Enabled = true;

			UpdateControls();
		}

		private void Form_Closed(object sender, FormClosedEventArgs e)
		{
		}

		private void cb_AcquisitionMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			AutoAcquisitionMode = cb_AcquisitionMode.SelectedIndex == 0;
			gb_ManualParameters.Enabled = AutoAcquisitionMode == false;
		}

		private void cb_WaveLength_SelectionChangeCommitted(object sender, EventArgs eventArgs)
		{
			if (cb_WaveLength.SelectedIndex != -1)
			{
				string str = (string)cb_WaveLength.Items[cb_WaveLength.SelectedIndex];
				if (otdr.Model == OTDRControl.OTDRModel.Anritsu_MT9083)
				{
					WaveLength = (str == "ALL") ? -1 : int.Parse(str);
				}
				else if (otdr.Model == OTDRControl.OTDRModel.VIAVI_SmartOTDR)
				{
					WaveLength = (str == "1310_1550") ? -1 : int.Parse(str);
				}
			}
		}

		private void cb_Range_SelectionChangeCommitted(object sender, EventArgs eventArgs)
		{
			try
			{
				// 선택된 Range에 따라 사용 가능한 Resolution이 다르므로 이를 확인하여 Resolution List를 생성한다.
				if (cb_Range.SelectedIndex != -1)
				{
					string text = (string)cb_Range.Items[cb_Range.SelectedIndex];
					if (float.TryParse(text, out float value))
					{
						Range = value;
						GlobalVariables.Range = value;
					}
					if (otdr.Model == OTDRControl.OTDRModel.Anritsu_MT9083)
					{
						cb_Resolution.Enabled = true;
						cb_Resolution.Items.Clear();

						string response = "0";
						otdr.SendCommand("sour:ran " + text + ";*WAI;sour:res:ava?;*WAI?");
						// 선택된 Range Set; Range Set 완료 대기
						// 선택된 Range에서 사용 가능한 Resolution 확인
						otdr.WaitResponse();
						response = otdr.Response;
						otdr.SendCommand("sour:ran 1550");

						// Resolution은 Low(0), High(1), Very High(2)로써, 최소 1개, 최대 3개의 어레이로 구성
						string[] strs = response.Split(new string[] { ", " }, StringSplitOptions.None);
						foreach (string str in strs)
						{
							switch (str.Trim())
							{
								case "0":
									cb_Resolution.Items.Add("Low");
									break;

								case "1":
									cb_Resolution.Items.Add("High");
									break;

								case "2":
									cb_Resolution.Items.Add("Very High");
									break;
							}
						}
						cb_Resolution.Text = "Low";
					}
					else if (otdr.Model == OTDRControl.OTDRModel.VIAVI_SmartOTDR)
					{
						cb_PulseWidth.Items.Clear();
						foreach (var pw in smartOTDRPulseWidthTable[cb_Range.SelectedIndex])
						{
							cb_PulseWidth.Items.Add(pw.ToString());
						}
						AdjustPulseWidthForComboBoxItems();
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void cb_Resolution_SelectionChangeCommitted(object sender, EventArgs eventArgs)
		{
			try
			{
				if (cb_Resolution.SelectedIndex != -1)
				{
					Resolution = cb_Resolution.SelectedIndex;
					if (otdr.Model == OTDRControl.OTDRModel.Anritsu_MT9083)
					{
						// 선택된 Resolution에 따라 사용 가능한 PulseWidth가 다르므로 이를 확인하여 PulseWidth List를 생성한다.
						cb_PulseWidth.Enabled = true;
						cb_PulseWidth.Items.Clear();

						otdr.SendCommand("sour:res " + Resolution.ToString() + ";*WAI;sour:puls:ava?");
						otdr.WaitResponse();
						string response = otdr.Response;
						otdr.SendCommand("sour:res 2");		// 가능한 pulsewidth 확인 후 다시 초기화
						string[] strs = response.Split(new string[] { ", " }, StringSplitOptions.None); // 수신된 PulseWidth List를 Array로 변경
						foreach (string str in strs)
						{
							cb_PulseWidth.Items.Add(str.Trim());
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void cb_PulseWidth_SelectionChangeCommitted(object sender, EventArgs eventArgs)
		{
			if (cb_PulseWidth.SelectedIndex != -1)
			{
				string text = (string)cb_PulseWidth.Items[cb_PulseWidth.SelectedIndex];
				PulseWidth = int.Parse(text);
			}
		}

		private void cb_AverageTime_SelectionChangeCommitted(object sender, EventArgs eventArgs)
		{
			if (cb_AverageTime.SelectedIndex != -1)
			{
				string text = (string)cb_AverageTime.Items[cb_AverageTime.SelectedIndex];
				int seconds = int.Parse(text.Split(' ')[0]);
				if (text.IndexOf("min") != -1)
				{
					seconds *= 60;
				}
				AverageTime = seconds;
			}
		}

		private void txt_IOR_TextChanged(object sender, EventArgs eventArgs)
		{
			if (double.TryParse(txt_IOR.Text, out double value))
			{
				IOR = value;
			}
		}

		private void txt_BSC_TextChanged(object sender, EventArgs eventArgs)
		{
			if (double.TryParse(txt_BSC.Text, out double value))
			{
				BSC = value;
			}
		}

		private void cb_OTDRSetSave_Click(object sender, EventArgs eventArgs)
		{
			try
			{
				DialogResult RetMsg = MessageBox.Show("설정을 저장 하시겠습니까?", "저장 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				if (RetMsg == DialogResult.Yes)
				{
					if (ValidateValue())
					{
						UpdateControls();
						Program.MainWindow.txt_Messages.SelectedText =
							DateTime.Now.ToString("HH:mm:ss") + " OTDR Setting is Saved!" + Environment.NewLine;
						SaveToGlobalVariables();
						SendAllParameters(otdr);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}

			Close();
		}

		private void cb_OTDRSetClose_Click(object sender, EventArgs eventArgs)
		{
			Close();
		}

		private void cb_OTDRSetInit_Click(object sender, EventArgs eventArgs)
		{
			try
			{
				DialogResult RetMsg = MessageBox.Show("설정을 초기화 하시겠습니까?", "초기화 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				if (RetMsg == DialogResult.Yes)
				{
					AutoAcquisitionMode = false;
					if (otdr.Model == OTDRControl.OTDRModel.Anritsu_MT9083)
					{
						WaveLength = 1550;
						Range = 100;
						Resolution = 1;
						PulseWidth = 4000;
						AverageTime = 60;
						IOR = 1.466700;
						BSC = -81.50;

						otdr.SendCommand("INST:SEL OTDR_STD;*WAI;INST:STAT ON;*WAI;");
					}
					else if (otdr.Model == OTDRControl.OTDRModel.VIAVI_SmartOTDR)
					{
						WaveLength = 1550;
						Range = 100;
						Resolution = 1;
						PulseWidth = 3000;
						AverageTime = 60;
						IOR = 1.466700;
						BSC = -81.50;
					}

					UpdateControls();
					Application.DoEvents();
					Program.MainWindow.txt_Messages.SelectedText =
						DateTime.Now.ToString("HH:mm:ss") + " OTDR Setting is Initialized!" + Environment.NewLine;
					SaveToGlobalVariables();
					SendAllParameters(otdr);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}

			Close();
		}

		private int AdjustRangeForComboBoxItems()
		{
			int selectedIndex = -1;

			for (int i = 0; i < cb_Range.Items.Count; ++i)
			{
				if (selectedIndex == -1 && float.Parse((string)cb_Range.Items[i]) >= Range)
				{
					selectedIndex = i;
				}
			}
			if (selectedIndex == -1)
			{
				selectedIndex = cb_Range.Items.Count - 1;
			}
			Range = float.Parse((string)cb_Range.Items[selectedIndex]);
			cb_Range.SelectedIndex = selectedIndex;

			return selectedIndex;
		}

		private int AdjustPulseWidthForComboBoxItems()
		{
			int selectedIndex = -1;

			for (int i = 0; i < cb_PulseWidth.Items.Count; ++i)
			{
				if (selectedIndex == -1 && int.Parse((string)cb_PulseWidth.Items[i]) >= PulseWidth)
				{
					selectedIndex = i;
				}
			}
			if (selectedIndex == -1)
			{
				selectedIndex = cb_PulseWidth.Items.Count - 1;
			}
			PulseWidth = int.Parse((string)cb_PulseWidth.Items[selectedIndex]);
			cb_PulseWidth.SelectedIndex = selectedIndex;

			return selectedIndex;
		}

		void UpdateControls()
		{
			cb_AcquisitionMode.SelectedIndex = AutoAcquisitionMode ? 0 : 1;
			gb_ManualParameters.Enabled = AutoAcquisitionMode == false;
			cb_Range.Text = string.Format("{0:#0.0#}", Range);
			if (otdr.Model == OTDRControl.OTDRModel.Anritsu_MT9083)
			{
				cb_WaveLength.Text = (WaveLength == -1) ? "ALL" : WaveLength.ToString();
				cb_Resolution.SelectedIndex = (Resolution < cb_Resolution.Items.Count) ? Resolution : 1;
			}
			else if (otdr.Model == OTDRControl.OTDRModel.VIAVI_SmartOTDR)
			{
				cb_WaveLength.Text = (WaveLength == -1) ? "1310_1550" : WaveLength.ToString();
				cb_Resolution.SelectedIndex = 0;	// Auto only
			}
			cb_PulseWidth.Text = PulseWidth.ToString();
			cb_AverageTime.Text = AverageTime.ToString() + " sec.";
			txt_IOR.Text = string.Format("{0:#0.000000#}", IOR);
			txt_BSC.Text = string.Format("{0:#0.00#}", BSC);
		}

		bool ValidateValue()
		{
			if (IOR < 1.4f || IOR > 1.699999f)
			{
				MessageBox.Show("IOR: Out Of Range!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				txt_IOR.Text = "1.466700";
				return false;
			}
			if (BSC < -90 || BSC > -40)
			{
				MessageBox.Show("BSC: Out Of Range!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				txt_BSC.Text = "-81.50";
				return false;
			}

			return true;
		}

		public static void SendAllParameters(OTDRControl otdr)
		{
			if (otdr.Model == OTDRControl.OTDRModel.Anritsu_MT9083)
			{
				string strWaveLength = (GlobalVariables.WaveLength == -1) ? "ALL" : GlobalVariables.WaveLength.ToString();
				otdr.SendCommand(
					"sour:wav " + strWaveLength + ";*WAI;" +
					"sour:ran " + GlobalVariables.Range.ToString() + ";*WAI;" +
					"sour:res " + GlobalVariables.Resolution.ToString() + ";*WAI;" +
					"sour:puls " + GlobalVariables.PulseWidth.ToString() + ";#WAI;" +
					"sour:aver:tim " + GlobalVariables.AverageTime.ToString() + ";*WAI;" +
					"sens:fib:ior " + GlobalVariables.IOR.ToString() + ";*WAI;" +
					"sens:fib:bsc " + GlobalVariables.BSC.ToString());
			}
			else if (otdr.Model == OTDRControl.OTDRModel.VIAVI_SmartOTDR)
			{
				string strWaveLength = (GlobalVariables.WaveLength == -1) ? "L1310_1550" : "L" + GlobalVariables.WaveLength.ToString();
				if (GlobalVariables.AutoAcquisitionMode)
				{
					string command = "OTDS:ACQ AUTO";
					otdr.SendCommand(command);
					Utilities.Delay(200);

					command =
						"OTDS:LAS " + strWaveLength + ";" +
						"OTDS:MAXT " + GlobalVariables.AverageTime.ToString() + ";" +
						"OTDS:UIOR U1,L" + strWaveLength + "," + GlobalVariables.IOR.ToString() + ";" +
						"OTDS:PSC USER;OTDS:K L" + strWaveLength + "," + GlobalVariables.BSC.ToString();
					otdr.SendCommand(command);
				}
				else
				{
					string command = "OTDS:ACQ MAN";
					otdr.SendCommand(command);
					Utilities.Delay(200);

				    string pulseWidthString =
							(GlobalVariables.PulseWidth < 1000) ?
								GlobalVariables.PulseWidth.ToString() + "NS" : (GlobalVariables.PulseWidth / 1000).ToString() + "US";
					//string resolutionString = (GlobalVariables.Resolution == 1) ? "HRES" : ((GlobalVariables.Resolution == 2) ? "MANU,0.04" : "MANU,100");
					string resolutionString = "AUTO";
				    command =
					    "OTDS:LAS " + strWaveLength + ";" +
					    "OTDS:KMR " + GlobalVariables.Range.ToString() + ";" +
					    "OTDS:RES " + resolutionString + ";" +
					    "OTDS:PULS P" + pulseWidthString + ";" +
					    "OTDS:MAXT " + GlobalVariables.AverageTime.ToString() + ";" +
					    "OTDS:UIOR U1,L" + strWaveLength + "," + GlobalVariables.IOR.ToString() + ";" +
					    "OTDS:PSC USER;OTDS:K L" + strWaveLength + "," + GlobalVariables.BSC.ToString();
				    otdr.SendCommand(command);
				    Utilities.Delay(200);
				    Application.DoEvents();
				    otdr.SendCommand(command);  // 파라메터 끼리 설정가능한 범위에 영향을 주어 설정한 값이 조정되었을 수 있으므로 한 번 더 설정 커맨드 전송
				}
			}
		}

		void SaveToGlobalVariables()
		{
			GlobalVariables.AutoAcquisitionMode = AutoAcquisitionMode;
			GlobalVariables.WaveLength = WaveLength;
			GlobalVariables.Range = Range;
			GlobalVariables.Resolution = Resolution;
			GlobalVariables.PulseWidth = PulseWidth;
			GlobalVariables.AverageTime = AverageTime;
			GlobalVariables.IOR = IOR;
			GlobalVariables.BSC = BSC;
			GlobalVariables.SaveOTDRSettings();
		}
	}
}
