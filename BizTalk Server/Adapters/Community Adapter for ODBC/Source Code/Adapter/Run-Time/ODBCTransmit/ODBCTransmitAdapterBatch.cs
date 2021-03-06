//---------------------------------------------------------------------
// File: ODBCTransmitAdapterBatch.cs
// 
// Summary: This class is the implementation of the batched transmit-
// adapter. Make your changes to the SendODBCRequest
// function. In general there is no need to alter any other code in 
// this class.
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
using System.Text;
using System.IO;
using System.Xml;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Microsoft.BizTalk.TransportProxy.Interop;
using Microsoft.BizTalk.Message.Interop;

using Microsoft.Samples.BizTalk.Adapters.BaseAdapter;

namespace Microsoft.BizTalk.Adapters.ODBC.RunTime.ODBCTransmitAdapter
{
    public class ODBCTransmitAdapterBatch : AsyncTransmitterBatch
    {
        private const int LARGE_DATA_THRESHHOLD = 49152;  // 48K

        #region General ODBCTransmitAdapterBatch functions
        
        public ODBCTransmitAdapterBatch( int maxBatchSize, string propertyNamespace, IBTTransportProxy transportProxy, AsyncBatchedTransmitter asyncTransmitter ) : base( maxBatchSize, propertyNamespace, transportProxy, asyncTransmitter )
        {
            base.createProperties = new ConfigProperties.CreateProperties( CreateProperties );
        }

        /// <summary>
        /// Implementation for IThreadpoolWorkItem.ProcessWorkItem where, the thread 
        /// pool will execute this method when we need to do work on the batch
        /// </summary>
        public override void ProcessWorkItem( )
        {
            try
            {
                System.Diagnostics.Trace.WriteLine("BizTalk ODBC Adapter::ODBCTransmitAdapterBatch::ProcessWorkItem()");
                ArrayList messages = this.Messages;
                StandardTransmitBatchHandler btsBatch = new StandardTransmitBatchHandler( transportProxy, null );
                IBaseMessage responseMsg = null;

                for ( int c = 0; c < messages.Count; c++ )
                {
                    TransmitterMessage msg = ( TransmitterMessage )messages[ c ];
                    // Try to send the message and, if successfull, delete the message
                    // from the AppQ
                    if ( msg.PortIsTwoWay )
                    {
                        // If the solicit message was successfully sent, submit the response
                        // and delete the solicit message form the AppQueue..
                        if ( TwoWaySend( msg, out responseMsg ) )
                        {

                            // The submission of the response message and deletion
                            // of the solicit message should be in the same batch...
                            btsBatch.SubmitResponseMessage( msg.Message, responseMsg, null );
                            btsBatch.DeleteMessage( msg.Message, null );
                        }
                        // Otherwise, resubmit. Note, the batch handler will take care
                        // or resubmit failing etc.
                        else
                            btsBatch.Resubmit( msg.Message, null );
                    }
                    else
                    {
                        // If we successfully sent the message, delete it from the AppQueue...
                        if ( OneWaySend( msg ) )
                            btsBatch.DeleteMessage( msg.Message, null );
                        // Otherwise, resubmit. Note, the batch handler will take care
                        // or resubmit failing etc.
                        else
                            btsBatch.Resubmit( msg.Message, null );
                    }
                }

                try
                {
                    // Commit the batch, specifying the callback on completion...
                    btsBatch.BeginDone( null, new AsyncCallback( BatchComplete ), ( object )responseMsg );
                }
                catch ( COMException e )
                {
                    Trace.WriteLine( string.Format( "Exception caught in ODBCAdapterWorkItem.ProcessWorkItem(): {0}", e ), "ODBC Transmit Adapter: Error" );

                    // If the BizTalk service is shutting done, drop this batch
                    if ( e.ErrorCode != BTTransportProxy.BTS_E_MESSAGING_SHUTTING_DOWN )
                    {
                        this.transportProxy.SetErrorInfo( e );
                        throw e;
                    }
                }
            }
            catch ( Exception e )
            {
                // We should never get here, if we do it means messages will be 
                // left in the AppQ and not delivered. We should always handle 
                // any failures in Delete / Resubmit
                Trace.WriteLine( string.Format( "Exception caught in ODBCAdapterWorkItem.ProcessWorkItem(): {0}", e ), "ODBC Transmit Adapter: Error" );
                this.transportProxy.SetErrorInfo( e );

                throw new ODBCAdapterException( ODBCAdapterException.UnhandledTransmit_Error + "[ProcessWorkItem]", e );
            }
            finally
            {
                this.asyncTransmitter.Leave( );
            }
        }

