///////////////////////////////////////////////////////////////////////////////
// Client1.cs - CommService client sends and receives messages               //
// ver 2.1                                                                   //
// Source : Jim Fawcett, CSE681 - Software Modeling and Analysis, Project #4 //    
// Application: Demonstration for CSE681-SMA, Project#4                      //
// Language:    C#, ver 5.0, Visual Studio 2015                              //
// Author:      Ishan Gupta, Syracuse University                            //
//              315-935-1141, igupt100@syr.edu                              //
///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////
/*
 * Operation Overview
 * -------------------
 * Client is the Writer Client : This will carry out Add, Delete, Edit, Persist
 * and Augment operations
 *
 * Additions to C# Console Wizard generated code:
 * - Added using System.Threading
 * - Added reference to ICommService, Sender, Receiver, Utilities,
 *   MakeMessage, TestTimer
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
 *
 * Public Methods : 
 * getMsgLength
 * readMsgTemplate
 * processCommandLine
 * travelLatency
 *
 * Maintenance History:
 * --------------------
 * ver 2.2 : Made changes to meet the requirements 
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
using System.Collections;


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
        List<MessageTemplate> listOfMsgs;
        List<string> listOfKeys = new List<string>();
        string localUrl { get; set; } = "http://localhost:8081/CommService";
        string remoteUrl { get; set; } = "http://localhost:8080/CommService";
        HiResTimer hrt = new HiResTimer();


        //----< retrieve urls from the CommandLine if there are any >--------

        public int getMsgLength(String file)
        {
            int messageSize = 0;
            MessageTemplate msgTemplate = new MessageTemplate();
            listOfMsgs = msgTemplate.GetMessageList(file);
            if (listOfMsgs.Count > 0)
            {
                for (int i = 0; i < listOfMsgs.Count; i++)
                {
                    if (listOfMsgs[i].messageType.Equals("delete"))
                        messageSize = messageSize + 1;
                    else messageSize = messageSize + listOfMsgs[i].messageSize;
                }
            }
            return messageSize;
        }

        public void readMsgTemplate(String file, Sender sndr)
        {
            MessageTemplate msgTemplate = new MessageTemplate();
            listOfMsgs = msgTemplate.GetMessageList(file);
            if (listOfMsgs.Count > 0)
            {
                for (int i = 0; i < listOfMsgs.Count; i++)
                {
                    parseMsgTemplate(listOfMsgs[i], sndr);
                }
            }
            else
            {
                if (Util.verbose)
                    Console.WriteLine("message template list is empty");
            }

        }

        private void parseMsgTemplate(MessageTemplate msgTemplate, Sender sndr)
        {

            if (msgTemplate.messageType.Equals("largeAdd"))
            {
                //if(Util.verbose)
                Console.WriteLine("Add Request Sent");
                sendAddMsg(msgTemplate, sndr);
            }
            if (msgTemplate.messageType.Equals("delete"))
            {
                //if(Util.verbose)
                Console.WriteLine("Delete Request Sent");
                sendDeleteMsg(msgTemplate, sndr);

            }
            if (msgTemplate.messageType.Equals("edit"))
            {
                //if(Util.verbose)
                Console.WriteLine("Edit Request Sent");
                sendEditedMsg(msgTemplate, sndr);
            }
            if (msgTemplate.messageType.Equals("persist"))
            {
                // if(Util.verbose)
                Console.WriteLine("Persist DB Request Sent");
                sendPersistedMsg(msgTemplate, sndr);
            }
            if (msgTemplate.messageType.Equals("augment"))
            {
                //if(Util.verbose)
                Console.WriteLine("Augment DB from xml request sent");
                sendAugmentedMsg(msgTemplate, sndr);
            }

        }
        private void sendAugmentedMsg(MessageTemplate msgTemplate, Sender sndr)
        {
            Random random = new Random();
            for (int i = 0; i < msgTemplate.messageSize; i++)
            {
                XElement messageNode = new XElement("message");
                XAttribute att = new XAttribute("commandType", "augment");
                messageNode.Add(att);

                Message msg = new Message();
                msg.fromUrl = localUrl;
                msg.toUrl = remoteUrl;
                msg.content = messageNode.ToString();
                // Console.WriteLine("msg : " + msg.content);
                if (!sndr.sendMessage(msg))
                    return;
            }
        }

        private void sendPersistedMsg(MessageTemplate msgTemplate, Sender sndr)
        {
            Random random = new Random();
            for (int i = 0; i < msgTemplate.messageSize; i++)
            {
                XElement messageNode = new XElement("message");
                XAttribute att = new XAttribute("commandType", "persist");
                messageNode.Add(att);
                Message msg = new Message();
                msg.fromUrl = localUrl;
                msg.toUrl = remoteUrl;
                msg.content = messageNode.ToString();
                // Console.WriteLine("msg : " + msg.content);
                if (!sndr.sendMessage(msg))
                    return;
            }
        }

        private void sendEditedMsg(MessageTemplate msgTemplate, Sender sndr)
        {
            Random random = new Random();
            for (int i = 0; i < msgTemplate.messageSize; i++)
            {
                XElement messageNode = new XElement("message");
                XAttribute att = new XAttribute("commandType", "edit");
                messageNode.Add(att);
                string rndKey = listOfKeys[random.Next(listOfKeys.Count)];
                XElement keyTypenode = new XElement("keyType", "string");
                XElement valueTypeNode = new XElement("valueType", "ListOfString");
                XElement keynode = new XElement("key", rndKey.ToString());
                XElement valueNode = new XElement("value");
                XElement name = new XElement("name", "Updated-name : " + i.ToString());
                XElement desc = new XElement("desc", "Updated-desc : " + i.ToString());
                XElement timestamp = new XElement("time", DateTime.Now);
                XElement payload = new XElement("payload");
                int rndPayloadSize = random.Next(2, 10);
                for (int j = 0; j < rndPayloadSize; j++)
                {
                    XElement item = new XElement("item", "Updated-Payload" + j + " of " + rndKey);
                    payload.Add(item);
                }
                XElement children = new XElement("children");
                int childrenSize = random.Next(3, 5);
                for (int j = 1; j < childrenSize; j++)
                {
                    XElement item = new XElement("item", "updated-child" + j.ToString());
                    children.Add(item);
                }
                valueNode.Add(name);
                valueNode.Add(desc);
                valueNode.Add(timestamp);
                valueNode.Add(payload);
                valueNode.Add(children);
                messageNode.Add(keyTypenode);
                messageNode.Add(valueTypeNode);
                messageNode.Add(keynode);
                messageNode.Add(valueNode);
                Message msg = new Message();
                msg.fromUrl = localUrl;
                msg.toUrl = remoteUrl;
                msg.content = messageNode.ToString();
                if (!sndr.sendMessage(msg))
                    return;
            }
        }

        private void sendDeleteMsg(MessageTemplate msgTemplate, Sender sndr)
        {
            XElement messageNode = new XElement("message");
            XAttribute att = new XAttribute("commandType", "delete");
            messageNode.Add(att);
            XElement keyTypenode = new XElement("keyType", "string");
            XElement valueTypeNode = new XElement("valueType", "ListOfString");
            XElement numKeys = new XElement("numKeys", msgTemplate.messageSize.ToString());
            messageNode.Add(keyTypenode);
            messageNode.Add(valueTypeNode);
            messageNode.Add(numKeys);
            Message msg = new Message();
            msg.fromUrl = localUrl;
            msg.toUrl = remoteUrl;
            msg.content = messageNode.ToString();
            if (!sndr.sendMessage(msg))
                return;
        }

        private void sendAddMsg(MessageTemplate msgTemplate, Sender sndr)
        {
            Random random = new Random();
            for (int i = 0; i < msgTemplate.messageSize; i++)
            {
                XElement messageNode = new XElement("message");
                XAttribute att = new XAttribute("commandType", "add");
                messageNode.Add(att);
                int rndKey = random.Next(1, 10000); XElement keyTypenode = new XElement("keyType", "string");
                XElement valueTypeNode = new XElement("valueType", "ListOfString");
                XElement keynode = new XElement("key", rndKey.ToString());
                listOfKeys.Add(rndKey.ToString());
                XElement valueNode = new XElement("value");
                XElement name = new XElement("name", "name : " + i.ToString());
                XElement desc = new XElement("desc", "desc : " + i.ToString());
                XElement timestamp = new XElement("time", DateTime.Now);
                XElement payload = new XElement("payload");
                int rndPayloadSize = random.Next(2, 10);
                for (int j = 0; j < rndPayloadSize; j++)
                {
                    XElement item = new XElement("item", "item" + j + " of " + rndKey);
                    payload.Add(item);
                }
                XElement children = new XElement("children");
                int childrenSize = random.Next(3, 5);
                for (int j = 1; j < childrenSize; j++)
                {
                    XElement item = new XElement("item", j.ToString());
                    children.Add(item);
                }
                valueNode.Add(name);
                valueNode.Add(desc);
                valueNode.Add(timestamp);
                valueNode.Add(payload);
                valueNode.Add(children);
                messageNode.Add(keyTypenode);
                messageNode.Add(valueTypeNode);
                messageNode.Add(keynode);
                messageNode.Add(valueNode);
                Message msg = new Message();
                msg.fromUrl = localUrl;
                msg.toUrl = remoteUrl;
                msg.content = messageNode.ToString();
                if (!sndr.sendMessage(msg))
                    return;
            }
        }

        public void processCommandLine(string[] args)
        {
            if (args.Length == 0)
                return;
            localUrl = Util.processCommandLineForLocal(args, localUrl);
            remoteUrl = Util.processCommandLineForRemote(args, remoteUrl);
            Util.verbose = Util.processCommandLineForLogging(args);
        }

        public void travelLatency(ulong latency, Sender sndr)
        {
            Message msg = new Message();
            msg.fromUrl = localUrl;
            msg.toUrl = remoteUrl;
            msg.content = "Writer Client latency" + " = " + latency.ToString();
            if (!sndr.sendMessage(msg))
                return;
        }

        static void Main(string[] args)
        {
            

            Console.Write("\n  starting CommService client");
            Console.Write("\n =============================\n");

            Console.Title = "Writer Client";

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
            rcvr.setTotalMessageSize(clnt.getMsgLength("XMLWriter.xml"));
            clnt.readMsgTemplate("XMLWriter.xml", sndr);
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
