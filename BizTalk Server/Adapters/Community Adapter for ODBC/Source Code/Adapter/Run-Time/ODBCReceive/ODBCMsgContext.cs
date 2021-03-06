//---------------------------------------------------------------------
// File: ODBCMsgContext.cs
// 
// Summary: This class is the actual implementation of the adapter
// functionality. In most cases there is no need to change anything
// to this file. Just create an instance from a client and pass it 
// to the ProcessRequest function in ODBCReceive
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
using System.IO;

namespace Microsoft.BizTalk.Adapters.ODBC.RunTime.ODBCReceiveAdapter
{
    /// <summary>
    /// This class is the message context for a receive adapter.
    /// With the properties in this class the receive adapter
    /// has everything it needs to work with.
    /// </summary>
    public class ODBCMsgContextCCC
    {
        private Stream _MsgDataStreamIn = null;
        private Stream _MsgDataStreamOut = null;
        private string _Uri = null;
        private string _ContentType = null;
        private string _CharSet = null;

        public ODBCMsgContextCCC( ) { }

        public string Uri
        {
            get
            {
                return this._Uri;
            }
            set
            {
                this._Uri = value;
            }
        }

        public string ContentType
        {
            get
            {
                return this._ContentType;
            }
            set
            {
                this._ContentType = value;
            }
        }

        public string CharSet
        {
            get
            {
                return this._CharSet;
            }
            set
            {
                this._CharSet = value;
            }
        }

        public System.IO.Stream MsgDataStreamOut
        {
            get
            {
                return this._MsgDataStreamOut;
            }
            set
            {
                this._MsgDataStreamOut = value;
            }
        }

        public System.IO.Stream MsgDataStreamIn
        {
            get
            {
                return this._MsgDataStreamIn;
            }
            set
            {
                this._MsgDataStreamIn = value;
            }
        }
    }
}