using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOMSSubmarine
{
	// romee
    public static class g
    {
        private static MainWindow main = null;

        internal static MainWindow Main { get => main; set => main = value; }
        public static string FolderPath { get; private set; }

		public static void Init()
		{
			MakeDir();
		}

        public static void MakeDir()
		{
			// Make Folder >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
			DateTime dt = DateTime.Now;
			string FolderName = dt.ToString("yyyy-MM");
			FolderPath = g.Main.baseDirectory + "Log\\" + FolderName;
			Directory.CreateDirectory(FolderPath);
		}

		public static void Log(string str1)
		{
			try
			{
				string fn = FolderPath + "\\Log" + DateTime.Now.ToLongDateString() + ".csv";
				StreamWriter writer = new StreamWriter(File.Open(fn, FileMode.Append));
				writer.Write(str1);
				writer.Close();
			}
			catch (Exception e1)
			{
				Console.WriteLine(e1.Message);
			}
		}

		public static void Log2(string str1)
		{
			try
			{
				string fn = FolderPath + "\\Loss" + DateTime.Now.ToLongDateString() + ".csv";
				StreamWriter writer = new StreamWriter(File.Open(fn, FileMode.Append));
				writer.WriteLine(str1);
				writer.Close();
			}
			catch (Exception e1)
			{
				Console.WriteLine(e1.Message);
			}
		}

	}
}
