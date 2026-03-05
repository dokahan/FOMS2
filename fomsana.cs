using System;
using System.Runtime.InteropServices;

namespace FOMSSubmarine
{
	internal static class fomsana
	{

		//FOMSAna - 2011 LS Cable & System, Anyang, Korea - kimscom@lscns.com
		//FOMSAna(vx1.y1.z1-ex2.y2.z2), x1(Tool)-x2(Engine), x(Major), y(Minor), z(Debug)
		public const string MyVER = "v1.9.0-e1.7.0"; //20121224~20130215-20130305-20130311
		const double BAD_RESULT = -1E+30d;

		public static float g_sectionx = 0;
		public static int g_sectionidx = 0;
		public static int g_scnt = 0;
		public static bool g_diff = false;
		public static bool g_avgf = false;
		public static bool g_licf = false;

		//110620
		public static bool g_dloadf = false; //(True/False)
		public static int g_dftype = 0; //(0/1/...)(SOR/CSV/...)
		public static int g_eidx = 0; //Event Grid Index
		//''''Public g_ddat() As Single 'Distance
		public static double[] g_ddat = null; //Distance
		public static float[] g_tdat = null; //Trace data (Telcordia SR-4731)
		public static float[] g_sdat = null; //Moving Averaged Smoothing Data
		public static float[] g_ldat = null; //LSA Data

		public static string g_strTraceFileName = ""; //Current Opend Data File Name only
		public static string g_strOpenFileName = ""; //Current Saved Data File incl. Folder'110622
		public static string g_strSaveFileName = ""; //Current Saved Data File incl. Folder'110622

		static readonly public double g_C0 = 2.9979246d * Math.Pow(10, 8); //m/sec
		public const double g_MRL = 6.6d; //dB
		public const double g_IEC = 1.56d; //dB
		public const double g_PWID = 1.5d; //dB
		public const double g_FRNL = 13.9d; //dB'20120716
		public const double g_EOF = 3d; //dB'20121226
		//|---20m(FLPNT)---L1-------100m(FRPNT1)--------------------L2----1000m(FRPNT2)--|-----
		public const int g_FLENGTH = 20; //[m]
		public const int g_FRLENGTH1 = 100; //[m]
		public const int g_FRLENGTH2 = 1000; //[m]
		public const double g_LSAATTN = 0.3d; //[dB/km@1650nm]

		public static float g_SPAN = 0; //Scan Length[Km]
		//''''Public g_MPNT As Single 'm/pnt = g_C0 x t_DS[sec] / 10000 / t_GI
		public static double g_MPNT = 0; //m/pnt = g_C0 x t_DS[sec] / 10000 / t_GI
		public static float g_PNTM = 0; //pnt/m = 1 / g_MPNT
		public static int g_PNTML = 0; //pnt/m = CLang(1 / g_MPNT)
		public static int g_PWPNT = 0; //Point Resolution by Pulsewidth
		public static int g_PWM = 0; //Distance Resolution by Pulsewidth [m]
		public static int g_FOFFSETPNT = 0; //Front Panel Offset as Points

		//110620 Convert the Values
		private const double g_MaxULongAnd = 4294967296d;
		private const double g_MaxSLongAnd = 2147483648d;
		private const int g_MaxUInt = 65536;
		private const int g_MaxSIntAnd = 32768;

		//110621 (Telcordia SR-4731 Important Data)
		public static int t_VER = 0; //Trace Version (xxxyz)
		public static string t_UD = ""; //Unit of Distance (km/mt/ft/kf/mi)
		public static float t_WAVEL = 0; //Measured Wavelength[nm] (1650)
		public static int t_PW = 0; //Pulse Width[ns]
		public static int t_DS = 0; //Data Spacing[100psec] one-way time for 10000 data points
		public static int t_NPNT = 0; //Number of Data Points
		public static float t_GI = 0; //Group Index (1.46xxx)
		public static float t_BC = 0; //Backscatter Coefficient[dB]
		public static int t_AVG = 0; //Numbers of Average (Sampling)
		public static int t_AVGT = 0; //Times of Average
		public static int t_SPAN1 = 0; //Acquisition Range[100psec]
		public static int t_SPAN2 = 0; //Acquisition Range[m]
		public static float t_FOFFSET = 0; //Front Panel Offset[m]
		public static float t_NOISEL = 0; //Noise Floor Level[dB]
		public static float t_LT = 0; //Loss Threshold[dB]
		public static float t_RT = 0; //Reflectance Threshold[dB]
		public static float t_ET = 0; //End-of-Fiber Event Threshold[dB]
		public static string t_TT = ""; //Trace Type
		public static int t_NEVENT = 0; //Number of Key Events
		public static string t_VENDOR = ""; //OTDR Vendor Name
		public static string t_MODEL = ""; //OTDR Model
		//20130123 Added
		public static float t_NFSFactor = 0;
		public static float t_POffset = 0;
		public static float t_DSFactor = 0;

