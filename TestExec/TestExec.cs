///////////////////////////////////////////////////////////////
// TestExec.cs - Test Requirements for Project #4            //
// Ver 1.0                                                   //
// Application: Demonstration for CSE681-SMA, Project#4      //
// Language:    C#, Visual Studio 2015                       //
// Author:      Ishan Gupta, Syracuse University           //
//              315-935-1141, igupt100@syr.edu            //
///////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package begins the demonstration of meeting requirements.
 * All other packages are called from here.
 *
 * Required Files:
 * Utilities.cs 
 *
 * Build Process:  devenv Project4Starter.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Activities
 * -------------------
 * On starting the Test Executive, one instance of each wpf client and 
 * server will start.
 * Server will be using the port 8080
 * 
 * Command Line Arguments for Test Executives
 * -------------------------------------------
 * eg 1 1 Y :- "No. of Read Clients" "No. of write Clients" "Message Logging on the Console"
 * 
 * Maintenance History:
 * --------------------
 * ver 1.0 : 24 Nov 15
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.IO;
using System.ComponentModel;
using System.Threading;

namespace Project4Starter
{
    using System.Diagnostics;
    using Util = Utilities;
    class TestExec
    {
        int Readers { get; set; } = 1;
        int Writers { get; set; } = 1;
        string logger { get; set; } = "N";

        static int serverPort = 8080;
        static int writerPort = 8082;
        static int readerPort = 9082;
        public void processCommandLine(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("  Invalid command line arguments.");
            }
            else if (args.Length == 3)
            {
                try
                {
                    Readers = Int32.Parse(args[0]);
                    Writers = Int32.Parse(args[1]);
                    logger = (args[2] == "Y") ? "Y" : "N";
                }
                catch (Exception e)
                {
                    Console.WriteLine("processCommandLine Exception :" + e.StackTrace);
                }
            }
            else
            {
                Console.WriteLine("  Invalid command line arguments.");
            }

        }

        public string findPath(string pkg)
        {
            string temp = AppDomain.CurrentDomain.BaseDirectory;
            int j = 0;
            while (j != 4)
            {
                int i = temp.LastIndexOf("\\");
                temp = temp.Substring(0, i);
                j++;
            }
            //temp = temp + "\\";
            pkg = temp + "\\" + pkg + "\\bin\\debug\\" + pkg + ".exe";
            return pkg;
        }

        void R1()
        {
            "Demonstrating Requirement 1".title();
            WriteLine();
            WriteLine("Project#4 implemented in C# using the facilities of the .Net Framework Class Library and Visual Studio 2015");
            WriteLine("as provided in the ECS clusters.All communication between processes and machines is implemented \nusing Windows Communication Foundation(WCF)");

            Console.WriteLine("-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*");
            WriteLine("\n");
        }
        void R2()
        {
            "Demonstrating Requirement 2".title();
            WriteLine();
            WriteLine("NoSQL DB from Project#2 is used by Reader clients to query the DB  \nand is used by Writer Clients to perform Insert/Update/Delete/Edit/Persist and Augmentation of DB");
            Console.WriteLine("-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*");
            WriteLine("\n");
        }
        void R3()
        {
            "Demonstrating Requirement 3".title();
            WriteLine();
            WriteLine("Used WCF to communicate between clients and a server that exposes the noSQL database \nthrough messages that are sent by clients and enqueued by the server.");
            WriteLine("Each message is processed by the server to interact with the database and results are sent,\nas messages, back to queues provided by each client request.");
            Console.WriteLine("-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*");
            WriteLine("\n");
        }

        void R4()
        {
            "Demonstrating Requirement 4".title();
            WriteLine();
            WriteLine("add, delete, and edit key/value pairs is demonstrated in Writer Client");
            WriteLine("persist and restore the database from an XML file is demonstrated in Writer Client");
            WriteLine("All types of queries are demonstrated in Reader Clients");
            WriteLine("All of the above operation requests shall be sent to the remote database in the form of messages\ndescribed by a WCF Data Contract2. Replies are returned to the requestor in the form of WCF messages\nusing a suitable Data Contract");
            Console.WriteLine("-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*");
            WriteLine("\n");
        }

        void R5()
        {
            "Demonstrating Requirement 5".title();
            WriteLine();
            WriteLine("Write Client: is a CONSOLE Client that send data to the remote database");
            WriteLine("The content of several different messages is defined in an XML file --XMLWriter.xml--");
            Console.WriteLine("-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*");
            WriteLine("\n");
        }
        void R6()
        {
            "Demonstrating Requirement 6".title();
            WriteLine();
            WriteLine("Pass Command Line Argument{3} as Y in test executives arguments to enable logging and N to disable");
            Console.WriteLine("-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*");
            WriteLine("\n");
        }
        void R7()
        {
            "Demonstrating Requirement 7".title();
            WriteLine();
            WriteLine("Read Client: is a CONSOLE Client that send query request to the remote database");
            WriteLine("The content of several different messages is defined in an XML file --XMLReader.xml--");
            Console.WriteLine("-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*");
            WriteLine("\n");
        }
        void R8()
        {
            "Demonstrating Requirement 8".title();
            WriteLine();
            WriteLine("Read Client: will display the result of the query if the logging in ON");
            WriteLine("The content of several different messages is defined in an XML file --XMLReader.xml--");
            Console.WriteLine("-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*");
            WriteLine("\n");
        }
        static void Main(string[] args)
        {
            Console.Write("\n Test-Executive  ");
            Console.Write("\n ***********************\n");
            Console.Title = "Test Executive";
            TestExec testExec = new TestExec();

            testExec.R1();
            testExec.R2();
            testExec.R3();
            testExec.R4();
            testExec.R5();
            testExec.R6();
            testExec.R7();
            testExec.R8();

            try
            {
                Process.Start(testExec.findPath("Server"));
            }
            catch (Exception e)
            {
                Console.WriteLine("****************************Please check Server.exe file*********************");
            }
            try
            {
                Process.Start(testExec.findPath("WpfClient"));
            }
            catch (Exception e)
            {
                Console.WriteLine("****************************Please check WpfClient.exe file*********************");
            }

            if (args.Length == 3)
            {
                string arg = "";
                testExec.processCommandLine(args);

                int i = 0;
                while (i < testExec.Writers)
                {
                    arg = "/R http://localhost:" + TestExec.serverPort + "/CommService /L http://localhost:" + (TestExec.writerPort + i) + "/CommService " + testExec.logger;
                    try
                    {
                        Process.Start(testExec.findPath("Client"), arg);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("****************************Please check Client.exe file*********************");
                    }
                    i++;
                }
                i = 0;
                Thread.Sleep(1500);
                while (i < testExec.Readers)
                {
                    arg = "/R http://localhost:" + TestExec.serverPort + "/CommService /L http://localhost:" + (TestExec.readerPort + i) + "/CommService " + testExec.logger;
                    try
                    {
                        Process.Start(testExec.findPath("Client2"), arg);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("****************************Please check Client2.exe file*********************");
                    }
                    i++;
                }
                return;
            }
            else
            {
                return;
            }
        }
    }
}