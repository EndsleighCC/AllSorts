// TestAppDomainCPP.cpp : main project file.

#include "stdafx.h"

using namespace System;

int main(array<System::String ^> ^args)
{
    AppDomain ^currentAppDomain = AppDomain::CurrentDomain;

    Console::WriteLine("Executing in AppDomain \"{0}\" at path \"{1}\"",
        currentAppDomain->FriendlyName,
        currentAppDomain->BaseDirectory); 

    Console::WriteLine("Executing in AppDomain \"{0}\" at path \"{1}\"",
        AppDomain::CurrentDomain->FriendlyName,
        AppDomain::CurrentDomain->BaseDirectory); 

    Console::ReadLine();

    return 0;
}
