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
using System.Reflection;
using System.Collections;

using CIL.NativeWrapper.Api;
using CIL.NativeWrapper.Api.Data;
using CIL.NativeWrapper.Api;

namespace CIL.NativeWrapper
{
	/// <summary>
	/// Packer class provides root packing functionality.
	/// </summary>
	public class Packer
	{
		const string strTemplate = "CIL.NativeWrapper.Template.exe";

		// constructors...
		/// <summary>
		/// Creates new instance of the Packer class.
		/// </summary>
		public Packer()
		{
		}

		// private methods...
		byte[] LoadTemplate()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			using (Stream stream = assembly.GetManifestResourceStream(strTemplate))
			{
				BinaryReader reader = new BinaryReader(stream);
				return reader.ReadBytes((int)stream.Length);
			}
		}
		void WriteTemplate(Stream stream)
		{
			byte[] template = LoadTemplate();
			stream.Write(template, 0, template.Length);
		}
		void WriteStreams(Stream stream, string[] files, string packerType, string runtime)
		{
			PackedData data = new PackedData();
			data.Options.Packer = packerType;
			data.Options.Runtime = runtime;

			DataPacker packer = new DataPacker(data.Options.Packer);
			data.AddStreams(files);
			data.PackStreams(packer);
			data.Save(stream);
		}
		void Pack(string[] files, Stream stream, string packer, string runtime)
		{
			WriteTemplate(stream);
			WriteStreams(stream, files, packer, runtime);
		}

		// public methods...
		/// <summary>
		/// Packs the specified assembly files using the given destination file path.
		/// </summary>
    /// <param name="files">The list of files to pack.</param>
    /// <param name="destination">The destination path.</param>
		public void Pack(string[] files, string destination, string packer, string runtime)
		{
      using (FileStream stream = new FileStream(destination, FileMode.Create, FileAccess.Write))
				Pack(files, stream, packer, runtime);
		}
	}
}