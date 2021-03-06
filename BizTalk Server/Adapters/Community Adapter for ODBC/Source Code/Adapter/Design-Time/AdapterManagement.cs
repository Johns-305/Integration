//---------------------------------------------------------------------
// File: AdapterManagement.cs
// 
// Summary: This class does the designtime property-management. If you 
// add or remove properties to or from your adapter be sure to add
// some code to the appropriate validation routines.
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
using System.Data.OleDb;
using System.Data.Odbc;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Windows.Forms;
using System.Runtime.Serialization;

using Microsoft.BizTalk.Adapters.ODBC.SchemaWizard; 
using Microsoft.BizTalk.Adapter.Framework;

namespace Microsoft.BizTalk.Adapters.ODBC.ODBCDesignTime
{
    /// <summary>
    /// Class AdapterManagement implements
    /// IAdapterConfig and IStaticAdapterConfig interfaces for
    /// management to illustrate a static adapter that uses the
    /// adapter framework
    /// </summary>
    public class AdapterManagement : IAdapterConfig, IDynamicAdapterConfig, IAdapterConfigValidation, IAdapterInfo
    {
        private static ResourceManager resourceManager = new ResourceManager( "Microsoft.BizTalk.SDKSamples.Adapters.DotNetFile.Designtime.DotNetFileResource", Assembly.GetExecutingAssembly( ) );

        //IAdapterConfig
        public string GetConfigSchema( ConfigType type )
        {
            switch ( type )
            {
                case ConfigType.ReceiveHandler:
                    return GetResource( "Microsoft.BizTalk.Adapters.ODBC.ODBCDesignTime.ReceiveHandler.xsd" );
                case ConfigType.ReceiveLocation:
                    return GetResource( "Microsoft.BizTalk.Adapters.ODBC.ODBCDesignTime.ReceiveLocation.xsd" );
                case ConfigType.TransmitHandler:
                    return GetResource( "Microsoft.BizTalk.Adapters.ODBC.ODBCDesignTime.TransmitHandler.xsd" );
                case ConfigType.TransmitLocation:
                    return GetResource( "Microsoft.BizTalk.Adapters.ODBC.ODBCDesignTime.TransmitLocation.xsd" );
                default:
                    return null;
            }
        }

        /// <summary>
        /// Helper to get resource from manafest.  Replace with ResourceManager.GetString if .resources or
        /// .resx files are used for managing this assemblies resources.
        /// </summary>
        /// <param name="resource">Full resource name</param>
        /// <returns>Resource value</returns>
        private string GetResource( string resource )
        {
            string value = null;
            if ( null != resource )
            {

                Stream stream = Assembly.GetExecutingAssembly( ).GetManifestResourceStream( resource );
                try
                {
                    StreamReader reader = null;
                    using ( reader = new StreamReader( stream ) )
                    {
                        value = reader.ReadToEnd( );
                    }
                }
                catch ( Exception e )
                {
                    MessageBox.Show( e.Message );
                }
            }

            return value;
        }

        protected string LocalizeSchema( string schema, ResourceManager resourceManager )
        {
            XmlDocument document = new XmlDocument( );
            document.LoadXml( schema );

            XmlNodeList nodes = document.SelectNodes( "/descendant::*[@_locID]" );
            foreach ( XmlNode node in nodes )
            {
                string locID = node.Attributes[ "_locID" ].Value;
                node.InnerText = resourceManager.GetString( locID );
            }

            StringWriter writer = new StringWriter( );
            document.WriteTo( new XmlTextWriter( writer ) );

            string localizedSchema = writer.ToString( );
            return localizedSchema;
        }

        //Used to get the WSDL file name for the selected WSDL.
        public string[ ] GetServiceDescription( string[ ] wsdls )
        {
            string[ ] result = null;
            return result;
        }

        //Used to acquire externally referenced xsd's
        public Result GetSchema( string xsdLocation, string xsdNamespace, out string xsdFileName )
        {
            xsdFileName = string.Empty;
            return Result.Continue;
        }

