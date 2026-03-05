using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Comms;
using VncSharp;

namespace FOMSSubmarine
{
	//***********************************************************
	// FOMS Submarine
	// Last Modifying: 2014.11.27 (E-mail Sending Update)
	// OTDR: Anritsu MT9083
	// Optical Switch: PPI OSWL-144
	//***********************************************************

	internal partial class MainWindow : Form
	{
		private EventAlarmWindow windowEventAlarm;

		private string baseDirectory = "FOMS Submarine\\";

		private string ipOTDR = "192.168.1.2";
		private int portOSW = 3;
		private OTDRControl otdr;
		private OSWControl osw;
		private ushort otdrPort = 2288;
		private AccessDB accessDB;

		int Module_Cal_Complete = 0;  //210413 삭제후 재 기입

		//******************* Optical Switch Parameters **********
		int Port_Idx = 3; //Serial COM Port Number
		string CH_Num = ""; //OSW Channel Number, 210413 삭제후 재 기입
		string Cur_ch_num = ""; //OSW의 현채 Channel Number      *
		string Date_ = ""; //날짜                                *
		//********************************************************

		//******************** Auto Measure Parameters ***********
		int Auto_Save_Ready = 0; //자동 측정을 통해 SOR 저장 가능 상태인지 확인   *
		int Switching_Count = 0; //OSW Channel Switching Count Number             *
		string FolderName = ""; //Set Path 안할 경우 자동으로 생성되는 폴더의 이름*
		string FolderPath = ""; //자동으로 생성된 폴더의 경로(생성된 폴더 포함)   *
		int Measure_Stop = 0; //반복 측정 정지                                    *
		int Start_Ch = 0; //자동 측정시 OSW의 시작 채널 설정, 210413 삭제후 재 기입
		string CH_CMD = ""; //OSW Channel Number Command Type (3digit)            *
		private int Fiber_Num = 0;
		int OTDR_Ana_Write_Complete = 0;
        //*************************************************************************

		//******************** Loss Calculation Parameters *********************
	    string Loss_Mode = ""; //Loss Mode Number  210413 삭제후 재 기입       *

		//******************** Bi-directional Measure Parameters *****//210413*********
		double Mod_Val = 0;
		int Even_Ch_Idx = 0;
		int Odd_Ch_Idx = 0;
		
		//Breakage_Cable Table 
		bool Bending_Cable_Exist = false; //Bending Cable Research     *
		bool Breakage_Cable_Exist = false; //Reflect Cable Research   *

		//************************** Excel Write ******************
		Excel.Application oXL = null;
		Excel.Workbook oWB = null;
		Excel.Worksheet oSheet = null;
		Excel.Range oRng = null; //210413 삭제후 재 기입
		string Xls_File_Path = "";
		string Xls_NewFile_Path = "";
		string Org_Xls_File = ""; //210413 삭제후 재 기입
		int Length_Write_OK = 0; 
		//************************************************************

		//******************** For Report ****************************
		string Cell_Name = "";
		int Length_Row_Num = 0;
		string[] Report_Excel_Length_Index = new string[] { "", "", "", "", "" };
		string[] Report_Excel_Atten_Index = new string[] { "", "", "", "", "" };
		string[] Report_Excel_TotalLoss_Index = new string[] { "", "", "", "", "" };
		string dBpkm_Loss = "";
		string Total_Loss = "";
		int Current_Time_State = 0;

		int Now_Time = 0;
		int[] Check_Time = new int[5];
		int One_Time_Complete_Idx = 0;//210413 삭제후 재 기입

		int Create_File_OK = 0;
		int Data_Wirte_OK = 0;//210413 삭제후 재 기입

		int First_Time_Write_Ready = 0;
		int Second_Time_Write_Ready = 0;
		int Third_Time_Write_Ready = 0;
		int Fourth_Time_Write_Ready = 0;

		int Write_Count_Number = 0;
		int Report_Complete = 0;
		int Loop_Count = 0;
		// For E-mail(Gmail) Sending -------------------------------
		string str_Report_Subject = "";
		string str_Report_TextBody = "";
		string[] emails = new string[Constants.MaxEmailReceiver];
		string emailServer;
		bool emailServerEncryptedConnection;
		string emailServerUserID;
		string emailServerPassword;
		uint emailServerPort;
		bool emailServerUseDefaultPort;
		string emailSendFrom;
		string emailSendTo;
		string emailSendTheme;

		//***********************************************************

		//******************** E-mail Sending ***********************
		string AttachFileName = ""; //Attached SOR FileName                                          '
									//***********************************************************

		//******************** FOMS Analysis Parameters ****//210413 start
		private bool cerrorF = false; //Calculation Error//210413 삭제후 재 기입

		private int mAvgv = 0; //Moving Average +/- Point //210413 삭제후 재 기입
		private float Diffdeltav = 0; //Differential Delta[m]//210413
		private int Diffdeltapnt = 0; //Differential Delta[pnt]//210413 

		private double lpnt = 0;
		private int rpnt = 0; 
		private int Flpnt = 0; //Front Linear Start Point after Launching
		private int Frpnt = 0; //Front Linear End Point after Launching
		
		//20130311 Start-----------------------------------------------------
		private int Pcnt = 0; 
		private int[] Ppnt = null; 
		private int[] E1pnt = null; 
		
		private int Bcnt = 0; 
		private int[] Bpnt = null; 
		private int[] Blpnt = null; //Next Linear Point
		private bool[] Btype = null; 
		
		private int Ecnt = 0; 
		private int[] Epnt = null; 
		private float[] Evloss = null; 
		private bool[] Etype = null; //True/False(Peak/Bend)
		private bool[] Ecalc = null; //True/False(LSA/2P)

		//20130311 End-----------------------------------------------------
		private int[,] FSectionPnt = null; //20121221
		private float[] Dmax = new float[3]; 
		private float[] dmin = new float[3]; 
		private float[] PThvl = new float[4]; 
		private float[] PThvh = new float[4];
		//Private pthvt(2) As Single
		//Private plvlt(2) As Single
		private float PThvlvl = 0; //Level
		private float PThvhgt = 0; //Height
		private float[] BThvl = new float[4];   
		//''''Private TThvLvl As Single
		private float LLevel = 0; //Launching Level
		private float SLevel = 0; //1st Splitter Base Level
		private int EofPnt = 0; private float PDeltav = 0; //Default Ripple = 0.5dB
		private float PBendv = 0; //Default Bend = 0.5dB'20121218
		
		// private OTDR1stEventPnt = 0;;

		private int[] PWIDPNT = null;
		private float S1loss = 0; //Splitter Loss
		private float S2loss = 0; //Splitter Loss
		private float S3loss = 0; //Splitter Loss

		private float RMSLvl = 0; //Noise Level (SNR=1)
		private float RMSSdv = 0; //Noise Level STDEV
		private float RMSDNR = 0; //RMS Dynamic Range (Launching Level - RMSLvl)
		private float IEC98Lvl = 0; //RMS
		private float IEC98DNR = 0; //IEC Dynamic Range (Launching Level - IEC98Lvl)
		private float NoisePeak = 0; //Measurement Range Level
		
		private int S1pnt = 0; //0~1st Splitter'Just after 1st Splitter before Peak Candidate
		private int S2pnt = 0; //S1~EndofONT' Just after Last ONT before Peak Candidate
		//private int Spntwidth = 0 ; //only for sbar
		private int sratio = 0; //Total Splitting, ex) 1x4 - 1x8 = 32

		private double LA = 0; 
		private double LB = 0; 
		private float LR2 = 0; //Y=AX+B, R2=Relation of LSA
		
		//110704 for Differentiation
		private Array D1dat = null; //1st order Differential
		private Array D2dat = null; //2nd order Differential
		
		const double DIFF_BAD_RESULT = -1E+30d;
		const double BAD_RESULT = -999.999d;
		
		private int rSectionLine = 0; 
		private float pthvt = 0;
		private float plvlt = 0;
		private float ripplet = 0;
		
		//210413 END----------------
		private float bendt = 0;
		private string Time_ = "";
		private string Measure_Time = "";

		private int Sbaridx = 0;
		private float[] lydat = null;

		//***********************************************************

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Form_Load(object sender, EventArgs e)
		{
			baseDirectory = Path.GetPathRoot(Utilities.StartupPath) + baseDirectory;

			// load OTDR settings
			var otdrModel = OTDRControl.OTDRModel.Anritsu_MT9083;	// default OTDR model
			string str = Utilities.GetAppRegistryString("OTDR_Model", OTDRControl.ModelNames[(int)otdrModel]);
			int index = OTDRControl.ModelNames.IndexOf(str);
			if (index != -1)
			{
				otdrModel = (OTDRControl.OTDRModel)index;
			}
			otdr = new OTDRControl(otdrModel);
			otdr.DataReceived += OTDR_DataArrival;
			ipOTDR = Utilities.GetAppRegistryString("OTDR_IPAddress", ipOTDR);
			txt_IPaddress.Text = ipOTDR;
			if (otdrModel == OTDRControl.OTDRModel.Anritsu_MT9083)
			{
				//WebBrowser.Navigate(new Uri("http://" + ipOTDR));    // OTDR Access Master
				otdrPort = 2288;
			}
			else if (otdrModel == OTDRControl.OTDRModel.VIAVI_SmartOTDR)
			{
				WebBrowser.Visible = false;
				VNCViewer.Visible = true;
				otdrPort = 8002;
			}

			// load OSW settings
			var oswModel = OSWControl.OSWModel.PPI_OSWL144;	// default OSW model
			str = Utilities.GetAppRegistryString("OSW_Model", OSWControl.ModelNames[(int)oswModel]);
			index = OSWControl.ModelNames.IndexOf(str);
			if (index != -1)
			{
				oswModel = (OSWControl.OSWModel)index;
			}
			osw = new OSWControl(oswModel);
			if (oswModel == OSWControl.OSWModel.FOD_5510)
			{
				cbox_PortName.Enabled = false;
			}
			else
			{
				portOSW = int.Parse(Utilities.GetAppRegistryString("OSW_COMPort", Port_Idx.ToString()));
				Port_Idx = portOSW;
				cbox_PortName.Enabled = true;
				cbox_PortName.SelectedIndex = Port_Idx - 1;
			}

			//chkbox_AllFibers.CheckState = CheckState.Unchecked;

			// Analysis =============================================================
			bendt = Single.Parse(txt_Threshold.Text);   // Threshold of Bending Loss

			// Auto Measure ======================================================
			//   Start_Ch = 1;//210413 삭제후 재 기입
			GlobalVariables.Save_Button_On = 0;
			Auto_Save_Ready = 0;

			GlobalVariables.Initialize();

			// Ch# Index for Bidirection Measure ==========================
			for (int i = 0; i < 72; ++i)
			{
				//Even_Ch_Arr[i] = (i + 1) * 2;
				//Odd_Ch_Arr[i] = (i + 1) * 2 - 1;
			}
			// Loss Calculation =======================
			//Loss_Mode = "4"; // 4: LSA dB/km Loss//210413 삭제후 재 기입

			// MS Access Connection ============================================
			accessDB = new AccessDB(baseDirectory + "DB\\DB_QP_1_1.mdb");
			accessDB.Connect();

			windowEventAlarm = new EventAlarmWindow();
			windowEventAlarm.Show();

			// MS Excel Write for Report =======================================
			Xls_File_Path = baseDirectory + "Report\\DPR_FO_Monitoring_";
			//Org_Xls_File = baseDirectory + "Report\\DPR_FO_Monitoring.xlsx";
			Now_Time = 0;
			Fiber_Num = 12;
			Measure_Stop = 0;
			//Data_Wirte_OK = 0;
			Write_Count_Number = 0;
			Report_Complete = 0;
			Create_File_OK = 1;
			Loop_Count = 1;
			Check_Time[0] = 3;
			Check_Time[1] = 9; //15 '9 '11
			Check_Time[2] = 15; //30 '13 '17
			Check_Time[3] = 21; //45 '17 '23
			//(3, 9, 15, 21 are hours unit / 3, 15, 30, 45 are minutes unit for test)

			First_Time_Write_Ready = 0;
			Second_Time_Write_Ready = 0;
			Third_Time_Write_Ready = 0;
			Fourth_Time_Write_Ready = 0;
			Length_Write_OK = 0;
			str_Report_Subject = "Optical Fiber Measurement Daily Report"; //Subject of E-mail
								// Cell Index of Excel ----------

			Report_Excel_Length_Index[0] = "C";
			Report_Excel_Length_Index[1] = "F";
			Report_Excel_Length_Index[2] = "I";
			Report_Excel_Length_Index[3] = "L";
			Report_Excel_Atten_Index[0] = "D";
			Report_Excel_Atten_Index[1] = "G";
			Report_Excel_Atten_Index[2] = "J";
			Report_Excel_Atten_Index[3] = "M";
			Report_Excel_TotalLoss_Index[0] = "E";
			Report_Excel_TotalLoss_Index[1] = "H";
			Report_Excel_TotalLoss_Index[2] = "K";
			Report_Excel_TotalLoss_Index[3] = "N";
			Current_Time_State = 100;

			for (int i = 0; i < Constants.MaxEmailReceiver; ++i)
			{
				emails[i] = Utilities.GetAppRegistryString("EmailReceiver" + i.ToString(), "");
			}
			emailServer = Utilities.GetAppRegistryString("EmailServer", "smtp.gmail.com");
			emailServerEncryptedConnection = Utilities.GetAppRegistry("EmailServerEncryptedConnection", true);
			emailServerUserID = Utilities.GetAppRegistryString("EmailServerUserID", "foms.submarine@gmail.com");
			emailServerPassword = Utilities.GetAppRegistryString("EmailServerPassword", "001a4097");
			emailServerPort = (uint)Utilities.GetAppRegistry("EmailServerPort", 25);
			emailServerUseDefaultPort = Utilities.GetAppRegistry("EmailServerUseDefaultPort", true);
			emailSendFrom = Utilities.GetAppRegistryString("EmailSendFrom", "foms.submarine@gmail.com");
			emailSendTo = Utilities.GetAppRegistryString("EmailSendTo", "foms.qp.report@gmail.com");
			emailSendTheme = Utilities.GetAppRegistryString("EmailSendTheme", "LIC - FOMS #2");

			str_Report_Subject = "Optical Fiber Measurement Daily Report"; //Subject of E-mail
		}

		private void Form_Closed(object sender, FormClosedEventArgs e)
		{
			if (VNCViewer.IsConnected)
				VNCViewer.Disconnect();

			if (accessDB != null)
			{
				accessDB.Close();
			}

			windowEventAlarm.Close();

			Utilities.KillProcess("FOMSSubmarine");
		}

		//********************************************************
		//* Instruments Connection                               *
		//********************************************************

