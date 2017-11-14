// dllmain.h : Declaration of module class.

class CTestSafeArrayCOMObjectModule : public ATL::CAtlDllModuleT< CTestSafeArrayCOMObjectModule >
{
public :
	DECLARE_LIBID(LIBID_TestSafeArrayCOMObjectLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_TESTSAFEARRAYCOMOBJECT, "{EA7BF265-39AF-4134-9C88-95B1331455F5}")
};

extern class CTestSafeArrayCOMObjectModule _AtlModule;
