///////////////////////////////////////////////////////////////////////////////
// MessageMaker.cs - Construct ICommService Messages                         //
// ver 2.2                                                                   //
// Source : Jim Fawcett, CSE681 - Software Modeling and Analysis, Project #4 //    
// Application: Demonstration for CSE681-SMA, Project#4                      //
// Language:    C#, ver 5.0, Visual Studio 2015                              //
// Author:      Ishan Gupta, Syracuse University                            //
//              315-935-1141, igupt100@syr.edu                              //
///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////
/*
 * Purpose:
 *----------
 * This is a placeholder for application specific message construction
 *
 * Additions to C# Console Wizard generated code:
 * - references to ICommService and Utilities
 *
 * Required Fiiles : Utilities
 * Build Process:  devenv Project2Starter.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 */
/*
 * Public Methods: GetMessageList, makeMessage
 *
 * Maintenance History:
 * --------------------
 * ver 1.0 : 29 Oct 2015
 * - first release
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Project4Starter
{
    public class MessageTemplate
    {
        public string messageType { get; set; }
        public int messageSize { get; set; }
        public string content { get; set; }

        public List<MessageTemplate> GetMessageList(String fileName)
        {

            List<MessageTemplate> listOfMsgs = new List<MessageTemplate>();
            XDocument xdoc = new XDocument();
            try
            {
                xdoc = XDocument.Load(fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine("Please check the Template files");
            }
            try
            {
                var elem = from c in xdoc.Descendants("queries") select c;

                foreach (var item in elem.Elements())
                {
                    MessageTemplate msg = new MessageTemplate();
                    msg.messageType = item.Attribute("type").Value;
                    foreach (var childs in item.Elements())
                    {
                        if (childs.Name.LocalName.Equals("size"))
                            msg.messageSize = Convert.ToInt32(childs.Value);
                        if (childs.Name.LocalName.Equals("messageContent"))
                            msg.content = childs.ToString();
                    }
                    listOfMsgs.Add(msg);
                }
            }
            catch (Exception e) { Console.WriteLine("Please chheck the XML file"); }

            return listOfMsgs;
        }
    }

    public class MessageParameterState
    {
        public bool keyType = false;
        public bool valueType = false;
        public bool key = false;
        public bool value = false;
        public bool valName = false;
        public bool valTime = false;
        public bool valDesc = false;
        public bool valPayLoad = false;
        public bool valChild = false;
    }

    public class MessageMaker
    {

        List<MessageTemplate> listOfMsgs;
        List<string> listOfKeys = new List<string>();

        public static int msgCount { get; set; } = 0;


        public Message makeMessage(string fromUrl, string toUrl)
        {
            Message msg = new Message();
            msg.fromUrl = fromUrl;
            msg.toUrl = toUrl;



           
            msg.content = String.Format("\n  message #{0}", ++msgCount);
            return msg;
        }



#if (TEST_MESSAGEMAKER)
        static void Main(string[] args)
        {
            MessageMaker mm = new MessageMaker();
            Message msg = mm.makeMessage("fromFoo", "toBar");
            Utilities.showMessage(msg);
            Console.Write("\n\n");
        }
#endif
    }
}
