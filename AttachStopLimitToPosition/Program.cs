using System;
using System.Collections.Specialized;
using System.Text;
using System.Configuration;
using fxcore2;

namespace AttachStopLimitToPosition
{
    class Program
    {
        static void Main(string[] args)
        {
            O2GSession session = null;

            try
            {
                LoginParams loginParams = new LoginParams(ConfigurationManager.AppSettings);
                SampleParams sampleParams = new SampleParams(ConfigurationManager.AppSettings);

                PrintSampleParams("AttachStopLimitToPosition", loginParams, sampleParams);

                session = O2GTransport.createSession();
                SessionStatusListener statusListener = new SessionStatusListener(session, loginParams.SessionID, loginParams.Pin);
                session.subscribeSessionStatus(statusListener);
                statusListener.Reset();
                session.login(loginParams.Login, loginParams.Password, loginParams.URL, loginParams.Connection);
                if (statusListener.WaitEvent() && statusListener.Connected)
                {
                    ResponseListener responseListener = new ResponseListener(session);
                    session.subscribeResponse(responseListener);

                    O2GAccountRow account = GetAccount(session, sampleParams.AccountID);
                    if (account == null)
                    {
                        if (string.IsNullOrEmpty(sampleParams.AccountID))
                        {
                            throw new Exception("No valid accounts");
                        }
                        else
                        {
                            throw new Exception(string.Format("The account '{0}' is not valid", sampleParams.AccountID));
                        }
                    }
                    sampleParams.AccountID = account.AccountID;

                    O2GOfferRow offer = GetOffer(session, sampleParams.Instrument);
                    if (offer == null)
                    {
                        throw new Exception(string.Format("The instrument '{0}' is not valid", sampleParams.Instrument));
                    }

                    O2GLoginRules loginRules = session.getLoginRules();
                    if (loginRules == null)
                    {
                        throw new Exception("Cannot get login rules");
                    }
                    O2GTradingSettingsProvider tradingSettingsProvider = loginRules.getTradingSettingsProvider();
                    int iBaseUnitSize = tradingSettingsProvider.getBaseUnitSize(sampleParams.Instrument, account);
                    int iAmount = iBaseUnitSize * sampleParams.Lots;

                    double dRate;
                    double dRateStop;
                    double dRateLimit;
                    double dBid = offer.Bid;
                    double dAsk = offer.Ask;
                    double dPointSize = offer.PointSize;

                    // For the purpose of this example we will place entry order 8 pips from the current market price
                    // and attach stop and limit orders 10 pips from an entry order price
                    if (sampleParams.OrderType.Equals(Constants.Orders.LimitEntry))
                    {
                        if (sampleParams.BuySell.Equals(Constants.Buy))
                        {
                            dRate = dAsk - 8 * dPointSize;
                            dRateLimit = dRate + 10 * dPointSize;
                            dRateStop = dRate - 10 * dPointSize;
                        }
                        else
                        {
                            dRate = dBid + 8 * dPointSize;
                            dRateLimit = dRate - 10 * dPointSize;
                            dRateStop = dRate + 10 * dPointSize;
                        }
                    }
                    else
                    {
                        if (sampleParams.BuySell.Equals(Constants.Buy))
                        {
                            dRate = dAsk + 8 * dPointSize;
                            dRateLimit = dRate + 10 * dPointSize;
                            dRateStop = dRate - 10 * dPointSize;
                        }
                        else
                        {
                            dRate = dBid - 8 * dPointSize;
                            dRateLimit = dRate - 10 * dPointSize;
                            dRateStop = dRate + 10 * dPointSize;
                        }
                    }

                    O2GRequest request = CreateTrueMarketOrderRequest(session, offer.OfferID, sampleParams.AccountID, iAmount, sampleParams.BuySell);
                    if (request == null)
                    {
                        throw new Exception("Cannot create request");
                    }
                    responseListener.SetRequestID(request.RequestID);
                    session.sendRequest(request);
                    if (!responseListener.WaitEvents())
                    {
                        throw new Exception("Response waiting timeout expired");
                    }

                    O2GTradeRow tradeOrder = FindPosition(session, sampleParams.AccountID, responseListener.GetLastOrderID(), responseListener);
                    if (tradeOrder == null)
                    {
                        throw new Exception("Order '{0}' not found");
                    }

                    request = AddOrderForEntryRequest(session, tradeOrder, "L", dRateLimit);
                    if (request == null)
                    {
                        throw new Exception("Cannot create request");
                    }
                    responseListener.SetRequestID(request.RequestID);
                    session.sendRequest(request);
                    if (!responseListener.WaitEvents())
                    {
                        throw new Exception("Response waiting timeout expired");
                    }

                    request = AddOrderForEntryRequest(session, tradeOrder, "S", dRateStop);
                    if (request == null)
                    {
                        throw new Exception("Cannot create request");
                    }
                    responseListener.SetRequestID(request.RequestID);
                    session.sendRequest(request);
                    if (!responseListener.WaitEvents())
                    {
                        throw new Exception("Response waiting timeout expired");
                    }

                    Console.WriteLine("Done!");
                    statusListener.Reset();
                    session.logout();
                    statusListener.WaitEvent();
                    session.unsubscribeResponse(responseListener);
                }
                session.unsubscribeSessionStatus(statusListener);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
            }
            finally
            {
                if (session != null)
                {
                    session.Dispose();
                }
            }
        }

