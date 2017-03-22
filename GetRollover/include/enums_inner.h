#pragma once
typedef enum
{
   UTC,
   Local,
   EST,
   Server
} TimeZone;

typedef enum
{
   Integer,
   Double,
   String,
   Date,
   Boolean
}  O2GTableColumnType;

typedef enum
{
    Disconnected = 0,
    Connecting = 1,
    TradingSessionRequested = 2,
    Connected = 3,
    Reconnecting = 4,
    Disconnecting = 5,
    SessionLost = 6,
    PriceSessionReconnecting = 7
} O2GSessionStatus;