		//Event Table
		[Serializable]
		public struct t_EVENT
		{
			public short enum_Renamed; //Event Number
			public float eloc; //Event Location[m]
			public float eac; //Attenuation Coefficient Lead-in Time[dB/Km]
			public float Eloss; //Event Loss[dB]
			public float erl; //Event Reflectance[dB]
			public string ecode; //Event Code
			public string emethod; //Loss Measurement Technique
			public string enote; //Event Comments
			public static t_EVENT CreateInstance()
			{
				t_EVENT result = new t_EVENT();
				result.ecode = String.Empty;
				result.emethod = String.Empty;
				result.enote = String.Empty;
				return result;
			}
		}
		public static t_EVENT[] t_EVENTS = null;
		public static float t_E2EL = 0; //Link's End-to-End Loss[dB]
		public static float t_ORL = 0; //Link's Optical Return Loss
		public static int t_NDAT = 0; //Number of Data Points(Scale Factor)
		public static float[, ] t_DAT = null; //(Scale Factor)(OTDR Data)

		public static float rt_E2EL = 0; //Link's End-to-End Loss[dB]
		public static float rt_ORL = 0; //Link's Optical Return Loss
		public static int rt_NDAT = 0; //Number of Data Points(Scale Factor)
		public static float[] rt_DAT = null; //(Scale Factor)(OTDR Data)

		//Deriv'110628
		public const double g_DERIV_DEFAULT_INCR = 0.01d;
		public const double g_DERIV_BAD_RESULT = -1E+30d;
		//--------------------------------------------------------------------------------------
		public const double BASTAT_EPS = 1E-30d;

