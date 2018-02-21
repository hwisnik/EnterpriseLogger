using EnterpriseLogger;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Repository.Hierarchy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace UnitTestEntLogger
{
    [TestClass]
    public class EntLoggerUnitTests
    {
        private TestContext TestContextInstance;
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return TestContextInstance;
            }
            set
            {
                TestContextInstance = value;
            }
        }

        [TestMethod]
        public void TestLoggerNameIsSetAfterInitialize()
        {
            // Arrange
            var loggingInstance = EntLogger.Initialize();

            // Act
            var name = loggingInstance.Logger.Name;

            // Assert
            Assert.AreEqual("EnterpriseLogger.EntLogger", name);

            //Display
            TestContextInstance.WriteLine("Logger.Name = " + name);
        }

        [TestMethod]
        public void TestLoggerApplicationNameIsSetAfterInitialize()
        {
            // Arrange
            var loggingInstance = EntLogger.Initialize();
            
            // Act
            var appName = GlobalContext.Properties["ApplicationName"];
            
            // Assert
            Assert.AreEqual("EnterpriseLogger.EntLogger", appName);

            //Display
            TestContextInstance.WriteLine("ApplicationName = " + appName);
        }
        [TestMethod]
        public void TestLoggerCorrelationIdIsSetAFterInitialize()
        {
            // Arrange
            var loggingInstance = EntLogger.Initialize();

            // Act
            var corrId = GlobalContext.Properties["CorrelationId"];

            // Assert
            Assert.IsNotNull(corrId);
            Assert.IsTrue(corrId.GetType() == typeof(Guid));

            //Display
            TestContextInstance.WriteLine("CorrelationId = " + corrId);
        }

        [TestMethod]
        public void VerifyLogDebugInvokedOnce()
        {
            // Arrange
            var moqEntLogger = new Mock<ILog>();

            // Act
            EntLogger.LogDebug(moqEntLogger.Object, "MockDebugMessage");

            // Assert
            moqEntLogger.Verify(log => log.Debug("MockDebugMessage"), Times.Once());
            
        }

        [TestMethod]
        public void LogDebugGeneratesLoggingEvent()
        {
            //Helper class removes configured appender and replaces it with a memory appender
            //Allows unit test to generate and test that a log4net message was created (in memory)

            // Arrange
            var loggingInstance = EntLogger.Initialize();

            // Act
            var loggingEvents = LogInstanceMethodHelper(loggingInstance,"Test LogDebug Method");

            // Assert
            if (loggingEvents == null) Assert.Fail("LoggingEvents is null");
            foreach (var loggingEvent in loggingEvents)
            {
                Assert.AreEqual("Test LogDebug Method", loggingEvent.RenderedMessage);

                //Display
                TestContextInstance.WriteLine("RenderedMessage = " + loggingEvent.RenderedMessage);
            }
        }


        [TestMethod]
        public void LogExceptionGeneratesLoggingEvent()
        {
            //Helper class removes configured appender and replaces it with a memory appender
            //Allows unit test to generate and test that a log4net message was created (in memory)

            // Arrange
            var loggingInstance = EntLogger.Initialize();

            // Act
            var loggingEvents = LogInstanceMethodHelper(loggingInstance, "Test LogException Method", new Exception("Exception contents"));

            // Assert
            if (loggingEvents == null) Assert.Fail("LoggingEvents is null");
            foreach (var loggingEvent in loggingEvents)
            {
                Assert.AreEqual("Test LogException Method", loggingEvent.RenderedMessage);

                //Display
                TestContextInstance.WriteLine("RenderedMessage = " + loggingEvent.RenderedMessage);
            }
        }

        [TestMethod]
        public void CallingEntLoggerLogDebugWithoutInitializeThrowsException()
        {
            //Arrange 
            //Note that loggingInstance is null

            //Assert
            Assert.ThrowsException<NullReferenceException> (() => 
                LogInstanceMethodHelper(null, "Log Debug Without Init Test"),"Wrong Exception Type");

        }


        [TestMethod]
        public void CallingEntLoggerLogExceptionWithoutInitializeThrowsException()
        {
            //Arrange 
            //Note that loggingInstance is null

            //Assert
            Assert.ThrowsException<NullReferenceException>(() => 
                LogInstanceMethodHelper(null, "Log Exception Without Init Test", new Exception("Not Init Test Exception")), "Wrong Exception Type");

        }

        private static LoggingEvent[] LogInstanceMethodHelper(ILog loggingInstance,string debugMessage, Exception exceptionMessage = null)
        {
            //if (!LogManager.GetRepository().Configured)
            //    BasicConfigurator.Configure();  //If you need to know how to configure a repository

            //Use hierarchy to to get Repository and then get Root to access IAppenderAttachable
            var hierarchy = (Hierarchy)LogManager.GetRepository();
            //Remove all appenders and setup a memory appender so that we don't write to the database
            var attachable = hierarchy.Root as IAppenderAttachable;
            //if (attachable == null) return ("IappenderAttachable is null");
            attachable.RemoveAllAppenders();
            var appender = new MemoryAppender();
            attachable.AddAppender(appender);
            if (exceptionMessage == null)
            {
                EntLogger.LogDebug(loggingInstance, debugMessage);
            }
            else
            {
                EntLogger.LogException(loggingInstance, debugMessage,exceptionMessage);
            }

            var loggingEvents = appender.GetEvents();
            //Cleanup
            attachable.RemoveAppender(appender);
            return loggingEvents;
        }

        //[TestMethod]
        //public void DoTest()
        //{
        //    var testing = Log4NetTestHelper.RecordLog(() =>
        //    {
        //        var log = LogManager.GetLogger(typeof(EntLogger));
        //        log.Error("Testing!");
        //    });

        //    Assert.AreEqual("ERROR - EnterpriseLogger.EntLogger | Testing!", testing[0]);
        //}
    }

    //public static class Log4NetTestHelper
    //{
    //    public static string[] RecordLog(Action action)
    //    {
    //        if (!LogManager.GetRepository().Configured)
    //            BasicConfigurator.Configure();
    //        var logMessages = new List<string>();
    //        var root = ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root;
    //        var attachable = root as IAppenderAttachable;

    //        var appender = new MemoryAppender();
    //        if (attachable != null)
    //            attachable.AddAppender(appender);

    //        try
    //        {
    //            action();
    //        }
    //        finally
    //        {
    //            var loggingEvents = appender.GetEvents();
    //            foreach (var loggingEvent in loggingEvents)
    //            {
    //                var stringWriter = new StringWriter();
    //                loggingEvent.WriteRenderedMessage(stringWriter);
    //                logMessages.Add(string.Format("{0} - {1} | {2}", loggingEvent.Level.DisplayName, loggingEvent.LoggerName, stringWriter.ToString()));
    //            }
    //            if (attachable != null)
    //                attachable.RemoveAppender(appender);
    //        }

    //        return logMessages.ToArray();
    //    }
    //}

}

