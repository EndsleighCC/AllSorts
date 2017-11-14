// TestResource.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"


int _tmain(int argc, _TCHAR* argv[])
{

	HRESULT     hr;
	CRegParser  parser(this);
	HINSTANCE   hInstResDll;
	HRSRC       hrscReg;
	HGLOBAL     hReg;
	DWORD       dwSize;
	LPSTR       szRegA;
	CTempBuffer<TCHAR, 1024> szReg;

	LPCTSTR lpszBSTRFileName = OLE2CT_EX(bstrFileName, _ATL_SAFE_ALLOCA_DEF_THRESHOLD);
#ifndef _UNICODE
	if (lpszBSTRFileName == NULL)
	{
		return E_OUTOFMEMORY;
	}
#endif // _UNICODE

	hInstResDll = LoadLibraryEx(lpszBSTRFileName, NULL, LOAD_LIBRARY_AS_DATAFILE);

	if (NULL == hInstResDll)
	{
		ATLTRACE(atlTraceRegistrar, 0, _T("Failed to LoadLibrary on %s\n"), bstrFileName);
		hr = AtlHresultFromLastError();
		goto ReturnHR;
	}

	hrscReg =FindResource((HMODULE)hInstResDll, szID, szType);

	if (NULL == hrscReg)
	{
		ATLTRACE(atlTraceRegistrar, 0, (HIWORD(szID) == 0) ?
			_T("Failed to FindResource on ID:%d TYPE:%s\n") :
			_T("Failed to FindResource on ID:%s TYPE:%s\n"),
			szID, szType);
		hr = AtlHresultFromLastError();
		goto ReturnHR;
	}
	hReg = LoadResource((HMODULE)hInstResDll, hrscReg);

	if (NULL == hReg)
	{
		ATLTRACE(atlTraceRegistrar, 0, _T("Failed to LoadResource\n"));
		hr = AtlHresultFromLastError();
		goto ReturnHR;
	}

	dwSize = SizeofResource((HMODULE)hInstResDll, hrscReg);
	szRegA = (LPSTR)hReg;

	// Allocate extra space for NULL.
	if (dwSize + 1 < dwSize)
	{
		hr = E_OUTOFMEMORY;
		goto ReturnHR;
	}

	ATLTRY(szReg.Allocate(dwSize + 1));
	if (szReg == NULL)
	{
		hr = E_OUTOFMEMORY;
		goto ReturnHR;
	}

#ifdef _UNICODE
	DWORD uniSize = ::MultiByteToWideChar(_AtlGetConversionACP(), 0, szRegA, dwSize, szReg, dwSize);
	if (uniSize == 0)
	{
		hr = AtlHresultFromLastError();
		goto ReturnHR;
	}
	// Append a NULL at the end.
	szReg[uniSize] = _T('\0');
#else
	Checked::memcpy_s(szReg, dwSize, szRegA, dwSize);
	// Append a NULL at the end.
   	szReg[dwSize] = _T('\0');
#endif

	return 0;
}

