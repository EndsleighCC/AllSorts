// TestHelloWorld.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <string>
#include <iostream>
using namespace std;

class CHelloWorld
{
public:
    CHelloWorld(const string & str)
    {
        m_str = str;
    }
    string What(void)
    {
        return m_str;
    }
private:
    string m_str;
};

int main()
{
    CHelloWorld helloWorld("fred");

    cout << "Hello " << helloWorld.What() << " World" << endl;

    return 0;
}

