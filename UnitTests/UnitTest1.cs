using System;
using CmdStepsCore;
using CmdStepsWindowsImplementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.NetworkInformation;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test1()
        {
            var ping = new Ping();
            var reply = ping.Send("prod-web01");
            var status = reply.Status;
        }

        [TestMethod]
        public void RequestStepTest()
        {
            var step1 = GetTestRequestStep1();

            string ret = step1.ExecuteAsync().Result;
        }

        [TestMethod]
        public void SaveStepCollectionTest()
        {
            var WindowsSaver = new WindowsSaver();
            var StepCollection = new StepCollection(WindowsSaver)
            {
                Encrypt = true,
                Id = "TESTER1"
            };

            StepCollection.Add(GetTestRequestStep1());
            StepCollection.Save();
        }

        [TestMethod]
        public StepCollection LoadStepCollectionTest()
        {
            var WindowsSaver = new WindowsSaver();
            var StepCollection = new StepCollection(WindowsSaver)
            {
                Encrypt = true
            };

            StepCollection.Load("TESTER1");
            return StepCollection;
        }

        [TestMethod]
        public void LoadAndExecuteTest()
        {
            var collection = LoadStepCollectionTest();

            foreach(var step in collection)
            {
                var x = step.ExecuteAsync().Result;
            }
        }


        private Step GetTestRequestStep1()
        {
            var requester1 = new RequestStepExecutor();
            var serializer1 = new WindowsObjectSerializer();
            var step1 = new Step(requester1, serializer1)
            {
                Input = "http://jsonplaceholder.typicode.com/posts/1",
                OutputFullResult = true,
                OutputType = StepOutputType.Json
            };

            step1.Variables.Add(new StepVariable()
            {
                DataKey = "title"
            });
            return step1;
        }
    }
}
