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
	/// Reader for the PackedData.
	/// </summary>
	public class PackedDataReader
	{
		PackedData data;

		// constructors...
		/// <summary>
		/// Creates new instance of the PackedDataReader class.
		/// </summary>
		/// <param name="data">The data to use.</param>
		public PackedDataReader(PackedData data)
		{
			this.data = data;
		}

		// private methods...
		PackedDataHeader ReadHeader(BinaryReader reader)
		{
			Stream baseStream = reader.BaseStream;
			int length = (int)baseStream.Length;
			baseStream.Seek(length - 4, SeekOrigin.Begin);
			int start = reader.ReadInt32();
			baseStream.Seek(start, SeekOrigin.Begin);
			PackedDataHeader header = new PackedDataHeader();
			header.Start = start;
			header.Read(reader);
			ReadOptions(reader);
			return header;
		}
		void ReadOptions(BinaryReader reader)
		{
			PackedDataOptions options = new PackedDataOptions();
			options.Read(reader);
		}
		ArrayList ReadStreams(Stream file)
		{
			BinaryReader reader = new BinaryReader(file);
			PackedDataHeader header = ReadHeader(reader);
			PackedDataTable table = header.DataTable;
			ArrayList result = new ArrayList();
			int count = table.RowCount;
			for (int i = 0; i < count; i++)
			{
				byte[] data = ReadStream(reader, header.Start, table[i]);
				result.Add(data);
			}
			return result;
		}
		byte[] ReadStream(BinaryReader reader, int headerStart, PackedDataRow row)
		{
			int length = row.Length;
			return reader.ReadBytes(length);
		}
		void AddStreams(ArrayList streams)
		{
			if (streams == null)
				return;
			int count = streams.Count;
			for (int i = 0; i < count; i ++)
				data.AddStream((byte[])streams[i]);
		}

		// public methods...
		/// <summary>
		/// Reads packed data from the specified stream.
		/// </summary>
		/// <param name="stream">The stream to read data from.</param>
		public void Read(Stream stream)
		{
			ArrayList streams = ReadStreams(stream);
			AddStreams(streams);
		}
	}
}