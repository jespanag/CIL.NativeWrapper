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

namespace CIL.NativeWrapper.Api
{
	/// <summary>
	/// Writes C code for the specified dll.
	/// </summary>
	public class ApiCodeWriter
	{
		/// <summary>
		/// Creates new instance of the ApiCodeWriter class.
		/// </summary>
		public ApiCodeWriter()
		{
		}

    // private methods...
		byte[] LoadDll(string dll)
		{
			byte[] result = null;
			using (FileStream stream = new FileStream(dll, FileMode.Open, FileAccess.Read))
			{
				result = new byte[stream.Length];
				stream.Read(result, 0, (int)stream.Length);
			}
			return result;
		}
		void WriteCode(string file, byte[] data)
		{
			if (data == null)
				return;
			using (StreamWriter writer = new StreamWriter(file))
			{
				int count = data.Length;
				writer.WriteLine("const int packerApiLibLength = {0};", count);
				writer.WriteLine("unsigned char packerApiLib[packerApiLibLength] =");
				writer.WriteLine("{");
				for (int i = 0; i < count; i++)
				{
					if ((i % 16) == 0)
						writer.WriteLine();
					writer.Write("0x" + data[i].ToString("X2"));
					if (i < count - 1)
						writer.Write(", ");
				}
				writer.WriteLine();
				writer.WriteLine("};");
			}
		}

		// public methods...
    /// <summary>
    /// Writes application packer api in binary form using C code.
    /// </summary>
    /// <param name="dll">The path to the application packer api assembly.</param>
    /// <param name="file">The path of the output file.</param>
		public void WriteCode(string dll, string file)
		{
			if (dll == null || dll.Length == 0)
				return;
			if (file == null || file.Length == 0)
				return;
			byte[] data = LoadDll(dll);
			WriteCode(file, data);
		}
	}
}