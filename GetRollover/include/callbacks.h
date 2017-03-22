#pragma once
typedef void (*SESSION_STATUS_CALLBACK)(O2GSessionStatus eSessionStatus);
typedef void (*LOGIN_FAILED_CALLBACK)(const char *error);

typedef void (*REQUEST_COMPLETED_CALLBACK)(const char *requestID, FXCHandle response);
typedef void (*REQUEST_FAILED_CALLBACK)(const char *requestId, const char *error);
typedef void (*TABLES_UPDATES_CALLBACK)(FXCHandle response);

typedef void (*TABLE_ROW_CALLBACK)(const char *rowID, FXCHandle rowData);
typedef void (*TABLE_STATUS_CHANGED_CALLBACK)(O2GTableStatus status);

typedef void (*TABLE_MANAGER_STATUS_CHANGED_CALLBACK)(O2GTableManagerStatus status, FXCHandle tableManager);
