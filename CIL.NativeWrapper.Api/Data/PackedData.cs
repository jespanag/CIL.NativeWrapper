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
	/// Stores all data packed into a file.
	/// </summary>
	public class PackedData
	{
		PackedDataOptions options;
		ArrayList streams;

		// constructors...
		/// <summary>
		/// Creates new instance of the PackedData class.
		/// </summary>
		public PackedData()
		{
			options = new PackedDataOptions();
			streams = new ArrayList();
		}

		// public methods...
		/// <summary>
		/// Adds stream to the packed data.
		/// </summary>
		/// <param name="path">The path of the file to add.</param>
		public void AddStream(string path)
		{
			using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
				AddStream(stream);
		}
		/// <summary>
		/// Adds stream to the packed data.
		/// </summary>
		/// <param name="stream">The stream to add.</param>
		public void AddStream(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream);
			byte[] data = reader.ReadBytes((int)stream.Length);
			AddStream(data);
		}
		/// <summary>
		/// Adds byte stream to the packed data.
		/// </summary>
		/// <param name="data">The byte data to add.</param>
		public void AddStream(byte[] data)
		{
			if (data == null)
				return;
			PackedDataStream stream = new PackedDataStream(data);
			streams.Add(stream);
		}
		/// <summary>
		/// Adds all specified files to the packed data.
		/// </summary>
		/// <param name="files">The array of file paths to add.</param>
		public void AddStreams(string[] files)
		{
			int count = files.Length;
			for (int i = 0; i < count; i++)
				AddStream(files[i]);
		}
		/// <summary>
		/// Loads packed data from the given stream.
		/// </summary>
		/// <param name="stream">The stream to load data from.</param>
		public void Load(Stream stream)
		{
			PackedDataReader reader = new PackedDataReader(this);
			reader.Read(stream);
		}
		/// <summary>
		/// Saves data to the specified file.
		/// </summary>
		/// <param name="path">The path to the file to save data to.</param>
		public void Save(string path)
		{
			using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Write))
				Save(file);
		}
		/// <summary>
		/// Saves data to the specified stream.
		/// </summary>
		/// <param name="stream">The stream to save data to.</param>
		public void Save(Stream stream)
		{
			PackedDataWriter writer = new PackedDataWriter(this);
			writer.Write(stream);
		}
		/// <summary>
		/// Packs data streams.
		/// </summary>
		/// <param name="packer">The data packer to use.</param>
		public void PackStreams(DataPacker packer)
		{
			foreach (PackedDataStream stream in streams)
				stream.Pack(packer);
		}
		/// <summary>
		/// Unpacks data streams.
		/// </summary>
		/// <param name="packer">The data packer to use.</param>
		public void UnpackStreams(DataPacker packer)
		{
			foreach (PackedDataStream stream in streams)
				stream.Unpack(packer);
		}

		// public static methods...
		/// <summary>
		/// Loads packed data from the given file path.
		/// </summary>
		/// <param name="path">The path of the file to use for loading.</param>
		public static PackedData LoadFrom(string path)
		{
			using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
				return LoadFrom(file);
		}
		/// <summary>
		/// Loads packed data from the given stream.
		/// </summary>
		/// <param name="stream">The stream to use for loading.</param>
		public static PackedData LoadFrom(Stream stream)
		{
			PackedData data = new PackedData();
			data.Load(stream);
			return data;
		}

		// public properties...
		/// <summary>
		/// Gets packed data stream by the given index.
		/// </summary>
		public PackedDataStream this[int index]
		{
			get { return (PackedDataStream)streams[index]; }
		}
		/// <summary>
		/// Returns the number of streams inside the packed data.
		/// </summary>
		public int StreamCount
		{
			get { return streams.Count; }
		}
		/// <summary>
		/// Gets packed data options.
		/// </summary>
		public PackedDataOptions Options
		{
			get { return options; }
		}
	}
}