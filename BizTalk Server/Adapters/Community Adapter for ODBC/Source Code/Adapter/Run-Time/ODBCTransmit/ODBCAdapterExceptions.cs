//---------------------------------------------------------------------
// File: ODBCAdapterExceptions.cs
// 
// Summary: This class holds a custom adapter exception. If you need 
// more exception interfaces you can add them here. Otherwise, there
// is no need to alter this code.
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
using System.Runtime.Serialization;

using Microsoft.Samples.BizTalk.Adapters.BaseAdapter;

namespace Microsoft.BizTalk.Adapters.ODBC.RunTime.ODBCTransmitAdapter
{
    internal class ODBCAdapterException : ApplicationException
    {
        #region Adapter exception class
        
        public static string UnhandledTransmit_Error = "The ODBCAdapter encounted an error transmitting a batch of messages.";

        public ODBCAdapterException( ) { }

        public ODBCAdapterException( string msg ) : base( msg ) { }

        public ODBCAdapterException( Exception inner ) : base( String.Empty, inner ) { }

        public ODBCAdapterException( string msg, Exception e ) : base( msg, e ) { }

        protected ODBCAdapterException( SerializationInfo info, StreamingContext context ) : base( info, context ) { }
        
        #endregion
    }
}