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
	/// Contains packed byte data.
	/// </summary>
	public class PackedDataStream
	{
		byte[] data;

		// constructors...
		/// <summary>
		/// Creates new instance of the stream.
		/// </summary>
		public PackedDataStream(byte[] data)
		{
			this.data = data;
		}

		// private methods...
		byte[] PackData(DataPacker packer, byte[] data)
		{
			return packer.Pack(data);
		}
		byte[] UnpackData(DataPacker packer, byte[] data)
		{
			return packer.Unpack(data);
		}

		// public methods...
		/// <summary>
		/// Packs the data stream.
		/// </summary>
		/// <param name="packer">The data packer to use.</param>
		public byte[] Pack(DataPacker packer)
		{
			data = PackData(packer, data);
			return data;
		}
		/// <summary>
		/// Unpacka the data stream.
		/// </summary>
		/// <param name="packer">The data packer to use.</param>
		public byte[] Unpack(DataPacker packer)
		{
			data = UnpackData(packer, data);
			return data;
		}

		// public properties...
		/// <summary>
		/// Gets stream raw data.
		/// </summary>
		public byte[] Data
		{
			get { return data; }
		}
	}
}