        /// <summary>
        /// Find valid account
        /// </summary>
        private static O2GAccountRow GetAccount(O2GSession session, string sAccountID)
        {
            O2GAccountRow account = null;
            bool bHasAccount = false;
            O2GResponseReaderFactory readerFactory = session.getResponseReaderFactory();
            if (readerFactory == null)
            {
                throw new Exception("Cannot create response reader factory");
            }
            O2GLoginRules loginRules = session.getLoginRules();
            O2GResponse response = loginRules.getTableRefreshResponse(O2GTableType.Accounts);
            O2GAccountsTableResponseReader accountsResponseReader = readerFactory.createAccountsTableReader(response);
            for (int i = 0; i < accountsResponseReader.Count; i++)
            {
                account = accountsResponseReader.getRow(i);
                string sAccountKind = account.AccountKind;
                if (sAccountKind.Equals("32") || sAccountKind.Equals("36"))
                {
                    if (account.MarginCallFlag.Equals("N"))
                    {
                        if (string.IsNullOrEmpty(sAccountID) || sAccountID.Equals(account.AccountID))
                        {
                            bHasAccount = true;
                            break;
                        }
                    }
                }
            }
            if (!bHasAccount)
            {
                return null;
            }
            else
            {
                return account;
            }
        }

        /// <summary>
        /// Find valid offer by instrument name
        /// </summary>
        private static O2GOfferRow GetOffer(O2GSession session, string sInstrument)
        {
            O2GOfferRow offer = null;
            bool bHasOffer = false;
            O2GResponseReaderFactory readerFactory = session.getResponseReaderFactory();
            if (readerFactory == null)
            {
                throw new Exception("Cannot create response reader factory");
            }
            O2GLoginRules loginRules = session.getLoginRules();
            O2GResponse response = loginRules.getTableRefreshResponse(O2GTableType.Offers);
            O2GOffersTableResponseReader offersResponseReader = readerFactory.createOffersTableReader(response);
            for (int i = 0; i < offersResponseReader.Count; i++)
            {
                offer = offersResponseReader.getRow(i);
                if (offer.Instrument.Equals(sInstrument))
                {
                    if (offer.SubscriptionStatus.Equals("T"))
                    {
                        bHasOffer = true;
                        break;
                    }
                }
            }
            if (!bHasOffer)
            {
                return null;
            }
            else
            {
                return offer;
            }
        }


        /// <summary>
        /// Create true market order request
        /// </summary>
        private static O2GRequest CreateTrueMarketOrderRequest(O2GSession session, string sOfferID, string sAccountID, int iAmount, string sBuySell)
        {
            O2GRequest request = null;
            O2GRequestFactory requestFactory = session.getRequestFactory();
            if (requestFactory == null)
            {
                throw new Exception("Cannot create request factory");
            }
            O2GValueMap valuemap = requestFactory.createValueMap();
            valuemap.setString(O2GRequestParamsEnum.Command, Constants.Commands.CreateOrder);
            valuemap.setString(O2GRequestParamsEnum.OrderType, Constants.Orders.TrueMarketOpen);
            valuemap.setString(O2GRequestParamsEnum.AccountID, sAccountID);
            valuemap.setString(O2GRequestParamsEnum.OfferID, sOfferID);
            valuemap.setString(O2GRequestParamsEnum.BuySell, sBuySell);
            valuemap.setInt(O2GRequestParamsEnum.Amount, iAmount);
            valuemap.setString(O2GRequestParamsEnum.CustomID, "TrueMarketOrder");
            request = requestFactory.createOrderRequest(valuemap);
            if (request == null)
            {
                Console.WriteLine(requestFactory.getLastError());
            }
            return request;
        }


