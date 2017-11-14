

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 7.00.0555 */
/* at Thu Jun 28 14:42:52 2012
 */
/* Compiler settings for TestCOMObject.idl:
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

#ifndef __TestCOMObject_i_h__
#define __TestCOMObject_i_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __ITestSimpleATLObject_FWD_DEFINED__
#define __ITestSimpleATLObject_FWD_DEFINED__
typedef interface ITestSimpleATLObject ITestSimpleATLObject;
#endif 	/* __ITestSimpleATLObject_FWD_DEFINED__ */


#ifndef __TestSimpleATLObject_FWD_DEFINED__
#define __TestSimpleATLObject_FWD_DEFINED__

#ifdef __cplusplus
typedef class TestSimpleATLObject TestSimpleATLObject;
#else
typedef struct TestSimpleATLObject TestSimpleATLObject;
#endif /* __cplusplus */

#endif 	/* __TestSimpleATLObject_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"

#ifdef __cplusplus
extern "C"{
#endif 


#ifndef __ITestSimpleATLObject_INTERFACE_DEFINED__
#define __ITestSimpleATLObject_INTERFACE_DEFINED__

/* interface ITestSimpleATLObject */
/* [unique][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_ITestSimpleATLObject;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("5DD1E5BF-CB11-4331-AA46-0BEEF2F54F47")
    ITestSimpleATLObject : public IDispatch
    {
    public:
        virtual /* [id] */ HRESULT STDMETHODCALLTYPE AddOne( 
            LONG lInput,
            /* [retval][out] */ LONG *plResult) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ITestSimpleATLObjectVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ITestSimpleATLObject * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ITestSimpleATLObject * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ITestSimpleATLObject * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ITestSimpleATLObject * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ITestSimpleATLObject * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ITestSimpleATLObject * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ITestSimpleATLObject * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [id] */ HRESULT ( STDMETHODCALLTYPE *AddOne )( 
            ITestSimpleATLObject * This,
            LONG lInput,
            /* [retval][out] */ LONG *plResult);
        
        END_INTERFACE
    } ITestSimpleATLObjectVtbl;

    interface ITestSimpleATLObject
    {
        CONST_VTBL struct ITestSimpleATLObjectVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ITestSimpleATLObject_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ITestSimpleATLObject_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ITestSimpleATLObject_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define ITestSimpleATLObject_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define ITestSimpleATLObject_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define ITestSimpleATLObject_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define ITestSimpleATLObject_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define ITestSimpleATLObject_AddOne(This,lInput,plResult)	\
    ( (This)->lpVtbl -> AddOne(This,lInput,plResult) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ITestSimpleATLObject_INTERFACE_DEFINED__ */



#ifndef __TestCOMObjectLib_LIBRARY_DEFINED__
#define __TestCOMObjectLib_LIBRARY_DEFINED__

/* library TestCOMObjectLib */
/* [version][uuid] */ 


EXTERN_C const IID LIBID_TestCOMObjectLib;

EXTERN_C const CLSID CLSID_TestSimpleATLObject;

#ifdef __cplusplus

class DECLSPEC_UUID("8C63A659-A8B2-4F19-9C60-48B7CE378DBC")
TestSimpleATLObject;
#endif
#endif /* __TestCOMObjectLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


