// TestSimpleATLObject.cpp : Implementation of CTestSimpleATLObject

#include "stdafx.h"
#include "TestSimpleATLObject.h"


// CTestSimpleATLObject



STDMETHODIMP CTestSimpleATLObject::AddOne(LONG lInput, LONG* plResult)
{
    *plResult = lInput + 1 ;

    return S_OK;
}
