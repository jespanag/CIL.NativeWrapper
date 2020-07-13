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

#include "stdafx.h"
#include "NativeLauncher.h"
#include "Logger.h"

HINSTANCE appInstance;

LPWSTR CreateWideString(char* str, int length)
{
	LPWSTR wideStr = new WCHAR[length + 1];
	wideStr[length] = '\0';

	for (int i = 0; i < length; i++)
		wideStr[i] = (WCHAR)str[i];

	return wideStr;
}

LPTSTR CreateString(char* str, int length)
{
	LPTSTR resStr = new TCHAR[length + 1];
	resStr[length] = '\0';

	for (int i = 0; i < length; i++)
		resStr[i] = (TCHAR)str[i];

	return resStr;
}

LPWSTR ReadRuntimeVersion(LPSTR fileName)
{
	HANDLE hFile; 
 
	hFile = CreateFile(fileName, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL); 
	if (hFile == INVALID_HANDLE_VALUE)
		return NULL;

	DWORD size = GetFileSize(hFile, NULL);
	SetFilePointer(hFile, size - 8, NULL, FILE_BEGIN);

	DWORD length;
	DWORD read;
	bool result = ReadFile(hFile, &length, 4, &read, NULL);
	if (!result)
	{
		CloseHandle(hFile);
		return NULL;
	}

	SetFilePointer(hFile, size - 8 - length, NULL, FILE_BEGIN);

	char* runtime = new char[length];
	result = ReadFile(hFile, runtime, length, &read, NULL);
	if (!result)
	{
		CloseHandle(hFile);
		return NULL;
	}

	CloseHandle(hFile);

	LPWSTR runtimeStr = CreateWideString(runtime, length);

	delete runtime;

	return runtimeStr;
}

LPTSTR CreateLogFileName(char* fileName)
{
	int length = strlen(fileName);
	LPTSTR fileNameStr = CreateString(fileName, length);
	PathRemoveExtension(fileNameStr);
	PathAddExtension(fileNameStr, TEXT(".tlog"));
	return fileNameStr;
}

int APIENTRY _tWinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPTSTR lpCmdLine, int nCmdShow)
{
	appInstance = hInstance;
	
	char fileName[MAX_PATH];
	int res = GetModuleFileName(hInstance, fileName, MAX_PATH);
	if (res == 0)
		return -1;

	LPTSTR logFileName = CreateLogFileName(fileName);
	Logger::CreateLogger(logFileName);

	LogMessage(TEXT("Reading runtime version\r\n"));

	LPWSTR runtime = ReadRuntimeVersion(fileName);
	if (runtime == NULL)
	{
		LogMessage(TEXT("ERROR: Cannot read runtime version!\r\n"));
		return -1;
	}

	LogMessage(TEXT("Runtime version:"));
	LogMessage(runtime);
	LogMessage(TEXT("\r\n"));

	LogMessage(TEXT("Starting NativeLauncher...\r\n"));

	NativeLauncher* launcher = new NativeLauncher();
	int result = launcher->Launch(runtime, lpCmdLine);

	LogMessage(TEXT("Stopped\r\n"));
	
	delete runtime;
	runtime = NULL;

	delete launcher;
	launcher = NULL;

	return result;
}