        //Display the custom UI here.
        [CLSCompliant( false )]
        public Result DisplayUI( Microsoft.BizTalk.Component.Interop.IPropertyBag endPointConfiguration, System.Windows.Forms.IWin32Window owner, out string[ ] wsdlList )
        {
            string[ ] result = null;

            ODBCAdapterWizardForm SchemaWizard = new ODBCAdapterWizardForm( );

            SchemaWizard.Start( );
            SchemaWizard.ShowDialog( );

            if ( SchemaWizard.DialogResult == DialogResult.OK )
            {
                // Deterime how many schemas are required to be returned
                if ( SchemaWizard.portType == ODBCSchemaHelper.PortType.Send )
                {
                    if ( SchemaWizard.CommandType == ODBCSchemaHelper.AdapterCommandType.SQL &&
                        SchemaWizard.StatementType == ODBCSchemaHelper.AdapterStatementType.Input )
                    {
                        //This is a oneway transmit senerio so only send back the input schema
                        result = new string[ 1 ];
                        result[ 0 ] = WSDLGen.CreateWSDL( SchemaWizard.strInputSchema, SchemaWizard.strTargetNamespace, SchemaWizard.strInputRoot, true );//    GetResource("Microsoft.BizTalk.Adapters.ODBC.ODBCDesignTime.service1.wsdl");
                    }
                    else
                    {
                        result = new string[ 2 ];
                        result[ 0 ] = WSDLGen.CreateWSDL( SchemaWizard.strInputSchema, SchemaWizard.strTargetNamespace, SchemaWizard.strInputRoot, true );//    GetResource("Microsoft.BizTalk.Adapters.ODBC.ODBCDesignTime.service1.wsdl");
                        result[ 1 ] = WSDLGen.CreateWSDL( SchemaWizard.strOutputSchema, SchemaWizard.strTargetNamespace, SchemaWizard.strOutputRoot, false );//    GetResource("Microsoft.BizTalk.Adapters.ODBC.ODBCDesignTime.service1.wsdl");
                    }
                }
                else
                {
                    result = new string[ 1 ];
                    result[ 0 ] = WSDLGen.CreateWSDL( SchemaWizard.strOutputSchema, SchemaWizard.strTargetNamespace, SchemaWizard.strOutputRoot, true );//    GetResource("Microsoft.BizTalk.Adapters.ODBC.ODBCDesignTime.service1.wsdl");
                }

                wsdlList = result;

                // FIX THIS: Should probably return something other if we fail
                return Result.Continue;
            }
            else
            {
                wsdlList = result;
                return Result.Cancel;
            }
        }

        #region Validation

        private void UpdateConfigurationFromUri( XmlDocument configXMLInst, string rootName )
        {
            XmlNode node = null;

            node = configXMLInst.SelectSingleNode( rootName + "/uri" );
            if ( node != null )
            {
                // Extract server and database from URI
                string uri = node.InnerText;
                string[ ] uriParts = uri.Split( new char[ ] { '/' } ); // SQL://server/database/
                if ( uriParts.Length != 5 )
                    return;
                string server = uriParts[ 2 ];
                string database = uriParts[ 3 ];

                // Update connection string property with values obtained from URI
                node = configXMLInst.SelectSingleNode( rootName + "/connectionString" );
                if ( node != null )
                {
                    const string ServerProp = "Data Source=";
                    const string DatabaseProp = "Initial Catalog=";

                    string connectionString = node.InnerText;
                    string[ ] connectionStringParts = connectionString.Split( new char[ ] { ';' } );
                    foreach ( string connectionStringPart in connectionStringParts )
                    {
                        if ( connectionStringPart.StartsWith( ServerProp ) )
                        {
                            connectionString = connectionString.Replace( connectionStringPart, ServerProp + server );
                        }
                        else if ( connectionStringPart.StartsWith( DatabaseProp ) )
                        {
                            connectionString = connectionString.Replace( connectionStringPart, DatabaseProp + database );
                        }
                    }
                    node.InnerText = connectionString;

                    Validator.ValidateConnectionString( node.InnerText );
                }
            }
        }

