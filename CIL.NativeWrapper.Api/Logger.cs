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
using System.Collections;
using System.IO;
using System.Text;

namespace CIL.NativeWrapper.Api
{
	/// <summary>
	/// Logging support.
	/// </summary>
	public sealed class Logger
	{
		static FileStream stream;
		static StreamWriter log;
    static LoggerState state = LoggerState.Closed;

    const char chIndentChar = ' ';
    static int indentSize;
    static string indent;
    static bool indentIsValid;

    static Stack sections;

    enum LoggerState
    {
      Closed,
      Opened
    }

		// constructors...
		static Logger()
		{
		}

    // private methods...
    static void Indent()
    {
      indentSize ++;
      indentIsValid = false;
    }
    static void Unindent()
    {
      indentSize --;
      indentIsValid = false;
    }
    static string GetIndent()
    {
      if (indentIsValid)
        return indent;
      indent = new String(chIndentChar, indentSize);
      indentIsValid = true;
      return indent;
    }

		// public methods...
    /// <summary>
    /// Opens this logger using the given log file path.
    /// </summary>
    /// <param name="path">The path to the log file to use.</param>
    public static void Open(string path)
    {
      if (state == LoggerState.Opened)
        throw new InvalidOperationException("Can't open the log, because it is already opened!");
			stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
      log = new StreamWriter(stream);
      state = LoggerState.Opened;
    }
    /// <summary>
    /// Closes this logger.
    /// </summary>
    public static void Close()
    {
      if (state == LoggerState.Closed)
        throw new InvalidOperationException("Can't close the log, because it is already closed!");
      log.Close();
			stream.Close();
      state = LoggerState.Closed;
    }
    /// <summary>
    /// Saves log to a file with the given path.
    /// </summary>
    public static void Flush()
    {
      log.Flush();
    }
    /// <summary>
    /// Opens log section using the given name.
    /// </summary>
    /// <param name="name">The name of the section to open.</param>
    public static void OpenSection(string name)
    {
      if (sections == null)
        sections = new Stack();
      sections.Push(name);
      WriteLine(">>> {0}", name);
      Indent();
    }
    /// <summary>
    /// Closes active log section.
    /// </summary>
    public static void CloseSection()
    {
      if (sections == null || sections.Count == 0)
        return;
			Unindent();
      string name = sections.Pop() as string;
      WriteLine("<<< {0}", name);
    }
    /// <summary>
    /// Writes empty line to the log.
    /// </summary>
    public static void WriteLine()
    {
      WriteLine("");
    }
		/// <summary>
		/// Writes line to the log using the specified format string and arguments.
		/// </summary>
		/// <param name="format">The format string.</param>
		/// <param name="args">The arguments to write.</param>
		public static void WriteLine(string format, params object[] args)
		{
			Write(String.Format("{0}\r\n", format), args);
		}
    /// <summary>
    /// Writes message to the log using the specified format string and arguments.
    /// </summary>
    /// <param name="format">The format string.</param>
    /// <param name="args">The arguments to write.</param>
    public static void Write(string format, params object[] args)
    {
      string indent = GetIndent();
      log.Write(String.Concat(indent, format), args);
    }
		/// <summary>
		/// Writes exception to the log.
		/// </summary>
		/// <param name="ex">The exception to write.</param>
		public static void WriteException(Exception ex)
		{
			WriteLine(ex.Message);
			WriteLine(ex.Source);
			WriteLine(ex.StackTrace);
			if (ex.InnerException != null)
				WriteException(ex.InnerException);
		}    
	}
}