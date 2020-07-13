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

#pragma once

class Logger
{
private:
	static Logger* instance;
	
	HANDLE logFile;
	bool created;

	Logger(LPTSTR fileName)
	{
		CreateLogFile(fileName);
	}

	void CreateLogFile(LPTSTR fileName)
	{
		logFile = CreateFile(fileName, GENERIC_WRITE | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, NULL, OPEN_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
		if (logFile == INVALID_HANDLE_VALUE)
			return;
		created = true;
	}

	void WriteMsg(LPTSTR msg)
	{
		int length = strlen(msg);
		DWORD written;
		WriteFile(logFile, msg, length, &written, NULL);
	}
	void WriteMsg(LPWSTR msg)
	{
		int length = lstrlenW(msg);
		for (int i = 0; i < length; i++)
		{
			DWORD written;
			WriteFile(logFile, &msg[i], 1, &written, NULL);
		}
	}

public:	
	~Logger(void)
	{
		if (created)
		{
			CloseHandle(logFile);
			created = false;
		}
	}

	static void Write(LPTSTR msg)
	{
		instance->WriteMsg(msg);
	}
	static void Write(LPWSTR msg)
	{
		instance->WriteMsg(msg);
	}

	static void CreateLogger(LPTSTR fileName)
	{
		instance = new Logger(fileName);
	}
	static void DeleteLogger()
	{
		delete instance;
		instance = NULL;
	}
	static Logger* GetInstance()
	{
		return instance;
	}
};

void LogMessage(LPTSTR msg);

void LogMessage(LPWSTR msg);