        public string ValidateConfiguration( ConfigType type, string config )
        {
            XmlDocument configXMLInst = new XmlDocument( );
            configXMLInst.LoadXml( config );
            XmlNode node = null;

            // Validation for the send side
            if ( type == ConfigType.TransmitLocation )
            {
                node = configXMLInst.SelectSingleNode( "Send/connectionString" );
                if ( node != null )
                    Validator.ValidateConnectionString( node.InnerText );
                else
                    throw new SqlValidationException( "Problem with: Send/connectionString" );

                node = configXMLInst.SelectSingleNode( "Send/uri" );
                if ( node != null )
                {
                    if ( node.InnerText.IndexOf( "ODBC://" ) < 0 )
                        throw new SqlValidationException( "Problem with URI" );

                    // Reconcile differences between URI and other adapter properties.
                    this.UpdateConfigurationFromUri( configXMLInst, "Send" );
                    config = configXMLInst.OuterXml;
                }
                else
                    throw new SqlValidationException( "Some king of configuraiton problem" );

            }
            else if ( type == ConfigType.ReceiveLocation )
            {
                node = configXMLInst.SelectSingleNode( "Receive/SQLCommand" );
                if ( node == null )
                {
                    throw new SqlValidationException( "You must select a valid ODBC adapter schema containing the SQL to execute against the target database" );
                }

                // RootName and Namespace are ALL derived form the schema select in the SQLCommand. These are flagged as hiddne
                // in the adapter receivelocation xml file. 
                node = configXMLInst.SelectSingleNode( "Receive/Namespace" );
                if ( node != null )
                    Validator.ValidateTargetNamespace( node.InnerText );
                else
                    throw new SqlValidationException( "You must select a BizTalk Project containing an ODBC adapter based schema" );

                node = configXMLInst.SelectSingleNode( "Receive/RootName" );
                if ( node != null )
                    Validator.ValidateTargetNamespace( node.InnerText );
                else
                    throw new SqlValidationException( "You must select a BizTalk Project containing an ODBC adapter based schema" );

                node = configXMLInst.SelectSingleNode( "Receive/PollingInterval" );
                if ( node != null )
                {
                    int i = Convert.ToInt32( node.InnerText );
                    if ( i < 0 || i > 65535 )
                        throw new SqlValidationException( "Problem with: Receive/PollingInterval" );
                }
                else
                    throw new SqlValidationException( "Polling interval too large or small!" );

                node = configXMLInst.SelectSingleNode( "Receive/uri" );
                if ( node != null )
                {
                    if ( node.InnerText.IndexOf( "ODBC://" ) < 0 )
                        throw new SqlValidationException( "Problem with URI" );

                    // Reconcile differences between URI and other adapter properties.
                    this.UpdateConfigurationFromUri( configXMLInst, "Receive" );
                    config = configXMLInst.OuterXml;
                }
                else
                    throw new SqlValidationException( "Problem with URI" );

                node = configXMLInst.SelectSingleNode( "Receive/ConnectionString" );
                if ( node != null )
                    Validator.ValidateConnectionString( node.InnerText );
                else
                    throw new Exception( "Problem with the connection string" );



            }
            else if ( type == ConfigType.TransmitHandler )
            {
                //  Any of these elements can be null
                node = configXMLInst.SelectSingleNode( "Send/connectionString" );
                if ( node != null && node.InnerText.Length > 0 )
                    Validator.ValidateConnectionString( node.InnerText );

            }

            return config;
        }

        #endregion

