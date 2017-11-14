// TestCPlusPlusCLRObject.h

#pragma once

using namespace System;

namespace TestCPlusPlusCLRObject
{

	public ref class ECTestCPlusPlusCLRObject
	{
    public:
		System::IntPtr ^ ProducePointerToCommonCplusCplusData( void ) ;

        void ConsumePointerToCommonCplusCplusData( System::IntPtr ^ intPtr ) ;
	};
}
