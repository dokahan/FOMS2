
namespace FOMSSubmarine
{
	internal static class GlobalVariables
	{
		//******************** Loss Calculation Parameters ***********************************************
		public static int Save_Button_On = 0;		// Single Ctrl시에 SOR 파일 저장 방법 결정을 위함
		//************************************************************************************************

		//******************** Loss Calculation Parameters ***********************************************
		public static float ACursor = 0;			// A Cursor
		public static float BCursor = 0;			// B Cursor
		public static float LLSACursorStart = 0;	// Left LSA Start Cursor
		public static float LLSACursorEnd = 0;		// Left LSA End Cursor
		public static float RLSACursorStart = 0;	// Right LSA Start Cursor
		public static float RLSACursorEnd = 0;		// Right LSA End Cursor
		//************************************************************************************************

		//******************** Cable Setting Parameters **************************************************
		public static float Cable_Len = 52;			// Cable Length   (Default: 50000m)
		public static float Fiber_NumSM = 12;		// Single Mode Fiber Numbers  (Default: 52EA)
		public static float Fiber_NumMM = 0;		// Multi Mode Fiber Numbers  (Default: 4EA)
		public static float Fiber_Number = 0;       // Total Fiber Numbers
		public static float Maximum_Limit = 0.25f;  // Maximum Loss Limit-Alarm Threshold(Default: 0.195dB/km)
		public static float Bend_Limit = 0.2f;      // Maximum Bend Limit-Alarm Threshold(Default: 0.2dB/km) - romee
		public static float Loss_Dev = 0;           // Loss Deviation-Alarm Threshold(Default: 0.005dB/km)
		public static float DummyCable_Len = 2600;  // Dummy Cable Length (Default: 2600m)
		public static float ACursorDelta = 0.5f;
		public static float BCursorDelta = 0.5f;
		//************************************************************************************************

		//******************** OTDR Setting/Measure Parameters *******************************************
		public static bool AutoAcquisitionMode;
		public static int WaveLength = 1550;
		public static float Range = 100.0f;			// Measure Range
		public static int Resolution = 1;			// High
		public static int PulseWidth = 4000;
		public static int AverageTime = 60;			// avg time -> 60 / 180 추가 필요 -> 진도 = 제주 버젼 
		public static double IOR = 1.466700;
		public static double BSC = -81.50;
		public static int Data_Size = 0;			// SOR Data의 사이즈
		//************************************************************************************************

		//******************** Test Variables ************************************************************
		//Public Module_Cal_Complete As Integer
		public static int Module_Cal_Comp_Check = 0;
		//************************************************************************************************

		public static string MA_Fiber_Num = "";

		public static void Initialize()
		{
			Cable_Len = Utilities.GetAppRegistry("CableLength", Cable_Len);
			Fiber_NumSM = Utilities.GetAppRegistry("FiberNumSM", Fiber_NumSM);
			Fiber_NumMM = Utilities.GetAppRegistry("FiberNumMM", Fiber_NumMM);
			Maximum_Limit = Utilities.GetAppRegistry("MaxLimit", Maximum_Limit);
			Bend_Limit = Utilities.GetAppRegistry("BendLimit", Bend_Limit);
			DummyCable_Len = Utilities.GetAppRegistry("DummyCableLength", DummyCable_Len);
			ACursorDelta = Utilities.GetAppRegistry("ACursorDelta", ACursorDelta);
			BCursorDelta = Utilities.GetAppRegistry("BCursorDelta", BCursorDelta);
			Fiber_Number = Fiber_NumSM + Fiber_NumMM;

			ACursor = 2.5f;
			BCursor = Cable_Len;
			LLSACursorStart = ACursor;
			LLSACursorEnd = ACursor + 1;
			RLSACursorStart = BCursor - 1;
			RLSACursorEnd = BCursor;

			AutoAcquisitionMode = Utilities.GetAppRegistry("AquisitionMode", "Manual") == "Auto";
			WaveLength = (int)Utilities.GetAppRegistry("WaveLength", WaveLength);
			Range = (int)Utilities.GetAppRegistry("Range", Range);
			Resolution = (int)Utilities.GetAppRegistry("Resolution", Resolution);
			PulseWidth = Utilities.GetAppRegistry("PulseWidth", PulseWidth);
			AverageTime = Utilities.GetAppRegistry("AverageTime", AverageTime);
			IOR = Utilities.GetAppRegistry("IOR", IOR);
			BSC = Utilities.GetAppRegistry("BSC", BSC);
		}

		public static void SaveCableSettings()
		{
			Utilities.SetAppRegistryString("CableLength", Cable_Len.ToString());
			Utilities.SetAppRegistryString("FiberNumSM", Fiber_NumSM.ToString());
			Utilities.SetAppRegistryString("FiberNumMM", Fiber_NumMM.ToString());
			Utilities.SetAppRegistryString("MaxLimit", Maximum_Limit.ToString());
			Utilities.SetAppRegistryString("BendLimit", Bend_Limit.ToString());
			Utilities.SetAppRegistryString("DummyCableLength", DummyCable_Len.ToString());
			Utilities.SetAppRegistryString("ACursorDelta", ACursorDelta.ToString());
			Utilities.SetAppRegistryString("BCursorDelta", BCursorDelta.ToString());
		}

		public static void SaveOTDRSettings()
		{
			Utilities.SetAppRegistryString("AquisitionMode", AutoAcquisitionMode ? "Auto" : "Manual");
			Utilities.SetAppRegistryString("WaveLength", WaveLength.ToString());
			Utilities.SetAppRegistryString("Range", Range.ToString());
			Utilities.SetAppRegistryString("Resolution", Resolution.ToString());
			Utilities.SetAppRegistryString("PulseWidth", PulseWidth.ToString());
			Utilities.SetAppRegistryString("AverageTime", AverageTime.ToString());
			Utilities.SetAppRegistryString("IOR", IOR.ToString());
			Utilities.SetAppRegistryString("BSC", BSC.ToString());
		}
	}
}
