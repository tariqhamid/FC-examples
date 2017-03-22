#pragma once
#include "definitions.h"
#include "enums_inner.h"
#include "callbacks.h"

#ifdef __cplusplus
extern "C"
{
#endif

extern ForexConnectC void addRef(FXCHandle);

extern ForexConnectC void release(FXCHandle);

extern ForexConnectC FXCHandle SessionStatusListener_create(
    SESSION_STATUS_CALLBACK sessionStatus,
    LOGIN_FAILED_CALLBACK loginFailed);

extern ForexConnectC void SessionStatusListener_setSessionStatus(FXCHandle listener, SESSION_STATUS_CALLBACK sessionStatus);

extern ForexConnectC void SessionStatusListener_setLoginFailed(FXCHandle listener, LOGIN_FAILED_CALLBACK loginFailed);

extern ForexConnectC FXCHandle ResponseListener_create(
    REQUEST_COMPLETED_CALLBACK requestCompleted,
    REQUEST_FAILED_CALLBACK requestFailed,
    TABLES_UPDATES_CALLBACK tablesUpdates);

extern ForexConnectC void ResponseListener_setRequestCompleted(FXCHandle listener, REQUEST_COMPLETED_CALLBACK requestCompleted);

extern ForexConnectC void ResponseListener_setRequestFailed(FXCHandle listener, REQUEST_FAILED_CALLBACK requestFailed);

extern ForexConnectC void ResponseListener_setTablesUpdates(FXCHandle listener, TABLES_UPDATES_CALLBACK tablesUpdates);

extern ForexConnectC FXCHandle TableListener_create(TABLE_ROW_CALLBACK onAdded,
        TABLE_ROW_CALLBACK onChanged,
        TABLE_ROW_CALLBACK onDeleted,
        TABLE_STATUS_CHANGED_CALLBACK onStatus);

extern ForexConnectC void TableListener_setOnAdded(FXCHandle listener, TABLE_ROW_CALLBACK onAdded);

extern ForexConnectC void TableListener_setOnChanged(FXCHandle listener, TABLE_ROW_CALLBACK onChanged);

extern ForexConnectC void TableListener_setOnDeleted(FXCHandle listener, TABLE_ROW_CALLBACK onDeleted);

extern ForexConnectC void TableListener_setOnStatus(FXCHandle listener, TABLE_STATUS_CHANGED_CALLBACK onStatus);

extern ForexConnectC DATE TimeConverter_convert(FXCHandle instance, DATE date, TimeZone from, TimeZone to);

extern ForexConnectC O2GTableColumnType TableColumn_getType(FXCHandle instance);

extern ForexConnectC bool TradingSettingsProvider_getMargins(FXCHandle instance, const char *instrument, FXCHandle accountRow, double *mmr, double *emr, double *lmr);

extern ForexConnectC const char *SystemPropertiesReader_getProperty(FXCHandle instance, int index, const char **value);

extern ForexConnectC int Session_getReportURL(FXCHandle instance, char *outBuf, int bufferSize, FXCHandle account, DATE dateFrom, DATE dateTo, const char *format, const char *reportType, const char *langID, int ansiCP);

extern ForexConnectC bool AccountsTable_getNextRow(FXCHandle instance, TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool AccountsTable_findRow(FXCHandle instance, const char *id, FXCHandle *row);

extern ForexConnectC bool TradesTable_getNextRow(FXCHandle instance, TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool TradesTable_findRow(FXCHandle instance, const char *id, FXCHandle *row);

extern ForexConnectC bool ClosedTradesTable_getNextRow(FXCHandle instance, TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool ClosedTradesTable_findRow(FXCHandle instance, const char *id, FXCHandle *row);

extern ForexConnectC bool OffersTable_getNextRow(FXCHandle instance, TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool OffersTable_findRow(FXCHandle instance, const char *id, FXCHandle *row);

extern ForexConnectC bool OrdersTable_getNextRow(FXCHandle instance, TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool OrdersTable_findRow(FXCHandle instance, const char *id, FXCHandle *row);

extern ForexConnectC bool MessagesTable_getNextRow(FXCHandle instance, TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool MessagesTable_findRow(FXCHandle instance, const char *id, FXCHandle *row);

extern ForexConnectC bool SummaryTable_getNextRow(FXCHandle instance, TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool SummaryTable_findRow(FXCHandle instance, const char *id, FXCHandle *row);

extern ForexConnectC bool AccountsTable_getNextRowByColumnValue(FXCHandle instance, const char *columnID, const void *columnValueAsVariant, TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool TradesTable_getNextRowByColumnValue(FXCHandle instance, const char *columnID, const void *columnValueAsVariant, TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool ClosedTradesTable_getNextRowByColumnValue(FXCHandle instance, const char *columnID, const void *columnValueAsVariant, TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool OffersTable_getNextRowByColumnValue(FXCHandle instance, const char *columnID, const void *columnValueAsVariant, TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool OrdersTable_getNextRowByColumnValue(FXCHandle instance, const char *columnID, const void *columnValueAsVariant, TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool MessagesTable_getNextRowByColumnValue(FXCHandle instance, const char *columnID, const void *columnValueAsVariant, TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool SummaryTable_getNextRowByColumnValue(FXCHandle instance, const char *columnID, const void *columnValueAsVariant, TableIterator *iterator, FXCHandle *row);

extern ForexConnectC void Table_forEachRow(FXCHandle instance, TABLE_ROW_CALLBACK onEachRow);

extern ForexConnectC void Session_useTableManager(FXCHandle instance, O2GTableManagerMode mode, TABLE_MANAGER_STATUS_CHANGED_CALLBACK onTableManagerStatusChanged);

extern ForexConnectC O2GSessionStatus Session_getSessionStatus(FXCHandle instance);

extern ForexConnectC bool Table_getNextGenericRow(FXCHandle instance, TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool Table_getNextGenericRowByColumnValue(FXCHandle instance, const char * columnID, const void *columnValueAsVariant, TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool AccountsTable_getNextRowByColumnValues(FXCHandle instance, const char *columnName, const int valueCount, const void *columnValues[], TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool AccountsTable_getNextRowByMultiColumnValues(FXCHandle instance, const int columnCount, const char *columnNames[], const void *columnValues[], TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool TradesTable_getNextRowByColumnValues(FXCHandle instance, const char *columnName, const int valueCount, const void *columnValues[], TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool TradesTable_getNextRowByMultiColumnValues(FXCHandle instance, const int columnCount, const char *columnNames[], const void *columnValues[], TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool ClosedTradesTable_getNextRowByColumnValues(FXCHandle instance, const char *columnName, const int valueCount, const void *columnValues[], TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool ClosedTradesTable_getNextRowByMultiColumnValues(FXCHandle instance, const int columnCount, const char *columnNames[], const void *columnValues[], TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool OffersTable_getNextRowByColumnValues(FXCHandle instance, const char *columnName, const int valueCount, const void *columnValues[], TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool OffersTable_getNextRowByMultiColumnValues(FXCHandle instance, const int columnCount, const char *columnNames[], const void *columnValues[], TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool OrdersTable_getNextRowByColumnValues(FXCHandle instance, const char *columnName, const int valueCount, const void *columnValues[], TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool OrdersTable_getNextRowByMultiColumnValues(FXCHandle instance, const int columnCount, const char *columnNames[], const void *columnValues[], TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool MessagesTable_getNextRowByColumnValues(FXCHandle instance, const char *columnName, const int valueCount, const void *columnValues[], TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool MessagesTable_getNextRowByMultiColumnValues(FXCHandle instance, const int columnCount, const char *columnNames[], const void *columnValues[], TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool SummaryTable_getNextRowByColumnValues(FXCHandle instance, const char *columnName, const int valueCount, const void *columnValues[], TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool SummaryTable_getNextRowByMultiColumnValues(FXCHandle instance, const int columnCount, const char *columnNames[], const void *columnValues[], TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool Table_getNextGenericRowByColumnValues(FXCHandle instance, const char *columnName, const int valueCount, const void *columnValues[], TableIterator *iterator, FXCHandle *row);

extern ForexConnectC bool Table_getNextGenericRowByMultiColumnValues(FXCHandle instance, const int columnCount, const char *columnNames[], const void *columnValues[], TableIterator *iterator, FXCHandle *row);

#ifdef __cplusplus
}
#endif
