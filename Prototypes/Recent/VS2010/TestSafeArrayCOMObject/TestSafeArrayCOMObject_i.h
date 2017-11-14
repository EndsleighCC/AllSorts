

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 7.00.0555 */
/* at Thu Apr 14 16:19:17 2016
 */
/* Compiler settings for TestSafeArrayCOMObject.idl:
    Oicf, W1, Zp8, env=Win32 (32b run), target_arch=X86 7.00.0555 
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 475
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __TestSafeArrayCOMObject_i_h__
#define __TestSafeArrayCOMObject_i_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __ITestSafeArray_FWD_DEFINED__
#define __ITestSafeArray_FWD_DEFINED__
typedef interface ITestSafeArray ITestSafeArray;
#endif 	/* __ITestSafeArray_FWD_DEFINED__ */


#ifndef __TestSafeArray_FWD_DEFINED__
#define __TestSafeArray_FWD_DEFINED__

#ifdef __cplusplus
typedef class TestSafeArray TestSafeArray;
#else
typedef struct TestSafeArray TestSafeArray;
#endif /* __cplusplus */

#endif 	/* __TestSafeArray_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"

#ifdef __cplusplus
extern "C"{
#endif 


#ifndef __ITestSafeArray_INTERFACE_DEFINED__
#define __ITestSafeArray_INTERFACE_DEFINED__

/* interface ITestSafeArray */
/* [unique][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_ITestSafeArray;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("5C8904BB-7583-4F22-BA95-34DCF431E7A8")
    ITestSafeArray : public IDispatch
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE Test( 
            /* [in] */ int value) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetSafeArrayOfLongs( 
            /* [retval][out] */ SAFEARRAY * *psa) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetStoredSafeArrayOfLongs( 
            /* [retval][out] */ SAFEARRAY * *psa) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetSafeArrayOfBSTRs( 
            /* [retval][out] */ SAFEARRAY * *psa) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ITestSafeArrayVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ITestSafeArray * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ITestSafeArray * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ITestSafeArray * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ITestSafeArray * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ITestSafeArray * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ITestSafeArray * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ITestSafeArray * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        HRESULT ( STDMETHODCALLTYPE *Test )( 
            ITestSafeArray * This,
            /* [in] */ int value);
        
        HRESULT ( STDMETHODCALLTYPE *GetSafeArrayOfLongs )( 
            ITestSafeArray * This,
            /* [retval][out] */ SAFEARRAY * *psa);
        
        HRESULT ( STDMETHODCALLTYPE *GetStoredSafeArrayOfLongs )( 
            ITestSafeArray * This,
            /* [retval][out] */ SAFEARRAY * *psa);
        
        HRESULT ( STDMETHODCALLTYPE *GetSafeArrayOfBSTRs )( 
            ITestSafeArray * This,
            /* [retval][out] */ SAFEARRAY * *psa);
        
        END_INTERFACE
    } ITestSafeArrayVtbl;

    interface ITestSafeArray
    {
        CONST_VTBL struct ITestSafeArrayVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ITestSafeArray_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ITestSafeArray_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ITestSafeArray_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define ITestSafeArray_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define ITestSafeArray_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define ITestSafeArray_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define ITestSafeArray_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define ITestSafeArray_Test(This,value)	\
    ( (This)->lpVtbl -> Test(This,value) ) 

#define ITestSafeArray_GetSafeArrayOfLongs(This,psa)	\
    ( (This)->lpVtbl -> GetSafeArrayOfLongs(This,psa) ) 

#define ITestSafeArray_GetStoredSafeArrayOfLongs(This,psa)	\
    ( (This)->lpVtbl -> GetStoredSafeArrayOfLongs(This,psa) ) 

#define ITestSafeArray_GetSafeArrayOfBSTRs(This,psa)	\
    ( (This)->lpVtbl -> GetSafeArrayOfBSTRs(This,psa) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ITestSafeArray_INTERFACE_DEFINED__ */



#ifndef __TestSafeArrayCOMObjectLib_LIBRARY_DEFINED__
#define __TestSafeArrayCOMObjectLib_LIBRARY_DEFINED__

/* library TestSafeArrayCOMObjectLib */
/* [version][uuid] */ 


EXTERN_C const IID LIBID_TestSafeArrayCOMObjectLib;

EXTERN_C const CLSID CLSID_TestSafeArray;

#ifdef __cplusplus

class DECLSPEC_UUID("87F67CB4-8D38-410C-98D8-972BCF6562DB")
TestSafeArray;
#endif
#endif /* __TestSafeArrayCOMObjectLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

unsigned long             __RPC_USER  LPSAFEARRAY_UserSize(     unsigned long *, unsigned long            , LPSAFEARRAY * ); 
unsigned char * __RPC_USER  LPSAFEARRAY_UserMarshal(  unsigned long *, unsigned char *, LPSAFEARRAY * ); 
unsigned char * __RPC_USER  LPSAFEARRAY_UserUnmarshal(unsigned long *, unsigned char *, LPSAFEARRAY * ); 
void                      __RPC_USER  LPSAFEARRAY_UserFree(     unsigned long *, LPSAFEARRAY * ); 

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


