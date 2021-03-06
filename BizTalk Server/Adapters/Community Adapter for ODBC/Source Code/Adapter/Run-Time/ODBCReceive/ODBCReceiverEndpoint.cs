//---------------------------------------------------------------------
// File: ODBCReceiverEndpoint.cs
// 
// Summary: Implementation of an adapter framework sample adapter using the ODBC provider for ADO.NET. 
//
// Sample: Adapter framework runtime adapter.
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
using System.Threading;
using System.Net;
using System.Collections;
using System.Security;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Diagnostics;

using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.TransportProxy.Interop;
using Microsoft.BizTalk.Adapters.ODBC;

using Microsoft.XLANGs.BaseTypes;

using Microsoft.Samples.BizTalk.Adapters.BaseAdapter;

namespace Microsoft.BizTalk.Adapters.ODBC.RunTime.ODBCReceiveAdapter
{
    public class ODBCReceiverEndpoint : ReceiverEndpoint
    {
        private object LockObject = new object();

        private System.Threading.Timer timer = null;
        int errorCount;
        //private ControlledTermination controlledTermination;

        // Endpoint is active
        private bool _endpointValid = true;

        private Microsoft.Samples.BizTalk.Adapters.BaseAdapter.ThreadPool threadPool = new Microsoft.Samples.BizTalk.Adapters.BaseAdapter.ThreadPool( );
        private ODBCAdapterProperties _properties;//  _properties
        private IBTTransportProxy _transportProxy;//  handle to the EPM
        private IBaseMessageFactory _messageFactory;// used to create new messages / message parts etc.
        private string _transportType = "ODBC";//  used in the creation of messages
        private string _propertyNamespace;//  used in the creation of messages
        private string _uri;//  used in the creation of messages
        IManageEndpoints _manageEndpoints;//  used if we want to remove ourselves

        //support for Update
        IPropertyBag _updatedConfig;
        IPropertyBag _updatedBizTalkConfig;
        IPropertyBag _updatedHandlerPropertyBag;

        //constants
        private const string MESSAGE_BODY = "body";

        public ODBCReceiverEndpoint( ) { }

        #region Initialize, update and remove functions
        
        public override void Initialize( string uri, IPropertyBag config, IPropertyBag bizTalkConfig, IPropertyBag handlerPropertyBag, IBTTransportProxy transportProxy, string transportType, string propertyNamespace, IManageEndpoints manageEndpoints )
        {
            this._properties = new ODBCAdapterProperties( uri );

            //  Location _properties - possibly override some Handler _properties
            this._properties.LocationConfiguration( config, bizTalkConfig );
            //  Location _properties - possibly override some Handler _properties
            //ODBCAdapterProperties.HandlerConfiguration( handlerPropertyBag, bizTalkConfig );  //Doesn't do anything.
            //  this is our handle back to the EPM

            this._transportProxy = transportProxy;
            // used to create new messages / message parts etc.
            this._messageFactory = this._transportProxy.GetMessageFactory( );
            //  used in the creation of messages
            this._transportType = transportType;
            //  used in the creation of messages
            this._propertyNamespace = propertyNamespace;
            //  used in the creation of messages
            this._uri = uri;
            //  used to remove us from the receiver's hashtable if we pass the error threshold
            this._manageEndpoints = manageEndpoints;

            //Get the number of processors
            int NrOfProcessors = NativeCalls.GetNumberOfProcessors( );

            //Calculate the number of threads
            //Only multiple threads allowed if it is a single-receive adapter
            Start();
        }

        public override void Update( IPropertyBag config, IPropertyBag bizTalkConfig, IPropertyBag handlerPropertyBag )
        {
            lock ( LockObject )
            {
                Stop();
                //  keep handles to these property bags until we are ready
                this._updatedConfig = config;
                this._updatedBizTalkConfig = bizTalkConfig;
                this._updatedHandlerPropertyBag = handlerPropertyBag;

                if ( null != config )
                {
                    this._properties.LocationConfiguration( config, bizTalkConfig );
                }
                Start();
            }
        }

        public void Dispose()
        {
            Trace.WriteLine("[ODBC Adapter] Dispose called");
            //  stop the schedule
            Stop();
        }

        private void Start()
        {
            this.timer = new Timer(new TimerCallback(ControlledEndpointTask));
            this.timer.Change(0, this._properties.PollingIntervalMilliseconds);
        }

        private void Stop()
        {
            this.timer.Dispose();
        }

        /// <summary>
        /// Removes an endpoint
        /// </summary>
        public override void Remove()
        {
            Stop();
            this._endpointValid = false;
        }
        /// <summary>
        /// this method is called from the task scheduler when the polling interval has elapsed.
        /// </summary>
        public void ControlledEndpointTask(object val)
        {
            if (this._manageEndpoints.Enter())
            {
                try
                {
                    lock (LockObject)
                    {
                        this.EndpointTask();
                    }
                    GC.Collect();
                }
                finally
                {
                    this._manageEndpoints.Leave();
                }
            }
        }

        /// <summary>
        /// Handle the work to be performed each polling interval
        /// </summary>
        private void EndpointTask()
        {
            try
            {
                GetRequestMessagesAndSubmit();

                //Success, reset the error count
                errorCount = 0;
            }
            catch (Exception e)
            {
                _transportProxy.SetErrorInfo(e);
                //Track number of failures
                errorCount++;
            }
            CheckErrorThreshold();
        }

