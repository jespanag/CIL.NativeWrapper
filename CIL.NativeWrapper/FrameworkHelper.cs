using System;
using System.Collections;
using System.IO;
using Microsoft.Win32;

namespace CIL.NativeWrapper
{
	/// <summary>
	/// .Net framework helper.
	/// </summary>
	public class FrameworkHelper
	{
		// constructors...
		FrameworkHelper()
		{
		}

		// public methods...
		/// <summary>
		/// Gets .Net framework install root.
		/// </summary>
		/// <returns>.Net framework install root.</returns>
		public static string GetInstallRoot()
		{
			RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework");
			if (key == null)
				return null;
			return key.GetValue("InstallRoot") as string;
		}
		/// <summary>
		/// Gets .Net runtime versions.
		/// </summary>
		/// <returns>.Net runtime versions.</returns>
		public static string[] GetRuntimeVersions()
		{
			string root = GetInstallRoot();
			if (root == null)
				return new string[0];
			DirectoryInfo info = new DirectoryInfo(root);			
			ArrayList result = new ArrayList();
			DirectoryInfo[] dirs = info.GetDirectories("v*");
			foreach (DirectoryInfo dir in dirs)
			{
				string s = dir.Name;
				if (Char.IsDigit(s[1]))
					result.Add(s);
			}
			return (string[])result.ToArray(typeof(string));
		}
	}
}