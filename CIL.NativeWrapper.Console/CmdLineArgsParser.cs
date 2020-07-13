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
using System.Collections;

namespace CIL.NativeWrapper.Console
{
	/// <summary>
	/// Command line aruments parser.
	/// </summary>
	public class CmdLineArgsParser
	{
		string[] argStarts;

		// private methods...
		string[] GetArgStarts()
		{
			if (argStarts != null)
				return argStarts;
			string argStart = ArgStart;
			if (argStart == null)
				return null;
			argStarts = argStart.Split(';');
			return argStarts;
		}

		// protected methods...
		/// <summary>
		/// Checks given name for the argument start string and removes it..
		/// </summary>
		/// <param name="name">The name to check.</param>
		protected virtual string RemoveArgStart(string name)
		{
			if (name == null)
				return null;
			string[] starts = GetArgStarts();
			if (starts == null)
				return name;
			foreach (string s in starts)
				if (name.StartsWith(s))
					return name.Remove(0, s.Length);
			return name;
		}

		// public methods...
		/// <summary>
		/// Parsers the given command line arguments.
		/// </summary>
		/// <param name="args">The arguments to parse.</param>
		/// <returns>CmdLineArgs instance.</returns>
		public virtual CmdLineArgs Parse(string[] args)
		{
			CmdLineArgs arguments = new CmdLineArgs();
			if (args == null)
				return arguments;
			int count = args.Length;
			for (int i = 0; i < count; i++)
			{
				CmdLineArg arg = Parse(args[i]);
				if (arg != null)
					arguments.Add(arg);
			}
			return arguments;
		}

		/// <summary>
		/// Parsers single command line argument.
		/// </summary>
		/// <param name="arg">The string argument to parse.</param>
		/// <returns>CmdLineArg instance.</returns>
		public virtual CmdLineArg Parse(string arg)
		{
			if (arg == null)
				return null;			
			
			string name = arg;
			string value = "";
			
			int colonPos = arg.IndexOf(":");
			if (colonPos >= 0)
			{
				name = arg.Substring(0, colonPos);
				value = arg.Substring(colonPos + 1);
			}							

			name = RemoveArgStart(name);
			return new CmdLineArg(name, value);
		}

		// public properties...
		/// <summary>
		/// Gets string with argument start strings, separated by ";".
		/// </summary>
		public virtual string ArgStart
		{
			get { return "-;+;/;\\;"; }
		}
	}
}