        public void BatchComplete( IAsyncResult ar )
        {
            IBaseMessage responseMsg = ( IBaseMessage )ar.AsyncState;
            if ( null != responseMsg )
                responseMsg.BodyPart.GetOriginalDataStream( ).Close( );
        }

        /// <summary>
        /// Used to transmit an individual message. The message stream is read into 
        /// a buffer and written to disc. Note, we need to process the data in a 
        /// streaming fashion, since the strean we receive is most likely a forward- 
        /// only stream we have no idea how big it is. We therefore need to process
        /// it in a streaming fashion to avoid an out-of-memory (OOM) situation
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool OneWaySend( TransmitterMessage msg )
        {
            Stream ResponseStream = null;
            return SendODBCRequest( msg, false, out ResponseStream );
        }

        private bool TwoWaySend( TransmitterMessage msg, out IBaseMessage responseMsg )
        {
            bool result = false;
            IBaseMessage respMsg;
            responseMsg = null;

            Stream ResponseStream = null;
            try
            {
                if ( SendODBCRequest( msg, true, out ResponseStream ) )
                {
                    IBaseMessageFactory mf = transportProxy.GetMessageFactory( );
                    respMsg = mf.CreateMessage( );
                    IBaseMessagePart body = mf.CreateMessagePart( );
                    body.Data = ResponseStream;
                    respMsg.AddPart( "Body", body, true );
                    responseMsg = respMsg;
                    result = true;
                }
                else
                    result = false;
            }
            catch ( Exception e )
            {
                // If we fail, raise an error to the event log...
                transportProxy.SetErrorInfo( e );
            }
        
            return result;
        }

        #endregion

