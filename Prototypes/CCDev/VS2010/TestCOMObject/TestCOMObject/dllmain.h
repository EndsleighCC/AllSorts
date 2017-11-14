// dllmain.h : Declaration of module class.

class CTestCOMObjectModule : public ATL::CAtlDllModuleT< CTestCOMObjectModule >
{
public :
	DECLARE_LIBID(LIBID_TestCOMObjectLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_TESTCOMOBJECT, "{037504BE-2054-409D-9E29-678702CF7477}")
};

extern class CTestCOMObjectModule _AtlModule;
