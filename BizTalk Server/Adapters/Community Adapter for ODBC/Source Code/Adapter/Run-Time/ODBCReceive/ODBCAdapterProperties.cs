//---------------------------------------------------------------------
// File: ODBCAdapterProperties.cs
// 
// Summary: This class provides the adapter with the properties
// for the handler and the endpoint. This file must be modified if
// handler- or endpointproperties are being added, changed or removed.
//
//
//---------------------------------------------------------------------
// This file is part of the Microsoft BizTalk Server 2004 SDK
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// This source code is intended only as a supplement to Microsoft BizTalk
// Server 2004 release and/or on-line documentation. See these other
// materials for detailed information regarding Microsoft code samples.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, WHETHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
// PURPOSE.
//---------------------------------------------------------------------
using System;
using System.Xml;

using Microsoft.BizTalk.Component.Interop;

using Microsoft.Samples.BizTalk.Adapters.BaseAdapter;

namespace Microsoft.BizTalk.Adapters.ODBC.RunTime.ODBCReceiveAdapter
{
    public class ODBCAdapterProperties : ConfigProperties
    {
        bool _TwoWay = false;

        //Handler properties...
        private static int _ErrorThreshold;
        //private static int _BatchSize = 20;
        //private static int _WaitTime = 2 * 1000;//2 seconds
        private static long _BinaryReadBufferSize;

        //Endpoint properties...
        private string _ConnectionString;
        private string _PollingIntervalUnitOfMeasure;
        private string _SQLCommand;
        private int _PollingInterval;
        private string _RootNodeName;
        private string _Namespace;
        private bool _PollWhileDataFound;
        private System.Data.IsolationLevel _TransactionISOLevel;

        public ODBCAdapterProperties( string url ) : base( url ) { }

        public static void HandlerConfiguration( XmlDocument handlerConfig )
        {
            //Handler properties
            ODBCAdapterProperties._ErrorThreshold = ExtractInt( handlerConfig, "/Receive/ErrorThreshold" );
            ODBCAdapterProperties._BinaryReadBufferSize = ExtractLong( handlerConfig, "/Receive/BinaryReadBufferSize" );
        }

        //public static void HandlerConfiguration( IPropertyBag config, IPropertyBag bizTalkConfig )
        //{
        //    //Handler properties
        //    //XmlDocument handlerConfig = ExtractConfigDomImpl(config, true);
        //}

        public void LocationConfiguration( IPropertyBag config, IPropertyBag bizTalkConfig )
        {
            XmlDocument endpointConfig = ExtractConfigDomImpl( config, true );

            this._ConnectionString = Extract( endpointConfig, "/Receive/ConnectionString" );
            this._PollingIntervalUnitOfMeasure = Extract( endpointConfig, "/Receive/PollingUnitOfMeasure" );
            this._SQLCommand = Extract( endpointConfig, "/Receive/SQLCommand" );
            this._PollingInterval = ExtractInt( endpointConfig, "/Receive/PollingInterval" );
            this._RootNodeName = Extract( endpointConfig, "/Receive/RootName" );
            this._Namespace = Extract( endpointConfig, "/Receive/Namespace" );
            this._PollWhileDataFound = ExtractBool(endpointConfig, "/Receive/PollWhileDataFound");

            switch ( Extract( endpointConfig, "/Receive/TransactionSupport" ) )
            {
                case "Chaos":
                    this._TransactionISOLevel = System.Data.IsolationLevel.Chaos;
                    break;
                case "ReadCommitted":
                    this._TransactionISOLevel = System.Data.IsolationLevel.ReadCommitted;
                    break;
                case "ReadUncommitted":
                    this._TransactionISOLevel = System.Data.IsolationLevel.ReadUncommitted;
                    break;
                case "RepeatableRead":
                    this._TransactionISOLevel = System.Data.IsolationLevel.RepeatableRead;
                    break;
                case "Serializable":
                    this._TransactionISOLevel = System.Data.IsolationLevel.RepeatableRead;
                    break;
                default:
                    this._TransactionISOLevel = System.Data.IsolationLevel.Unspecified;
                    break;
            }
        }

        public System.Data.IsolationLevel TransactionISOLevel
        {
            get
            {
                return this._TransactionISOLevel;
            }
        }

        public int PollingIntervalMilliseconds
        {
            get 
            {
                switch (_PollingIntervalUnitOfMeasure)
                {
                    case "Hours":
                        return _PollingInterval * 3600 * 1000;
                    case "Minutes":
                        return _PollingInterval * 60 * 1000;
                    case "Seconds":
                        return _PollingInterval * 1000;
                    default:
                        return _PollingInterval * 60 * 1000;
                }
            }
        }

        public bool PollWhileDataFound
        {
            get { return _PollWhileDataFound; }
        }

        public int ErrorThreshold
        {
            get
            {
                return _ErrorThreshold;
            }
        }

        public string ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
        }

        public string PollingIntervalUnitOfMeasure
        {
            get
            {
                return _PollingIntervalUnitOfMeasure;
            }
        }

        public string SQLCommand
        {
            get
            {
                return _SQLCommand;
            }
        }

        public string Namespace
        {
            get
            {
                return _Namespace;
            }
        }

        public string RootNodeName
        {
            get
            {
                return _RootNodeName;
            }
        }

        public int PoolingInterval
        {
            get
            {
                return _PollingInterval;
            }
        }
}
}