		private void cb_OTDRConnect_Click(object sender, EventArgs eventArgs)  // Using TCP/IP
		{
			string ipAddress = txt_IPaddress.Text;
			if (string.IsNullOrEmpty(ipAddress))
            {
				MessageBox.Show(this, "Please input correct address!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			cb_OTDRConnect.Enabled = false;

			try
			{
				if (otdr.IsConnected)
				{
					DisconnectOTDR();
					return;
				}
				else if (otdr.ConnectionStatus == ConnectionStatus.Disconnected)
				{
					otdr.Connect(ipAddress, otdrPort);
				}
				else
                {
					DebuggingHelper.Output("Connecting...");
					return;
				}

				Application.DoEvents();

				// Write to message text box
				txt_Messages.Text = "";
				txt_Messages.SelectedText = "Connection Status: Connecting" + Environment.NewLine;
				txt_Messages.Refresh();

				while (otdr.ConnectionStatus == ConnectionStatus.Connecting)
				{
					Utilities.Delay(100);
					if (otdr.IsConnected)
					{
						break;
					}
					Application.DoEvents();
				}

				if (otdr.IsConnected)
				{
					txt_Messages.SelectedText = otdr.ModelDescription;

					if (otdr.Model == OTDRControl.OTDRModel.Anritsu_MT9083)
                    {
						WebBrowser.Navigate(new Uri("http://" + ipAddress));
					}
					else if (otdr.Model == OTDRControl.OTDRModel.VIAVI_SmartOTDR)
					{
						try
						{
							VNCViewer.Connect(ipAddress, viewOnly: false, scaled: true);
						}
						catch (VncProtocolException vex)
						{
							MessageBox.Show(this,
								string.Format("Unable to connect to VNC host:\n\n{0}.\n\nCheck that a VNC host is running there.", vex.Message),
								string.Format("Unable to Connect to {0}", ipAddress),
								MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						}
						catch (Exception ex)
						{
							MessageBox.Show(this,
								string.Format("Unable to connect to host.  Error was: {0}", ex.Message),
								string.Format("Unable to Connect to {0}", ipAddress),
								MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						}
					}

					if (otdr.Model == OTDRControl.OTDRModel.Anritsu_MT9083)
					{
						otdr.SendCommand("*CLS;*RST;*WAI;INST:SEL OTDR_STD;*WAI;INST:STAT ON;*WAI;INST:STAT?;*WAI");
					}
					else if (otdr.Model == OTDRControl.OTDRModel.VIAVI_SmartOTDR)
					{
						otdr.SendCommand("*CLS;OTDS:DEF;*STB?");
					}
					Utilities.Delay(200);
					int count100ms = 0;
					while (string.IsNullOrEmpty(otdr.Response))
					{
						if (++count100ms > 10)
						{
							txt_Messages.Text = "No Response!" + Environment.NewLine;
							otdr.Disconnect();
							Utilities.Delay(200);
							txt_Messages.SelectedText = "Connection Status: Disconnected" + Environment.NewLine;
							cb_OTDRConnect.Enabled = true;
							return;
						}
						Utilities.Delay(100);
					}

					// Write to message text box
					txt_Messages.Text = "";
					txt_Messages.SelectedText = DateTime.Now.ToString("HH:mm:ss") + ": TCP/IP Connection Status: Connected" + Environment.NewLine;

					// OTDR Parameters Initialization
					// Wave Length: 1550nm / Range: 100km / Resolution: High / Pulse Width: 4us / Average Time: 60sec / Fiber IOR: 1.4667 / Fiber BSC: -81.5
#if false   // initialize OTDR parameters when connection
					if (otdr.Model == OTDRControl.OTDRModel.Anritsu_MT9083)
					{
						otdr.SendCommand("sour:wav 1550;*WAI;sour:ran 100;*WAI;sour:res 1;*WAI;sour:puls 4000;sour:aver:tim 30;*WAI;" +
							"sens:fib:ior 1.4667;*WAI;sens:fib:bsc -81.5");
					}
					else if (otdr.Model == OTDRControl.OTDRModel.VIAVI_SmartOTDR)
					{
						otdr.SendCommand("OTDS:LAS L1550;OTDS:KMR 100;OTDS:RES AUTO;OTDS:PULS P3US;OTDS:MAXT 30;" +
							"OTDS:UIOR U1,L1550,1.4667;OTDS:PSC USER;OTDS:K L1550,-81.5");
					}
#else
					OTDRSettingsForm.SendAllParameters(otdr);
#endif
					txt_Messages.SelectedText = DateTime.Now.ToString("HH:mm:ss") + ": OTDR Ready!" + Environment.NewLine + Environment.NewLine;

					if (ipOTDR != ipAddress)
					{
						ipOTDR = ipAddress;
						Utilities.SetAppRegistryString("OTDR_IPAddress", ipOTDR);
					}

					cb_OTDRConnect.Text = "Disconnect";

					GlobalVariables.Save_Button_On = 0;
					Auto_Save_Ready = 0;
				}
				else
				{
					txt_Messages.SelectedText = DateTime.Now.ToString("HH:mm:ss") + ": Connection Status: Connection Failed!" + Environment.NewLine;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			finally
			{
				cb_OTDRConnect.Enabled = true;
			}
		}

		private void DisconnectOTDR()
		{
			Measure_Stop = 1;
			Application.DoEvents();

			otdr.Disconnect();

			Application.DoEvents();

			Utilities.Delay(500);
			Application.DoEvents();

			txt_Messages.SelectedText = "Connection Status: Disconnected" + Environment.NewLine;

			cb_OTDRConnect.Enabled = true;
			cb_OTDRConnect.Text = "Connect";

			if (otdr.Model == OTDRControl.OTDRModel.VIAVI_SmartOTDR)
			{
				if (VNCViewer.IsConnected)
					VNCViewer.Disconnect();
			}
		}

		/// Serial COM Port # Selection
		private void cbox_PortName_SelectionChangeCommitted(object sender, EventArgs eventArgs)
		{
			Port_Idx = cbox_PortName.SelectedIndex + 1;
			txt_Messages.SelectedText = "Serial Port Number = COM" + Port_Idx.ToString() + Environment.NewLine;
		}

		/// Using RS232
		private void cb_OSWConnect_Click(object sender, EventArgs eventArgs)
		{
			try
			{
				if (osw.IsConnected)
				{
					DisconnectOSW();
					return;
				}

				if (osw.Model == OSWControl.OSWModel.FOD_5510)
				{
					if (osw.Connect(0x273e, 0x0007) == false)
					{
						txt_Messages.SelectedText = Date_ + "Can't open " + "USB HID 273E:0007 device!" + Environment.NewLine;
						return;
					}
				}
				else
				{
					if (osw.Connect("COM" + Port_Idx.ToString(), 9600) == false)
					{
						txt_Messages.SelectedText = Date_ + "Can't open " + "COM" + Port_Idx.ToString() + "port!" + Environment.NewLine;
						return;
					}
				}

				cb_OSWConnect.Enabled = false;

				while (osw.ConnectionStatus == ConnectionStatus.Connecting)
				{
					Utilities.Delay(200);
					Application.DoEvents();
				}

				if (osw.IsConnected)
				{
					Cur_ch_num = osw.CurrentPort;
					txt_ChNo.Text = Cur_ch_num;

					Date_ = DateTime.Now.ToString("HH:mm:ss");
					txt_Messages.SelectedText = Date_ + ": COM" + Port_Idx.ToString() + ": Open" + Environment.NewLine;
					if (osw.Model == OSWControl.OSWModel.PPI_OSWL144)
						txt_Messages.SelectedText = Date_ + ": COM Port Connection & OSW Initialization are " + osw.Response + "!" + Environment.NewLine;
					else if (osw.Model == OSWControl.OSWModel.LIGHTech_LT900)
						txt_Messages.SelectedText = Date_ + ": COM Port Connection & OSW Model is " + osw.ModelDescription + Environment.NewLine;
					else if (osw.Model == OSWControl.OSWModel.FOD_5510)
						txt_Messages.SelectedText = Date_ + ": USB HID Connection & OSW Model is " + osw.ModelDescription + Environment.NewLine;
					txt_Messages.SelectedText = Date_ + ": OSW is Ready!" + Environment.NewLine;

					cb_OSWConnect.Enabled = true;
					cb_OSWConnect.Text = "Disconnect";
					if (portOSW != Port_Idx)
					{
						portOSW = Port_Idx;
						Utilities.SetAppRegistryString("OSW_COMPort", portOSW.ToString());
					}
				}
				else
				{
					txt_Messages.SelectedText = Date_ + "OSW is not connected!" + Environment.NewLine;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			finally
			{
				cb_OSWConnect.Enabled = true;
			}
		}

		private void DisconnectOSW()
		{
			if (osw.IsConnected)
			{
				osw.Disconnect();
				cb_OSWConnect.Enabled = true;
				cb_OSWConnect.Text = "Connect";
			}

			string Date_ = DateTime.Now.ToString("HH:mm:ss");
			txt_Messages.SelectedText = Date_ + ": COM" + Port_Idx + ": Close" + Environment.NewLine;
		}

		//********************************************************************************************
		//* Reading SOR                                                                              *
		//********************************************************************************************

		private string ReadNullTerminatedString(BinaryReader reader)
		{
			string str = "";

			try
			{
				while (true)
				{
					char ch = reader.ReadChar();
					if (ch != '\0')
						str += ch;
					else
						break;
				}
			}
			catch
			{
			}

			return str;
		}

		private void ReadSORFile(string filename)
		{
			// Telcordia SR-4731, Issue 2, Jul 2011
			// OSR format reference: https://morethanfootnotes.blogspot.com/2015/07/the-otdr-optical-time-domain.html

			BinaryReader reader = null;

			string str__Subject = "";
			string str__TextBody;
			string parseFailedHeader = "ParseHeader:";

			try
			{
				Cursor = Cursors.WaitCursor;

				reader = new BinaryReader(File.Open(filename, FileMode.Open), Encoding.ASCII);

				str__Subject = "System Error"; //Subject of E-mail

				//---Start of Reading SOR File ('Don't change the variable types)------
				int k = 0;
				string blockStr;
				int bLong = 0;
				short bShort = 0;
				int nBlock = 0;     // Number of Block, Count of Block

				Time_ = DateTime.Now.ToString("HH:mm:ss");

				byte[] bytes = reader.ReadBytes(4);
				blockStr = Encoding.ASCII.GetString(bytes, 0, 3);
				if (blockStr.StartsWith("Map"))
				{
					fomsana.g_strTraceFileName = filename;
				}
				else
				{
					throw new Exception(parseFailedHeader + "SOR(v.2) Data가 아닙니다.");
				}

				bShort = reader.ReadInt16();    // Revision Number
				if (bShort < 200)
				{
					throw new Exception(parseFailedHeader + "SOR(v.2) Data가 아닙니다.");
				}
				fomsana.t_VER = bShort;

				bLong = reader.ReadInt32();
				bShort = reader.ReadInt16();
				nBlock = bShort;

				// Let's get Blocks Info. without Map Block
				var blockInfos = new Dictionary<string, int>();
				for (int i = 1; i < nBlock; ++i)
				{
					blockStr = ReadNullTerminatedString(reader);
					bShort = reader.ReadInt16();    // Block Revision Number(1-650)
					bLong = reader.ReadInt32();     // size of Block in bytes(ex, 166 bytes)
					blockInfos.Add(blockStr, bLong);
					//DebuggingHelper.Output("{0}:{1},{2}", i, blockStr, bLong);
				}
				if (blockInfos.ContainsKey("GenParams") == false ||
					blockInfos.ContainsKey("FxdParams") == false ||
					blockInfos.ContainsKey("DataPts") == false)
				{
					throw new Exception(parseFailedHeader + "ThereIsNoRequiredBlock-SOR(v2) Data가 아닙니다.");
				}

				fomsana.t_NEVENT = 0;

				while (blockInfos.Count > 0)
				{
					blockStr = ReadNullTerminatedString(reader);
					if (blockStr.StartsWith("GenParams"))
					{
						ReadGeneralParametersBlock(reader);
					}
					else if (blockStr.StartsWith("SupParams"))
					{
						ReadSupplierParametersBlock(reader);
					}
					else if (blockStr.StartsWith("FxdParams"))
					{
						ReadFixedParametersBlock(reader);
					}
					else if (blockStr.StartsWith("KeyEvents"))
					{
						ReadKeyEventsBlock(reader);
					}
					else if (blockStr.StartsWith("DataPts"))
					{
						ReadDataPointsBlock(reader);
					}
					else
					{
						if (blockInfos.TryGetValue(blockStr, out int blockSize))
						{
							reader.BaseStream.Seek(blockSize - (blockStr.Length + 1), SeekOrigin.Current);
						}
						else
						{
							throw new Exception(parseFailedHeader + "BlockNotExist-SOR(v2) Data가 아닙니다.");
						}
					}

					if (blockInfos.Remove(blockStr) == false)
					{
						throw new Exception(parseFailedHeader + "BlockNotExist-SOR(v2) Data가 아닙니다.");
					}
				}
				reader.Close();
				Cursor = Cursors.Default;

				//DebuggingHelper.Output(DateTime.Now.ToString("HH:mm:ss") + ": SOR Reading Complete");
				//---End of Reading SOR File ('Don't change the variable types)------

				//--- Parameters Rename -----------------------------------
				if (fomsana.t_SPAN1 > 0)
				{
					// 거리 정보 'm'로 변환
					fomsana.g_SPAN = (float)(fomsana.g_C0 * fomsana.t_SPAN1 * Math.Pow(10d, -10d) / fomsana.t_GI); //[m]
					//DebuggingHelper.Output("Acquisition Distance [m] = " + fomsana.g_SPAN.ToString());
				}
				else
				{
					switch (fomsana.t_UD)
					{
						case "mt":
							fomsana.g_SPAN = fomsana.t_SPAN2;  //[m] 
							break;
						case "km":
							fomsana.g_SPAN = (float)(fomsana.t_SPAN2 * 1000d);  //[m] 
							break;
						case "ft":
							fomsana.g_SPAN = (float)(fomsana.t_SPAN2 * 0.305d);  //[m] 
							break;
						case "mi":
							fomsana.g_SPAN = (float)(fomsana.t_SPAN2 * 1609.344d);  //[m] 
							break;
					}
					//DebuggingHelper.Output("Acquisition Distance [m] = " + fomsana.g_SPAN.ToString());
				}
				fomsana.g_MPNT = fomsana.g_SPAN / ((float)fomsana.t_NDAT);	// delta = 0.3890m@9208.628m
				fomsana.g_MPNT = fomsana.g_C0 * fomsana.t_DS * Math.Pow(10d, -10d) / fomsana.t_GI / 10000d; //[m/Point]
				fomsana.g_PNTM = (float)(1d / fomsana.g_MPNT);
				fomsana.g_PNTML = Convert.ToInt32(fomsana.g_PNTM - 0.5d);	// 12.7-0.5=12.2->12, 12.7->13
				fomsana.g_FOFFSETPNT = Convert.ToInt32(fomsana.t_FOFFSET * (fomsana.g_PNTM - 0.5d)); //110708
				fomsana.g_PWM = Convert.ToInt32((float)(fomsana.g_C0 * fomsana.t_PW * Math.Pow(10, -9d) / fomsana.t_GI / 2d));
				fomsana.g_PWPNT = Convert.ToInt32(fomsana.g_PWM * fomsana.g_PNTM);
				if (fomsana.g_PWPNT < 1)
				{
					fomsana.g_PWPNT = 1;
				}

				// Trace Data Setting
				fomsana.g_ddat = new double[fomsana.t_NDAT + 1];    // Distance
				fomsana.g_tdat = new float[fomsana.t_NDAT + 1];     // Trace Data (Telcordia SR-4731)
				fomsana.g_sdat = new float[fomsana.t_NDAT + 1];     // Moving Averaged Smoothing Data
				fomsana.g_ddat[0] = 0d;
				fomsana.g_tdat[0] = -1 * fomsana.t_DAT[0, 0];
				for (int i = 1; i < fomsana.t_NDAT; i++)
				{
					fomsana.g_ddat[i] = fomsana.g_ddat[i - 1] + fomsana.g_MPNT;
					fomsana.g_tdat[i] = (float)((-1d) * fomsana.t_DAT[0, i]); //20130124
				}

				k = (int)(0.2d * 2 * fomsana.g_PWPNT / 2d);
				if (k > 0)
				{
					if (k % 2 != 0)
					{
						mvavg(k);
					}
					else
					{
						mvavg(k - 1);
					}
				}
				else
				{
					mvavg(0);
				}

				float mlen = 0;
				mlen = (float)fomsana.g_ddat[fomsana.t_NDAT - 1];

				fomsana.g_dloadf = true;

				//Me.txt_Messages.SelText = Format(Now, "hh:mm:ss") & ": Read SOR: " & FileName & vbCrLf

				// Call ontb_Sel  'FOMS 해석 엔진으로 진입하는 함수 호출

				DebuggingHelper.Output("OTDR_Ana_Write");
				bool shouldSaveToExcel = (otdr.Model == OTDRControl.OTDRModel.Anritsu_MT9083);
				OTDR_Ana_Write(shouldSaveToExcel);   // OTDR 자체 해석 결과를 사용하기 위해 호출

				if (OTDR_Ana_Write_Complete == 1)
				{
					return;
				}
			}
			catch (Exception ex)
			{
				if (ex.Message.StartsWith(parseFailedHeader))
				{
					MessageBox.Show(ex.Message.Replace(parseFailedHeader, ""), Constants.InternalAppName);
				}
				else
				{
					DebuggingHelper.Output("Exception in ReadSORFile:{0}", ex.ToString());
					str__TextBody = "The FOMS system error has occurred." + Environment.NewLine + Environment.NewLine +
						"- Error Time : " + DateTime.Now.ToString("HH:mm:ss") + Environment.NewLine + Environment.NewLine +
						"- Error No: " + ex.HResult.ToString() + Environment.NewLine + Environment.NewLine +
						" - Error Description: " + ex.Message + Environment.NewLine + Environment.NewLine +
						Environment.NewLine + "Thank you!";
					if (GmailSend(str__Subject, str__TextBody))
					{
						txt_Messages.SelectedText = DateTime.Now.ToString("HH:mm:ss") +
							": Error Message Sent by E-mail" + Environment.NewLine + Environment.NewLine;
					}
					MessageBox.Show(DateTime.Now.ToString("HH:mm:ss") + ex.ToString(), Constants.InternalAppName);
				}
			}
			finally
			{
				if (reader != null)
				{
					reader.Close();
				}
				Cursor = Cursors.Default;
			}
		}

		private void ReadGeneralParametersBlock(BinaryReader reader)
		{
			short int16;
			int int32;
			string str;

			str = Encoding.ASCII.GetString(reader.ReadBytes(2));    // Language Code (ex, EN)
			str = ReadNullTerminatedString(reader);     // Cable ID
			str = ReadNullTerminatedString(reader);     // Fiber ID
			int16 = reader.ReadInt16();                 // Fiber Type (ex, 652 = SMF)
			int16 = reader.ReadInt16();                 // Nominal Wavelength (ex, 1650)
			str = ReadNullTerminatedString(reader);     // Original Location
			str = ReadNullTerminatedString(reader);     // Termination Location
			str = ReadNullTerminatedString(reader);     // Cable Code
			str = Encoding.ASCII.GetString(reader.ReadBytes(2));    // Build Condition (ex, BC, CC, RC, OT)
			int32 = reader.ReadInt32(); // User Offset (ex, 0 psec)(Jumper Length in Time, OTDR Frontend ~ Beginning of Optical Link)
			int32 = reader.ReadInt32(); // User Offset Distance (ex, 0 m)(Jumper Length in UD, OTDR Frontend ~ Beginning of Optical Link)
			str = ReadNullTerminatedString(reader);     // Operator
			str = ReadNullTerminatedString(reader);     // Comment

			//>>>>>>>>>>>>>>>>>>>>>>>> Modify Point (2013.11.13)
			// block 사이에는 일반적으로 1개의 '0' 값이 존재하는데, 특이한 경우 몇 개의 '0'과'32'가 더 존재함
			// 이에 따라 next block start pointer를 '0'과 '32' 값이 아닌 곳에서부터 시작하도록 코드 추가
			while (true)
			{
				int ch = reader.PeekChar();
				if (ch == 0 && ch == 32)
					_ = reader.ReadChar();
				else
					break;
			}
			//<<<<<<<<<<<<<<<<<<<<<<<<
		}

		private void ReadSupplierParametersBlock(BinaryReader reader)
		{
			string str;
			// Supplier Parameters (Required but Some vendor treat it as Optional) (Skip)
			str = ReadNullTerminatedString(reader);    // Supplier Name
			fomsana.t_VENDOR = str;
			str = ReadNullTerminatedString(reader);    // OTDR Mainframe ID
			fomsana.t_MODEL = str;
			str = ReadNullTerminatedString(reader);    // OTDR Mainframe S/N
			str = ReadNullTerminatedString(reader);    // Optical Module ID
			str = ReadNullTerminatedString(reader);    // Optical Module S/N
			str = ReadNullTerminatedString(reader);    // Software Revision
			str = ReadNullTerminatedString(reader);    // Other
		}

		private void ReadFixedParametersBlock(BinaryReader reader)
		{
			short int16;
			int int32;
			string str;

			int32 = reader.ReadInt32();     // Date/Time Stamp (1970.1.1 + elapsed seconds since 00:00 UCT), (ex, 1300206101)
			str = Encoding.ASCII.GetString(reader.ReadBytes(2));    // Unit of Distance (ex, km)
			fomsana.t_UD = str;
			int16 = reader.ReadInt16();    // Actual Wavelength (ex, 1625)
			fomsana.t_WAVEL = (float)(int16 / 10d);
			int32 = reader.ReadInt32();     // Acquisition Offset in 100psec(ex, 0 psec)(Jumper Length in Time, OTDR Frontend ~Beginning of Optical Link)
			int32 = reader.ReadInt32();     // Acquisition Offset Distance (ex, 0 m)(Jumper Length in UD, OTDR Frontend ~ Beginning of Optical Link)
			int nPulseWidth = reader.ReadInt16();   // Total Number of Pulse Widths Used
													// for each Pulse Width
			for (int i = 0; i < nPulseWidth; ++i)
			{
				if (i == 0)
				{
					fomsana.t_PW = reader.ReadInt16();      // Pulse Widths Used
					fomsana.t_DS = reader.ReadInt32();      // Data Spacing (one-way time, unit of 100psec, for 10000 data points)
					fomsana.t_NPNT = reader.ReadInt32();    // Number of Data Points for each Pulse Width (one-way time, unit of 100psec, for 10000 data points)
				}
				else
				{
					_ = reader.ReadInt16();
					_ = reader.ReadInt32();
					_ = reader.ReadInt32();
				}
			}

			int32 = reader.ReadInt32();     // Group Index for 1ns Pulse (ex, 146800 equals 1.46800)
			fomsana.t_GI = (float)(int32 / 100000d);
			int16 = reader.ReadInt16();    // Backscatter Coefficient (ex, 800 equals 80.0 dB)
			fomsana.t_BC = (float)(int16 / 10d);
			int32 = reader.ReadInt32();     // Number of Averages (Measurement Samples)
			fomsana.t_AVG = int32;

			// Averaging Time in sec.
			int16 = reader.ReadInt16();
			fomsana.t_AVGT = (int)(int16 / 10d);
			fomsana.t_SPAN1 = reader.ReadInt32();   // Acquition Range (one-way time, unit of 100psec, for distance range selected)
			fomsana.t_SPAN2 = reader.ReadInt32();   // Acquition Range Distance for distance range selected
			int32 = reader.ReadInt32();             // Front Panel Offset (one-way time, unit of 100psec, for distance to the front panel of OTDR)
			fomsana.t_FOFFSET = (float)(fomsana.g_C0 * int32 * Math.Pow(10, -10) / fomsana.t_GI); //[m]
			int16 = reader.ReadInt16();             // Noise Floor Level (ex, 55000 equals -55.000dB)
			fomsana.t_NFSFactor = reader.ReadInt16();   // Noise Floor Scale Factor (ex, 1000 equals 1.000)
			fomsana.t_NOISEL = (float)(-1 * int16 / 1000d);
			//t_NOISEL = CSng(-1 * bLong / t_NFSFactor)
			int16 = reader.ReadInt16();            // Power Offset at First Point(additional added attenuation)(ex, 10000 equals 10.000dB)
			fomsana.t_POffset = (float)(int16 / 1000d); //20130124
			int16 = reader.ReadInt16();            // Loss Threshold (minimum loss of event flagged as key) (ex, 200 equals 0.200dB)
			fomsana.t_LT = (float)(int16 / 1000d);
			int16 = reader.ReadInt16();    // Reflectance Threshold (minimum reflectance of event flagged as key) (ex, 40000 equals -40.000dB)
			fomsana.t_RT = (float)(-1 * int16 / 1000d);
			int16 = reader.ReadInt16();    // End-of-Fiber Threshold (minimum loss of event flagged as key) (ex, 3000 equals 3.000dB)
			fomsana.t_ET = (float)(int16 / 1000d);
			str = Encoding.ASCII.GetString(reader.ReadBytes(2));   // Trace Type (ex, ST=Standard)
			fomsana.t_TT = str;
			for (int i = 0; i < 4; ++i)
			{
				_ = reader.ReadInt32();     // Window Coordinates (the upper left and lower right of display) (ex, 0,0,0,0)
			}
		}

		private void ReadKeyEventsBlock(BinaryReader reader)
		{
			short int16;
			int int32;
			string str;
			float sumAttenuation = 0;
			int countValidEvents = 0;

			fomsana.t_NEVENT = reader.ReadInt16();  // Number of Key Events (ex, 8)'Some Error !?
			fomsana.t_EVENTS = new fomsana.t_EVENT[fomsana.t_NEVENT + 1];
			for (int i = 0; i < fomsana.t_NEVENT; ++i)
			{
				int16 = reader.ReadInt16();    // Event Number (ex, 1)
				fomsana.t_EVENTS[i].enum_Renamed = int16;
				int32 = reader.ReadInt32();     // Event Propagation Time (one-way time to event from start of link, unit of 100psec)
				fomsana.t_EVENTS[i].eloc = (float)(fomsana.g_C0 * int32 * 100d * Math.Pow(10, -12) / fomsana.t_GI); //[m]
				int16 = reader.ReadInt16();    // Attenuation Coefficient Lead-in Fiber in dB/Km (ex, 350 equals 0.350)
				if (int16 != 0)
				{
					sumAttenuation += int16;
					++countValidEvents;
				}
				fomsana.t_EVENTS[i].eac = (float)(int16 / 1000d);
				int16 = reader.ReadInt16();    // Event Loss in dB (ex, 500 equals 0.500)
				fomsana.t_EVENTS[i].Eloss = (float)(int16 / 1000d);
				int32 = reader.ReadInt32();     // Event Reflectance in dB (ex, -45000 equals -45.000)
				fomsana.t_EVENTS[i].erl = (float)(int32 / 1000d);
				str = Encoding.ASCII.GetString(reader.ReadBytes(6));   // Event Code
				fomsana.t_EVENTS[i].ecode = str;
				str = Encoding.ASCII.GetString(reader.ReadBytes(2));   // Loss Measurement Technique (ex, 2P)
				fomsana.t_EVENTS[i].emethod = str;
				for (int j = 0; j < 5; ++j)
				{
					_ = reader.ReadInt32(); // Marker Locations (ex, 0,0,0,0,0)
				}
				str = ReadNullTerminatedString(reader);    // Comment about the event
				fomsana.t_EVENTS[i].enote = str;
			}

			int32 = reader.ReadInt32();     // End-to-End Loss of Link in dB (ex, 25000 equals 25.000)
			fomsana.t_E2EL = (float)(int32 / 1000d);
			_ = reader.ReadInt32();         // End-to-End Marker Position1 in 100psec(ex, 246004)
			_ = reader.ReadInt32();         // End-to-End Marker Position2 in 100psec(ex, 444422)
			int16 = reader.ReadInt16();     // Optical Return Loss in dB (ex, 25000 equals 25.000)
			fomsana.t_ORL = (float)(int16 / 1000d);
			_ = reader.ReadInt32();         // Optical Return Loss Marker Position1 in 100psec(ex, 246004)
			_ = reader.ReadInt32();         // Optical Return Loss Marker Position2 in 100psec(ex, 444422)

			if (otdr.Model == OTDRControl.OTDRModel.VIAVI_SmartOTDR)
			{
				float averageAttenuation = (sumAttenuation / countValidEvents) / 1000;
				txt_SectionLoss.Text = string.Format("{0:#0.000#}", averageAttenuation);
			}
		}

		private void ReadDataPointsBlock(BinaryReader reader)
		{
			int int32;

			fomsana.t_NDAT = reader.ReadInt32();    // Number of data points
			int nTraces = reader.ReadUInt16();      // Number of traces
			fomsana.t_DAT = new float[nTraces, fomsana.t_NDAT];

			for (int i = 0; i < nTraces; ++i)
			{
				int32 = reader.ReadInt32();     // Total Data Points Using Sacale Factor i + 1, only for checking the t_NDAT
				ushort factor = reader.ReadUInt16();    // Scale Factor i (ex, 1000 equals 1.000)
				fomsana.t_DSFactor = (float)(factor / 1000d);   // only for Loss Calculation	//20130124

				// Relative Power Level using Scale Factor i + 1 (ex, 25000 equals 25.000 dB
				for (int j = 0; j < fomsana.t_NDAT; ++j)
				{
					ushort level = reader.ReadUInt16();
					fomsana.t_DAT[i, j] = (float)(level / 1000d);
				}
			}
		}


		// 20210413 이후삭제 내용 아래로 추가
		private object Auto_Threshold()
		{
			if (!fomsana.g_dloadf)
			{
				return null;
			}
			
			Diffdeltav = (float)fomsana.g_MPNT; //[m]'20110823
			Diffdeltapnt = Convert.ToInt32(Diffdeltav * fomsana.g_PNTM);
			
			double lpnt = 0;
			double rpnt = fomsana.t_NDAT - 1;
			int scalepnt = Convert.ToInt32(rpnt - lpnt - 1);
			S1pnt = Convert.ToInt32(lpnt + scalepnt / 2d);

			//S1pnt : just After 1st-Splitter Peak and just Before Next Peak Candidate   
			//S2pnt : just After Last ONT Peak including Ghost Peak and just Before Next Peak Candidate   
			//20130124
            mAvgv = 0; //val(mavgc.Text)
			if (Convert.ToSingle(rSectionLine) < 0f || Convert.ToSingle(rSectionLine) > fomsana.g_SPAN - 1)
			{
				return null;
			}
			S1pnt = Convert.ToInt32(Convert.ToSingle(rSectionLine) * fomsana.g_PNTM);
			S2pnt = S1pnt;
			int dlpnt = 0;
			int drpnt = S2pnt - 1;
			
			D1dat = Array.CreateInstance(typeof(float), new int[] { drpnt - dlpnt + 1 }, new int[] { dlpnt });
			D2dat = Array.CreateInstance(typeof(float), new int[] { drpnt - dlpnt + 1 }, new int[] { dlpnt });
			D1dat.SetValue(fomsana.g_sdat[dlpnt + 1] - fomsana.g_sdat[dlpnt], dlpnt);
			Dmax[0] = (float)D1dat.GetValue(dlpnt);
			dmin[0] = Dmax[0];
			
			//1) 1st Differentiation---------------------------------------------------------
			if (Diffdeltav < 0.1f)
			{
				Diffdeltav = 0.1f;
			} //Diffdeltav=Differential Delta[m]
			D1dat.SetValue(fomsana.g_sdat[1] - fomsana.g_sdat[0], 0);
			int tempForEndVar = drpnt - 2;
			for (double i = 1; i <= tempForEndVar; i++)
			{
				//drpnt:Drop Point:S2pnt-1 <- drpnt-2까지 loop하는 이유는 Xcdiff1st에서 데이터의 index+2 point까지 연산하기 때문
			    //''''D1dat(i) = cdiff1st(Diffdeltav, i)
				D1dat.SetValue(Xcdiff1st(Diffdeltav, Convert.ToInt32(i)), Convert.ToInt32(i));
				if (((float)D1dat.GetValue(Convert.ToInt32(i))) > Dmax[0])
				{
					Dmax[0] = (float)D1dat.GetValue(Convert.ToInt32(i));
				} //Dmax value update
				if (((float)D1dat.GetValue(Convert.ToInt32(i))) <= dmin[0])
				{
					dmin[0] = (float)D1dat.GetValue(Convert.ToInt32(i));
				} //Dmin value update
            }
			D1dat.SetValue(fomsana.g_sdat[drpnt - 1] - fomsana.g_sdat[drpnt - 2], drpnt - 1);

			//2) 2nd Differentiation---------------------------------------------------------
			D2dat.SetValue((float)D1dat.GetValue(dlpnt + 1), dlpnt);
			Dmax[1] = (float)D2dat.GetValue(dlpnt);
			dmin[1] = Dmax[1];
			D2dat.SetValue((float)D1dat.GetValue(0), 0);
			int tempForEndVar2 = drpnt - 2;
			for (double i = 1; i <= tempForEndVar2; i++)
			{
				//''''D2dat(i) = cdiff2nd(Diffdeltav, i)
				D2dat.SetValue(Xcdiff2nd(Diffdeltav, Convert.ToInt32(i)), Convert.ToInt32(i));
				if (((float)D2dat.GetValue(Convert.ToInt32(i))) > Dmax[1])
				{
					Dmax[1] = (float)D2dat.GetValue(Convert.ToInt32(i));
				}
				if (((float)D2dat.GetValue(Convert.ToInt32(i))) <= dmin[1])
				{
					dmin[1] = (float)D2dat.GetValue(Convert.ToInt32(i));
				}
			}
			D2dat.SetValue((float)D1dat.GetValue(drpnt - 2), drpnt - 1);

			//3) Peak Threshold Calculation---------------------------------------------
			RMSLvl = fomsana.t_NOISEL; //Default
            IEC98Lvl = (float)(RMSLvl + fomsana.g_IEC);
			RMSSdv = (float)((fomsana.g_IEC + fomsana.g_MRL) / 2.4d); //Default
			//    Select Case SIdx   
			//        Case 0     'Only P2P Case
			lpnt = 0;
			rpnt = S1pnt - 1;
			PThvh[0] = getThvhgt(Convert.ToInt32(lpnt), Convert.ToInt32(rpnt));
			pthvt = Single.Parse(StringsHelper.Format(PThvh[0], "#0.0#"));
			if (S1pnt < fomsana.t_NDAT - 1)
			{
				lpnt = S1pnt;
				rpnt = fomsana.t_NDAT - 1;
				RMSSdv = getThvhgt(Convert.ToInt32(lpnt), Convert.ToInt32(rpnt));
				RMSLvl = (float)((-1d) * getRms(Convert.ToInt32(lpnt), Convert.ToInt32(rpnt)));
				IEC98Lvl = (float)(RMSLvl + fomsana.g_IEC);
			}
			NoisePeak = (float)(RMSLvl + 2.4d * RMSSdv);
			plvlt = Single.Parse(StringsHelper.Format(NoisePeak, "#0.0#"));
			
			
			//4) Leanear Start Point after Launching : Restore Fiber Attn <= 0.3dB/km-----------
			double dbv = 0;
			double mtv = 0;
			float xx1 = 0;
			float xx2 = 0;
			double yy1 = 0;
			double yy2 = 0;
			float dbkmv = 0;
			int lsawidthpnt = 10 * fomsana.g_PWPNT; 
			Flpnt = Convert.ToInt32(fomsana.g_PWPNT + fomsana.g_FLENGTH * fomsana.g_PNTM); //Default
			double lpt = fomsana.g_PWPNT;
			int tempForEndVar3 = drpnt - lsawidthpnt - 1; 
			for (double i = lpt; i <= tempForEndVar3; i++)
			{
				LSA(i, i + lsawidthpnt);
				if (!cerrorF)
				{
					xx1 = (float)i;
					xx2 = (float)(i + lsawidthpnt);
					yy1 = LA * fomsana.g_ddat[Convert.ToInt32(xx1)] + LB; 
					yy2 = LA * fomsana.g_ddat[Convert.ToInt32(xx2)] + LB; dbv = (-1d) * (yy2 - yy1); 
					mtv = fomsana.g_ddat[Convert.ToInt32(xx2)] - fomsana.g_ddat[Convert.ToInt32(xx1)];
					if (mtv > 0.1d) 
					{
						dbkmv = (float)(dbv / mtv * 1000d); 
					}
					else 
					{ 
						dbkmv = 0f;
					}
					if (dbkmv > 0f && dbkmv < ((float)fomsana.g_LSAATTN))
					{
						Flpnt = Convert.ToInt32(i + fomsana.g_PWPNT); 
						break; 
					}
				}
			}
			LLevel = fomsana.g_sdat[Flpnt]; //Default
			
			S1loss = 0.2f;   
			S2loss = (float)(fomsana.Log10(4d) * 3d / fomsana.Log10(2));   
			S3loss = (float)(fomsana.Log10(8d) * 3d / fomsana.Log10(2));   
			
			//    panab.Enabled = True '!!!   
			//    diffsaveb.Enabled = True   
			//    Screen.MousePointer = 0
			
			//    Me.txt_Messages.SelText = Format(Now, "hh:mm:ss") & ": Auto Threshold OK" & vbCrLf
			
			Module_Cal_Complete = 0; 
			Analysis_Block(); 
			return null;
		}

		private void Analysis_Block()
		{
			double cptv1 = 0; 
			double cpt = 0;
			if (!fomsana.g_dloadf) 
			{ 
				return; 
			}   
			//    If Not bSplitF Then Exit Sub
			double i = 0;   
			//20130311 Start----------------
            Ppnt = new int[201];   
			E1pnt = new int[201];   
			
			Bpnt = new int[101];   
			Blpnt = new int[101];   
			Btype = new bool[101]; //True = Refl, False = Bend
			
			Epnt = new int[101]; //20130311
			Evloss = new float[101];   
			Etype = new bool[101]; //True = Refl, False = Bend
			Ecalc = new bool[101]; //True = LSA, False = 2P           
									//20130311 End---------------

			string Event_Time = "";
			int Event_Number = 0; 
			float[] Dis_Arr = new float[101];   
			//
            Cursor = Cursors.WaitCursor;   
			//   
			//            If g_diff Then Unload foms_diff '20121114   
			//
            lpnt = Flpnt; //1st Linear Point

			//Only P2P Case -------------------
			S1pnt = Convert.ToInt32(Convert.ToSingle(rSectionLine) * fomsana.g_PNTM);
			// just After 1st-Splitter Peak and just Before Next Peak Candidate

			if (S1pnt < Flpnt)   
			{
				S1pnt = Flpnt;   
			}   
			if (S2pnt > fomsana.t_NDAT - 1)   
			{    
				S2pnt = fomsana.t_NDAT - 1;   
			}

			//EoF & Threshold------------------
            rpnt = S1pnt - 1;   
			EofPnt = S1pnt;   
			
			PThvh[0] = pthvt; //Threshold for Peak Height
            PThvl[0] = plvlt; //Threshold for Peak Level
			S2pnt = S1pnt;   
			//
            if (rpnt > EofPnt - 1)   
			{
				rpnt = EofPnt - 1;
			}
			
			//1)Reflect Peak Search Start---------------------
			double p0pnt = 0; 
			int e1minpnt = 0; 
			double plpnt = 0; 
			double prpnt = 0; 
			float e1minlvl = 0; 
			float pmax = 0; 
			float maxdelta = 0; //20121120
								//    Debug.Print Format(Now, "hh:mm:ss") & ": 1) REflect Peak Search Start"

			float old_e1minlvl = 0; //Default'20120713
            float old_plvl = 0; //20120713-1 //Default'20120713-1
			Pcnt = 0;   
			float pheight = 0; //20120713
			double old_pwidth = 2 * fomsana.g_PWPNT; //g_PWPNT(Point Resolution by PW) = g_PWM(Distance Res by PW[m]) x g_PNTM(point/m)
			int new_pwidth = 2 * fomsana.g_PWPNT;
			double edeltapnt = 2 * fomsana.g_PWPNT; 
			
			ripplet = 0.5f; //Ripple Level
            bendt = Single.Parse(txt_Threshold.Text); //Loss Threshold

			PDeltav = ripplet; 
			PBendv = bendt; //20121218
            int sv0 = 1;   
			int sv1 = 1;   
			sratio = sv0 * sv1;   
			
			int tempForEndVar = rpnt - 1;   
			for (i = lpnt + 1; i <= tempForEndVar; i++)   
			{    
				if (Pcnt > 199)    
				{    
					break;    
				}    
				if (i > EofPnt)    
				{     
					break;    
				}    
				//1-1)Peak Condition
				if (((float)D1dat.GetValue(Convert.ToInt32(i - 1))) >= 0 && ((float)D2dat.GetValue(Convert.ToInt32(i))) <= 0 && ((float)D1dat.GetValue(Convert.ToInt32(i + 1))) <= 0)
				{
					//(1)1st(+/-), 2nd(-), Positive Peak Point      
					//P2P
                    PThvlvl = PThvl[0]; //Threshold for Peak Level
                    PThvhgt = PThvh[0]; //Threshold for Peak Height     
					
					//1-2) Peak Candidate
					if (fomsana.g_sdat[Convert.ToInt32(i)] > PThvlvl)
					{
						//Threshold보다 큰 값이면
                        p0pnt = i;      
						//''''Candidates Display@Current Zoom for Debug      
						//''''tp.DrawWidth = 2      
						//''''tp.PSet (p0pnt, g_sdat(p0pnt)), QBColor(15)

						//1-3) Event Level for Peak Candidate Search
                        e1minpnt = Convert.ToInt32(p0pnt - 1); //Default
						e1minlvl = fomsana.g_sdat[e1minpnt]; //Default
						maxdelta = fomsana.g_sdat[Convert.ToInt32(p0pnt)] - e1minlvl;
						double tempForEndVar2 = p0pnt - edeltapnt; 
						for (double j = p0pnt - 1; j >= tempForEndVar2; j--)
						{
							//Backward for Default Exact Min '20121120
                            if (fomsana.g_sdat[Convert.ToInt32(p0pnt)] - fomsana.g_sdat[Convert.ToInt32(j)] > maxdelta)       
							{
								maxdelta = fomsana.g_sdat[Convert.ToInt32(p0pnt)] - fomsana.g_sdat[Convert.ToInt32(j)];        
								e1minpnt = Convert.ToInt32(j);
								e1minlvl = fomsana.g_sdat[Convert.ToInt32(j)];
							}
							else
							{
								if (fomsana.g_sdat[Convert.ToInt32(p0pnt)] - fomsana.g_sdat[Convert.ToInt32(j)] < (maxdelta - PDeltav))
								{ 
									//20121123
                                    break;        
								}
							}      
						}      
						
						//1-4) Real Peak Decision
						pheight = fomsana.g_sdat[Convert.ToInt32(p0pnt)] - e1minlvl; //Peak Height
						if (pheight >= PThvhgt)      
						{
							Ppnt[Pcnt] = Convert.ToInt32(p0pnt);
							pmax = fomsana.g_sdat[Convert.ToInt32(p0pnt)]; 
							double tempForEndVar3 = p0pnt + ((float)(0.5d * fomsana.g_PWPNT)); 
							for (int k = Convert.ToInt32(p0pnt + 1); k <= tempForEndVar3; k++)
							{
								if (k > (fomsana.t_NDAT - 1)) 
								{ 
									break; 
								}
								if (fomsana.g_sdat[k] > pmax)
								{
									pmax = fomsana.g_sdat[k]; 
									Ppnt[Pcnt] = k; //Peak Vary in test by test -> Maximum of Positive Peak
                                    p0pnt = k;
								}       
							}
							
							//PeakWidth@PThvhgt Down for Overlap Check
                            plpnt = p0pnt - fomsana.g_PWPNT;       
							int tempForEndVar4 = Flpnt;       
							for (int k = Convert.ToInt32(p0pnt); k >= tempForEndVar4; k--)       
							{        
								if ((fomsana.g_sdat[Convert.ToInt32(p0pnt)] - fomsana.g_sdat[k]) > PThvhgt)        
								{         
									plpnt = k;         
									break;        
								}       
							}
							prpnt = p0pnt + fomsana.g_PWPNT; 
							int tempForEndVar5 = rpnt - 1; 
							for (int k = Convert.ToInt32(p0pnt); k <= tempForEndVar5; k++)
							{
								if ((fomsana.g_sdat[Convert.ToInt32(p0pnt)] - fomsana.g_sdat[k]) > PThvhgt) 
								{
									prpnt = k;
									break;
								}
							}
							old_pwidth = prpnt - plpnt; 
							old_e1minlvl = fomsana.g_sdat[e1minpnt];
							old_plvl = fomsana.g_sdat[Convert.ToInt32(p0pnt)];
							i = Convert.ToInt32(p0pnt + fomsana.g_PWPNT * 1d); //Shift for Non-Overlap Resolution
							Pcnt++;      
						}
						else if (i > S1pnt && pheight > PDeltav)
						{
							//1-5)Overlap Range (- Distribution Splitter - Drop Splitter - ONT)
							//PeakWidth@PThvhgt Down for Overlap Check
                            plpnt = p0pnt - fomsana.g_PWPNT;     
							int tempForEndVar6 = Flpnt;
							for (int k = Convert.ToInt32(p0pnt); k >= tempForEndVar6; k--)
							{
								if ((fomsana.g_sdat[Convert.ToInt32(p0pnt)] - fomsana.g_sdat[k]) > PThvhgt)
								{
									plpnt = k; 
									break; 
								}
							}
							prpnt = p0pnt + fomsana.g_PWPNT; 
							int tempForEndVar7 = rpnt - 1;
							for (int k = Convert.ToInt32(p0pnt); k <= tempForEndVar7; k++)
							{
								if ((fomsana.g_sdat[Convert.ToInt32(p0pnt)] - fomsana.g_sdat[k]) > PThvhgt)
								{ 
									prpnt = k; 
									break; 
								}
							}
							new_pwidth = Convert.ToInt32(prpnt - plpnt - 1);       
							
							//1-6) Overlap Decision
							if ((fomsana.g_sdat[Convert.ToInt32(p0pnt)] - old_e1minlvl) > PThvhgt || new_pwidth > (old_pwidth * 1d))
							{
								if (Math.Abs(old_plvl - fomsana.g_sdat[Convert.ToInt32(p0pnt)]) < fomsana.g_FRNL)
								{ 
									//Attenuation Region by Time-Domain Multi-Reflection
                                    Ppnt[Pcnt] = Convert.ToInt32(p0pnt);

									//1-7) Calibration of Peak Location
                                    pmax = fomsana.g_sdat[Convert.ToInt32(p0pnt)];
									double tempForEndVar8 = p0pnt + ((float)(1d * fomsana.g_PWPNT));
									for (int k = Convert.ToInt32(p0pnt + 1); k <= tempForEndVar8; k++)
									{
										//0.5 -> 1.0 for Peak Search Error at 3ns P.W.
                                        if (k > S2pnt)       
										{
											break;          
										}          
										if (fomsana.g_sdat[k] > pmax)
										{
											pmax = fomsana.g_sdat[k]; 
											Ppnt[Pcnt] = k; 
											p0pnt = k; 
										}
									}
									i = Convert.ToInt32(p0pnt + fomsana.g_PWPNT * 1d); //Shift for Overlap Resolution  '0.5 -> 1.0 for Peak Search Error at 3ns P.W.
									Pcnt++;
									}
								}
							}
						}
					}
				}

				//2) Peak Event Location Start(20130305)--------------
				double d2maxpnt = 0; 
				double dminpnt = 0; 
				double oldpeakpnt = 0; 
				int newpeakpnt = 0; 
				float dminlvl = 0; 
				float d2maxlvl = 0;
				
				//    Debug.Print Format(Now, "hh:mm:ss") & ": 2) Peak Event Location Start"
				
				if (Pcnt > 0)
				{
					edeltapnt = 5 * fomsana.g_PWPNT; //20130110
					int tempForEndVar9 = Pcnt - 1;    
					for (i = 0; i <= tempForEndVar9; i++)
					{
						p0pnt = Ppnt[Convert.ToInt32(i)]; 
						if (i == 0) 
						{
							oldpeakpnt = Ppnt[0] - edeltapnt; 
						} //20130304'Default
						if (oldpeakpnt <= Flpnt) 
						{ 
							oldpeakpnt = Flpnt; 
						} //20130304
                         newpeakpnt = Convert.ToInt32(p0pnt);
						
						//2-1) Backward for Default
                        dminpnt = newpeakpnt - 1;     
						dminlvl = fomsana.g_sdat[Convert.ToInt32(dminpnt)];     
						double tempForEndVar10 = oldpeakpnt;     
						for (double j = newpeakpnt - 1; j >= tempForEndVar10; j--)     
						{      
							if (fomsana.g_sdat[Convert.ToInt32(j)] < dminlvl)
							{ 
								dminpnt = j; 
								dminlvl = fomsana.g_sdat[Convert.ToInt32(j)]; 
							}
						}
						
						//2-2) Max of 2nd-Diff.
                        d2maxpnt = dminpnt - 1;     
						d2maxlvl = (float)D2dat.GetValue(Convert.ToInt32(d2maxpnt));     
						int tempForEndVar11 = newpeakpnt - 1;     
						for (double j = dminpnt; j <= tempForEndVar11; j++)
						{
							if (((float)D2dat.GetValue(Convert.ToInt32(j))) >= d2maxlvl) 
							{
								d2maxpnt = j; d2maxlvl = (float)D2dat.GetValue(Convert.ToInt32(j));
							}
						}
						
						//2-3) Decision
						if (((float)D1dat.GetValue(Convert.ToInt32(d2maxpnt - 1))) < 0f && ((float)D1dat.GetValue(Convert.ToInt32(d2maxpnt))) > 0f && ((float)D2dat.GetValue(Convert.ToInt32(d2maxpnt))) > 0f)
						{
							//1st(-/+), 2nd(+), Negative Peak
                            dminpnt = d2maxpnt;     
						}     
						else
						{      
							dminpnt = d2maxpnt - 1;      
							dminlvl = fomsana.g_sdat[Convert.ToInt32(dminpnt)];      
							int tempForEndVar12 = newpeakpnt - 1;      
							for (double j = d2maxpnt; j <= tempForEndVar12; j++)     
							{
								if (((float)D2dat.GetValue(Convert.ToInt32(j))) > 0f && fomsana.g_sdat[Convert.ToInt32(j)] <= dminlvl) 
								{
									dminpnt = j; 
									dminlvl = fomsana.g_sdat[Convert.ToInt32(j)]; 
								}
							}
						}
						E1pnt[Convert.ToInt32(i)] = Convert.ToInt32(dminpnt); 
						oldpeakpnt = p0pnt;
					}
				}

			//2) Peak Event Location End(20130305)-----------------------

			//3) 1st Peak for Launching Level-------------------------------------------------------------------------------------------------------------
			//    Debug.Print Format(Now, "hh:mm:ss") & ": 3) 1st Peak for Launching Level"
            if (Pcnt > 0)    
			{    
				Frpnt = E1pnt[0] - fomsana.g_PWPNT;   
			}

			//4) Check the Frpnt
            if (Frpnt > (S1pnt - 2 * fomsana.g_PWPNT))   
			{    
				Frpnt = S1pnt - 2 * fomsana.g_PWPNT;   
			}
			//    Debug.Print Format(Now, "hh:mm:ss") & ": 4) Check the Frpnt"
			
			//5) Launching Level-------------------------------------------------------------------------------------------------------
			//    Debug.Print Format(Now, "hh:mm:ss") & ": 5) Launching Level"
            if ((OTDR1stEventPnt > (Flpnt + 10 * fomsana.g_PWPNT)) && (OTDR1stEventPnt < Frpnt)) 
			{    
				Frpnt = OTDR1stEventPnt;   
			}
			LSA(Flpnt, Frpnt);
			if (!cerrorF)
			{
				LLevel = (float)(LA * fomsana.g_ddat[0] + LB);
			}

			//----------------------------------------------------------------------------------------
			RMSDNR = LLevel - RMSLvl;
			IEC98DNR = LLevel - IEC98Lvl;
			//----------------------------------------------------------------------------------------

			//6) Calculating of the 1st Splitter Base Level-----------------------------------------------------------------------------------------------
			//    Debug.Print Format(Now, "hh:mm:ss") & ": 6) Calculating of the 1st Splitter Base Level"
			SLevel = LLevel; //Default
			if (Pcnt > 0)
			{
				int tempForEndVar13 = Pcnt - 1;
				for (i = 0; i <= tempForEndVar13; i++)
				{
					if (E1pnt[Convert.ToInt32(i)] < S1pnt)
					{
						SLevel = fomsana.g_sdat[E1pnt[Convert.ToInt32(i)] - fomsana.g_PWPNT];
					}
					else
					{
						break;
					}
				}
			}

			//7) Calculating the Peak Width@PThv.-------------------------------------------------------------------------------------
			//    Debug.Print Format(Now, "hh:mm:ss") & ": 7) Calculating the Peak Width@PThv."
			PWIDPNT = new int[Pcnt + 1];
			if (Pcnt > 0)
			{
				int tempForEndVar14 = Pcnt - 1;
				for (i = 0; i <= tempForEndVar14; i++)
				{
					plpnt = Ppnt[Convert.ToInt32(i)] - fomsana.g_PWPNT;
					int tempForEndVar15 = Flpnt;
					for (double j = Ppnt[Convert.ToInt32(i)]; j >= tempForEndVar15; j--)
					{
						if ((fomsana.g_sdat[Ppnt[Convert.ToInt32(i)]] - fomsana.g_sdat[Convert.ToInt32(j)]) > ((float)fomsana.g_PWID))
						{
							plpnt = j;
							break;
						}
					}
					prpnt = Ppnt[Convert.ToInt32(i)] + fomsana.g_PWPNT;
					int tempForEndVar16 = Ppnt[Pcnt - 1];
					for (double j = Ppnt[Convert.ToInt32(i)]; j <= tempForEndVar16; j++)
					{
						if ((fomsana.g_sdat[Ppnt[Convert.ToInt32(i)]] - fomsana.g_sdat[Convert.ToInt32(j)]) > ((float)fomsana.g_PWID))
						{
							prpnt = j;
							break;
						}
					}
					PWIDPNT[Convert.ToInt32(i)] = Convert.ToInt32(prpnt - plpnt + 1);
				}
				EofPnt = E1pnt[Pcnt - 1]; //Candidate
			}
			//
			//    Call tdraw(1)
			//
			//8) Scan the Feeder Loss Events using the LSA1 Scan between Peaks------------
			//    Debug.Print Format(Now, "hh:mm:ss") & ": 8) Scan the Feeder Loss Events using the LSA1 Scan between Peaks"
			//8-1) Peak Count on Feeder
			int FPCnt = 0;
			if (Pcnt > 0)
			{
				FPCnt = 0;
				int tempForEndVar17 = Pcnt - 1;
				for (i = 0; i <= tempForEndVar17; i++)
				{
					if (E1pnt[Convert.ToInt32(i)] < S1pnt)
					{
						FPCnt++; //Incl. 1st-Splitter Peak
					}
				}
			}
			//    Debug.Print Format(Now, "hh:mm:ss") & ": 8-1 Finish"

			//20130311~20130314~-----------------------------------------------
			//8-2) Peak2Peak Sectioning and Loss Calculation
			double dbv = 0;
			double mtv = 0;
			double yy1 = 0;
			double yy2 = 0;
			float dbkmv = 0;
			double lpt = 0;
			double rpt = 0;
			double bmaxpnt = 0;
			int bendingwidth = 0;
			float bmaxv = 0;
			bool lsaf = false; //True/False(LSA/2P)
			int oldbpnt = 0; //20130313

			int lsawidthpnt = 10 * fomsana.g_PWPNT; //Default

			//---------- 종단 피크 없는 경우 해석을 수행하지 않기 때문에 케이블 종단점을 강제로 종단피크 위치로 설정
			if (FPCnt == 0)
			{
				FPCnt = 1;
				E1pnt[0] = (int)GlobalVariables.Cable_Len; //S1pnt
														   //ElseIf 최종 피크가 50km 미만인 경우에도, E1pnt 강제 적용해줘야 함.
			}
			//-------------------------------------------

			int p2pnt = 0;
			float cptv2 = 0;
			if (FPCnt > 0)
			{
				FSectionPnt = new int[FPCnt + 1, 3];
				FSectionPnt[0, 0] = Flpnt;
				FSectionPnt[0, 1] = E1pnt[0];
				int tempForEndVar18 = FPCnt - 1;
				for (i = 1; i <= tempForEndVar18; i++)
				{
					FSectionPnt[Convert.ToInt32(i), 0] = Ppnt[Convert.ToInt32(i - 1)] + fomsana.g_PWPNT; //Default
																										 //8-2-1) Linear Start Point after Peak : Restore Fiber Attn <= 0.3dB/km+PWPNT
					int tempForEndVar19 = E1pnt[Convert.ToInt32(i)] - lsawidthpnt - 1;
					for (double j = FSectionPnt[Convert.ToInt32(i), 0]; j <= tempForEndVar19; j++)
					{
						lpt = j;
						rpt = j + lsawidthpnt;
						if (rpt > E1pnt[Convert.ToInt32(i)] - 1)
						{
							rpt = E1pnt[Convert.ToInt32(i)] - 1;
						}
						LSA(lpt, rpt);
						if (!cerrorF)
						{
							yy1 = LA * fomsana.g_ddat[Convert.ToInt32(lpt)] + LB; //Left LSA
							yy2 = LA * fomsana.g_ddat[Convert.ToInt32(rpt)] + LB; //Right LSA
							dbv = (-1d) * (yy2 - yy1);
							mtv = fomsana.g_ddat[Convert.ToInt32(rpt)] - fomsana.g_ddat[Convert.ToInt32(lpt)]; //Distance[m] of between rpt and lpt
							if (mtv > 0.1d)
							{
								dbkmv = (float)(dbv / mtv * 1000d);
							}
							else
							{
								dbkmv = 0f;
							}
							if (dbkmv > 0f && dbkmv < ((float)fomsana.g_LSAATTN))
							{
								FSectionPnt[Convert.ToInt32(i), 0] = Convert.ToInt32(j + fomsana.g_PWPNT);
								j += fomsana.g_PWPNT;
								break;
							}
						}
					}
					//           Debug.Print Format(Now, "hh:mm:ss") & ": 8-2-1 Finish"
					//8-2-2) End of Section
					FSectionPnt[Convert.ToInt32(i), 1] = E1pnt[Convert.ToInt32(i)]; //Feeder End@Splitter
				}
				//Debug.Print Format(Now, "hh:mm:ss") & ": 8-2-2 Finish"

				//8-2-3) Event Scan by LSA1 and Differentiation
				Bcnt = 0;
				bendingwidth = 2 * fomsana.g_PWPNT; //Default(10% Rising, 10% Settle)
													//       Debug.Print Format(Now, "hh:mm:ss") & ": FPCnt = " & FPCnt
				int tempForEndVar20 = FPCnt - 1;
				for (i = 0; i <= tempForEndVar20; i++)
				{
					if (Bcnt > 99)
					{
						break;
					}
					oldbpnt = FSectionPnt[Convert.ToInt32(i), 0]; //Default
																  //8-2-3-1) Bending Scan by LSA1
					int tempForEndVar21 = FSectionPnt[Convert.ToInt32(i), 1] - 1;
					for (double j = FSectionPnt[Convert.ToInt32(i), 0]; j <= tempForEndVar21; j++)
					{
						lpt = j;
						rpt = j + bendingwidth;
						if (rpt > FSectionPnt[Convert.ToInt32(i), 1] - 1)
						{
							rpt = FSectionPnt[Convert.ToInt32(i), 1] - 1;
						}
						yy1 = fomsana.g_sdat[Convert.ToInt32(lpt)]; //2P
						yy2 = fomsana.g_sdat[Convert.ToInt32(rpt)]; //2P
						cerrorF = true;
						LSA(lpt, rpt);
						if (!cerrorF)
						{ //LSA
							yy1 = LA * fomsana.g_ddat[Convert.ToInt32(lpt)] + LB;
							yy2 = LA * fomsana.g_ddat[Convert.ToInt32(rpt)] + LB;
						}
						dbv = (-1d) * (yy2 - yy1);
						if (((float)dbv) > PBendv)
						{ //Candidate
							bmaxpnt = j;
							bmaxv = (float)dbv;
							double tempForEndVar22 = j;
							for (int k = Convert.ToInt32(j - bendingwidth); k <= tempForEndVar22; k++)
							{
								lpt = k;
								rpt = k + bendingwidth;
								yy1 = fomsana.g_sdat[Convert.ToInt32(lpt)]; //2P
								yy2 = fomsana.g_sdat[Convert.ToInt32(rpt)]; //2P
								cerrorF = true;
								LSA(lpt, rpt);
								if (!cerrorF)
								{ //LSA
									yy1 = LA * fomsana.g_ddat[Convert.ToInt32(lpt)] + LB;
									yy2 = LA * fomsana.g_ddat[Convert.ToInt32(rpt)] + LB;
								}
								dbv = (-1d) * (yy2 - yy1);
								if (((((float)dbv) > bmaxv) ? -1 : 0) <= 0d && ((float)D1dat.GetValue(Convert.ToInt32(j))) < 0f && ((float)D2dat.GetValue(Convert.ToInt32(j))) < 0f)
								{
									bmaxpnt = k;
									bmaxv = (float)dbv;
								}
							}
							if ((bmaxpnt - oldbpnt) > bendingwidth)
							{
								Bpnt[Bcnt] = Convert.ToInt32(bmaxpnt);
								oldbpnt = Convert.ToInt32(bmaxpnt);
								Blpnt[Bcnt] = Convert.ToInt32(bmaxpnt + bendingwidth);
								Btype[Bcnt] = false;
								j = bmaxpnt + bendingwidth;
								Bcnt++;
							}
							if (Bcnt > 99)
							{
								break;
							}
						}
					}

					//8-2-3-3) Peak Scan (Excluding the 1st-Splitter Peak)
					if (i < (FPCnt - 1))
					{
						if (Bcnt > 99)
						{
							break;
						}
						Bpnt[Bcnt] = FSectionPnt[Convert.ToInt32(i), 1];
						Blpnt[Bcnt] = FSectionPnt[Convert.ToInt32(i + 1), 0];
						Btype[Bcnt] = true;
						Bcnt++;
						if (Bcnt > 99)
						{
							break;
						}
					}
				}
				//       Debug.Print Format(Now, "hh:mm:ss") & ": 8-2-3 Finish"

				//8-2-4) Event Loss by LSA2 or 2P
				Ecnt = 0;
				if (Bcnt > 0)
				{
					//--------------------------------
					Bcnt++;
					Bpnt[Bcnt - 1] = E1pnt[FPCnt - 1]; //Feeder End of 1st-Splitter
													   //--------------------------------
					oldbpnt = Flpnt;
					int tempForEndVar23 = Bcnt - 2;
					for (i = 0; i <= tempForEndVar23; i++)
					{ //-1
						if (Ecnt > 99)
						{
							break;
						}
						if (Btype[Convert.ToInt32(i)])
						{ //Peak Loss
						  //Left LSA or 2P Loss
							lsaf = true;
							cpt = Bpnt[Convert.ToInt32(i)];
							lpt = cpt - lsawidthpnt;
							if (lpt < Flpnt)
							{
								lpt = Flpnt;
							}
							if (i > 0)
							{
								if (lpt < (Bpnt[Convert.ToInt32(i - 1)] + bendingwidth))
								{
									lpt = Bpnt[Convert.ToInt32(i - 1)] + bendingwidth;
								}
							}
							rpt = cpt;
							if ((rpt - lpt) < lsawidthpnt)
							{
								lsaf = false;
							}
							cptv1 = fomsana.g_sdat[Convert.ToInt32(cpt)]; //2P
							if (lsaf)
							{ //LSA
								cerrorF = true;
								LSA(lpt, rpt);
								if (!cerrorF)
								{
									cptv1 = LA * fomsana.g_ddat[Convert.ToInt32(cpt)] + LB;
								}
							}

							//Right LSA or 2P Loss
							lsaf = true;
							lpt = Blpnt[Convert.ToInt32(i)];
							rpt = lpt + lsawidthpnt;
							if (rpt > Bpnt[Convert.ToInt32(i + 1)] - 1)
							{
								rpt = Bpnt[Convert.ToInt32(i + 1)] - 1;
							}
							if ((rpt - lpt) < lsawidthpnt)
							{
								lsaf = false;
								p2pnt = Convert.ToInt32(rpt);
							}

							if (lsaf)
							{
								cerrorF = true;
								LSA(lpt, rpt);
								if (!cerrorF)
								{
									cptv2 = (float)(LA * fomsana.g_ddat[Convert.ToInt32(cpt)] + LB);
								}
								else
								{
									//2P
									cptv2 = fomsana.g_sdat[Convert.ToInt32(lpt)];
								}
							}
							else
							{
								//2P
								cptv2 = fomsana.g_sdat[p2pnt];
							}

							dbv = (-1d) * (cptv2 - cptv1);
							//20130313
							if (((float)dbv) > PBendv && (cpt - oldbpnt) > bendingwidth)
							{
								Epnt[Ecnt] = Convert.ToInt32(cpt);
								oldbpnt = Convert.ToInt32(cpt);
								Evloss[Ecnt] = (float)dbv; // x t_DSFactor
								Etype[Ecnt] = true;
								if (lsaf)
								{
									Ecalc[Ecnt] = true;
								}
								else
								{
									Ecalc[Ecnt] = false;
								}
								Ecnt++;
								if (Ecnt > 99)
								{
									break;
								}
							}
						}
						else
						{
							//Bend Loss
							//Left LSA or 2P Loss
							lsaf = true;
							cpt = Bpnt[Convert.ToInt32(i)];
							lpt = cpt - lsawidthpnt;
							if (lpt < Flpnt)
							{
								lsaf = false;
							}
							if (i > 0)
							{
								if (lpt < (Bpnt[Convert.ToInt32(i - 1)] + bendingwidth))
								{
									lpt = (Bpnt[Convert.ToInt32(i - 1)] + bendingwidth);
								}
							}
							rpt = cpt;
							if ((rpt - lpt) < lsawidthpnt)
							{
								lsaf = false;
							}
							cptv1 = fomsana.g_sdat[Convert.ToInt32(cpt)]; //2P
							if (lsaf)
							{
								cerrorF = true;
								LSA(lpt, rpt);
								if (!cerrorF)
								{
									cptv1 = LA * fomsana.g_ddat[Convert.ToInt32(cpt)] + LB;
								}
							}

							//Right LSA or 2P Loss
							lsaf = true;
							lpt = Blpnt[Convert.ToInt32(i)];
							rpt = lpt + lsawidthpnt;
							if (rpt > Bpnt[Convert.ToInt32(i + 1)] - 1)
							{
								rpt = Bpnt[Convert.ToInt32(i + 1)] - 1;
							}
							if ((rpt - lpt) < lsawidthpnt)
							{
								lsaf = false;
								p2pnt = Convert.ToInt32(rpt);
							}
							if (lsaf)
							{
								cerrorF = true;
								LSA(lpt, rpt);
								if (!cerrorF)
								{
									cptv2 = (float)(LA * fomsana.g_ddat[Convert.ToInt32(cpt)] + LB);
								}
								else
								{
									//2P
									cptv2 = fomsana.g_sdat[Convert.ToInt32(lpt)];
								}
							}
							else
							{
								//2P
								cptv2 = fomsana.g_sdat[p2pnt];
							}

							dbv = (-1d) * (cptv2 - cptv1);
							if (((float)dbv) > PBendv && (cpt - oldbpnt) > bendingwidth)
							{
								Epnt[Ecnt] = Convert.ToInt32(cpt);
								oldbpnt = Convert.ToInt32(cpt);
								Evloss[Ecnt] = (float)dbv; // x t_DSFactor
								Etype[Ecnt] = false;
								if (lsaf)
								{
									Ecalc[Ecnt] = true;
								}
								else
								{
									Ecalc[Ecnt] = false;
								}
								Ecnt++;
								if (Ecnt > 99)
								{
									break;
								}
							}
						}
					}
				}
			}

			//20130311~20130314~--------------------------------------------------------------------------------------
			//End of Analysis-----------------------------------------------------------------------------------------

			int channel;
			int.TryParse(Cur_ch_num, out channel);
			{
				channel -= 1;
			}
			if (channel == 0)
			{
				channel = Fiber_Num;
			}
			GlobalVariables.MA_Fiber_Num = string.Format("{0:D3}", channel);

			//Results Output -------------------------------------------------------------------------------
			AccessDataTable accessDT = accessDB.GetAccessDataTable("Event_Manage", "");

			txt_Messages.SelectedText = DateTime.Now.ToString("HH:mm:ss") + ": Ecnt is " + Ecnt.ToString() + Environment.NewLine;
			txt_Messages.SelectedText = DateTime.Now.ToString("HH:mm:ss") + ": Pcnt is " + Pcnt.ToString() + Environment.NewLine;
			int MA_E_Num = 0;
			if (Ecnt > 0 || Pcnt > 0)
			{

				Event_Time = DateTime.Now.ToString("yyyy-MM-dd") + "-" + DateTime.Now.ToString("HH:mm:ss");
				//PlayErrorSound();

				//Reflection Check -------------------------------------------------------------------------
				if (Pcnt > 0)
				{
					if (fomsana.g_ddat[E1pnt[Pcnt - 1]] < 100000)
					{
						accessDT.Add();
						accessDT["Event_Time"] = Event_Time + "-" + "1";
						accessDT["Fiber_Num"] = GlobalVariables.MA_Fiber_Num;
						accessDT["Event_Num"] = 1;
						accessDT["Event_dis"] = string.Format("{0:#0.00#}", fomsana.g_ddat[E1pnt[Pcnt - 1]]);
						accessDT["Event_type"] = "Breakage";

						Event_Alarm_Window.DefInstance.fgrid_EventAlarm.AddItem(Event_Time + "\t" + GlobalVariables.MA_Fiber_Num +
						 "\t" + "Breakage" + "\t" + StringsHelper.Format(fomsana.t_EVENTS[Convert.ToInt32(i)].eloc, "#0.00#") + "\t" + "-");
						MA_E_Num = 1;
					}
					else
					{
						MA_E_Num = 0;
					}
				}
				else
				{
					MA_E_Num = 0;
				}

				Event_Number = MA_E_Num;
				for (i = 0; i < Ecnt; i++)
				{
					accessDT.Add();
					accessDT["Event_Time"] = Event_Time + "-" + (Event_Number + 1).ToString();
					accessDT["Fiber_Num"] = GlobalVariables.MA_Fiber_Num;
					accessDT["Event_Num"] = Event_Number + 1;
					accessDT["Event_dis"] = string.Format("{0:#0.00#}", fomsana.g_ddat[Epnt[Convert.ToInt32(i)]]);
					accessDT["Event_loss"] = string.Format("{0:#0.00#}", Evloss[Convert.ToInt32(i)]);
					if (Etype[Convert.ToInt32(i)])
					{
						accessDT["Event_type"] = "Reflection";
						Event_Alarm_Window.DefInstance.fgrid_EventAlarm.AddItem(Event_Time + "\t" + GlobalVariables.MA_Fiber_Num + "\t" + "Reflection" + "\t" + StringsHelper.Format(fomsana.g_ddat[Epnt[Convert.ToInt32(i)]], "#0#") + "\t" + StringsHelper.Format(Evloss[Convert.ToInt32(i)], "#0.00#"));
					}
					else
					{
						accessDT["Event_type"] = "Bend";
						Event_Alarm_Window.DefInstance.fgrid_EventAlarm.AddItem(Event_Time + "\t" + GlobalVariables.MA_Fiber_Num + "\t" + "Bending" + "\t" + StringsHelper.Format(fomsana.g_ddat[Epnt[Convert.ToInt32(i)]], "#0#") + "\t" + StringsHelper.Format(Evloss[Convert.ToInt32(i)], "#0.00#"));
					}
					Event_Number++;
					PlayErrorSound();
				}
			}

			accessDT.Update();

			txt_Messages.SelectedText = DateTime.Now.ToString("HH:mm:ss") + ": Analysis_Block OK" + Environment.NewLine + Environment.NewLine;

			Bending_Cable_Exist = true;
			Breakage_Cable_Exist = true;
			Module_Cal_Complete = 1;

			if (Measure_Stop == 1)
			{
				Measure_Stop = 0;
			}
		}
		// 2021/04/13 이후삭제 내용 위로 추가


		/// MS Access Data Write For Analysis Results  * This Section for only Jeju-Jindo
		private void OTDR_Ana_Write(bool shouldSaveToExcel)
		{
			string Event_Time;
			int Event_Number;
			float[] Dis_Arr = new float[101]; //210413 재 기입
			int Bending_Event_Exist = 0; //1: 벤딩 이벤트 있음 / 0: 없음, 주형준//210413 재 기입
			int Breakage_Event_Exist = 0; //1: 단선 이벤트 있음 / 0: 없음
			int Num_of_Bending;
			int Num_of_Breakage;

			// For e-mail sending -----------------
			string strTextBody;
			string strSubject = "Event Alarm occurrence"; //Subject of E-mail

			try
			{
				OTDR_Ana_Write_Complete = 1;

				Event_Time = DateTime.Now.ToString("yyyy-MM-dd") + "-" + DateTime.Now.ToString("HH:mm:ss");

				AccessDataTable accessDT;
				//accessDT = accessDB.GetAccessDataTable("Bending_Cable", "Fiber_Num = '" + GlobalVariables.MA_Fiber_Num + "'");
				//Bending_Cable_Exist = (accessDT.RecordCount > 0);
				//accessDT = accessDB.GetAccessDataTable("Breakage_Cable", "Fiber_Num = '" + GlobalVariables.MA_Fiber_Num + "'");
				//Breakage_Cable_Exist = (accessDT.RecordCount > 0);
				accessDT = accessDB.GetAccessDataTable("Event_Manage", "");

				Bending_Event_Exist = 0; //210413 재 기입
				Breakage_Event_Exist = 0;//210413 재 기입
				Event_Number = 1;
				Num_of_Bending = 0;
				Num_of_Breakage = 0;

				//--------- For Excel Report
				if (Current_Time_State == 0)
				{
					Cell_Name = Report_Excel_Length_Index[0] + Length_Row_Num.ToString();
				}
				else if (Current_Time_State == 1)
				{
					Cell_Name = Report_Excel_Length_Index[1] + Length_Row_Num.ToString();
				}
				else if (Current_Time_State == 2)
				{
					Cell_Name = Report_Excel_Length_Index[2] + Length_Row_Num.ToString();
				}
				else if (Current_Time_State == 3)
				{
					Cell_Name = Report_Excel_Length_Index[3] + Length_Row_Num.ToString();
				}

				//Event 발생 시 Event_Manage 테이블에 무조건 기재
				for (int k = 0; k < fomsana.t_NEVENT; ++k)
				{
					if ((fomsana.t_EVENTS[k].eloc) >= GlobalVariables.DummyCable_Len)	// Dummy Fiber(2km) 이후의 이벤트만 인식하도록..
					{
						string ecode = fomsana.t_EVENTS[k].ecode;
						if (string.IsNullOrEmpty(ecode))
						{
							ecode = "00";
						}
						else if (ecode.Length == 1)
						{
							ecode += "0";
						}
						if (ecode[0] != '2')
						{
							//Key Events Block에 대한 OTDR Data Format 문서 참조
							accessDT.Add();
							accessDT["Event_Time"] = Event_Time + "-" + (k + 1).ToString();
							accessDT["Fiber_Num"] = GlobalVariables.MA_Fiber_Num;
							accessDT["Event_Num"] = Event_Number;
							accessDT["Event_dis"] = string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].eloc);

							if (ecode[0] == '0' && ecode[1] == 'F')
							{
								if (double.Parse(string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].eloc)) >= GlobalVariables.Cable_Len * 1000)
								{
									accessDT["Event_type"] = "End Event";
									accessDT["Event_loss"] = string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].Eloss);
									if (Length_Write_OK == 1)
									{
										if (shouldSaveToExcel && Cell_Name != ""/* && oWB != null && oSheet != null*/)
										{
											oXL = new Excel.Application();
											oXL.Visible = true;
											oWB = oXL.Workbooks.Open(Xls_NewFile_Path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
											oSheet = (Excel.Worksheet)oWB.ActiveSheet;
											oSheet.Range[Cell_Name].Value = string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].eloc - 2000);
											oWB.Save();
											oXL.Quit();
										}
									}
								}
								else if (double.Parse(string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].eloc)) < GlobalVariables.Cable_Len * 1000 &&
									double.Parse(string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].Eloss)) >= 0.1d)
								{
									accessDT["Event_type"] = "Bending";
									accessDT["Event_loss"] = string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].Eloss);
									if (double.Parse(string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].Eloss)) >= 0.2d)
									{
										windowEventAlarm.AddItem(Event_Time + "\t" + GlobalVariables.MA_Fiber_Num + "\t" + "Bending" + "\t" +
											string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].eloc) + "\t" +
											string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].Eloss));
										PlayErrorSound();
										// Put the e-mail sending code
										strTextBody = " - Event Time : " + Event_Time + Environment.NewLine +
											" - Fiber Number : " + GlobalVariables.MA_Fiber_Num + Environment.NewLine +
											" - Event Type : Bending" + Environment.NewLine +
											" - Bending Distance : " + string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].eloc / 1000) + " [km]" + Environment.NewLine +
											" - Bending Loss : " + string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].Eloss) + " [dB]";
										if (GmailSend(strSubject, strTextBody))
										{
											txt_Messages.SelectedText = DateTime.Now.ToString("HH:mm:ss") +
												": Alarm Message Sent by E-mail" + Environment.NewLine + Environment.NewLine;
										}
									}
									Num_of_Bending++;
								}
							}
							else if (ecode[0] == '1' && ecode[1] == 'F')
							{
								if (double.Parse(string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].eloc)) >= GlobalVariables.Cable_Len * 1000)
								{
									accessDT["Event_type"] = "End Event";
									accessDT["Event_loss"] = string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].Eloss);
									if (Length_Write_OK == 1)
									{
										if (shouldSaveToExcel && Cell_Name != ""/* && oWB != null && oSheet != null*/)
										{
											oXL = new Excel.Application();
											oXL.Visible = true;
											oWB = oXL.Workbooks.Open(Xls_NewFile_Path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
											oSheet = (Excel.Worksheet)oWB.ActiveSheet;
											oSheet.Range[Cell_Name].Value = string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].eloc - 2000);
											oWB.Save();
											oXL.Quit();
										}
									}
								}
								else if (double.Parse(string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].eloc)) < GlobalVariables.Cable_Len * 1000)
								{
									accessDT["Event_type"] = "Breakage";
									accessDT["Event_loss"] = "-";
									if (shouldSaveToExcel && Cell_Name != ""/* && oWB != null && oSheet != null*/)
									{
										oXL = new Excel.Application();
										oXL.Visible = true;
										oWB = oXL.Workbooks.Open(Xls_NewFile_Path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
										oSheet = (Excel.Worksheet)oWB.ActiveSheet;
										oSheet.Range[Cell_Name].Value = string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].eloc);
										oWB.Save();
										oXL.Quit();
									}
									windowEventAlarm.AddItem(Event_Time + "\t" + GlobalVariables.MA_Fiber_Num + "\t" +
										"Breakage" + "\t" + string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].eloc) + "\t" + "-");
									PlayErrorSound();
									// Put the e-mail sending code
									strTextBody = " - Event Time : " + Event_Time + Environment.NewLine +
										" - Fiber Number: " + GlobalVariables.MA_Fiber_Num + Environment.NewLine +
										" - Event Type : Breakage" + Environment.NewLine +
										" - Breakage Distance : " + string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].eloc / 1000) + " [km]";
									if (GmailSend(strSubject, strTextBody))
									{
										txt_Messages.SelectedText = DateTime.Now.ToString("HH:mm:ss") +
											": Alarm Message Sent by E-mail" + Environment.NewLine + Environment.NewLine;
									}

									Num_of_Breakage++;
								}
							}
							else if (ecode[1] == 'E')
							{
								if (double.Parse(string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].eloc)) >= GlobalVariables.Cable_Len * 1000)
								{
									accessDT["Event_type"] = "End Event";
									accessDT["Event_loss"] = string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].Eloss);
									if (Length_Write_OK == 1)
									{
										if (shouldSaveToExcel && Cell_Name != ""/* && oWB != null && oSheet != null*/)
										{
											oXL = new Excel.Application();
											oXL.Visible = true;
											oWB = oXL.Workbooks.Open(Xls_NewFile_Path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
											oSheet = (Excel.Worksheet)oWB.ActiveSheet;
											oSheet.Range[Cell_Name].Value = string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].eloc - 2000);
											oWB.Save();
											oXL.Quit();
										}
									}
								}
								else if (double.Parse(string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].eloc)) < GlobalVariables.Cable_Len * 1000)
								{
									accessDT["Event_type"] = "Breakage";
									accessDT["Event_loss"] = "-";
									if (Length_Write_OK == 1)
									{
										if (shouldSaveToExcel && Cell_Name != ""/* && oWB != null && oSheet != null*/)
										{
											oXL = new Excel.Application();
											oXL.Visible = true;
											oWB = oXL.Workbooks.Open(Xls_NewFile_Path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
											oSheet = (Excel.Worksheet)oWB.ActiveSheet;
											oSheet.Range[Cell_Name].Value = string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].eloc);
											oWB.Save();
											oXL.Quit();
										}
									}
									windowEventAlarm.AddItem(Event_Time + "\t" + GlobalVariables.MA_Fiber_Num + "\t" +
										"Breakage" + "\t" + string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].eloc) + "\t" + "-");
									PlayErrorSound();
									// Put the e-mail sending code
									strTextBody = " - Event Time : " + Event_Time + Environment.NewLine +
										" - Fiber Number: " + GlobalVariables.MA_Fiber_Num + Environment.NewLine +
										" - Event Type : Breakage" + Environment.NewLine +
										" - Breakage Distance : " + string.Format("{0:#0.00#}", fomsana.t_EVENTS[k].eloc / 1000) + " [km]";
									if (GmailSend(strSubject, strTextBody))
									{
										txt_Messages.SelectedText = DateTime.Now.ToString("HH:mm:ss") +
											": Alarm Message Sent by E-mail" + Environment.NewLine + Environment.NewLine;
									}

									Num_of_Breakage++;
								}
							}
							Event_Number++;
						}
					}
				}

				accessDT.Update();

				Event_Number = 0;
				if (Length_Write_OK == 1)
				{
					oXL.Quit();
				}

				if (Report_Complete == 1)
				{
					Utilities.KillProcess("Excel");
					str_Report_TextBody = DateTime.Now.ToString("yyyy-MM-dd") + Environment.NewLine + Environment.NewLine +
						"The FOMS #2 system Measurement Report from RLIC" + Environment.NewLine + Environment.NewLine + "Thank you!";
					AttachFileName = Xls_NewFile_Path;
					if (GmailSend(str_Report_Subject, str_Report_TextBody))
					{
						txt_Messages.SelectedText = DateTime.Now.ToString("HH:mm:ss") +
							": Report is sent by E-mail" + Environment.NewLine + Environment.NewLine;
					}
					Report_Complete = 0;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		/// (+/-2 pnum Points) pnum * 2 + 1 Points for me
		private void mvavg(int pnum)
		{
			float val;
			fomsana.g_sdat = new float[fomsana.t_NDAT + 1];

			if (pnum == 0)
			{
				// Raw Data
				for (int i = 0; i < fomsana.t_NDAT; ++i)
				{
					fomsana.g_sdat[i] = fomsana.g_tdat[i];
				}
			}
			else
			{
				// Moving Average +/-pnum
				for (int i = 0; i < pnum; ++i)
				{
					val = 0f;
					for (int j = 0; j <= (pnum + i); ++j)
					{
						val += fomsana.g_tdat[j];
					}
					fomsana.g_sdat[i] = (float)(val / (pnum + i + 1));
				}
				for (int i = pnum; i <= fomsana.t_NDAT - (pnum + 1); ++i)
				{
					val = 0f;
					for (int j = 0; j <= pnum * 2; ++j)
					{
						val += fomsana.g_tdat[j + i - pnum];
					}
					fomsana.g_sdat[i] = (float)(val / (pnum * 2 + 1));
				}
				for (int i = 0; i < pnum; ++i)
				{
					val = 0f;
					for (int j = 0; j <= (pnum + i); ++j)
					{
						val += fomsana.g_tdat[fomsana.t_NDAT - j - 1];
					}
					fomsana.g_sdat[fomsana.t_NDAT - i - 1] = (float)(val / (pnum + i + 1));
				}
			}
		}

		private bool GmailSend(string subject, string body)
		{
			string from = emailSendFrom;
			string to = emailSendTo;

			if (string.IsNullOrEmpty(emailSendTheme) == false)
			{
				subject = "[" + emailSendTheme + "] " + subject;
			}

			try
			{
				SmtpClient smtp = new SmtpClient()
				{
					Host = emailServer,
					DeliveryMethod = SmtpDeliveryMethod.Network,
					EnableSsl = emailServerEncryptedConnection,
					Credentials = new NetworkCredential(emailServerUserID, emailServerPassword)
				};
				if (emailServerUseDefaultPort == false)
				{
					smtp.Port = (int)emailServerPort;
				}
				MailMessage mail = new MailMessage(from, to, subject, body);
				for (int i = 0; i < Constants.MaxEmailReceiver; ++i)
				{
					if (string.IsNullOrEmpty(emails[i]) == false && emails[i].Contains("@"))
					{ 
						mail.To.Add(emails[i]);
					}
				}
				if (string.IsNullOrEmpty(AttachFileName) == false)
				{
					var contentType = new System.Net.Mime.ContentType()
					{
						MediaType = System.Net.Mime.MediaTypeNames.Application.Octet,
						Name = Path.GetFileName(AttachFileName)
					};
					mail.Attachments.Add(new Attachment(AttachFileName, contentType));
				}
				smtp.Send(mail);
			}
			catch (Exception ex)
			{
				DebuggingHelper.Trace(ex);
				return false;
			}

			return true;
		}

		public void TimeSleep(int ms)
        {
			int interval = 10;
			int count = ms / interval;
			for (int i = 0; i < count; i++)
			{
				Utilities.Delay(interval);
				Application.DoEvents();
			}
		}

		//********************************************************************************************
		//* Auto Measuring                                                                           *
		//********************************************************************************************

		private void cb_MeasureStop_Click(object sender, EventArgs eventArgs)
		{
			Measure_Stop = 1;
		}

		private void cb_MeasureStart_Click(object sender, EventArgs eventArgs)
		{
			string str_Subject = "System Error"; //Subject of E-mail

			//******************** Make Folder *********************20210413 재 기입
			object fso = new object(); string Date_ = "";

			//******************** Section Loss ********************20210413 재 기입
			string MDLoss = ""; //Section Loss Value (String Type)
			string MDLoss_5 = ""; //Section Loss Value for MA Write(5 digit)
			float[] Min_Value = new float[145]; //Minimum Loss Values Array

			AccessDataTable accessCursorInfo = accessDB.GetAccessDataTable("Cursor_Info", "");
			AccessDataTable accessLossManageData = accessDB.GetAccessDataTable("Loss_Manage", "");

			try
			{
				Measure_Stop = 0;
				Auto_Save_Ready = 0;

				int Data_Len_Idx = 0;
				string[] Dis_Item = null; //Re Declaration by Data Length

				// 사용자가 [Measure Stop] Button click 할 때까지 루프.
				while (Measure_Stop != 1)
				{
					// Make Folder >>>>>>>>>>>>>>>>>>>>>>>>>>>>>
					Date_ = DateTime.Now.ToString("yyyy-MM-dd");
					DateTime dt = DateTime.Now;
					FolderName = dt.ToString("yyyy-MM-dd") + "_" + dt.ToString("HH") + "h" + dt.ToString("mm") + "m" + dt.ToString("ss") + "s";
					if (string.IsNullOrEmpty(emailSendTheme))
					{
						FolderPath = baseDirectory + "Data\\" + Date_ + "\\" + FolderName;
					}
					else
					{
						FolderPath = baseDirectory + "Data\\" + emailSendTheme + "\\" + Date_ + "\\" + FolderName;
					}
					Directory.CreateDirectory(FolderPath);

					//=== Cursor Information Loading ===============================
					Dis_Item = new string[accessCursorInfo.RecordCount + 1];

					Data_Len_Idx = 0;
					for (int i = 0; i < accessCursorInfo.RecordCount; ++i)
					{
						// Loading and Save to Array
						Dis_Item[i] = accessCursorInfo["Dis_Item"] as string;
						Data_Len_Idx = i;
						accessCursorInfo.MoveNext();
					}
					//==============================================================

					// 144채널 측정 체크박스
					//if (chkbox_AllFibers.CheckState == CheckState.Checked)
					//{
					//	Fiber_Num = 144; //144심선 체크
					//}
					//else
					{
						// 체크박스 체크되어 있지 않으면 케이블 세팅의 값을 로딩(Default, 12)
						Fiber_Num = Convert.ToInt32(GlobalVariables.Fiber_Number);
					}

					int tempForEndVar2 = Fiber_Num;
					for (Switching_Count = 1; Switching_Count <= tempForEndVar2; Switching_Count++)
					{
						if (Measure_Stop == 1)
						{
							txt_Messages.SelectedText = DateTime.Now.ToString("HH:mm:ss") + "Auto Measure is Stopped!" + Environment.NewLine;
							return;
						}

						//PPI社 OSWL-144 Model은 스위치 구동 Command가 "SP XXX" 로써 포트#를 무조건 3바이트로 맞춰야 함.
						CH_CMD = string.Format("{0:D3}", Switching_Count);

						osw.SwitchChannel(Switching_Count);
						TimeSleep(200);

						Cur_ch_num = CH_CMD;
						DebuggingHelper.Output("Cur_ch_num=" + Cur_ch_num);
						txt_ChNo.Text = CH_CMD;
						Date_ = DateTime.Now.ToString("HH:mm:ss");
						txt_Messages.SelectedText = Date_ + ": OSW Set Ch. to " + Cur_ch_num + " : OK!" + Environment.NewLine;

						//If chkbox_AllFibers.Value = 0 Then
						Now_Time = DateTime.Now.Hour; //------------------------------ Create Excel Report at AM 01:00
						//1:   Now_Time = CInt(Mid$(Format(Now, "hh:mm:ss"), 4, 2)) '------------------------------ Create Excel Report at 01min.
						//     Now_Time = Now_Time - 6
						if (Now_Time < 0)
						{
							Now_Time += 60;
						}

						Utilities.KillProcess("Excel");

						//Debug.Print "Now_Time = " & Now_Time
						//Debug.Print "Create_File_OK = " & Create_File_OK
						if (Now_Time == 1 && Create_File_OK == 1)
						{
							// 1시이면서, 파일 생성 모드 On일 때 엑셀 파일 생성
							oXL = new Excel.Application();
							oXL.Visible = true;
							oWB = oXL.Workbooks.Open(baseDirectory + "Report\\DPR_FO_Monitoring.xlsx", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
							oSheet = (Excel.Worksheet)oWB.ActiveSheet;
							oSheet.Range["A6"].Value = DateTime.Now.ToString("yyyy-MM-dd");
							Xls_NewFile_Path = Xls_File_Path + DateTime.Now.ToString("yyyy-MM-dd") + "_" + Loop_Count.ToString() + ".xlsx";
							oWB.SaveAs(Xls_NewFile_Path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
							oXL.Quit();
							txt_Messages.SelectedText = DateTime.Now.ToString("HH:mm:ss") + ": Excel File Created!";
							Create_File_OK = 0; //파일 생성 후 파일 생성 모드 Off 변환
							//Data_Wirte_OK = 1;
						}
						else if (Now_Time == 2)
						{
							// 2시가 되면 파일 생성 모드 다시 On으로 변환
							Create_File_OK = 1;
							First_Time_Write_Ready = 1;
						}
						else if (Now_Time != Check_Time[0] && Now_Time != Check_Time[1] && Now_Time != Check_Time[2] && Now_Time != Check_Time[3])
						{
							//Data_Wirte_OK = 1; //데이터 저장하는 시간을 제외한 모든 시간에서는 데이터 쓰기 모드 On으로 변환
						} //--------------------------------------------------------------------------- Create Excel Report at AM 01:00

					ReStart_Point:
						//=== OTDR Measure ===============================================================================
						//--- Cursor Setting for Fully Length ------------------------------------------------------------
						if (Data_Len_Idx < accessCursorInfo.RecordCount)
						{
							accessCursorInfo.SelectRow("Dis_Item", Dis_Item[Data_Len_Idx]);
							GlobalVariables.ACursor = Convert.ToSingle(accessCursorInfo["ACursor"]);
							GlobalVariables.BCursor = Convert.ToSingle(accessCursorInfo["BCursor"]);
							GlobalVariables.LLSACursorStart = Convert.ToSingle(accessCursorInfo["LLSAStart"]);
							GlobalVariables.LLSACursorEnd = Convert.ToSingle(accessCursorInfo["LLSAEnd"]);
							GlobalVariables.RLSACursorStart = Convert.ToSingle(accessCursorInfo["RLSAStart"]);
							GlobalVariables.RLSACursorEnd = Convert.ToSingle(accessCursorInfo["RLSAEnd"]);
						}

						SendOTDRCursorSettings();

						Date_ = DateTime.Now.ToString("HH:mm:ss");
						txt_Messages.SelectedText = Date_ + ": " + "Measuring..." + Environment.NewLine;
						otdr.IsReceivedDataCallbackEnabled = false;
						DebuggingHelper.Output("OTDRWaitReady");
						bool succeed = false;
						if (otdr.Model == OTDRControl.OTDRModel.Anritsu_MT9083)
						{
							otdr.SendCommand("sens:loss:mode 4; *WAI"); // Loss_Mode (4:dB/km LSA Loss Mode)
							otdr.SendCommand("init; *WAI; *CLS; *WAI; sens:trac:ready?"); // Measure & SOR Read Check
							otdr.WaitResponse();
							string Complete_Msg = otdr.Response;
							if (Complete_Msg == "")
							{
								otdr.SendCommand("*CLS; *WAI; init; *WAI; *CLS; *WAI; sens:trac:ready?"); // Measure & SOR Read Check
								Date_ = DateTime.Now.ToString("HH:mm:ss");
								otdr.WaitResponse();
								Complete_Msg = otdr.Response;
							}
							if (Complete_Msg == "1")
							{ 
								succeed = true;
							}
						}
						else if (otdr.Model == OTDRControl.OTDRModel.VIAVI_SmartOTDR)
						{
							m_OTDRParamSet.Enabled = false;
							otdr.SendCommand("KEY STAR");
							var sw = new Stopwatch();
							sw.Start();
							while (sw.ElapsedMilliseconds < 180000)
							{
								otdr.SendCommand("STAT:ACQ?");
								try
								{
									otdr.WaitResponse(1000);
								}
								catch
								{
									// timeout
									DebuggingHelper.Output("STAT:ACQ? WaitResponse() Timeout!");
								}
								if (otdr.Response == "STOPPED")
								{
									succeed = true;
									break;
								}
								TimeSleep(200);
							}
							m_OTDRParamSet.Enabled = true;
							if (succeed == false)
							{
								throw new Exception("OTDR Timeout occured. Timeout = " + "120000msec");
							}
						}

						Date_ = DateTime.Now.ToString("HH:mm:ss");
						if (succeed)
						{
							// SOR 준비 되었으면 Complete Message 출력
							Date_ = DateTime.Now.ToString("HH:mm:ss");
							txt_Messages.SelectedText = Date_ + ": " + "Measurement Complete!" + Environment.NewLine + Environment.NewLine;

							if (otdr.Model == OTDRControl.OTDRModel.Anritsu_MT9083)
							{
								Cell_Name = "";
								CalculateLoss(accessCursorInfo, accessLossManageData, Data_Len_Idx, Dis_Item);
							}

							//=== SOR Save ===============================================================================
							Auto_Save_Ready = 1;
							GlobalVariables.Data_Size = 0;
							if (otdr.Model == OTDRControl.OTDRModel.Anritsu_MT9083)
							{
								otdr.SendCommand("*CLS;trac:load:sor?");	// SOR Data 호출
							}
							else if (otdr.Model == OTDRControl.OTDRModel.VIAVI_SmartOTDR)
							{
								otdr.SendCommand("*CLS;OTDR:SSOR?");
							}
							otdr.IsReceivedDataCallbackEnabled = true;
							DebuggingHelper.Output("DataReceiveReady");
						}
						else
						{
							if (Measure_Stop == 1)
								break;
							goto ReStart_Point; //Complete_Msg를 놓친 경우 리턴
						}

						{
							var sw = new Stopwatch();
							sw.Start();
							while (Auto_Save_Ready == 1)
							{
								TimeSleep(100);
								if (Measure_Stop == 1)
								{
									txt_Messages.SelectedText = DateTime.Now.ToString("HH:mm:ss") + "Auto Measure is Stopped!" + Environment.NewLine;
									return;
								}
								if (sw.ElapsedMilliseconds >= 180000)
								{
									MessageBox.Show(this, "No response!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
									txt_Messages.SelectedText = DateTime.Now.ToString("HH:mm:ss") + "Auto Measure is Stopped! (timeout)" + Environment.NewLine;
									return;
								}
							}
						}

						if (Switching_Count == Fiber_Num)
						{
							// Multi Mode 측정 완료했으면, Reset
							txt_Messages.SelectedText = Fiber_Num.ToString() + " Port Measuring is Complete!" + Environment.NewLine + Environment.NewLine;
						}
					}

					txt_Messages.Text = "";
				}

				if (Measure_Stop == 1)
				{
					txt_Messages.SelectedText = DateTime.Now.ToString("HH:mm:ss") + "Auto Measure is Stopped!" + Environment.NewLine;
				}
			}
			catch (Exception ex)
			{
				string str_TextBody = "The FOMS #2 system error has occurred" + Environment.NewLine + Environment.NewLine +
					"- Error Time : " + DateTime.Now.ToString("HH:mm:ss") + Environment.NewLine + Environment.NewLine +
					"- Error No: " + ex.HResult.ToString() + Environment.NewLine + Environment.NewLine +
					" - Error Description: " + ex.Message + Environment.NewLine +
					Environment.NewLine + Environment.NewLine + "Thank you!";
				if (GmailSend(str_Subject, str_TextBody))
				{
					txt_Messages.SelectedText = DateTime.Now.ToString("HH:mm:ss") +
						": Error Message Sent by E-mail" + Environment.NewLine + Environment.NewLine;
				}
				MessageBox.Show(ex.ToString());
			}
		}

		private void CalculateLoss(AccessDataTable accessCursorInfo, AccessDataTable accessLossManageData, int Data_Len_Idx, string[] Dis_Item)
		{
			//******************** Section Loss **************************************************************
			string MDLoss = ""; //Section Loss Value (String Type)
			string MDLoss_5 = ""; //Section Loss Value for MA Write(5 digit)
			string Event_Time = ""; //Section Loss Spec. Out Occuring Time

			//=== Loss Calculation & Wirte ===============================================================
			accessLossManageData.Add();
			accessLossManageData["Measure_Time"] = DateTime.Now.ToString("yyyy-MM-dd") + "-" + DateTime.Now.ToString("HH:mm:ss");
			accessLossManageData["Fiber_Num"] = CH_CMD;

			int tempForEndVar3 = Data_Len_Idx;
			for (int i = 0; i <= tempForEndVar3; i++)
			{
				if (i == Data_Len_Idx)
				{
					// Full Trace Loss Calculation
					if (i < accessCursorInfo.RecordCount)
					{
						accessCursorInfo.SelectRow("Dis_Item", Dis_Item[i]);
						GlobalVariables.ACursor = Convert.ToSingle(accessCursorInfo["ACursor"]);
						GlobalVariables.BCursor = Convert.ToSingle(accessCursorInfo["BCursor"]);
						GlobalVariables.LLSACursorStart = Convert.ToSingle(accessCursorInfo["LLSAStart"]);
						GlobalVariables.LLSACursorEnd = Convert.ToSingle(accessCursorInfo["LLSAEnd"]);
						GlobalVariables.RLSACursorStart = Convert.ToSingle(accessCursorInfo["RLSAStart"]);
						GlobalVariables.RLSACursorEnd = Convert.ToSingle(accessCursorInfo["RLSAEnd"]);
					}

					//2) dB/km LSA Loss Calculation & Write to DB
					otdr.SendCommand("sens:loss:mode 4"); // Loss_Mode (4:dB/km LSA Loss Mode)
					SendOTDRCursorSettings();
					otdr.SendCommand("trac:anal;*WAI;trac:mdlo?");
					otdr.WaitResponse();
					MDLoss = otdr.Response;
					MDLoss_5 = MDLoss.Substring(0, Math.Min(5, MDLoss.Length));
					accessLossManageData["dB/km_LSA"] = MDLoss_5;
					txt_SectionLoss.Text = MDLoss_5;
					dBpkm_Loss = MDLoss_5;
					double.TryParse(MDLoss_5, out double dbl);
					if (dbl > 0.25d)
					{
						Event_Time = DateTime.Now.ToString("yyyy-MM-dd") + "-" + DateTime.Now.ToString("HH:mm:ss");
						PlayErrorSound();
						windowEventAlarm.AddItem(Event_Time + "\t" + CH_CMD + "\t" + "Att. Out" + "\t" + "-" + "\t" + MDLoss_5 + "[dB/km]");
					}
					//3) 2-Point LSA Loss Calculation & Write to DB
					otdr.SendCommand("sens:loss:mode 2"); // Loss_Mode (2:2-Point LSA Loss Mode)
					otdr.SendCommand("trac:anal;*WAI;trac:mdlo?");
					otdr.WaitResponse();
					MDLoss = otdr.Response;
					MDLoss_5 = MDLoss.Substring(0, Math.Min(5, MDLoss.Length));
					Total_Loss = MDLoss_5;
					accessLossManageData["dB_2PLSA"] = MDLoss_5;

					//Write Excel Report
					if (Write_Count_Number == 0)
					{
						Length_Write_OK = 0; //Cable Length 쓰기 모드 비활성화
					}
					if (Check_Time[0] <= Now_Time && Now_Time < Check_Time[1] && First_Time_Write_Ready == 1 && Write_Count_Number < Fiber_Num + 1)
					{
						Current_Time_State = 0;
						//Excel_Row_Index(0)에 쓰기
						oXL = new Excel.Application();
						oXL.Visible = true;
						oWB = oXL.Workbooks.Open(Xls_NewFile_Path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
						oSheet = (Excel.Worksheet)oWB.ActiveSheet;
						Cell_Name = Report_Excel_Atten_Index[0] + (Switching_Count + 5).ToString();
						oSheet.Range[Cell_Name].Value = dBpkm_Loss;
						Cell_Name = Report_Excel_TotalLoss_Index[0] + (Switching_Count + 5).ToString();
						oSheet.Range[Cell_Name].Value = Total_Loss;
						Length_Row_Num = Switching_Count + 5;
						oWB.Save(); // Excel Save
						oXL.Quit();

						Length_Write_OK = 1; //Cable Length 쓰기 모드 활성화
						Write_Count_Number++;
						if (Write_Count_Number == Fiber_Num + 1)
						{
							First_Time_Write_Ready = 0;
							Second_Time_Write_Ready = 1;
							Write_Count_Number = 0;
							//Report_Complete = 1 '-------------------------------------------------------------------------------------------------- For Test
						}

					}
					else if (Check_Time[1] <= Now_Time && Now_Time < Check_Time[2] && Second_Time_Write_Ready == 1 && Write_Count_Number < Fiber_Num + 1)
					{
						Current_Time_State = 1;
						//Excel_Row_Index(1)에 쓰기
						oXL = new Excel.Application();
						oXL.Visible = true;
						oWB = oXL.Workbooks.Open(Xls_NewFile_Path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
						oSheet = (Excel.Worksheet)oWB.ActiveSheet;
						Cell_Name = Report_Excel_Atten_Index[1] + (Switching_Count + 5).ToString();
						oSheet.Range[Cell_Name].Value = dBpkm_Loss;
						Cell_Name = Report_Excel_TotalLoss_Index[1] + (Switching_Count + 5).ToString();
						oSheet.Range[Cell_Name].Value = Total_Loss;
						Length_Row_Num = Switching_Count + 5;
						oWB.Save();	//Excel Save
						oXL.Quit();

						Length_Write_OK = 1; //Cable Length 쓰기 모드 활성화
						Write_Count_Number++;
						if (Write_Count_Number == Fiber_Num + 1)
						{
							Second_Time_Write_Ready = 0;
							Third_Time_Write_Ready = 1;
							Write_Count_Number = 0;
							//Report_Complete = 1 '-------------------------------------------------------------------------------------------------- For Test
						}
					}
					else if (Check_Time[2] <= Now_Time && Now_Time < Check_Time[3] && Third_Time_Write_Ready == 1 && Write_Count_Number < Fiber_Num + 1)
					{
						Current_Time_State = 2;
						//Excel_Row_Index(2)에 쓰기
						oXL = new Excel.Application();
						oXL.Visible = true;
						oWB = oXL.Workbooks.Open(Xls_NewFile_Path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
						oSheet = (Excel.Worksheet)oWB.ActiveSheet;
						Cell_Name = Report_Excel_Atten_Index[2] + (Switching_Count + 5).ToString();
						oSheet.Range[Cell_Name].Value = dBpkm_Loss;
						Cell_Name = Report_Excel_TotalLoss_Index[2] + (Switching_Count + 5).ToString();
						oSheet.Range[Cell_Name].Value = Total_Loss;
						Length_Row_Num = Switching_Count + 5;
						oWB.Save();	//Excel Save
						oXL.Quit();

						Length_Write_OK = 1; //Cable Length 쓰기 모드 활성화
						Write_Count_Number++;
						if (Write_Count_Number == Fiber_Num + 1)
						{
							Third_Time_Write_Ready = 0;
							Fourth_Time_Write_Ready = 1;
							Write_Count_Number = 0;
						}
					}
					else if (Check_Time[3] <= Now_Time && Fourth_Time_Write_Ready == 1 && Write_Count_Number < Fiber_Num + 1)
					{
						Current_Time_State = 3;
						//Excel_Row_Index(3)에 쓰기
						oXL = new Excel.Application();
						oXL.Visible = true;
						oWB = oXL.Workbooks.Open(Xls_NewFile_Path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
						oSheet = (Excel.Worksheet)oWB.ActiveSheet;
						Cell_Name = Report_Excel_Atten_Index[3] + (Switching_Count + 5).ToString();
						oSheet.Range[Cell_Name].Value = dBpkm_Loss;
						Cell_Name = Report_Excel_TotalLoss_Index[3] + (Switching_Count + 5).ToString();
						oSheet.Range[Cell_Name].Value = Total_Loss;
						Length_Row_Num = Switching_Count + 5;
						oWB.Save(); //Excel Save
						oXL.Quit();

						Length_Write_OK = 1; //Cable Length 쓰기 모드 활성화
						Write_Count_Number++;
						if (Write_Count_Number == 12)
						{
							Fourth_Time_Write_Ready = 0;
							First_Time_Write_Ready = 1;
							Report_Complete = 1;
							Write_Count_Number = 0;
							txt_Messages.SelectedText = DateTime.Now.ToString("HH:mm:ss") + ": Report Complete!";
						}
					}
				}
				else
				{
					// Splicing Point 1EA
					if (i < accessCursorInfo.RecordCount)
					{
						accessCursorInfo.SelectRow("Dis_Item", Dis_Item[i]);
						GlobalVariables.ACursor = Convert.ToSingle(accessCursorInfo["ACursor"]);
						GlobalVariables.BCursor = Convert.ToSingle(accessCursorInfo["BCursor"]);
						GlobalVariables.LLSACursorStart = Convert.ToSingle(accessCursorInfo["LLSAStart"]);
						GlobalVariables.LLSACursorEnd = Convert.ToSingle(accessCursorInfo["LLSAEnd"]);
						GlobalVariables.RLSACursorStart = Convert.ToSingle(accessCursorInfo["RLSAStart"]);
						GlobalVariables.RLSACursorEnd = Convert.ToSingle(accessCursorInfo["RLSAEnd"]);
					}

					//1) Splicing Loss Calculation & Write to DB
					otdr.SendCommand("sens:loss:mode 0"); // Loss_Mode (0:Splicing Loss Mode)
					SendOTDRCursorSettings();
					otdr.SendCommand("trac:anal;*WAI;trac:mdlo?");
					otdr.WaitResponse();
					MDLoss = otdr.Response;
					MDLoss_5 = MDLoss.Substring(0, Math.Min(5, MDLoss.Length));
					accessLossManageData[Dis_Item[i]] = MDLoss_5;
				}
			}
			accessLossManageData.Update();
			//=== Loss Calculation & Wirte ===============================================================

		}

		private void cb_Close_Click(object sender, EventArgs eventArgs)
		{
			Close();

			windowEventAlarm.Close();

			Utilities.KillProcess("FOMSSubmarine");
		}

		private void vncViewer_ConnectComplete(object sender, ConnectEventArgs e)
		{
			// Update the Form to match the geometry of remote desktop (including the height of the menu bar in this form).
			//ClientSize = new Size(e.DesktopWidth, e.DesktopHeight + menuStrip1.Height);

			// Give the remote desktop focus now that it's connected
			//VNCViewer.Focus();
			DebuggingHelper.Output("Connection complete to VNC host.");
		}

		private void vncViewer_ConnectionLost(object sender, EventArgs e)
		{
			DebuggingHelper.Output("Lost Connection to VNC host.");
		}

		private void vncViewer_ClipboardChanged(object sender, EventArgs e)
		{
			DebuggingHelper.Output("Remote clipboard copied to local host.");
		}

#region Menu Operation

		public void m_CloseProgram_Click(object sender, EventArgs eventArgs)
		{
			Close();

			windowEventAlarm.Close();

			Utilities.KillProcess("FOMSSubmarine");

			return;
		}

		public void m_CableInfoSet_Click(object sender, EventArgs eventArgs)
		{
			var form = new CableSettingsForm();
			form.Owner = this;
			form.SetDeviceControls(otdr);
			form.Show();
		}

		public void m_OTDRParamSet_Click(object sender, EventArgs eventArgs)
		{
			var form = new OTDRSettingsForm();
			form.Owner = this;
			form.SetDeviceControls(otdr);
			form.ShowDialog();
		}

		public void m_SingleCtrl_Click(object sender, EventArgs eventArgs)
		{
			var form = new SingleControllerForm();
			form.Owner = this;
			form.SetDeviceControls(otdr, osw);
			form.Show();
		}

		private void m_EquipmentsSettings_Click(object sender, EventArgs e)
		{
			var form = new EquipmentsSettingsForm();
			form.Owner = this;
			form.StartPosition = FormStartPosition.CenterParent;
			form.SelectedOTDRIndex = (int)otdr.Model;
			form.SelectedOSWIndex = (int)osw.Model;
			DialogResult result = form.ShowDialog();
			if (result == DialogResult.OK)
			{
				if (otdr.Model != (OTDRControl.OTDRModel)form.SelectedOTDRIndex)
				{
					var otdrModel = (OTDRControl.OTDRModel)form.SelectedOTDRIndex;
					if (otdr.IsConnected)
					{
						DisconnectOTDR();
					}
					otdr.IsReceivedDataCallbackEnabled = false;
					otdr.DataReceived -= OTDR_DataArrival;
					otdr.Dispose();
					otdr = new OTDRControl(otdrModel);
					otdr.DataReceived += OTDR_DataArrival;
					if (otdrModel == OTDRControl.OTDRModel.Anritsu_MT9083)
					{
						VNCViewer.Visible = false;
						WebBrowser.Visible = true;
						//WebBrowser.Navigate(new Uri("http://" + ipOTDR));    // OTDR Access Master
						otdrPort = 2288;
					}
					else if (otdrModel == OTDRControl.OTDRModel.VIAVI_SmartOTDR)
					{
						WebBrowser.Visible = false;
						VNCViewer.Visible = true;
						otdrPort = 8002;
					}
				}
				Utilities.SetAppRegistryString("OTDR_Model", OTDRControl.ModelNames[(int)otdr.Model]);

				if (osw.Model != (OSWControl.OSWModel)form.SelectedOSWIndex)
				{
					var oswModel = (OSWControl.OSWModel)form.SelectedOSWIndex;
					bool shouldReconnect = false;
					if (osw.IsConnected)
					{
						DisconnectOSW();
						shouldReconnect = true;
					}
					osw.Dispose();
					osw = new OSWControl(oswModel);
					if (oswModel == OSWControl.OSWModel.FOD_5510)
					{
						cbox_PortName.Enabled = false;
					}
					else
					{
						portOSW = int.Parse(Utilities.GetAppRegistryString("OSW_COMPort", Port_Idx.ToString()));
						Port_Idx = portOSW;
						cbox_PortName.Enabled = true;
						cbox_PortName.SelectedIndex = Port_Idx - 1;
						if (shouldReconnect)
						{
							cb_OSWConnect_Click(null, null);
						}
					}
				}
				Utilities.SetAppRegistryString("OSW_Model", OSWControl.ModelNames[(int)osw.Model]);
			}
		}

		private void m_EmailSettings_Click(object sender, EventArgs e)
		{
			var form = new EmailSettingsForm();
			form.Owner = this;
			for (int i = 0; i < Constants.MaxEmailReceiver; ++i)
			{
				form.Emails[i] = emails[i];
			}
			form.Server = emailServer;
			form.EncryptedConnection = emailServerEncryptedConnection;
			form.UserID = emailServerUserID;
			form.Password = emailServerPassword;
			form.Port = emailServerPort;
			form.UseDefaultPort = emailServerUseDefaultPort;
			form.SendFrom = emailSendFrom;
			form.SendTo = emailSendTo;
			form.SendTheme = emailSendTheme;
			form.StartPosition = FormStartPosition.CenterParent;
			DialogResult result = form.ShowDialog();
			if (result == DialogResult.OK)
			{
				for (int i = 0; i < Constants.MaxEmailReceiver; ++i)
				{
					emails[i] = form.Emails[i];
					Utilities.SetAppRegistryString("EmailReceiver" + i.ToString(), emails[i]);
				}
				emailServer = form.Server;
				emailServerEncryptedConnection = form.EncryptedConnection;
				emailServerUserID = form.UserID;
				emailServerPassword = form.Password;
				emailServerPort = form.Port;
				emailServerUseDefaultPort = form.UseDefaultPort;
				emailSendFrom = form.SendFrom;
				emailSendTo = form.SendTo;
				emailSendTheme = form.SendTheme;
				Utilities.SetAppRegistryString("EmailServer", emailServer);
				Utilities.SetAppRegistryString("emailServerEncryptedConnection", emailServerEncryptedConnection.ToString());
				Utilities.SetAppRegistryString("emailServerUserID", emailServerUserID);
				Utilities.SetAppRegistryString("emailServerPassword", emailServerPassword);
				Utilities.SetAppRegistryString("emailServerPort", emailServerPort.ToString());
				Utilities.SetAppRegistryString("emailSendFrom", emailSendFrom);
				Utilities.SetAppRegistryString("emailSendTo", emailSendTo);
				Utilities.SetAppRegistryString("emailSendTheme", emailSendTheme);
			}
		}

#endregion Menu Operation

		/// SOR Read & Write >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
		/// 

		byte[] dataSOR = null;
		int indexSORDataStart = 0;

		public void OTDR_DataArrival(OTDRControl sender, byte[] data)
		{
			if (dataSOR == null)
			{
				dataSOR = new byte[data.Length];
				Array.Copy(data, dataSOR, data.Length);
				DebuggingHelper.Output("new data:{0}", dataSOR.Length);
			}
			else
			{
				// append data
				int destinationIndex = dataSOR.Length;
				Array.Resize(ref dataSOR, dataSOR.Length + data.Length);
				Array.Copy(data, 0, dataSOR, destinationIndex, data.Length);
				DebuggingHelper.Output("data:{0}->{1}", destinationIndex, dataSOR.Length);
			}

			if (dataSOR.Length >= 10)
			{
				if (GlobalVariables.Data_Size == 0)
				{
					if (dataSOR[0] == 0x23) // check header '#'
					{
						// 데이터 사이즈 정보 담은 비트 수 (두 번째 1바이트)
						// ex) #512345 이면, '#'은 헤더 / '5'는 이후 5바이트의 값이 데이터 길이임을 표시
						// '12345'5개의 바이트가 표시한 12345 바이트 길이만큼 이후 데이터가 존재함
						if (dataSOR[1] >= 0x30 && dataSOR[1] <= 0x39)
						{
							int n = dataSOR[1] - 0x30;
							string strNum = Encoding.ASCII.GetString(dataSOR, 2, n);
							if (int.TryParse(strNum, out int size))
							{
								GlobalVariables.Data_Size = size;
							}
							indexSORDataStart = n + 2;
						}
					}
				}
			}

			// 현재 버퍼에 있는 데이터 길이가 위에서 알아낸 데이터 길이보다 같거나 크다면..
			if (GlobalVariables.Data_Size != 0 && dataSOR.Length >= GlobalVariables.Data_Size + indexSORDataStart)
			{
				DebuggingHelper.Output("all data received:{0}+{1}={2}", GlobalVariables.Data_Size, indexSORDataStart, dataSOR.Length);
				BeginInvoke(new Action(() =>
				{
					OnSORDataReceived();
				}));
			}
		}

		private void OnSORDataReceived()
		{
			if (GlobalVariables.Save_Button_On == 1)
			{
				SaveFileDialog dlg = new SaveFileDialog()
				{
					Filter = "SOR (*.sor)|*.sor|All Files|*.*",
					Title = "SOR Trace File Save"
				};
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					SaveSORData(dlg.FileName);
				}

				GlobalVariables.Save_Button_On = 0;
				otdr.IsReceivedDataCallbackEnabled = false;
			}
			else if (Auto_Save_Ready == 1)
			{
				string filename = MakeAutoMeasuringSORFilename();
				AttachFileName = filename;
				SaveSORData(filename);
				DebuggingHelper.Output("SaveSORData:{0}", (object)filename);

				otdr.IsReceivedDataCallbackEnabled = false;

				ReadSORFile(filename);
				Auto_Save_Ready = 0;
			}

			dataSOR = null;
			indexSORDataStart = 0;
		}

		private void SaveSORData(string filename)
		{
			using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
			{
				writer.Write(dataSOR, indexSORDataStart, GlobalVariables.Data_Size);
				writer.Flush();
				writer.Close();
			}

			txt_Messages.SelectedText = DateTime.Now.ToString("HH:mm:ss") + " Save SOR: " + filename + Environment.NewLine;
		}

		private string MakeAutoMeasuringSORFilename()
		{
			DateTime dt = DateTime.Now;
			string filename = dt.ToString("yyyy-MM-dd") + "_" + dt.ToString("HH") + "h" + dt.ToString("mm") + "m" + dt.ToString("ss") + "s";
			int channel;

			Measure_Time = dt.ToString("yyyy-MM-dd") + "-" + dt.ToString("HH:mm:ss");

			if (Measure_Stop == 1)
			{
				channel = int.Parse(Cur_ch_num);
			}
			else
			{
				channel = int.Parse(Cur_ch_num);//- 1
			}

			if (channel == 0)
			{
				GlobalVariables.MA_Fiber_Num = string.Format("{0:D3}", Fiber_Num);
				filename += "_#" + string.Format("{0:D3}", Fiber_Num) + ".sor";
			}
			else
			{
				GlobalVariables.MA_Fiber_Num = string.Format("{0:D3}", channel);
				filename += "_#" + string.Format("{0:D3}", channel) + ".sor";
			}

			return FolderPath + "\\" + filename;
		}

		public void SendOTDRCursorSettings()
		{
			if (otdr.Model == OTDRControl.OTDRModel.Anritsu_MT9083)
			{
				float ac = GlobalVariables.ACursorDelta;
				float bc = GlobalVariables.Cable_Len - GlobalVariables.BCursorDelta;
				float llsaCS = ac;
				float llsaCE = ac + 0.5f;
				float rlsaCS = bc - 0.5f;
				float rlsaCE = bc;

				otdr.SendCommand(
					"sens:acur " + ac.ToString() + ";*WAI;" +
					"sens:bcur " + bc.ToString() + ";*WAI;" +
					"sens:lsal " + llsaCS.ToString() + "," + llsaCE.ToString() + ";*WAI;" +
					"sens:lsar " + rlsaCS.ToString() + "," + rlsaCE.ToString());
			}
			else if (otdr.Model == OTDRControl.OTDRModel.VIAVI_SmartOTDR)
			{
				int ac = (int)(GlobalVariables.ACursorDelta * 1000);
				int bc = (int)((GlobalVariables.Cable_Len - GlobalVariables.BCursorDelta) * 1000);

				otdr.SendCommand(
					"CUR:CUR:POS A," + ac.ToString() + ";" +
					"CUR:CUR:POS B," + bc.ToString());
			}
		}

		private void PlayErrorSound()
		{
			string playerPath = baseDirectory + "SW\\Application\\Sound_Play_Error.exe";
			if (File.Exists(playerPath))
			{
				ProcessStartInfo startInfo = new ProcessStartInfo(playerPath)
				{
					WindowStyle = ProcessWindowStyle.Normal
				};
				Process.Start(startInfo);
			}
			else
			{
				System.Media.SystemSounds.Hand.Play();
			}
		}

        private void txt_Threshold_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
