#ifdef WIN32
    #define ForexConnectC __declspec(dllimport)
#else
    #define ForexConnectC
#endif

#ifndef __SIZE_TYPE__
#   define __SIZE_TYPE__ unsigned long
typedef __SIZE_TYPE__ size_t;
#endif

#include "constants.h"
#include "definitions.h"
#include "enums.h"
#include "callbacks.h"
#include "cinterface.h"
#include "cinterface_manual.h"
