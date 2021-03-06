//---------------------------------------------------------------------
// File: ODBCTransmitter.cs
// 
// Summary: This class receives batched requests from BizTalk server and 
// delegates these requests to a threadpool. In general there is no need
// to alter any code in this class.
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
using Microsoft.BizTalk.Message.Interop;

using Microsoft.Samples.BizTalk.Adapters.BaseAdapter;

namespace Microsoft.BizTalk.Adapters.ODBC.RunTime.ODBCTransmitAdapter
{
    public sealed class ODBCTransmitter : AsyncBatchedTransmitter
    {
        #region ODBCTransmitter functions
        
        public ODBCTransmitter( )
            : base(
            "ODBC Transmit Adapter",
            "1.0",
            "ODBC Transmit Adapter",
            "ODBC",
            new Guid( "884c48a8-0856-409a-bafa-344257874914" ),
            "http://schemas.microsoft.com/ODBC",
            typeof( ODBCTransmitAdapterBatch ) ) { }

        public override void InitializeThreadPool( )
        {
            threadPool = new ThreadPool( );
        }

        public ConfigProperties CreateProperties( string uri )
        {
            ConfigProperties properties = new ODBCAdapterProperties( uri );
            return properties;
        }

        protected override void HandlerPropertyBagLoaded( )
        {
            IPropertyBag config = this.HandlerPropertyBag;
            if ( null != config )
            {
                XmlDocument handlerConfigDom = ConfigProperties.IfExistsExtractConfigDom( config );
                if ( null != handlerConfigDom )
                {
                    ODBCAdapterProperties.HandlerConfiguration( handlerConfigDom );
                }
            }
        }

        #endregion
    }
}