using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Install;
using System.Diagnostics;
using System.ComponentModel;

namespace Microsoft.BizTalk.Adapters.ODBC
{
    [RunInstaller(true)]
    public class ODBCPerformanceCounterInstaller : Installer
    {
        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);
            InstallODBCPerformanceCounters();
            InstallODBCEventSource();
        }

        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            base.Uninstall(savedState);
            UnInstallODBCPerformanceCounters();
            UnInstallODBCEventSource();
        }

        private void InstallODBCPerformanceCounters()
        {

            UnInstallODBCPerformanceCounters();         //Remove any leftover artifacts, just in case.

            CounterCreationDataCollection perfCounters = new CounterCreationDataCollection();
            CounterCreationData messageCount = new CounterCreationData();
            messageCount.CounterName = "MessageCount";
            messageCount.CounterHelp = "Tracks the number of data messages sent to the BTS ODBC Query Engine";
            messageCount.CounterType = PerformanceCounterType.NumberOfItems32;

            CounterCreationData messagesPerSecond = new CounterCreationData();
            messagesPerSecond.CounterName = "MessagesPerSecond";
            messagesPerSecond.CounterHelp = "Tracks the number of messages sent per second";
            messagesPerSecond.CounterType = PerformanceCounterType.RateOfCountsPerSecond32;

            CounterCreationData dbConnectionCount = new CounterCreationData();
            dbConnectionCount.CounterName = "DbConnectionCount";
            dbConnectionCount.CounterHelp = "Tracks the current number of database connections";
            dbConnectionCount.CounterType = PerformanceCounterType.NumberOfItems32;

            CounterCreationData errorCount = new CounterCreationData();
            errorCount.CounterName = "ErrorCount";
            errorCount.CounterHelp = "Tracks the number of errors generated by the BTS ODBC Query Engine";

            perfCounters.Add(messageCount);
            perfCounters.Add(messagesPerSecond);
            perfCounters.Add(dbConnectionCount);
            perfCounters.Add(errorCount);

            PerformanceCounterCategory category = PerformanceCounterCategory.Create("BtsOdbcQueryEngine", "BizTalk Server ODBC Query Engine", PerformanceCounterCategoryType.Unknown, perfCounters);
        
        }

        private void UnInstallODBCPerformanceCounters()
        {
            if (PerformanceCounterCategory.Exists("BtsOdbcQueryEngine"))
            {
                PerformanceCounterCategory.Delete("BtsOdbcQueryEngine");
            }
        }

        private void InstallODBCEventSource()
        {
            UnInstallODBCEventSource();
            EventLog.CreateEventSource("ODBCAdapter", "Application");
        }

        private void UnInstallODBCEventSource()
        {
            if (EventLog.SourceExists("ODBCAdapter"))
            { EventLog.DeleteEventSource("ODBCAdapter"); }
        }

        private void InitializeComponent()
        {

        }
    }
}
