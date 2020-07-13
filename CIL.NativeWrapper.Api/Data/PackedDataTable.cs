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
	/// Contains information about packed data streams.
	/// </summary>
	public class PackedDataTable
	{
		ArrayList rows;

		// constructors...
		/// <summary>
		/// Creates new instance of the PackedDataTable class.
		/// </summary>
		public PackedDataTable()
		{
			rows = new ArrayList();
		}

		// public methods...
		/// <summary>
		/// Adds packed data row to the table.
		/// </summary>
		/// <param name="length">The lenght of the raw data.</param>
		public void AddRow(int length)
		{
			rows.Add(new PackedDataRow(length));
		}
		/// <summary>
		/// Reads data using the specified binary reader.
		/// </summary>
		/// <param name="reader">The binary reader to use.</param>
		public void Read(BinaryReader reader)
		{
			int count = reader.ReadInt32();
			for (int i = 0 ; i < count; i++)
			{
				PackedDataRow row = new PackedDataRow();
				row.Read(reader);				
				rows.Add(row);
			}
		}
		/// <summary>
		/// Writes data using the specified binary writer.
		/// </summary>
		/// <param name="writer">The binary writer to use.</param>
		public void Write(BinaryWriter writer)
		{
			int count = rows.Count;
			writer.Write(count);
			for (int i = 0 ; i < count; i++)
				this[i].Write(writer);
		}

		// public properties...
		/// <summary>
		/// Returns the number of rows in the table.
		/// </summary>
		public int RowCount
		{
			get { return rows.Count; }
		}
		/// <summary>
		/// Gets packed data row.
		/// </summary>
		public PackedDataRow this[int index]
		{
			get { return (PackedDataRow)rows[index]; }
		}
	}
}