        /// <summary>
        /// Get trade by order ID
        /// </summary>
        private static O2GTradeRow FindPosition(O2GSession session, string sAccountID, string sOrderID, ResponseListener responseListener)
        {
            O2GRequestFactory requestFactory = session.getRequestFactory();
            if (requestFactory == null)
            {
                throw new Exception("Cannot create request factory");
            }
            O2GRequest request = requestFactory.createRefreshTableRequestByAccount(O2GTableType.Trades, sAccountID);
            if (request != null)
            {
                responseListener.SetRequestID(request.RequestID);
                session.sendRequest(request);
                if (!responseListener.WaitEvents())
                {
                    throw new Exception("Response waiting timeout expired");
                }
                O2GResponse tradeResponse = responseListener.GetResponse();
                if (tradeResponse != null)
                {
                    if (tradeResponse.Type == O2GResponseType.GetTrades)
                    {
                        O2GResponseReaderFactory responseReaderFactory = session.getResponseReaderFactory();
                        O2GTradesTableResponseReader responseReader = responseReaderFactory.createTradesTableReader(tradeResponse);
                        for (int i = 0; i < responseReader.Count; i++)
                        {
                            O2GTradeRow tradeRow = responseReader.getRow(i);
                            if (sOrderID.Equals(tradeRow.OpenOrderID))
                            {
                                return tradeRow;
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Attach L or S order to existing entry order
        /// </summary>
        private static O2GRequest AddOrderForEntryRequest(O2GSession session, O2GTradeRow trade, string sOrderType, double dRate)
        {
            if (!sOrderType.Equals("L") && !sOrderType.Equals("S"))
            {
                throw new Exception("Incorrect order type");
            }
            O2GRequest request = null;
            O2GRequestFactory requestFactory = session.getRequestFactory();
            if (requestFactory == null)
            {
                throw new Exception("Cannot create request factory");
            }
            O2GValueMap valuemap = requestFactory.createValueMap();
            valuemap.setString(O2GRequestParamsEnum.Command, Constants.Commands.CreateOrder);
            valuemap.setString(O2GRequestParamsEnum.OrderType, sOrderType); // Must be L or S
            valuemap.setString(O2GRequestParamsEnum.AccountID, trade.AccountID);
            valuemap.setString(O2GRequestParamsEnum.OfferID, trade.OfferID);
            valuemap.setString(O2GRequestParamsEnum.TradeID, trade.TradeID); // TradeID from existing Entry order
            string sOppositeDirection = trade.BuySell == Constants.Buy ? Constants.Sell : Constants.Buy;
            valuemap.setString(O2GRequestParamsEnum.BuySell, sOppositeDirection); // The order direction must be opposite to the direction of the order which was used to create the position
            valuemap.setInt(O2GRequestParamsEnum.Amount, trade.Amount);
            valuemap.setDouble(O2GRequestParamsEnum.Rate, dRate);
            valuemap.setString(O2GRequestParamsEnum.CustomID, "AttachedEntryOrder");
            request = requestFactory.createOrderRequest(valuemap);
            if (request == null)
            {
                Console.WriteLine(requestFactory.getLastError());
            }
            return request;
        }

        /// <summary>
        /// Print process name and sample parameters
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="loginPrm"></param>
        /// <param name="prm"></param>
        private static void PrintSampleParams(string procName, LoginParams loginPrm, SampleParams prm)
        {
            Console.WriteLine("{0}: Instrument='{1}', BuySell='{2}', Lots='{3}', AccountID='{4}', OrderType='{5}'", procName, prm.Instrument, prm.BuySell, prm.Lots, prm.AccountID, prm.OrderType);
        }

        class LoginParams
        {
            public string Login
            {
                get
                {
                    return mLogin;
                }
            }
            private string mLogin;

            public string Password
            {
                get
                {
                    return mPassword;
                }
            }
            private string mPassword;

            public string URL
            {
                get
                {
                    return mURL;
                }
            }
            private string mURL;

            public string Connection
            {
                get
                {
                    return mConnection;
                }
            }
            private string mConnection;

            public string SessionID
            {
                get
                {
                    return mSessionID;
                }
            }
            private string mSessionID;

            public string Pin
            {
                get
                {
                    return mPin;
                }
            }
            private string mPin;

            /// <summary>
            /// ctor
            /// </summary>
            /// <param name="args"></param>
            public LoginParams(NameValueCollection args)
            {
                mLogin = GetRequiredArgument(args, "Login");
                mPassword = GetRequiredArgument(args, "Password");
                mURL = GetRequiredArgument(args, "URL");
                if (!string.IsNullOrEmpty(mURL))
                {
                    if (!mURL.EndsWith("Hosts.jsp", StringComparison.OrdinalIgnoreCase))
                    {
                        mURL += "/Hosts.jsp";
                    }
                }
                mConnection = GetRequiredArgument(args, "Connection");
                mSessionID = args["SessionID"];
                mPin = args["Pin"];
            }

            /// <summary>
            /// Get required argument from configuration file
            /// </summary>
            /// <param name="args">Configuration file key-value collection</param>
            /// <param name="sArgumentName">Argument name (key) from configuration file</param>
            /// <returns>Argument value</returns>
            private string GetRequiredArgument(NameValueCollection args, string sArgumentName)
            {
                string sArgument = args[sArgumentName];
                if (!string.IsNullOrEmpty(sArgument))
                {
                    sArgument = sArgument.Trim();
                }
                if (string.IsNullOrEmpty(sArgument))
                {
                    throw new Exception(string.Format("Please provide {0} in configuration file", sArgumentName));
                }
                return sArgument;
            }
        }

        class SampleParams
        {
            public string Instrument
            {
                get
                {
                    return mInstrument;
                }
            }
            private string mInstrument;

            public string BuySell
            {
                get
                {
                    return mBuySell;
                }
            }
            private string mBuySell;

            public int Lots
            {
                get
                {
                    return mLots;
                }
            }
            private int mLots;

            public string OrderType
            {
                get
                {
                    return mOrderType;
                }
            }
            private string mOrderType;

            public string AccountID
            {
                get
                {
                    return mAccountID;
                }
                set
                {
                    mAccountID = value;
                }
            }
            private string mAccountID;

            /// <summary>
            /// ctor
            /// </summary>
            /// <param name="args"></param>
            public SampleParams(NameValueCollection args)
            {
                mInstrument = GetRequiredArgument(args, "Instrument");
                mBuySell = GetRequiredArgument(args, "BuySell");
                string sLots = args["Lots"];
                if (string.IsNullOrEmpty(sLots))
                {
                    sLots = "1"; // default
                }
                Int32.TryParse(sLots, out mLots);
                if (mLots <= 0)
                {
                    throw new Exception(string.Format("\"Lots\" value {0} is invalid; please fix the value in the configuration file", sLots));
                }
                mOrderType = args["OrderType"];
                if (string.IsNullOrEmpty(mOrderType))
                {
                    mOrderType = "LE"; // default
                }
                else
                {
                    if (!mOrderType.Equals(Constants.Orders.LimitEntry) &&
                        !mOrderType.Equals(Constants.Orders.StopEntry))
                    {
                        mOrderType = "LE"; // default
                    }
                }
                mAccountID = args["Account"];
            }

            /// <summary>
            /// Get required argument from configuration file
            /// </summary>
            /// <param name="args">Configuration file key-value collection</param>
            /// <param name="sArgumentName">Argument name (key) from configuration file</param>
            /// <returns>Argument value</returns>
            private string GetRequiredArgument(NameValueCollection args, string sArgumentName)
            {
                string sArgument = args[sArgumentName];
                if (!string.IsNullOrEmpty(sArgument))
                {
                    sArgument = sArgument.Trim();
                }
                if (string.IsNullOrEmpty(sArgument))
                {
                    throw new Exception(string.Format("Please provide {0} in configuration file", sArgumentName));
                }
                return sArgument;
            }
        }

    }
}