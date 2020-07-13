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

using CIL.NativeWrapper;
using CIL.NativeWrapper.Api;

namespace CIL.NativeWrapper.Console
{
	/// <summary>
	/// NativeWrapper console application.
	/// Use the following command line arguments:
	///   To pack assemblies run:
	///    CIL.NativeWrapper.Console.exe -p -input:file1;file2 -output:outfile [-runtime:version]
	///  
	///  version - .Net runtime version to use, e.g. specify -runtime:v2.0.50727 to use .Net 2.0 runtime.
	/// 
	/// To run the packed file:
	///   CIL.NativeWrapper.Console.exe -r:file [-args:cmdline]
	/// </summary>
	public class NativeWrapperConsole
	{
		static void ShowHeader()
		{
			System.Console.WriteLine("CIL NativeWrapper, Copyright 2020");
			System.Console.WriteLine("---------------------------------");
		}
		static void PackFiles(string[] files, string dstPath, string packerType, string runtime)
		{
			System.Console.WriteLine("Packing.....");			
			Packer packer = new Packer();
			packer.Pack(files, dstPath, packerType, runtime);
			System.Console.WriteLine(new String(' ', 12) + "done");
		}
		static void PackFiles(CmdLineArgs args)
		{
			string inputValue = args["input"].Value;
			string output = args["output"].Value;
			
			string packer = DefaultPacker.TypeName;
			if (args.Contains("packer"))
				packer = args["packer"].Value;

			string runtime = "v1.1.4322";
			if (args.Contains("runtime"))
				runtime = args["runtime"].Value;

			string[] files = inputValue.Split(';');
			PackFiles(files, output, packer, runtime);
		}
		static void Run(string path, string args)
		{
			if (path == null || path.Length == 0)
				return;
			System.Console.WriteLine("Running.....");
			NetLauncher launcher = new NetLauncher();
			launcher.Launch(path, args);
			System.Console.WriteLine(new String(' ', 12) + "done");
		}
		static void Run(CmdLineArgs args)
		{
			string run = args["r"].Value;
			string argsValue = String.Empty;
			if (args.Contains("args"))
				argsValue = args["args"].Value;
			Run(run, argsValue);
		}
		static void Process(CmdLineArgs args)
		{
			if (args.Contains("p"))
				PackFiles(args);
			else if (args.Contains("r"))
				Run(args);
		}
		static bool ValidateArguments(CmdLineArgs args)
		{			
			if (args.Contains("p"))
				return ValidatePackArguments(args);

			else if (args.Contains("r"))
				return ValidateRunArguments(args);

			return false;
		}
		static bool ValidatePackArguments(CmdLineArgs args)
		{
			if (args == null)
				return false;
			if (!args.Contains("input"))
				return false;
			if (!args.Contains("output"))
				return false;
			return true;
		}
		static bool ValidateRunArguments(CmdLineArgs args)
		{
			if (args == null)
				return false;
			string run = args["r"].Value;
			if (run == null || run == String.Empty)
				return false;
			return true;
		}
		static void ShowUsage()
		{
			System.Console.WriteLine("To pack assemblies:");
			System.Console.WriteLine("CIL.NativeWrapper.Console.exe -p -input:file1;file2 -output:outfile");
			System.Console.WriteLine();
			System.Console.WriteLine("To run packed file:");
			System.Console.WriteLine("CIL.NativeWrapper.Console.exe -r:file [-args:cmdline]");
		}

		[STAThread]
		static void Main(string[] args)
		{
			ShowHeader();
			try
			{
				CmdLineArgsParser cmdParser = new CmdLineArgsParser();
				CmdLineArgs cmdArgs = cmdParser.Parse(args);
				bool isValidCmdLine = ValidateArguments(cmdArgs);
				if (!isValidCmdLine)
					ShowUsage();
				else
					Process(cmdArgs);
			}
			catch (Exception ex)
			{
				System.Console.WriteLine(ex.Message);
				System.Console.WriteLine(ex.Source);
				System.Console.WriteLine(ex.StackTrace);
			}
		}
	}
}