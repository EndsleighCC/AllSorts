// AverageNumber.cpp : main project file.

#include "stdafx.h"
#include "Global.asax.h"

using namespace System;
using namespace System::Web;
using namespace System::Web::Services;

namespace AverageNumber {


	[WebService(Namespace = L"http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles::BasicProfile1_1)]

	public ref class AverageNumber : public System::Web::Services::WebService
	{
    public:
		
		[WebMethod(Description = L"returns Hello World string")]
        String ^HelloWorld()
		{
			return L"Hello World!";
		}

        [WebMethod(Description = L"returns an average of numbers")]
                UInt32 Average( array<UInt32> ^numbers)
        {

            UInt32 sum=0;

            for (int i=0; i< numbers->Length; i++)
                sum += numbers[i];

            return (sum /numbers->Length);
        }


        // TODO: Add the methods of your Web Service here
	};


}
