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
using CIL.NativeWrapper.Api;

namespace CIL.NativeWrapper.Api.Data
{
	/// <summary>
	/// Contains packed data options such as packer class name.
	/// </summary>
	public class PackedDataOptions
	{
		/// <summary>
		/// Gets full name of the packer class.
		/// </summary>
		public static readonly string DefaultPackerTypeName = DefaultPacker.TypeName;

		string packer;
		string runtime;

		// constructors...
		/// <summary>
		/// Creates new instance of the PackedDataOptions class.
		/// </summary>
		public PackedDataOptions()
		{
			Reset();
		}

		// public methods...
		/// <summary>
		/// Resets all options to defaults.
		/// </summary>
		public void Reset()
		{
			packer = DefaultPackerTypeName;
		}
		/// <summary>
		/// Reads options using the specified binary reader.
		/// </summary>
		/// <param name="reader">The reader to use.</param>
		public void Read(BinaryReader reader)
		{
			packer = reader.ReadString();
			runtime = reader.ReadString();
		}
		/// <summary>
		/// Writes options using the specified binary writer.
		/// </summary>
		/// <param name="writer">The writer to use.</param>
		public void Write(BinaryWriter writer)
		{
			writer.Write(packer);
			writer.Write(runtime);
		}

		// public properties...
		/// <summary>
		/// Gets or sets fullname of the packer class to use.
		/// </summary>
		public string Packer
		{
			get { return packer; }
			set { packer = value; }
		}
		/// <summary>
		/// Gets or sets .Net runtime version string.
		/// </summary>
		public string Runtime
		{
			get { return runtime; }
			set { runtime = value; }
		}
	}
}