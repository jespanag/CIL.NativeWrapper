#region Copyright (c) 2012-2020 CIL
/*
CIL.NativeWrapper Software Component Product
Copyright (c) 2012-2020 CIL


Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

	1.	Redistributions of source code must retain the above copyright notice,
			this list of conditions and the following disclaimer.

	2.	Redistributions in binary form must reproduce the above copyright notice,
			this list of conditions and the following disclaimer in the documentation
			and/or other materials provided with the distribution.

	3.	The names of the authors may not be used to endorse or promote products derived
			from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED “AS IS” AND ANY EXPRESSED OR IMPLIED WARRANTIES,
INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL CIL NativeWrapper
OR ANY CONTRIBUTORS TO THIS SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT,
INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA,
OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

*/
#endregion

using System;
using System.IO;
using System.Collections;
using System.Reflection;

using System.Windows.Forms;

using CIL.NativeWrapper.Api.Data;
using CIL.NativeWrapper.Api;

namespace CIL.NativeWrapper.Api
{
	/// <summary>
	/// Application launcher.
	/// </summary>
	public class NetLauncher
	{
		Hashtable assemblyHash;

		// constructors...
		/// <summary>
		/// Creates new instance of the NetLauncher class.
		/// </summary>
		public NetLauncher()
		{			
			assemblyHash = new Hashtable();
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ehAssemblyResolve);
		}		

		// private methods...
		void RegisterAssembly(Assembly assembly)
		{
			if (assembly == null)
				return;
			if (!assemblyHash.ContainsKey(assembly.FullName))
				assemblyHash.Add(assembly.FullName, assembly);
		}
		MethodInfo LoadAssembly(PackedDataStream data)
		{
			if (data == null)
				return null;
			byte[] rawAssembly = data.Data;
			try
			{
				Assembly assembly = Assembly.Load(rawAssembly);
				RegisterAssembly(assembly);
				return assembly.EntryPoint;
			}
			catch (Exception ex)
			{
        Logger.WriteException(ex);
				return null;
			}
		}
		MethodInfo LoadAssemblies(PackedData container)
		{
			if (container == null)
				return null;			
			MethodInfo entry = null;
			int count = container.StreamCount;
			for (int i = 0; i < count; i++)
			{
				PackedDataStream stream = container[i];
				MethodInfo entryPoint = LoadAssembly(stream);
				if (entry == null)
					entry = entryPoint;
			}
			return entry;
		}
		MethodInfo LoadAssemblies(string path)
		{
      Logger.OpenSection("Loading data streams");
      try
      {
        PackedData data = PackedData.LoadFrom(path);
        Logger.WriteLine("Number of streams read: {0}", data.StreamCount);

        string packerClass = data.Options.Packer;
        Logger.WriteLine("Packer class used: {0}", packerClass);

        Logger.WriteLine("Unpacking streams.");
        DataPacker packer = new DataPacker(packerClass);
        data.UnpackStreams(packer);
        Logger.WriteLine("All streams are unpacked.");

        Logger.WriteLine("Loading assemblies.");
        MethodInfo entry = LoadAssemblies(data);
        Logger.WriteLine("All assemblies are loaded.");

        return entry;
      }
      finally
      {
        Logger.CloseSection();
      }
		}
    int LaunchApplication(string path, string cmdLine)
    {
      MethodInfo entry = LoadAssemblies(path);
      ParameterInfo[] parameters = entry.GetParameters();
      string[] args = null;
      if (cmdLine != null &&
        parameters.Length == 1 &&
        parameters[0].ParameterType == typeof(string[]))
      {
        cmdLine = cmdLine.Trim();
        if (cmdLine.Length > 0)
          args = cmdLine.Split(' ');
      }

      object result = entry.Invoke(null, args);
      return result == null ? 0 : (int)result;
    }

    void OpenLogger(string path)
    {
      string appName = Path.GetFileNameWithoutExtension(path);
      string logFile = String.Format("{0}.log", appName);
      Logger.Open(logFile);
    }
    void CloseLogger()
    {
      Logger.Flush();
      Logger.Close();
    }

		// event handlers...
		Assembly ehAssemblyResolve(object sender, ResolveEventArgs args)
		{
			Logger.WriteLine("AssemblyResolve, name: {0}", args.Name);
			if (assemblyHash.ContainsKey(args.Name))
			{
				Logger.WriteLine("{0} was found.", args.Name);
				return assemblyHash[args.Name] as Assembly;
			}
			Logger.WriteLine("{0} was not found.", args.Name);
			return null;
		}

		// public methods...
		/// <summary>
		/// Starts application.
		/// </summary>
		/// <param name="path">The path to the packed file.</param>
		/// <param name="cmdLine">The command line for the application.</param>
		public int Launch(string path, string cmdLine)
		{
      OpenLogger(path);
      Logger.OpenSection("Launch");
			try
			{
				Logger.WriteLine("Starting: {0}", path);
				Logger.WriteLine("Command line: {0}", cmdLine);

        int result = LaunchApplication(path, cmdLine);

        Logger.WriteLine("Shutting down: {0}", path);
        Logger.WriteLine("Return code: {0}", result);

        return result;
			}
			catch (Exception ex)
			{
				Logger.WriteException(ex);
				return -1;
			}
			finally
			{
        Logger.CloseSection();
				Logger.WriteLine();
        CloseLogger();
			}
		}
	}
}