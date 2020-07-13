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

namespace CIL.NativeWrapper.Api.Data
{
	/// <summary>
	/// Contains info about packed data row.
	/// </summary>
	public class PackedDataRow
	{
		int length;

		// constructors...
		/// <summary>
		/// Creates new instance of the PackedDataRow class.
		/// </summary>
		public PackedDataRow()
		{
		}
		/// <summary>
		/// Creates new instance of the PackedDataRow class.
		/// </summary>
		/// <param name="length">The length of row data.</param>
		public PackedDataRow(int length)
		{
			this.length = length;
		}

		// public methods...
		/// <summary>
		/// Reads data using the specified binary reader.
		/// </summary>
		/// <param name="reader">The binary reader to use.</param>
		public void Read(BinaryReader reader)
		{
			length = reader.ReadInt32();
		}
		/// <summary>
		/// Writes data using the specified binary writer.
		/// </summary>
		/// <param name="writer">The binary writer to use.</param>
		public void Write(BinaryWriter writer)
		{
			writer.Write(length);
		}

		// public properties...
		/// <summary>
		/// Gets row data lenght in bytes.
		/// </summary>
		public int Length
		{
			get { return length; }
		}
	}
}