        private bool CheckErrorThreshold()
        {
            if ((0 != _properties.ErrorThreshold) && (this.errorCount > this._properties.ErrorThreshold))
            {
                this._transportProxy.ReceiverShuttingdown(this._properties.Uri, new ErrorThresholdExceeded());

                //Stop the timer.
                Stop();
                return false;
            }
            return true;
        }

        #endregion

        /// <summary>
        /// Create the serviced component and retrieve a batch of messages.
        /// </summary>
        public void GetRequestMessagesAndSubmit( )
        {
            System.Diagnostics.Trace.WriteLine("BizTalk ODBC Adapter::ODBCReceiverEndpoint::GetRequestMessagesAndSubmit()");
            try
            {
                BTSODBCQueryEngine QueryEngine = null;
                StandardReceiveBatchHandler batchHandler;

                do
                {
                    batchHandler = new StandardReceiveBatchHandler(this._transportProxy, null, false, 1);
                    VirtualStream NewMsgStream = null;

                    QueryEngine = new BTSODBCQueryEngine();
                    QueryEngine.ConnectionString = this._properties.ConnectionString;
                    QueryEngine.Namespace = this._properties.Namespace;
                    QueryEngine.RootNode = this._properties.RootNodeName;

                    //Let the engine know we are expecting a response!
                    QueryEngine.ReceivePort = true;

                    // Start our transaction to ensure that no one else gets access to the data
                    if (this._properties.TransactionISOLevel != System.Data.IsolationLevel.Unspecified)
                        QueryEngine.BeginTransaction(this._properties.TransactionISOLevel);

                    NewMsgStream = QueryEngine.BTSExecuteODBCCallFromSQL(this._properties.SQLCommand);

                    if (NewMsgStream == null)
                    {
                        break;
                    }

                    NewMsgStream.Flush();
                    NewMsgStream.Seek(0, SeekOrigin.Begin);

                    IBaseMessage btMessage = null;
                    try
                    {
                        btMessage = CreateMessage((Stream)NewMsgStream);
                        //Add the message to the batch

                        if (this._manageEndpoints.TerminateCalled) { break; }

                        batchHandler.SubmitMessage(btMessage, null);

                        // If the batch process correctly we should commit our transaction
                        // We may need to move this below based on feed back not sure?
                        if (this._properties.TransactionISOLevel != System.Data.IsolationLevel.Unspecified)
                            QueryEngine.Commit();

                        QueryEngine.Close();
                    }
                    catch
                    {
                        EventLog.WriteEntry("ODBCAdapter", String.Format("An error occured while trying to sumbit a message the the batch."), EventLogEntryType.Error);
                        errorCount++;
                        if (this._properties.TransactionISOLevel != System.Data.IsolationLevel.Unspecified)
                            QueryEngine.Rollback();

                        QueryEngine.Close(); // Close the DB connection!
                    }

                    BatchResult br = batchHandler.Done( null );
                    if ( !br.BatchSucceeded )
                    {
                        Trace.WriteLine( "An error occured while trying to submit a batch to the engine." );
                        throw new Exception( "Failed to process the ODBC Request" );
                    }
                    batchHandler = null;
                } while (this._properties.PollWhileDataFound); //end do

            }
            catch ( COMException eCom )
            {
                if ( eCom.ErrorCode == BTTransportProxy.BTS_E_MESSAGING_SHUTTING_DOWN )
                {
                    return;
                }
                else
                {
                    EventLog.WriteEntry( "ODBCAdapter", "GetBatchOfMessagesAndSubmit Error: " + eCom.Message, EventLogEntryType.Error );
                    throw;
                }
            }
            catch ( Exception ex )
            {
                EventLog.WriteEntry( "ODBCAdapter", "GetBatchOfMessagesAndSubmit Error: " + ex.Message, EventLogEntryType.Error );
                throw;
            }
        }

        private void StreamCopy( Stream dest, Stream src )
        {
            int bytesRead = 0;
            byte[] buff = new byte[4096];
            while ((bytesRead = src.Read(buff, 0, 4096)) > 0)
                dest.Write( buff, 0, bytesRead );
        }

        /// <summary>
        /// Creates a BizTalk message and populates the message context _properties
        /// </summary>
        /// <returns>BizTalk message</returns>
        private IBaseMessage CreateMessage( Stream msgData )
        {
            System.Diagnostics.Trace.WriteLine("BizTalk ODBC Adapter::ODBCReceiverEndpoint::CreateMessage()");
            IBaseMessagePart part = this._messageFactory.CreateMessagePart( );

            part.Data = msgData;

            IBaseMessage message = this._messageFactory.CreateMessage( );
            message.AddPart( MESSAGE_BODY, part, true );

            SystemMessageContext context = new SystemMessageContext( message.Context );
            context.InboundTransportLocation = _uri;
            context.InboundTransportType = _transportType;

            return message;
        }
    }
}

[Serializable()]
public class ErrorThresholdExceeded : AdapterException
{
    public ErrorThresholdExceeded() : base("Error Threshold exceeded") { }

    protected ErrorThresholdExceeded(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
