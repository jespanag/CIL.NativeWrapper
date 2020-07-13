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
using System.Reflection;

namespace CIL.NativeWrapper.Api
{
	/// <summary>
	/// Data packer class.
	/// </summary>
	public class DataPacker
	{
		string packerClass;
		object packer;

		// constructors...
		/// <summary>
		/// Creates a new instance of the DataPacker class.
		/// </summary>
		public DataPacker(string packerClass)
		{
			this.packerClass = packerClass;
		}

		// private methods...
		object CreatePacker(string packer)
		{
			if (packer == null || packer.Length == 0)
				return null;
			Type type = Type.GetType(packer);
			return Activator.CreateInstance(type);
		}
		object CallMethod(object obj, string name, params object[] args)
		{
			if (obj == null)
				throw new ArgumentNullException("obj");
			if (name == null)
				throw new ArgumentNullException("name");
			Type type = obj.GetType();
			MethodInfo method = type.GetMethod(name);
			return method.Invoke(obj, args);
		}
		object CallPackerMethod(string name, params object[] args)
		{
			if (packer == null)
				packer = CreatePacker(packerClass);
			return CallMethod(packer, name, args);
		}

		// public methods...
		/// <summary>
		/// Packs the specified array of bytes and returns packed array.
		/// </summary>
		/// <param name="data">The data to pack.</param>
		public byte[] Pack(byte[] data)
		{
			return (byte[])CallPackerMethod("Pack", data);
		}
		/// <summary>
		/// Unpacks the specified array of bytes and returns unpacked array.
		/// </summary>
		/// <param name="data">The data to unpack.</param>
		public byte[] Unpack(byte[] data)
		{
			return (byte[])CallPackerMethod("Unpack", data);
		}
	}
}