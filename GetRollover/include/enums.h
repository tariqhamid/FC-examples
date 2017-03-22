#pragma once
#include "enums_inner.h"

typedef enum
{
    TableUnknown = - 1,
    Offers = 0,
    Accounts = 1,
    Orders = 2,
    Trades = 3,
    ClosedTrades = 4,
    Messages = 5,
    Summary = 6
} O2GTable;

typedef enum
{
    ResponseUnknown = -1,
    TablesUpdates = 0,
    MarketDataSnapshot = 1,
    GetAccounts = 2,
    GetOffers = 3,
    GetOrders = 4,
    GetTrades = 5,
    GetClosedTrades = 6,
    GetMessages = 7,
    CreateOrderResponse = 8,
    GetSystemProperties = 9,
    CommandResponse = 10,
    MarginRequirementsResponse = 11,
} O2GResponseType;

typedef enum
{
    UpdateUnknown = - 1,
    Insert = 0,
    Update = 1,
    Delete = 2
} O2GTableUpdateType;

typedef enum
{
    PermissionDisabled = 0,
    PermissionEnabled = 1,
    PermissionHidden = -2
} O2GPermissionStatus;

typedef enum
{
    MarketStatusOpen = 0,     //!< Trading is allowed.
    MarketStatusClosed = 1    //!< Trading is not allowed.
} O2GMarketStatus;

typedef enum
{
    Default = 0,
    NoPrice = 1
} O2GPriceUpdateMode;

typedef enum
{
    No = 0,
    Yes = 1
} O2GTableManagerMode;

typedef enum
{
    Initial = 0,     // initial status.
    Refreshing = 1,  // refresh in progress.
    Refreshed = 2,   // refresh is finished, table filled.
    Failed = 3       // refresh is failed.
} O2GTableStatus;

typedef enum
{
    ReportUrlNotSupported = -1,
    ReportUrlTooSmallBuffer = -2,
    ReportUrlNotLogged = -3
} O2GReportUrlError;

typedef enum
{
    Tick = 0,                    //!< tick
    Min = 1,                    //!< 1 minute
    Hour = 2,                   //!< 1 hour
    Day = 3,                    //!< 1 day
    Week = 4,                   //!< 1 week
    Month = 5,                  //!< 1 month
    Year = 6
} O2GTimeframeUnit;

typedef enum
{
    UnknownParam = -1,
    Command = 1,
    AccountID = 2,
    OfferID = 3,
    TradeID = 4,
    BuySell = 5,
    Amount = 6,
    Rate = 7,
    RateStop = 8,
    RateLimit = 9,
    TrailStepStop = 10,
    TrailStep = 11,
    TimeInForce = 12,
    CustomID = 13,
    OrderID = 14,
    PegOffsetStop = 15,
    PegOffsetLimit = 16,
    PegTypeStop = 17,
    PegTypeLimit = 18,
    PegOffset = 19,
    PegType = 20,
    NetQuantity = 21,
    OrderType = 22,
    RateMin = 23,
    RateMax = 24,
    ContingencyID = 25,
    SubscriptionStatus = 26,
    ClientRate = 27,
    ContingencyGroupType = 28,
    PrimaryQID = 29
} O2GRequestParamsEnum;

typedef enum
{       
    TablesLoading = 0,
    TablesLoaded = 1,
    TablesLoadFailed = 2
} O2GTableManagerStatus;