		internal static float Lagrange(float[] xarr, float[] yarr, int n, int idx)
		{
			//''Test Code
			//''Static xarr(5) As Double
			//''Static yarr(5) As Double
			//''Dim n As Integer
			//''Dim x As Double, y As Double

			//''n = 5
			//''xarr(0) = 1
			//''xarr(1) = 2
			//''xarr(2) = 3
			//''xarr(3) = 4
			//''xarr(4) = 5
			//''yarr(0) = 1
			//''yarr(1) = 4
			//''yarr(2) = 9
			//''yarr(3) = 16
			//''yarr(4) = 25

			//''Debug.Print "Lagrange interpoplation"
			//''Debug.Print
			//''x = 2.5
			//''y = Lagrange(xarr(), yarr(), n, x)
			//''Debug.Print "f("; x; ") = "; y
			//''x = 3.5
			//''y = Lagrange(xarr(), yarr(), n, x)
			//''Debug.Print "f("; x; ") = "; y

			float prod = 0;

			double yint = 0;
			float x = xarr[idx];
			//Loop for each term
			int tempForEndVar = n - 1;
			for (int i = 0; i <= tempForEndVar; i++)
			{
				//Initialize term with yint[i]
				prod = yarr[i];
				//Build each term
				int tempForEndVar2 = n - 1;
				for (int j = 0; j <= tempForEndVar2; j++)
				{
					if (i != j)
					{
						prod = (float) (prod * (x - xarr[j]) / (xarr[i] - xarr[j]));
					}
				}
				yint += prod;
			}
			return (float) yint;
		}
		static double[] B_Q = new double[6];
		internal static double Q(double x)
		{
			//Function that returns the "Normal" Probability Distribution Integral
			const double twoPi = 6.283185308d;


			B_Q[1] = 0.31938153d;
			B_Q[2] = -0.356563782d;
			B_Q[3] = 1.781477937d;
			B_Q[4] = -1.821255978d;
			B_Q[5] = 1.330274429d;


			double tempo = 1d / (1d + 0.2316419d * Math.Abs(x));

			//Initialize Summation
			double sum = 0d;
			double xp = 1d;

			//Loop to obtain Summation Term
			for (int i = 0; i <= 4; i++)
			{
				xp *= tempo; //Update Power Factor
				sum += B_Q[i] * xp;
			}

			//Calculate Result
			double result = (Math.Exp((-x) * x / 2d) / Math.Sqrt(twoPi) * sum);
			if (x >= 0d)
			{
				return 1d - result;
			}
			else
			{
				return result;
			}
		}
		static double[] c_QInv = new double[5];
		static double[] d_QInv = new double[5];
		internal static double QInv(ref double x)
		{
			//Calculate the Inverse Normal
			double tempo = 0;

			//1st and 2nd Coefficient Array

			c_QInv[1] = 2.515517d;
			c_QInv[2] = 0.802853d;
			c_QInv[3] = 0.010328d;
			c_QInv[4] = 0d;

			d_QInv[1] = 1d;
			d_QInv[2] = 1.432788d;
			d_QInv[3] = 0.189269d;
			d_QInv[4] = 0.001308d;

			//Check limits of x
			if (x <= 0d)
			{
				x = 0.0001d;
			}
			if (x >= 1d)
			{
				x = 0.9999d;
			}

			if (x <= 0.5d)
			{
				tempo = Math.Sqrt(Math.Log(1 / Math.Pow(x, 2)));
			}
			else
			{
				tempo = Math.Sqrt(Math.Log(1d / Math.Pow(1d - x, 2)));
			}

			//Initialize Summations
			double sum1 = 0d;
			double sum2 = 0d;
			double xp = 1d;

			//Start Loop to calculate Summations
			for (int i = 0; i <= 3; i++)
			{
				sum1 += c_QInv[i] * xp;
				sum2 += d_QInv[i] * xp;
				xp *= tempo; //Update Power Factor
			}

			//Calculate the Result
			double result = tempo - sum1 / sum2;
			if (x > 0.5d)
			{
				return -result;
			}
			else
			{
				return result;
			}
		}
		internal static double t(double x, double df)
		{
			//Function will return the Probability of obtaining a student-t Statistic, x, at df Degrees of Freedom

			double xt = x * (1 - 0.25d / df) / Math.Sqrt(1 + Math.Pow(x, 2) / 2d / df);
			return 1d - Q(xt);
		}
		static double[] Pwr_TInv = new double[11];
		static double[] term_TInv = new double[11];
		internal static double TInv(ref double x, double df)
		{
			//Function will return the value of student-t Statistic for a given Probability, x, and df Degrees of Freedom

			//Check limits of x
			if (x <= 0d)
			{
				x = 0.0001d;
			}
			if (x >= 1d)
			{
				x = 0.9999d;
			}
			double xq = QInv(ref x);
			Pwr_TInv[1] = xq;

			//Loop to obtain the Array of Powers
			for (int i = 2; i <= 9; i++)
			{
				Pwr_TInv[i] = Pwr_TInv[i - 1] * xq;
			}

			//Calculate the 4 terms
			term_TInv[1] = 0.25d * (Pwr_TInv[3] + Pwr_TInv[1]);
			term_TInv[2] = (5 * Pwr_TInv[5] + 16 * Pwr_TInv[3] + 3 * Pwr_TInv[1]) / 96d;
			term_TInv[3] = (3 * Pwr_TInv[7] + 19 * Pwr_TInv[5] + 17 * Pwr_TInv[3] - 15 * Pwr_TInv[1]) / 384d;
			term_TInv[4] = (79 * Pwr_TInv[9] + 776 * Pwr_TInv[7] + 1482 * Pwr_TInv[5] - 1920 * Pwr_TInv[3] - 945 * Pwr_TInv[1]) / 92160d;

			//Initialize Summation and Power Factor
			double sum = xq;
			double xp = 1;

			//Loop to add terms
			for (int i = 1; i <= 4; i++)
			{
				xp *= df; //Update df Power Factor
				sum += term_TInv[i] / xp;
			}
			return sum;
		}
		//--------------------------------------------------------------------------------------
		internal static double Log10(double x)
		{
			if (x > 0)
			{
				return Math.Log(x) / Math.Log(10d);
			}
			else
			{
				return -999.9d;
			}
		}
		internal static float getBendRealLoss(int splitr, float otdrdb)
		{ //only one Bending
			float result = 0;
			if (splitr > 1 && otdrdb > 0)
			{
				result = (float) (splitr / (Math.Pow(10d, otdrdb / 5d)) - (splitr - 1));
				return (float) ((-10d) * Log10(result));
			}
			else
			{
				return 0;
			}
		}
		internal static float getBendOTDRLoss(int splitr, float realdb)
		{ //only one Bending
			float frac = 0;
			if (splitr > 1 && realdb > 0)
			{
				frac = (float) Math.Pow(10d, (-1d) * realdb / 10d);
				return (float) (10d * Log10(splitr / (splitr - 1d + frac))); //Loss in OTDR
			}
			else
			{
				return 0;
			}
		}
		internal static float getBreakOTDRDrop(int splitr)
		{ //only one Breakage
			if (splitr > 1)
			{
				return (float) ((-1d) * 10d * Log10((splitr - 1) / ((double) splitr))); //Drop in OTDR
			}
			else
			{
				return 0;
			}
		}
		internal static int SInt2ULong(int val)
		{
			return val & 0xFFFF;
		}
		internal static int ULong2SInt(int val)
		{
			if (val < 0 || val > g_MaxUInt - 1)
			{
				return 0;
			} //Overflow
			if (val < g_MaxSIntAnd)
			{
				return val;
			}
			else
			{
				return val - g_MaxUInt;
			}
		}
		internal static double SLong2UDouble(int val)
		{
			if ((val & unchecked((int) 0x80000000)) != 0)
			{
				return val + g_MaxULongAnd;
			}
			else
			{
				return val;
			}
		}
		internal static int Double2SLong(double val)
		{
			if (val < 0d || val > g_MaxULongAnd)
			{
				return 0;
			} //Overflow
			if (val < g_MaxSLongAnd)
			{
				return Convert.ToInt32(val);
			}
			else
			{
				return Convert.ToInt32(val - g_MaxULongAnd);
			}
		}
	}
}