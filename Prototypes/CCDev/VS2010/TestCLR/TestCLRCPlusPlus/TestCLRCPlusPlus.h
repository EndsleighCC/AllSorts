// TestCLRCPlusPlus.h

#pragma once

using namespace System;

using namespace TestCLRCommon ;

namespace TestCLRCPlusPlus
{

	public ref class TestCLRCPlusPlusClass
	{

        public :TestCLRCPlusPlusClass()
        {
        }

        public :int Unity( void );

        public :int Twice( int intValue ) ;

        public :int Multiply( int value, int multiplier ) ;

        public :int Multiply( TestCLRCommonClass ^ testCLRCommonClass , int multiplier ) ;

        public :System::String ^ Message();

        public :TestCLRCommonOutput ^ PerformFunction( TestCLRCommonInput ^ inputData );

        public :void PerformFunction( TestCLRCommonInput ^ inputData , [Runtime::InteropServices::Out] TestCLRCommonOutput ^ %outputData );

        public :void PerformFlatCalculation( TestCLRCommonInput ^ inputData , [Runtime::InteropServices::Out] TestCLRCommonOutput ^ %outputData );

	};
}