        private bool SendODBCRequest( TransmitterMessage msg, bool getResponse, out Stream ResponseStream )
        {
            System.Diagnostics.Trace.WriteLine("BizTalk ODBC Adapter::ODBCTransmitAdapterBatch::SendODBCRequest()");
            bool result = false;
            ResponseStream = null;
            BTSODBCQueryEngine queryEngine = null;
            ODBCAdapterProperties config = null;
            string sSchemaName = string.Empty; ;
            string sMessageSchema = string.Empty;
            try
            {
                IBaseMessagePart bodyPart = msg.Message.BodyPart;

                //Get the assembly namespace for the schema associated with this message. This is NOT
                //the target namepace specified in the document
                object contextObject = msg.Message.Context.Read( "SchemaStrongName", "http://schemas.microsoft.com/BizTalk/2003/system-properties" );
                if (contextObject != null) { sSchemaName = (string)contextObject; }
                else { msg.Message.SetErrorInfo(new Exception("SchemaStrongName property was not specified.  Make sure the message is strongly typed.")); }

                Stream s;

                // Note: both the body part and the stream maybe null, so we need to 
                // check for them
                if ( null != bodyPart && ( null != ( s = bodyPart.GetOriginalDataStream( ) ) ) )
                {
                    #region SSO Usage Example
                    
                    // If we needed to use SSO to lookup the remote system credentials
                    /*
                    string ssoAffiliateApplication = props.AffiliateApplication;
                    if (ssoAffiliateApplication.Length > 0)
                    {
                        SSOResult ssoResult = new SSOResult(msg, affiliateApplication);
                        string userName  = ssoResult.UserName;
                        string password  = ssoResult.Result[0];
                        string additionaldata = ssoResult.Result[1]; // (you can have additional metadata associated with the login in SSO) 
							
                        // use these credentials to login to the remote system
                        // ideally zero off the password memory once we are done
                    }					
                    */

                    #endregion

                    config = ( ODBCAdapterProperties )msg.Properties;

                    #region ODBC Implementation

                    XmlTextReader xmlTR = new XmlTextReader( s );
                    xmlTR.MoveToContent( );

                    // QUERY ENGINE COMPONENT HERE!!!!

                    // Setup the query engine 
                    queryEngine = new BTSODBCQueryEngine( );

                    // Lookup the connection string from the adapter properites
                    queryEngine.ConnectionString = config.ConnectionString;
                    Trace.WriteLine(string.Format("BizTalk ODBC Adapter::ODBCTransmitAdapterBatch::SendODBCRequest()::Adapter connection string-->{0}", config.ConnectionString.ToString()));     

                    //========================= START TRANSACTION ========================================
                    // See if they want to wrap the request in a transaction. This MUST be set after the connection
                    // string property or it will fail!
                    if ( config.TransactionSupport != System.Data.IsolationLevel.Unspecified )
                        queryEngine.BeginTransaction( config.TransactionSupport );

                    //Let the Query Engine know whether we need to send a response back
                    queryEngine.RequestResponseProcess = getResponse;

                    ResponseStream = new VirtualStream();

                    sMessageSchema = config.SchemaHelper.GetSchema(sSchemaName);
                    if (sMessageSchema == string.Empty) { msg.Message.SetErrorInfo(new Exception(string.Format("The Adapter could not retrieve the schema for the received message. [{0}]", sSchemaName))); }

                    Trace.WriteLine(string.Format("BizTalk ODBC Adapter::ODBCTransmitAdapterBatch::SendODBCRequest()::Calling-->BTSExecuteODBCCallFromSchema()"));     
                    if ( getResponse )
                    {
                        ResponseStream = queryEngine.BTSExecuteODBCCallFromSchema(xmlTR, sMessageSchema);
                        ResponseStream.Seek( 0, SeekOrigin.Begin );
                    }
                    else
                    {
                        queryEngine.BTSExecuteODBCCallFromSchema(xmlTR, sMessageSchema);
                    }

                    #endregion

                    result = true;
                }
            }
            catch ( Exception e )
            {
                // If we failed, we need to set the exception on 
                // the message. BTS will include this exception in 
                // the event log message and also in HAT
                msg.Message.SetErrorInfo( e );
                System.Diagnostics.Trace.WriteLine(string.Format("BizTalk ODBC Adapter::ODBCTransmitAdapterBatch::SendODBCRequest()::Exception caught-->{0}", e.Message.ToString() ));

                //If something went south roll the transaction back!
                if ( config.TransactionSupport != System.Data.IsolationLevel.Unspecified )
                    queryEngine.Rollback( );

                queryEngine.Close( );
            }

            if ( result )
            {
                try
                {
                    // If all went well then commit the transaction
                    if ( config.TransactionSupport != System.Data.IsolationLevel.Unspecified )
                        queryEngine.Commit( );

                    queryEngine.Close( );
                }
                catch ( Exception tre )
                {
                    // If we failed, we need to set the exception on 
                    // the message. BTS will include this exception in 
                    // the event log message and also in HAT
                    msg.Message.SetErrorInfo( tre );

                    result = false;
                }
            }
            else
            {
                //If something went south roll the transaction back!
                if ( config.TransactionSupport != System.Data.IsolationLevel.Unspecified )
                    queryEngine.Rollback( );

                queryEngine.Close( );
            }

            return result;
        }

        public ConfigProperties CreateProperties( string uri )
        {
            ConfigProperties properties = new ODBCAdapterProperties( uri );
            return properties;
        }
    }
}