        // IAdapterInfo
        public string GetHelpString( ConfigType type )
        {
            string help = null;
            switch ( type )
            {
                case ConfigType.TransmitHandler:
                    //FIXhelp = CSHID.cshidSqlThPropPage;
                    break;
                case ConfigType.TransmitLocation:
                    //FIXhelp = CSHID.cshidSqlTlPropPage;
                    break;
                case ConfigType.ReceiveHandler:
                    //FIX help = CSHID.cshidSqlRhPropPage;
                    break;
                case ConfigType.ReceiveLocation:
                    //FIXhelp = CSHID.cshidSqlRlPropPage;
                    break;
                default:
                    //FIXhelp = CSHID.cshidFallback;
                    break;
            }
            Trace.WriteLine( help );
            return help;
        }
    }
    
    public class Validator
    {
        public static void ValidateXlangKeyword( string input )
        {
            #region Reserved Xlang Keywords
            
            string[ ] reservedXlangKeywords = {  "activate",
												 "atomic",
												 "body",
												 "call",
												 "catch",
												 "checked",
												 "compensate",
												 "compensation",
												 "construct",
												 "correlation",
												 "correlationtype",
												 "delay",
												 "dynamic",
												 "else",
												 "exceptions",
												 "exec",
												 "exists",
												 "false",
												 "if",
												 "implements",
												 "initialize",
												 "internal",
												 "link",
												 "listen",
												 "longrunning",
												 "message",
												 "messagetype",
												 "method",
												 "module",
												 "new",
												 "null",
												 "oneway",
												 "out",
												 "parallel",
												 "port",
												 "porttype",
												 "private",
												 "public",
												 "receive",
												 "ref",
												 "request",
												 "requestresponse",
												 "response",
												 "scope",
												 "send",
												 "service",
												 "servicelink",
												 "servicelinktype",
												 "source",
												 "succeeded",
												 "suppressfailure",
												 "suspend",
												 "target",
												 "task",
												 "terminate",
												 "throw",
												 "timeout",
												 "transaction",
												 "transform",
												 "true",
												 "unchecked",
												 "until",
												 "uses",
												 "using",
												 "while",
												 "xpath",
			};

            #endregion
            
            for ( int i = 0; i < reservedXlangKeywords.Length; i++ )
                if ( input == reservedXlangKeywords[ i ] )
                    throw new SqlValidationException( "Key word problems" );
        }

        public static void ValidateRootElementName( string input )
        {
            char[ ] invalidChars = { '>', '<', '\'', '\"', '&', ':' };
            if ( input.IndexOfAny( invalidChars ) >= 0 )
                throw new SqlValidationException( "Invalid character in namespace" );
            ValidateXlangKeyword( input );
        }

        public static void ValidateTargetNamespace( string input )
        {
            XmlSchema xs = new XmlSchema( );
            xs.TargetNamespace = input;

            XmlSchemaSet xsSet = new XmlSchemaSet( );
            xsSet.ValidationEventHandler += new ValidationEventHandler( ValidationHandler );
            xsSet.Add( xs );
            xsSet.Compile( );
            
            ValidateXlangKeyword( input );
        }

        public static void ValidationHandler( object sender, ValidationEventArgs args )
        {
            throw new SqlValidationException( "Validation Error \n" + args.Message );
        }

        public static void ValidateConnectionString( string input )
        {
            if ( input.Length == 0 )
                throw new SqlValidationException( "No SQL Command Provided" );

            /*	OleDbConnection myConnection = null;
                try
                {
                    myConnection = new OleDbConnection(input);
                    myConnection.Open();
                    myConnection.Close();
                }
                catch(Exception e)
                {
                    throw new SqlValidationException(ODBCResourceHandler.GetResourceString("ValidationConnectionError")+" "+e.Message);
                }
                finally
                {
                    if (myConnection != null) 
                        myConnection.Close();
                } 
             */
        }
    }

    [Serializable( )]
    public class SqlValidationException : Exception
    {
        public SqlValidationException( ) { }

        public SqlValidationException( string s, Exception e ) : base( s, e ) { }

        protected SqlValidationException( SerializationInfo si, StreamingContext sc ) : base( si, sc ) { }

        public SqlValidationException( string errorMsg ) : base( errorMsg )
        {
            this.Source = "ODBC Adapter Admin";
        }
    }
}