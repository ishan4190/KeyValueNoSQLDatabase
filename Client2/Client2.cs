///////////////////////////////////////////////////////////////////////////////
// Client2.cs - CommService client sends and receives messages               //
// ver 2.1                                                                   //
// Source : Jim Fawcett, CSE681 - Software Modeling and Analysis, Project #4 //    
// Application: Demonstration for CSE681-SMA, Project#4                      //
// Language:    C#, ver 5.0, Visual Studio 2015                              //
// Author:       Ishan Gupta, Syracuse University                            //
//              315-935-1141, igupt100@syr.edu                              //
///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////
/*
 * Operation Overview
 * -------------------
 * Client2.cs is a Reader Client : All queries made to database will be carried out 
 * by this Client
 *
 * Additions to C# Console Wizard generated code:
 * - Added using System.Threading
 * - Added reference to ICommService, Sender, Receiver, Utilities,Test Timer
 *   MakeMessage
 *
 * Note:
 * - in this incantation the client has Sender and now has Receiver to
 *   retrieve Server echo-back messages.
 * - If you provide command line arguments they should be ordered as:
 *   remotePort, remoteAddress, localPort, localAddress
 */
/*
* Maintenance:
 * ------------
 * Required Files: 
 * 
 * TestTimer.cs, UtilityExtensions.cs,MakeMessage.cs
 * Sender.cs,Receiver.cs,ICommservice.cs,Utilities.cs
 *
 * Build Process:  devenv Project2Starter.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 * Maintenance History:
 * --------------------
 * ver 2.2 : Made changes to meet the Project 4 Requirements
 * ver 2.1 : 29 Oct 2015
 * - fixed bug in processCommandLine(...)
 * - added rcvr.shutdown() and sndr.shutDown() 
 * ver 2.0 : 20 Oct 2015
 * - replaced almost all functionality with a Sender instance
 * - added Receiver to retrieve Server echo messages.
 * - added verbose mode to support debugging and learning
 * - to see more detail about what is going on in Sender and Receiver
 *   set Utilities.verbose = true
 * ver 1.0 : 18 Oct 2015
 * - first release
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Project4Starter
{
    using System.Xml.Linq;
    using Util = Utilities;

    ///////////////////////////////////////////////////////////////////////
    // Client class sends and receives messages in this version
    // - commandline format: /L http://localhost:8085/CommService 
    //                       /R http://localhost:8080/CommService
    //   Either one or both may be ommitted

    class Client
    {
        string localUrl { get; set; } = "http://localhost:8082/CommService";
        string remoteUrl { get; set; } = "http://localhost:8080/CommService";

        List<MessageTemplate> listOfMsgs;

        HiResTimer hrt = new HiResTimer();

        //----< retrieve urls from the CommandLine if there are any >--------

        public void processCommandLine(string[] args)
        {
            if (args.Length == 0)
                return;
            localUrl = Util.processCommandLineForLocal(args, localUrl);
            remoteUrl = Util.processCommandLineForRemote(args, remoteUrl);
            Util.verbose = Util.processCommandLineForLogging(args);
        }

        public int getMsgLen(String fileName)
        {
            int messageSize = 0;
            MessageTemplate msgTemplate = new MessageTemplate();
            listOfMsgs = msgTemplate.GetMessageList(fileName);
            if (listOfMsgs.Count > 0)
            {
                for (int i = 0; i < listOfMsgs.Count; i++)
                {
                    messageSize = messageSize + listOfMsgs[i].messageSize;
                }
            }
            return messageSize;
        }

        public void readMsgTemplate(String fileName, Sender sndr)
        {
            MessageTemplate msgTemplate = new MessageTemplate();
            listOfMsgs = msgTemplate.GetMessageList(fileName);
            if (listOfMsgs.Count > 0)
            {
                for (int i = 0; i < listOfMsgs.Count; i++)
                {
                    parseMsgTemplate(listOfMsgs[i], sndr);
                }
            }
            else
            {
                Console.WriteLine("message template list is empty");
            }
        }

        private void parseMsgTemplate(MessageTemplate msgTemplate, Sender sndr)
        {
            if (msgTemplate.messageType.Equals("Query1"))
            {
                Console.WriteLine("Query1 called");
                CallQuery1(null, msgTemplate, sndr);
            }
            if (msgTemplate.messageType.Equals("Query2"))
            {
                Console.WriteLine("Query2 called");
                CallQuery2(null, msgTemplate, sndr);

            }
            if (msgTemplate.messageType.Equals("Query3"))
            {
                Console.WriteLine("Query3 called");
                CallQuery3(null, msgTemplate, sndr);

            }
            if (msgTemplate.messageType.Equals("Query4"))
            {
                Console.WriteLine("Query4 called");
                CallQuery4(null, msgTemplate, sndr);

            }
            if (msgTemplate.messageType.Equals("Query5"))
            {
                Console.WriteLine("Query5 called");
                CallQuery5(null, msgTemplate, sndr);
            }
        }

        private void CallQuery5(MessageParameterState msgState, MessageTemplate msgTemplate, Sender sndr)
        {
            DateTime toDate = new DateTime();
            DateTime fromDate = new DateTime(2015, 10, 1);
            for (int i = 0; i < msgTemplate.messageSize; i++)
            {
                XElement messageNode = new XElement("message");
                XAttribute att = new XAttribute("commandType", "query5");
                messageNode.Add(att);
                XElement keyTypenode = new XElement("keyType", "string");
                XElement valueTypeNode = new XElement("valueType", "ListOfString");
                XElement startTime = new XElement("startTime", fromDate);
                XElement endTime = new XElement("endTime", toDate);
                messageNode.Add(keyTypenode);
                messageNode.Add(valueTypeNode);
                messageNode.Add(startTime);
                messageNode.Add(endTime);
                Message msg = new Message();
                msg.fromUrl = localUrl;
                msg.toUrl = remoteUrl;
                msg.content = messageNode.ToString();
                //Console.WriteLine("msg : " + msg.content);
                if (!sndr.sendMessage(msg))
                    return;
            }
        }

        private void CallQuery4(MessageParameterState msgState, MessageTemplate msgTemplate, Sender sndr)
        {
            for (int i = 0; i < msgTemplate.messageSize; i++)
            {
                XElement messageNode = new XElement("message");
                XAttribute att = new XAttribute("commandType", "query4");
                messageNode.Add(att);
                XElement keyTypenode = new XElement("keyType", "string");
                XElement valueTypeNode = new XElement("valueType", "ListOfString");
                XElement searchParaMeter = new XElement("searchParaMeter", "1");
                messageNode.Add(keyTypenode);
                messageNode.Add(valueTypeNode);
                messageNode.Add(searchParaMeter);
                Message msg = new Message();
                msg.fromUrl = localUrl;
                msg.toUrl = remoteUrl;
                msg.content = messageNode.ToString();
                //Console.WriteLine("msg : " + msg.content);
                if (!sndr.sendMessage(msg))
                    return;
            }
        }

        private void CallQuery3(MessageParameterState msgState, MessageTemplate msgTemplate, Sender sndr)
        {
            for (int i = 0; i < msgTemplate.messageSize; i++)
            {
                XElement messageNode = new XElement("message");
                XAttribute att = new XAttribute("commandType", "query3");
                messageNode.Add(att);
                XElement keyTypenode = new XElement("keyType", "string");
                XElement valueTypeNode = new XElement("valueType", "ListOfString");
                XElement patternNode = new XElement("pattern", "1");
                messageNode.Add(keyTypenode);
                messageNode.Add(valueTypeNode);
                messageNode.Add(patternNode);
                Message msg = new Message();
                msg.fromUrl = localUrl;
                msg.toUrl = remoteUrl;
                msg.content = messageNode.ToString();
                //Console.WriteLine("msg : " + msg.content);
                if (!sndr.sendMessage(msg))
                    return;
            }
        }

        private void CallQuery2(MessageParameterState msgState, MessageTemplate msgTemplate, Sender sndr)
        {
            for (int i = 0; i < msgTemplate.messageSize; i++)
            {
                XElement messageNode = new XElement("message");
                XAttribute att = new XAttribute("commandType", "query2");
                messageNode.Add(att);
                XElement keyTypenode = new XElement("keyType", "string");
                XElement valueTypeNode = new XElement("valueType", "ListOfString");
                XElement keynode = new XElement("key", "123");
                messageNode.Add(keyTypenode);
                messageNode.Add(valueTypeNode);
                messageNode.Add(keynode);
                Message msg = new Message();
                msg.fromUrl = localUrl;
                msg.toUrl = remoteUrl;
                msg.content = messageNode.ToString();
                //Console.WriteLine("msg : " + msg.content);
                if (!sndr.sendMessage(msg))
                    return;
            }
        }

        private void CallQuery1(MessageParameterState msgState, MessageTemplate msgTemplate, Sender sndr)
        {
            for (int i = 0; i < msgTemplate.messageSize; i++)
            {
                XElement messageNode = new XElement("message");
                XAttribute att = new XAttribute("commandType", "query1");
                messageNode.Add(att);
                XElement keyTypenode = new XElement("keyType", "string");
                XElement valueTypeNode = new XElement("valueType", "ListOfString");
                XElement keynode = new XElement("key", "123");
                messageNode.Add(keyTypenode);
                messageNode.Add(valueTypeNode);
                messageNode.Add(keynode);
                Message msg = new Message();
                msg.fromUrl = localUrl;
                msg.toUrl = remoteUrl;
                msg.content = messageNode.ToString();
                //Console.WriteLine("msg : " + msg.content);
                if (!sndr.sendMessage(msg))
                    return;
            }
        }

        public void travelLatency(ulong latency, Sender sndr)
        {
            Message msg = new Message();
            msg.fromUrl = localUrl;
            msg.toUrl = remoteUrl;
            msg.content = "Reader Client latency" + " = " + latency.ToString();
            if (!sndr.sendMessage(msg))
                return;
        }

        static void Main(string[] args)
        {
            Thread.Sleep(2000);
            Console.Write("\n  starting CommService client");
            Console.Write("\n =============================\n");
            Console.Title = "Reader Client";
            Client clnt = new Client();
            clnt.processCommandLine(args);
            string localPort = Util.urlPort(clnt.localUrl);
            string localAddr = Util.urlAddress(clnt.localUrl);
            Receiver rcvr = new Receiver(localPort, localAddr);
            if (rcvr.StartService())
            {
                rcvr.doService(rcvr.defaultServiceAction());
            }
            Sender sndr = new Sender(clnt.localUrl);  // Sender needs localUrl for start message
            Message msg = new Message();
            msg.fromUrl = clnt.localUrl;
            msg.toUrl = clnt.remoteUrl;
            Console.Write("\n  sender's url is {0}", msg.fromUrl);
            Console.Write("\n  attempting to connect to {0}\n", msg.toUrl);
            if (!sndr.Connect(msg.toUrl))
            {
                Console.Write("\n  could not connect in {0} attempts", sndr.MaxConnectAttempts);
                sndr.shutdown();
                rcvr.shutDown();
                return;
            }
            clnt.hrt.Start();
            rcvr.setTotalMessageSize(clnt.getMsgLen("XMLReader.xml"));
            clnt.readMsgTemplate("XMLReader.xml", sndr);
            while (true)
            {
                if (rcvr.getLastFlag())
                {
                    clnt.hrt.Stop();
                    break;
                }
            }
            ulong latency = clnt.hrt.ElapsedMicroseconds;
            clnt.travelLatency(latency, sndr);
            msg.content = "done";
            sndr.sendMessage(msg);
            // Wait for user to press a key to quit.
            // Ensures that client has gotten all server replies.
            Util.waitForUser();
            // shut down this client's Receiver and Sender by sending close messages
            rcvr.shutDown();
            sndr.shutdown();
            Console.Write("\n\n");
        }
    }
}

