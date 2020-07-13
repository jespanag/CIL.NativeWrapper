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
	/// Command line arguments list.
	/// </summary>
	public class CmdLineArgs : CollectionBase
	{
		Hashtable argsMap;

		// constructors...
		/// <summary>
		/// Creates new instance of the CmdLineArgs class.
		/// </summary>
		public CmdLineArgs()
		{
			argsMap = new Hashtable();
		}

		// protected methods...
		protected override void OnClearComplete()
		{
			base.OnClearComplete();
			argsMap.Clear();
		}
		protected override void OnInsertComplete(int index, object value)
		{
			base.OnInsertComplete(index, value);
			CmdLineArg arg = (CmdLineArg)value;
			argsMap.Add(arg.Name, arg);
		}
		protected override void OnRemoveComplete(int index, object value)
		{
			base.OnRemoveComplete(index, value);
			CmdLineArg arg = (CmdLineArg)value;
			argsMap.Remove(arg.Name);
		}
		protected override void OnSetComplete(int index, object oldValue, object newValue)
		{
			base.OnSetComplete(index, oldValue, newValue);
			CmdLineArg oldArg = (CmdLineArg)oldValue;
			CmdLineArg newArg = (CmdLineArg)newValue;
			argsMap.Remove(oldArg.Name);
			argsMap.Add(newArg.Name, newArg);
		}

		// public methods...
		/// <summary>
		/// Adds new command line argument.
		/// </summary>
		/// <param name="arg">The argument to add.</param>
		/// <returns>The index of newly added argument.</returns>
		public int Add(CmdLineArg arg)
		{
			if (arg == null)
				return -1;
			argsMap.Add(arg.Name, arg);
			return InnerList.Add(arg);
		}
		/// <summary>
		/// Returns true if the specified argument is present.
		/// </summary>
		/// <param name="name">The name of the argument to check.</param>
		public bool Contains(string name)
		{
			return argsMap.ContainsKey(name);
		}

		// public properties...
		/// <summary>
		/// Gets or sets command line argument at the specified index.
		/// </summary>
		public CmdLineArg this[int index]
		{
			get
			{
				return (CmdLineArg)InnerList[index];
			}
		}
		/// <summary>
		/// Gets or sets command line argument with the specified name.
		/// </summary>
		public CmdLineArg this[string name]
		{
			get
			{
				if (name == null)
					return null;
				return (CmdLineArg)argsMap[name];
			}
		}
	}	
}