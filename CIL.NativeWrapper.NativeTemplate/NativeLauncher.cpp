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
#include "NPackerApi.h"
#include "Logger.h"

#import <mscorlib.tlb> raw_interfaces_only

using namespace mscorlib;

NativeLauncher::NativeLauncher()
{
}

int NativeLauncher::Launch(LPWSTR runtime, LPTSTR cmdLine)
{
	CComPtr<ICorRuntimeHost> pHost;
	HRESULT hr = CorBindToRuntimeEx(runtime, 
													NULL, 
													NULL,
													CLSID_CorRuntimeHost, 
													IID_ICorRuntimeHost, 
													(void **)&pHost);
	if (FAILED(hr))
	{
		LogMessage(TEXT("ERROR: Cannot bind to runtime host!\r\n"));
		return hr;
	}

	LogMessage(TEXT("Bind to runtime host is done\r\n"));
	
	hr = pHost->Start();
	if (FAILED(hr))
	{
		LogMessage(TEXT("ERROR: Cannot start runtime host!\r\n"));
		return hr;
	}

	LogMessage(TEXT("Runtime host is started\r\n"));
	
	CComPtr<IUnknown> pUnk;
	hr = pHost->GetDefaultDomain(&pUnk);
	if (FAILED(hr))
	{
		LogMessage(TEXT("ERROR: Cannot get default AppDomain!\r\n"));
		return hr;
	}

	LogMessage(TEXT("Default AppDomain is obtained\r\n"));

	CComPtr<_AppDomain> appDomain;
	hr = pUnk->QueryInterface(&appDomain.p);
	if (FAILED(hr))
	{
		LogMessage(TEXT("ERROR: Cannot query _AppDomain interface!\r\n"));
		return hr;
	}

	CComSafeArray<unsigned char> *rawAssembly = new CComSafeArray<unsigned char>();
	rawAssembly->Add(packerApiLibLength, packerApiLib);

	CComPtr<_Assembly> assembly;
	hr = appDomain->Load_3(*rawAssembly, &assembly);
	if (FAILED(hr))
	{
		LogMessage(TEXT("ERROR: Cannot load CIL.NativeWrapper.Api into default AppDomain!\r\n"));
		return hr;
	}

	LogMessage(TEXT("CIL.NativeWrapper.Api is loaded\r\n"));

	CComVariant launcher;
	hr = assembly->CreateInstance(_bstr_t("CIL.NativeWrapper.Api.NetLauncher"), &launcher);
	if (FAILED(hr))
	{
		LogMessage(TEXT("ERROR: Cannot create CIL.NativeWrapper.Api.NetLauncher!\r\n"));
		return hr;
	}

	if (launcher.vt != VT_DISPATCH)
	{
		LogMessage(TEXT("ERROR: NetLauncher reference is invalid!\r\n"));
		return E_FAIL;
	}

	LogMessage(TEXT("CIL.NativeWrapper.Api.NetLauncher is created\r\n"));

	CComPtr<IDispatch> disp = launcher.pdispVal;
	DISPID dispid;
	OLECHAR FAR* methodName = L"Launch";
	hr = disp->GetIDsOfNames(IID_NULL, &methodName, 1, LOCALE_SYSTEM_DEFAULT, &dispid);
	if (FAILED(hr))
	{
		LogMessage(TEXT("ERROR: Cannot obtain Launch method!\r\n"));
		return hr;
	}

	TCHAR szPath[MAX_PATH];
  if (!GetModuleFileName(NULL, szPath, MAX_PATH))
		return E_FAIL;

	LogMessage(TEXT("Calling Launch method\r\n"));

	CComVariant *path = new CComVariant(szPath);
	CComVariant *cmdLineArg = new CComVariant(cmdLine);
	CComVariant FAR args[] = {*cmdLineArg, *path};
	DISPPARAMS noArgs = {args, NULL, 2, 0};
	hr = disp->Invoke(dispid, IID_NULL, LOCALE_SYSTEM_DEFAULT, DISPATCH_METHOD, &noArgs, NULL, NULL, NULL);
	if (FAILED(hr))
	{
		LogMessage(TEXT("ERROR: Launch invoke failed!\r\n"));
		return hr;
	}

	LogMessage(TEXT("Application stopped\r\n"));

	hr = pHost->Stop();
	if (FAILED(hr))
	{
		LogMessage(TEXT("ERROR: Cannot stop runtime host!\r\n"));
		return hr;
	}

	return 0;
}