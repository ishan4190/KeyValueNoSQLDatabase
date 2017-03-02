///////////////////////////////////////////////////////////////////////////////
// Server.cs - CommService server                                            //
// ver 2.2                                                                   //
// Source : Jim Fawcett, CSE681 - Software Modeling and Analysis, Project #4 //    
// Application: Demonstration for CSE681-SMA, Project#4                      //
// Language:    C#, ver 5.0, Visual Studio 2015                              //
// Author:       Ishan Gupta, Syracuse University                            //
//              315-935-1141, igupt100@syr.edu                              //
///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////
/*
*
*
 * Additions to C# Console Wizard generated code:
 * - Added reference to ICommService, Sender, Receiver, Utilities
 *
 * Note:
 * - This server now receives and then sends back received messages.
 * This package calls the project#2 packages and carryout DB operations
 */
/*
 * Maintenance:
 * ------------
 * Required Files: 
 * DBElement.cs, DBEngine.cs, Display.cs, PersistEngine.cs,QueryEngine.cs,
 * TestTimer.cs,DBExtensions.cs, UtilityExtensions.cs,ReadXML.cs
 * ItemEditor.cs,Sender.cs,Receiver.cs,ICommservice.cs,Utilities.cs
 *
 * Build Process:  devenv Project2Starter.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 * Plans:
 * - Add message decoding and NoSqlDb calls in performanceServiceAction.
 * - Provide requirements testing in requirementsServiceAction, perhaps
 *   used in a console client application separate from Performance 
 *   Testing GUI.
 */
