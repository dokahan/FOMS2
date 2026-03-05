using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace FOMSSubmarine
{
	class Utilities
	{
		public static string StartupPath
		{
			get
			{
				string path = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
				path.TrimEnd('\\');

				return path;
			}
		}

		public static void SetAppRegistryString(string name, string value)
		{
			RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);

			key = key.CreateSubKey(Constants.CompanyName);
			key = key.CreateSubKey(Constants.InternalAppName);
			key.SetValue(name, value);
			key.Close();
		}

		public static string GetAppRegistryString(string name, string defaultValue)
		{
			RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);

			key = key.CreateSubKey(Constants.CompanyName);
			key = key.CreateSubKey(Constants.InternalAppName);
			string str = (string)key.GetValue(name, defaultValue);
			key.Close();

			return str;
		}

		public static T GetAppRegistry<T>(string name, T defaultValue)
		{
			string str = GetAppRegistryString(name, defaultValue.ToString());

			return ConvertFromString<T>(str);
		}

		public static T ConvertFromString<T>(string str)
		{
			try
			{
				var converter = TypeDescriptor.GetConverter(typeof(T));
				if (converter != null)
				{
					return (T)converter.ConvertFromString(str);
				}
			}
			catch
			{
			}
			return default;
		}

		internal static void Delay(int msDelay)
		{
			Thread.Sleep(msDelay);
		}

		internal static void KillProcess(string name)
		{
			try
			{
				Process[] processes = Process.GetProcessesByName(name);
				foreach (var process in processes)
				{
					process.Kill();
				}
				Thread.Sleep(1000);
			}
			catch (Exception ex)
			{
				System.Windows.Forms.MessageBox.Show(ex.ToString());
			}
		}
	}

	public class DebuggingHelper
	{
		[Conditional("DEBUG")]
		public static void Trace(Exception e)
		{
			// from current thread
			StackTrace stackTrace = new StackTrace(true);
			StackFrame stackFrame = stackTrace.GetFrame(1);

			String fromCurrentThread = String.Format("<TRACE> {0}:{1}:{2}:{3}",
				System.IO.Path.GetFileName(stackFrame.GetFileName()),
				stackFrame.GetMethod(),
				stackFrame.GetFileLineNumber(), stackFrame.GetFileColumnNumber());

			Debug.WriteLine(fromCurrentThread);

			// from exception
			stackTrace = new StackTrace(e, true);
			stackFrame = stackTrace.GetFrame(0);
			String fromException = String.Format("        {0}:{1}:{2}:{3}:{4}",
				(stackFrame.GetFileName() == null) ? "?" : System.IO.Path.GetFileName(stackFrame.GetFileName()),
				stackFrame.GetMethod(),
				stackFrame.GetFileLineNumber(), stackFrame.GetFileColumnNumber(), e.Message);

			Debug.WriteLine(fromException);
		}

		[Conditional("DEBUG")]
		public static void Output(string format, params object[] values)
		{
			Debug.WriteLine(String.Format(format, values));
		}
	}
}
