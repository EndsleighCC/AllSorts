// TestAnythingCPP.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <time.h>

int main()
{
	time_t tmtTime = 0;

	printf("sizeof time_t is %zd\n", sizeof(time_t));

	struct tm *ptm = NULL;

	time(&tmtTime);
	ptm = gmtime( &tmtTime );

	printf("%04d-%02d-%02d-%02d:%02d:%02d\n",
		ptm->tm_year + 1900,
		ptm->tm_mon + 1,
		ptm->tm_mday,
		ptm->tm_hour,
		ptm->tm_min,
		ptm->tm_sec);

    return 0;
}