/*
 * Public Methods:
 * processAugmentedMsg,ProcessCommandLine,getOperationType,getKeyValueType,getKey,
 * getMetadataPat,getPattern,getToDate,getFromDate,getNumberOfKeys,createAddDBElement,
 * dbElemBuild,processDelMsg,editMsg_Process,processAddMsg,processQuery5,processQuery4,
 * Q3Msg_Process,processQuery2,processQuery1,processPersistMsg,getDatafromXML,processAugmentedMsg
 *
 * Maintenance History:
 * --------------------
 * ver 2.4 : Made changes to meet the Project 4 requirements
 * ver 2.3 : 29 Oct 2015
 * - added handling of special messages: 
 *   "connection start message", "done", "closeServer"
 * ver 2.2 : 25 Oct 2015
 * - minor changes to display
 * ver 2.1 : 24 Oct 2015
 * - added Sender so Server can echo back messages it receives
 * - added verbose mode to support debugging and learning
 * - to see more detail about what is going on in Sender and Receiver
 *   set Utilities.verbose = true
 * ver 2.0 : 20 Oct 2015
 * - Defined Receiver and used that to replace almost all of the
 *   original Server's functionality.
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

    class Server
    {
        List<int> Ttime = new List<int>();
        List<int> Ttime1 = new List<int>();
        DBEngine<string, DBElement<string, List<string>>> db = new DBEngine<string, DBElement<string, List<string>>>();
        string address { get; set; } = "localhost";
        string port { get; set; } = "8080";

        //----< quick way to grab ports and addresses from commandline >-----

        public void ProcessCommandLine(string[] args)
        {
            if (args.Length > 0)
            {
                port = args[0];
            }
            if (args.Length > 1)
            {
                address = args[1];
            }
        }


        public static string getOperationType(XDocument xdoc)
        {
            var elem = xdoc.Root;
            return elem.Attribute("commandType").Value;
        }


        public static int getKeyValueType(XDocument xdoc)
        {
            var elem = from c in xdoc.Descendants("message") select c;
            foreach (var item in elem.Elements())
            {
                Console.WriteLine(item.Name);
                if (item.Name == "keyType")
                {
                    if (item.Value.Equals("string"))
                    {
                        return 1;
                    }
                    if (item.Value.Equals("Int"))
                    {
                        return 0;
                    }
                }
            }
            return 0;
        }

        public string getKey(XDocument xdoc)
        {
            string key = "";
            var elem = from c in xdoc.Descendants("message") select c;
            foreach (var item in elem.Elements())
            {
                if (item.Name == "key")
                    key = item.Value.ToString();
            }
            return key;
        }

        public string getMetadataPat(XDocument xdoc)
        {
            string pattern = "";
            var elem = from c in xdoc.Descendants("message") select c;
            foreach (var item in elem.Elements())
            {
                if (item.Name == "searchParaMeter")
                    pattern = item.Value.ToString();
            }
            return pattern;
        }

        public string getPattern(XDocument xdoc)
        {
            string pattern = "";
            var elem = from c in xdoc.Descendants("message") select c;
            foreach (var item in elem.Elements())
            {
                if (item.Name == "pattern")
                    pattern = item.Value.ToString();
            }
            return pattern;
        }
        public DateTime getToDate(XDocument xdoc)
        {
            DateTime toDate = new DateTime();
            var elem = from c in xdoc.Descendants("message") select c;
            foreach (var item in elem.Elements())
            {
                if (item.Name == "endTime")
                    toDate = Convert.ToDateTime(item.Value);
            }
            return toDate;
        }

        public DateTime getFromDate(XDocument xdoc)
        {
            DateTime fromDate = new DateTime();
            var elem = from c in xdoc.Descendants("message") select c;
            foreach (var item in elem.Elements())
            {
                if (item.Name == "startTime")
                    fromDate = Convert.ToDateTime(item.Value);
            }
            return fromDate;
        }

        public string getNumberOfKeys(XDocument xdoc)
        {
            string numKeys = "";
            var elem = from c in xdoc.Descendants("message") select c;
            foreach (var item in elem.Elements())
            {
                if (item.Name == "numKeys")
                    numKeys = item.Value.ToString();
            }
            return numKeys;
        }

        public static string createAddDBElement(XDocument xdoc, out DBElement<string, List<string>> element)
        {
            element = new DBElement<string, List<string>>();
            string key = "";
            var elem = from c in xdoc.Descendants("message") select c;
            foreach (var item in elem.Elements())
            {
                if (item.Name == "key")
                    key = item.Value.ToString();
                else if (item.Name == "value")
                {
                    foreach (var valueItem in item.Elements())
                    {
                        if (valueItem.Name == "name")
                            element.name = valueItem.Value;
                        else if (valueItem.Name == "desc")
                            element.descr = valueItem.Value;
                        else if (valueItem.Name == "time")
                            element.timeStamp = Convert.ToDateTime(valueItem.Value);
                        else if (valueItem.Name == "payload")
                        {
                            element.payload = new List<string>();
                            foreach (var payloadItem in valueItem.Elements())
                            {
                                if (payloadItem.Name == "item")
                                    element.payload.Add(payloadItem.Value);
                            }
                        }
                        else if (valueItem.Name == "children")
                        {
                            element.children = new List<string>();
                            foreach (var childs in valueItem.Elements())
                            {
                                if (childs.Name == "item")
                                    element.children.Add(childs.Value);
                            }
                        }
                    }
                }
            }
            return key;
        }

        public static void dbElemBuild(string keyType, string ValueType, string key, string name, string desc, DateTime timeStamp, List<string> payloadItems, List<string> childItems)
        {
            if (keyType == "string" && ValueType == "ListOfString")
            {
                DBElement<string, List<string>> elem = new DBElement<string, List<string>>();
                elem.name = name;
                elem.descr = desc;
                elem.timeStamp = timeStamp;
                elem.children = childItems;
                elem.payload = payloadItems;
            }
            else if (keyType == "int" && ValueType == "string")
            {
                DBElement<int, string> elem = new DBElement<int, string>();
                elem.name = name;
                elem.descr = desc;
                elem.timeStamp = timeStamp;
                List<int> childs = childItems.Select(s => int.Parse(s)).ToList();
                elem.children = childs;
                elem.payload = payloadItems[0];
            }
        }
        public void processDelMsg(XDocument xdoc, Sender sndr, Message msg)
        {
            IEnumerable<string> keys = db.Keys();
            int numKeys = Convert.ToInt32(getNumberOfKeys(xdoc));
            if (numKeys > keys.Count())
            {
                numKeys = keys.Count() - 1;
            }
            List<string> keyList = keys.ToList();
            for (int i = 0; i < numKeys; i++)
            {
                string keyToDeleted = keyList.ElementAt(i);
                if (db.delete(keyToDeleted))
                {
                    msg.content = "\n\n***************************\n" + keyToDeleted + " record is deleted.\n";
                }
                else
                {
                    msg.content = "\n*********************\n" + keyToDeleted + " record is  not deleted.";
                }
                Message testMsg = new Message();
                testMsg.toUrl = msg.fromUrl;
                testMsg.fromUrl = msg.toUrl;
                testMsg.content = msg.content;
                sndr.sendMessage(testMsg);
            }
        }
        public void processEditMsg(XDocument xdoc, Sender sndr, Message msg)
        {
            IEnumerable<string> keys = db.Keys();
            List<string> keyList = keys.ToList();
            string keyToEdit = getKey(xdoc);
            if (!keyList.Contains(keyToEdit))
            {
                Message testMsg = new Message();
                testMsg.toUrl = msg.fromUrl;
                testMsg.fromUrl = msg.toUrl;
                testMsg.content = "\n********************************\n" + "Key " + keyToEdit + " is not present in the DB\n";
                sndr.sendMessage(testMsg);
            }
            else
            {
                DBElement<string, List<string>> element = new DBElement<string, List<string>>();
                string key = createAddDBElement(xdoc, out element);
                Message testMsg = new Message();
                testMsg.toUrl = msg.fromUrl;
                testMsg.fromUrl = msg.toUrl;
                if (db.edit(key, element))
                {
                    testMsg.content = "\n***********************\n" + "Key= " + key + " is edited Successfully.\n";
                }
                else
                {
                    testMsg.content = "\n**************************\n" + key + " record is not edited.\n";
                }
                sndr.sendMessage(testMsg);
            }
        }

        public void processAddMsg(XDocument xdoc, Sender sndr, Message msg)
        {
            DBElement<string, List<string>> element = new DBElement<string, List<string>>();
            string key = createAddDBElement(xdoc, out element);
            Message testMsg = new Message();
            testMsg.toUrl = msg.toUrl;
            testMsg.fromUrl = msg.fromUrl;
            if (db.insert(key, element))
            {
                msg.content = "\n\n*****************************\nKey = " + key + " is inserted Successfully.\n\n";
            }
            else
            {
                msg.content = "\n******************************\n" + key + " record is not inserted.\n\n";
            }
            Util.swapUrls(ref msg);
            sndr.sendMessage(msg);
        }

        public void processQuery5(XDocument xdoc, Sender sndr, Message msg)
        {
            DateTime fromDate = getFromDate(xdoc);
            DateTime toDate = getToDate(xdoc);
            DBElement<string, List<string>> element = new DBElement<string, List<string>>();
            QueryEngine<string, string> QE1 = new QueryEngine<string, string>();
            Message testMsg = new Message();
            testMsg.toUrl = msg.fromUrl;
            testMsg.fromUrl = msg.toUrl;
            List<string> values = new List<string>();
            values = QE1.dateTimeSearch(fromDate, toDate, db);
            if (values.Count() > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string str in values)
                {
                    sb.Append(str);
                    sb.Append("\n");
                }
                testMsg.content = "\n**********************" + "\nResult of Query5" + "\nKeys within Date pattern are:\n" + sb + "\n";
            }
            else
            {
                testMsg.content = "\n*************************\n" + "\nKeys within Date pattern not found\n";
            }
            sndr.sendMessage(testMsg);
        }

        public void processQuery4(XDocument xdoc, Sender sndr, Message msg)
        {
            string pattern = getMetadataPat(xdoc);
            DBElement<string, List<string>> element = new DBElement<string, List<string>>();
            QueryEngine<string, string> QE1 = new QueryEngine<string, string>();
            Message testMsg = new Message();
            testMsg.toUrl = msg.fromUrl;
            testMsg.fromUrl = msg.toUrl;
            List<string> values = new List<string>();
            values = QE1.metaDataPattern(pattern, db);
            if (values.Count() > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string str in values)
                {
                    sb.Append(str);
                    sb.Append("\n");
                }
                testMsg.content = "\n***********************\n" + "\nResult of Query4\n" + "Keys with pattern like " + pattern + " in the metadata are :\n" + sb + "\n";
            }
            else
            {
                testMsg.content = "\n**********************\n" + "\nKeys with date pattern like  " + pattern + " in the metadata not found\n";
            }
            sndr.sendMessage(testMsg);
        }

        public void processQuery3(XDocument xdoc, Sender sndr, Message msg)
        {
            string pattern = getPattern(xdoc);
            DBElement<string, List<string>> element = new DBElement<string, List<string>>();
            QueryEngine<string, string> QE1 = new QueryEngine<string, string>();
            Message testMsg = new Message();
            testMsg.toUrl = msg.fromUrl;
            testMsg.fromUrl = msg.toUrl;
            List<string> values = new List<string>();
            values = QE1.keyPattern(pattern, db);
            if (values.Count() > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string str in values)
                {
                    sb.Append(str);
                    sb.Append("\n");
                }
                testMsg.content = "\n*************************\n" + "\nResult of Query3\n" + "Keys with pattern like " + pattern + " are :\n" + sb + "\n";
            }
            else
            {
                testMsg.content = "\n*************************\n" + "\nKeys with pattern like  " + pattern + " not found\n";
            }
            sndr.sendMessage(testMsg);
        }
        public void processQuery2(XDocument xdoc, Sender sndr, Message msg)
        {
            IEnumerable<string> keys = db.Keys();
            List<string> keyList = keys.ToList();
            string keyToSearch = keyList.First();
            if (!keyList.Contains(keyToSearch))
            {
                Console.WriteLine("Key {0} is not present in the DB", keyToSearch);
            }
            else
            {
                DBElement<string, List<string>> element = new DBElement<string, List<string>>();
                QueryEngine<string, string> QE1 = new QueryEngine<string, string>();
                Message testMsg = new Message();
                testMsg.toUrl = msg.fromUrl;
                testMsg.fromUrl = msg.toUrl;
                List<string> values = new List<string>();
                values = QE1.childrenByKey(keyToSearch, db);
                if (values.Count() > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string str in values)
                    {
                        sb.Append(str);
                        sb.Append("\n");
                    }
                    testMsg.content = "\n**********************\n" + "\nResult of Query2\n" + "Children of " + keyToSearch + " is :\n" + sb + "\n";
                }
                else
                {
                    testMsg.content = "\n*************************\n" + "\nChildren of " + keyToSearch + " not found\n";
                }
                sndr.sendMessage(testMsg);
            }
        }
        public void processQuery1(XDocument xdoc, Sender sndr, Message msg)
        {
            IEnumerable<string> keys = db.Keys();
            List<string> keyList = keys.ToList();
            //string keyToSearch = getKey(xdoc);
            string keyToSearch = keyList.First();
            if (!keyList.Contains(keyToSearch))
            {
                Console.WriteLine("Key {0} is not present in the DB", keyToSearch);
            }
            else
            {
                DBElement<string, List<string>> element = new DBElement<string, List<string>>();
                QueryEngine<string, string> QE1 = new QueryEngine<string, string>();
                Message testMsg = new Message();
                testMsg.toUrl = msg.fromUrl;
                testMsg.fromUrl = msg.toUrl;
                List<string> values = new List<string>();
                values = QE1.valueByKey(keyToSearch, db);
                if (values.Count() > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string str in values)
                    {
                        sb.Append(str);
                        sb.Append("\n");
                    }
                    testMsg.content = "\n*******************************\n" + "\nResult of Query1\n" + "Value of " + keyToSearch + " is :\n" + sb + "\n";
                }
                else
                {
                    testMsg.content = "\n**************************\n" + "\nValue of " + keyToSearch + " not found\n";
                }
                sndr.sendMessage(testMsg);
            }
        }

        public void processPersistMsg(XDocument xdoc, Sender sndr, Message msg)
        {
            PersistXML<string, List<string>> pe = new PersistXML<string, List<string>>();
            pe.createListStringXML(db);
            Message testMsg = new Message();
            testMsg.toUrl = msg.fromUrl;
            testMsg.fromUrl = msg.toUrl;
            if (pe.createListStringXML(db))
            {
                testMsg.content = "\n**************************\n" + "\nDatabase persisted successfully : Filename - Project4-Persisted.xml" + "\n";
            }
            else
            {
                testMsg.content = "\nDatabase persistance failed";
            }
            //Console.WriteLine(testMsg.content);
            sndr.sendMessage(testMsg);
            //srvr.db.showEnumerableDB();
        }

        public void getDatafromXML(DBEngine<string, DBElement<string, List<string>>> dbEngine, String inputFile)
        {
            XDocument document = new XDocument();
            String fileName = "";
            fileName = inputFile;
            try { document = XDocument.Load(fileName); }
            catch (Exception e) { Console.WriteLine("Please check the XML file used for augmentation"); }
            IEnumerable<XElement> elem = document.Descendants("elements");
            try
            {
                for (int i = 0; i < elem.Elements().Count(); i++)
                {
                    DBElement<string, List<string>> dbElement = new DBElement<string, List<string>>();
                    string key = elem.Elements().Attributes().ElementAt(i).Value;
                    for (int count = 0; count < elem.Elements().Attributes().ElementAt(i).Parent.Descendants().Count(); count++)
                    {
                        XElement elementRecord = elem.Elements().Attributes().ElementAt(i).Parent.Descendants().ElementAt(count);
                        if (elementRecord.Name.ToString().Equals("name"))
                            dbElement.name = elementRecord.Value;
                        else if (elementRecord.Name.ToString().Equals("desc"))
                            dbElement.descr = elementRecord.Value;
                        else if (elementRecord.Name.ToString().Equals("time"))
                            dbElement.timeStamp = DateTime.Parse(elementRecord.Value);
                        else if (elementRecord.Name.ToString().Equals("children"))
                        {
                            List<string> children = new List<string>();
                            for (int j = 0; j < elementRecord.Descendants().Count(); j++)
                            {
                                children.Add(elementRecord.Descendants().ElementAt(j).Value);
                            }
                            dbElement.children = children;
                        }
                        else if (elementRecord.Name.ToString().Equals("payload"))
                        {
                            List<string> payload = new List<string>();
                            for (int j = 0; j < elementRecord.Descendants().Count(); j++)
                            {
                                payload.Add(elementRecord.Descendants().ElementAt(j).Value);
                            }
                            dbElement.payload = payload;
                        }
                    }
                    dbEngine.insert(key, dbElement);
                }
            }
            catch (Exception e) { Console.WriteLine("Please check the xml file"); }
        }

        public void processAugmentedMsg(XDocument xdoc, Sender sndr, Message msg, Server srvr)
        {
            DBEngine<string, DBElement<string, List<string>>> dbEngine = new DBEngine<string, DBElement<string, List<string>>>();
            srvr.getDatafromXML(dbEngine, "augment.xml");
            Message testMsg = new Message();
            testMsg.toUrl = msg.fromUrl;
            testMsg.fromUrl = msg.toUrl;
            if (dbEngine.Keys().Count() > 0)
            {
                //dbEngine.showEnumerableDB();
                testMsg.content = "\n*****************************************\n" + "\nDatabase augmented successfully in Database instance: name - dbEngine ";
            }
            else
            {
                testMsg.content = "\nDatabase augmentation failed";
            }
            
            sndr.sendMessage(testMsg);
            
        }


        static void Main(string[] args)
        {
            
            Util.verbose = false;
            Server srvr = new Server();
            srvr.ProcessCommandLine(args);
            Console.Title = "Server";
            Console.Write(String.Format("\n  Starting CommService server listening on port {0}", srvr.port));
            Console.Write("\n ====================================================\n");

            Sender sndr = new Sender(Util.makeUrl(srvr.address, srvr.port));
            
            Receiver rcvr = new Receiver(srvr.port, srvr.address);

            // - serviceAction defines what the server does with received messages
            // - This serviceAction just announces incoming messages and echos them
            //   back to the sender.  
            // - Note that demonstrates sender routing works if you run more than
            //   one client.

            Action serviceAction = () =>
            {
                Message msg = null;
                while (true)
                {
                    msg = rcvr.getMessage();   // note use of non-service method to deQ messages
                    Console.Write("\n  Received message:");
                    Console.Write("\n  sender is {0}", msg.fromUrl);
                    //Console.Write("\n  content is {0}\n", msg.content);

                    if (msg.content == "connection start message")
                    {
                        continue; // don't send back start message
                    }
                    if (msg.content == "done")
                    {
                        Console.Write("\n  client has finished\n");
                        continue;
                    }
                    if (msg.content == "closeServer")
                    {
                        Console.Write("received closeServer");
                        break;
                    }
                    if (msg.content.Contains("Writer"))
                    {
                        int avg = srvr.Ttime.Sum() / srvr.Ttime.Count();
                        string lat = string.Join(null, System.Text.RegularExpressions.Regex.Split(msg.content, "[^\\d]"));
                        Message msgToWpf = new Message();
                        msgToWpf.fromUrl = Util.makeUrl(srvr.address, srvr.port);
                        msgToWpf.toUrl = Util.makeUrl("localhost", "8089");
                        msgToWpf.content = msg.content + " microseconds for Client: " + msg.fromUrl.ToString();
                        sndr.sendMessage(msgToWpf);
                        Message msgToWpf1 = new Message();
                        msgToWpf1.fromUrl = Util.makeUrl(srvr.address, srvr.port);
                        msgToWpf1.toUrl = Util.makeUrl("localhost", "8089");
                        msgToWpf1.content = "Server ThroughPut(Avg Time) for " + srvr.Ttime.Count().ToString() + " Write Operations = " + avg.ToString() + " microseconds";
                        sndr.sendMessage(msgToWpf1);
                        continue;
                    }
                    if (msg.content.Contains("Reader"))
                    {
                        int avg1 = srvr.Ttime1.Sum() / srvr.Ttime1.Count();
                        string lat = string.Join(null, System.Text.RegularExpressions.Regex.Split(msg.content, "[^\\d]"));
                        Message msgToWpf = new Message();
                        msgToWpf.fromUrl = Util.makeUrl(srvr.address, srvr.port);
                        msgToWpf.toUrl = Util.makeUrl("localhost", "8089");
                        msgToWpf.content = msg.content + " microseconds for Client: " + msg.fromUrl.ToString();
                        sndr.sendMessage(msgToWpf);
                        Message msgToWpf1 = new Message();
                        msgToWpf1.fromUrl = Util.makeUrl(srvr.address, srvr.port);
                        msgToWpf1.toUrl = Util.makeUrl("localhost", "8089");
                        msgToWpf1.content = "Server ThroughPut(Avg Time) for " + srvr.Ttime1.Count().ToString() + " Read Operations = " + avg1.ToString() + " microseconds";
                        sndr.sendMessage(msgToWpf1);
                        continue;
                    }
                    try
                    {
                        XDocument xdoc = XDocument.Parse(msg.content);
                        string operationCalled = getOperationType(xdoc);
                        if (operationCalled == "add")
                        {
                            Console.WriteLine("\nAdd Operation Called");
                            HiResTimer hrt = new HiResTimer();
                            hrt.Start();
                            srvr.processAddMsg(xdoc, sndr, msg);
                            hrt.Stop();
                            srvr.Ttime.Add(Convert.ToInt32(hrt.ElapsedMicroseconds));
                        }
                        else if (operationCalled == "delete")
                        {
                            Console.WriteLine("\nDelete Operation Called");
                            HiResTimer hrt = new HiResTimer();
                            hrt.Start();
                            srvr.processDelMsg(xdoc, sndr, msg);
                            hrt.Stop();
                            srvr.Ttime.Add(Convert.ToInt32(hrt.ElapsedMicroseconds));
                        }

                        else if (operationCalled == "edit")
                        {
                            Console.WriteLine("\nEdit Operation Called");
                            HiResTimer hrt = new HiResTimer();
                            hrt.Start();
                            srvr.processEditMsg(xdoc, sndr, msg);
                            hrt.Stop();
                            srvr.Ttime.Add(Convert.ToInt32(hrt.ElapsedMicroseconds));
                        }

                        else if (operationCalled == "query1")
                        {
                            Console.WriteLine("\nQuery1 Operation Called\n");
                            HiResTimer hrt = new HiResTimer();
                            hrt.Start();
                            srvr.processQuery1(xdoc, sndr, msg);
                            hrt.Stop();
                            srvr.Ttime1.Add(Convert.ToInt32(hrt.ElapsedMicroseconds));
                        }
                        else if (operationCalled == "query2")
                        {
                            Console.WriteLine("\nQuery2 Operation Called\n");
                            HiResTimer hrt = new HiResTimer();
                            hrt.Start();
                            srvr.processQuery2(xdoc, sndr, msg);
                            hrt.Stop();
                            srvr.Ttime1.Add(Convert.ToInt32(hrt.ElapsedMicroseconds));
                            Util.swapUrls(ref msg);
                            msg.content = "-----Query2 operation performed-----";
                            sndr.sendMessage(msg);
                        }
                        else if (operationCalled == "query3")
                        {
                            Console.WriteLine("\nQuery3 Operation Called\n");
                            HiResTimer hrt = new HiResTimer();
                            hrt.Start();
                            srvr.processQuery3(xdoc, sndr, msg);
                            hrt.Stop();
                            srvr.Ttime1.Add(Convert.ToInt32(hrt.ElapsedMicroseconds));
                            Util.swapUrls(ref msg);
                            msg.content = "-----Query3 operation performed-----";
                            sndr.sendMessage(msg);
                        }
                        else if (operationCalled == "query4")
                        {
                            Console.WriteLine("\nQuery4 Operation Called\n");
                            HiResTimer hrt = new HiResTimer();
                            hrt.Start();
                            srvr.processQuery4(xdoc, sndr, msg);
                            hrt.Stop();
                            srvr.Ttime1.Add(Convert.ToInt32(hrt.ElapsedMicroseconds));
                        }
                        else if (operationCalled == "query5")
                        {
                            Console.WriteLine("\nQuery5 Operation Called\n");
                            HiResTimer hrt = new HiResTimer();
                            hrt.Start();
                            srvr.processQuery5(xdoc, sndr, msg);
                            hrt.Stop();
                            srvr.Ttime1.Add(Convert.ToInt32(hrt.ElapsedMicroseconds));
                            Util.swapUrls(ref msg);
                            msg.content = "-----Query5 operation performed-----";
                            sndr.sendMessage(msg);
                        }
                        else if (operationCalled == "persist")
                        {
                            Console.WriteLine("\nPersist Operation Called");
                            HiResTimer hrt = new HiResTimer();
                            hrt.Start();
                            srvr.processPersistMsg(xdoc, sndr, msg);
                            hrt.Stop();
                            srvr.Ttime.Add(Convert.ToInt32(hrt.ElapsedMicroseconds));
                        }
                        else if (operationCalled == "augment")
                        {
                            Console.WriteLine("\nAugment Operation Called\n");
                            HiResTimer hrt = new HiResTimer();
                            hrt.Start();
                            srvr.processAugmentedMsg(xdoc, sndr, msg, srvr);
                            hrt.Stop();
                            srvr.Ttime.Add(Convert.ToInt32(hrt.ElapsedMicroseconds));
                        }
                    }

                    catch { }



#if (TEST_WPFCLIENT)
                    /////////////////////////////////////////////////
                    // The statements below support testing the
                    // WpfClient as it receives a stream of messages
                    // - for each message received the Server
                    //   sends back 1000 messages
                    //
                    int count = 0;
                    for (int i = 0; i < 2; ++i)
                    {
                        Message testMsg = new Message();
                        testMsg.toUrl = msg.toUrl;
                        testMsg.fromUrl = msg.fromUrl;
                        testMsg.content = String.Format("test message #{0}", ++count);
                        Console.Write("\n  sending testMsg: {0}", testMsg.content);
                        sndr.sendMessage(testMsg);
                    }
#else
                    /////////////////////////////////////////////////
                    // Use the statement below for normal operation

#endif                   

                }

            };

            if (rcvr.StartService())
            {
                rcvr.doService(serviceAction); // This serviceAction is asynchronous,
            }                                // so the call doesn't block.
            Util.waitForUser();
        }
    }
}
