//---------------------------------------------------------------------
// File: ODBCAdapterProperties.cs
// 
// Summary: This class handles the handler properties and the endpoint-
// properties at runtime. If you add or remove any properties to or from
// the designtime make sure you also alter the corresponding code in
// this class. Otherwise, there is no need to alter this code.
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
using System.Xml;
using System.Net;

using Microsoft.BizTalk.Message.Interop;

using Microsoft.Samples.BizTalk.Adapters.BaseAdapter;

namespace Microsoft.BizTalk.Adapters.ODBC.RunTime.ODBCTransmitAdapter
{
    public class ODBCAdapterProperties : ConfigProperties
    {
        // Handler properties...
        private static string _HandlerConnectionString;
        private static string _HandlerTransactionSupport;
        private static long _BinaryReadBufferSize;

        // This class was added to allow us to pull the schemas from 
        // BTS at run time. Remember the SQL to execute is stored in the schemas
        private static BtsSchemaHelper _BTSSchemaHelper = new BtsSchemaHelper( );

        // Endpoint properties...
        private string _ConnectionString;
        private System.Data.IsolationLevel _TransactionSupport;

        // If we needed to use SSO we will need this extra property. It is set in the
        // LocationConfiguration method below.
        // Additionally:
        //   TransmitLocation.xsd in the design-time project must also be edited to
        //   expose the necessary SSO properties.
        //   ODBCTransmitAdapterBatch within the run-time project must be
        //   edited to retrieve and populate the SSOResult class.
        //private string ssoAffiliateApplication;
        //public string AffiliateApplication { get { return ssoAffiliateApplication; } }

        public ODBCAdapterProperties( IBaseMessage msg ) : base( msg ) { }

        public ODBCAdapterProperties( string uri ) : base( uri ) { }

        public BtsSchemaHelper SchemaHelper
        {
            get
            {
                return _BTSSchemaHelper;
            }
        }

        public string ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
        }

        public System.Data.IsolationLevel TransactionSupport
        {
            get
            {
                return _TransactionSupport;
            }
        }

        public static void HandlerConfiguration( XmlDocument configDOM )
        {
            // Handler properties
            ODBCAdapterProperties._HandlerConnectionString = Extract( configDOM, "/Send/ConnectionString" );
            ODBCAdapterProperties._HandlerTransactionSupport = Extract( configDOM, "/Send/TransactionSupport" );
            ODBCAdapterProperties._BinaryReadBufferSize = ExtractLong( configDOM, "/Send/BinaryReadBufferSize" );
        }

        public override void LocationConfiguration( XmlDocument configDOM )
        {
            base.LocationConfiguration( configDOM );

            // If we needed to use SSO we will need this extra property
            //this.ssoAffiliateApplication = IfExistsExtract(configDOM, "/Config/ssoAffiliateApplication");

            this._ConnectionString = Extract( configDOM, "/Send/connectionString" );

            switch ( Extract( configDOM, "/Send/TransactionSupport" ) )
            {
                case "Chaos":
                    this._TransactionSupport = System.Data.IsolationLevel.Chaos;
                    break;
                case "ReadCommitted":
                    this._TransactionSupport = System.Data.IsolationLevel.ReadCommitted;
                    break;
                case "ReadUncommitted":
                    this._TransactionSupport = System.Data.IsolationLevel.ReadUncommitted;
                    break;
                case "RepeatableRead":
                    this._TransactionSupport = System.Data.IsolationLevel.RepeatableRead;
                    break;
                case "Serializable":
                    this._TransactionSupport = System.Data.IsolationLevel.RepeatableRead;
                    break;
                default:
                    this._TransactionSupport = System.Data.IsolationLevel.Unspecified;
                    break;
            }

            UpdateUriForDynamicSend( );
        }

        public override void UpdateUriForDynamicSend( )
        {
            // Strip off the adapters alias
            int i = this.Uri.LastIndexOf( "ODBC://" );
            if ( i > 0 )
            {
                string newUri = this.Uri.Substring( i );
                this.Uri = newUri;
            }
        }
    }
}