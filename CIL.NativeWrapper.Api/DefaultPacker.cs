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

namespace CIL.NativeWrapper.Api
{
	/// <summary>
	/// DefaultPacker class for managing packed data streams.
	/// </summary>
	public class DefaultPacker
	{
		/// <summary>
		/// Gets full type name of the default packer.
		/// </summary>
		public static readonly string TypeName = typeof(DefaultPacker).FullName;

		// constructors...
		/// <summary>
		/// Creates a new instance of the DataPacker class.
		/// </summary>
		public DefaultPacker()
		{
		}

		// private methods...
		byte[] ProcessData(byte[] data)
		{
			if (data == null)
				return new byte[0];
			byte key = 0x67;
			int count = data.Length;
			byte[] result = new byte[count];
			for (int i = 0; i < count; i++)
				result[i] = (byte)(data[i] ^ key);
			return result;
		}
		byte[] PackData(byte[] data)
		{
			return ProcessData(data);
		}
		byte[] UnpackData(byte[] data)
		{
			return ProcessData(data);
		}

		// public methods...
		/// <summary>
		/// Packs the specified array of bytes and returns packed array.
		/// </summary>
		/// <param name="data">The data to pack.</param>
		public byte[] Pack(byte[] data)
		{
			return PackData(data);
		}
		/// <summary>
		/// Unpacks the specified array of bytes and returns unpacked array.
		/// </summary>
		/// <param name="data">The data to unpack.</param>
		public byte[] Unpack(byte[] data)
		{
			return UnpackData(data);
		}
	}
}