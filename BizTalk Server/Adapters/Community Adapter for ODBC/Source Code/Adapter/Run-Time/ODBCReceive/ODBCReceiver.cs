//---------------------------------------------------------------------
// File: ODBCReceiver.cs
// 
// Summary: This class sets the baseclass values and starts the taskscheduler.
// In most cases there is no need to change anything
// to this file.
//
//---------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation. All rights reserved.
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
    public class ODBCReceiver : Receiver
    {
        public ODBCReceiver( )
            : base(
            "ODBC Adapter",
            "1.0",
            "Microsoft ODBC Receive Adapter",
            "ODBC",
            new Guid( "4b7d3786-5519-45e5-93e1-7ecfcd918c85" ),
            "http://schemas.microsoft.com/ODBC-properties",
            typeof( ODBCReceiverEndpoint ) ) { }

        private static TaskScheduler taskController = new TaskScheduler( );

        #region ODBCReceiver
        
        public static TaskScheduler Scheduler
        {
            get
            {
                return taskController;
            }
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