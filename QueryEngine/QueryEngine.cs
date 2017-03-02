///////////////////////////////////////////////////////////////
// QueryEngine.cs - Test Requirements for Project #2          //
// Ver 1.0                                                    //
// Application: Demonstration for CSE681-SMA, Project#2      //
// Language:    C#, ver 5.0, Visual Studio 2015              //
// Author:      Ishan Gupta , CSE, Syracuse University       //  
//              (315) 935-1141, igupt100@syr.edu               //
///////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package helps demonstration of meeting requirements.
 * querying the key and there corrosponding value is done in
   this package.
 */
/*
 * Maintenance:
 * ------------
 * Required Files: 
 * DBElement.cs, DBEngine.cs, Display.cs,
 * DBExtensions.cs, UtilityExtensions.cs,ReadXML.cs,ItemEditor.cs
 *
 * Build Process:  devenv Project2Starter.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Maintenance History:
 * --------------------
 * ver 1.0 : 7 Oct 15
 * - first release
 *
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using static System.Console;

namespace Project4Starter
{

#if (TEST_QUERY_ENGINE)
    class TestQueryEngine
    {
        static void Main(string[] args)
        {
            QueryEngine<int, string> QE = new QueryEngine<int, string>();
            QueryEngine<string, string> QE1 = new QueryEngine<string, string>();

            DBEngine<int, DBElement<int, List<string>>> dbPay = new DBEngine<int, DBElement<int, List<string>>>();
            DBEngine<string, DBElement<string, List<string>>> dbString = new DBEngine<string, DBElement<string, List<string>>>();
            DBElement<string, List<String>> elemString = new DBElement<string, List<String>>();   //Populating DBEngine 
            elemString.name = "Element2";                                                         //object dbString
            elemString.descr = "testelement2";                                                    //for testing data string and list of string type.
            elemString.timeStamp = DateTime.Now;
            elemString.children.AddRange(new List<string> { "SMA1", "Syracuse2", "NY3" });
            elemString.payload = new List<string>();
            elemString.payload.AddRange(new List<string> { "we", "rock", "the ", "world" });
            dbString.insert("Prohject2", elemString);
            DBElement<string, List<String>> elemString2 = new DBElement<string, List<String>>();    //Populating DBEngine 
            elemString2.name = "Element3";                                                          //object dbString
            elemString2.descr = "test element3";                                                    //for testing data string and list of string type.
            elemString2.timeStamp = DateTime.Now;
            elemString2.children.AddRange(new List<string> { "SMA2", "Syracuse22", "NY33" });
            elemString2.payload = new List<string>();
            elemString2.payload.AddRange(new List<string> { "Thug", "Life" });
            dbString.insert("Thug3", elemString2);
            DBElement<int, List<string>> elemPayload = new DBElement<int, List<string>>();          //Populating DBEngine
            elemPayload.name = "Element4";                                                          //object dbPay
            elemPayload.descr = "test element4";                                                    //for testing data int and list of string type.
            elemPayload.timeStamp = DateTime.Now;
            elemPayload.children.AddRange(new List<int> { 1, 2, 3 });
            elemPayload.payload = new List<string>();
            elemPayload.payload.AddRange(new List<string> { "Project 2", " ", "demo", " ", "starts" });
            dbPay.insert(1, elemPayload);
            DBElement<int, List<string>> elemPayload2 = new DBElement<int, List<string>>();         //Populating DBEngine
            elemPayload2.name = "Element5";                                                         //object dbPay
            elemPayload2.descr = "test element5";                                                   //for testing data int and list of string type.
            elemPayload2.timeStamp = DateTime.Now;
            elemPayload2.children.AddRange(new List<int> { 98, 22, 35 });
            elemPayload2.payload = new List<string>();
            elemPayload2.payload.AddRange(new List<string> { "we", "rock", "the", "world" });
            dbPay.insert(2, elemPayload2);

            QE.valueByKey(1, dbPay);
            WriteLine();

            QE.childrenByKey(1, dbPay);


            WriteLine();
            QE1.keyPattern(".*hjbbj.*", dbString);
            WriteLine();
            QE.metaDataPattern("t2", dbString);


            DateTime toDate = new DateTime(2015, 10, 7);
            DateTime fromDate = new DateTime(2015, 10, 1);


            QE.dateTimeSearch(fromDate, toDate, dbString);

        }
    }
#endif

    public class QueryEngine<Key, Value>
    {
        DBElement<Key, string> elem = new DBElement<Key, string>();
        DBElement<Key, List<string>> elemPay = new DBElement<Key, List<string>>();

        public List<string> valueByKey(Key key, DBEngine<Key, DBElement<Key, List<string>>> db)
        {


            if (db.getValue(key, out elemPay))
            {
                if (elemPay.payload.Count() > 0)
                {

                    return elemPay.payload;
                }
                return null;
            }
            return null;
        }

        public List<Key> childrenByKey(Key key, DBEngine<Key, DBElement<Key, List<string>>> db)
        {


            if (db.getValue(key, out elemPay))
            {
                if (elemPay.children.Count() > 0)
                {

                    return elemPay.children;
                }
                return null;
            }
            return null;
        }

        public List<Key> keyPattern(string pattern, DBEngine<string, DBElement<string, List<string>>> db)
        {

            List<Key> foundKeys = new List<Key>();
            IEnumerable<string> keys = db.Keys();
            foreach (string key in keys)
            {

                if (Regex.IsMatch(key, pattern))
                {
                    foundKeys.Add((Key)Convert.ChangeType(key, typeof(Key)));
                }

            }
            foreach (Key key1 in foundKeys)


                if (foundKeys.Count == 0)
                {

                    Key str = (Key)Convert.ChangeType("No match found - returning all keys", typeof(Key));//"No match found - returning all keys";
                    foundKeys.Insert(0, str);

                    return foundKeys;
                }

            return foundKeys;
        }

        public List<string> metaDataPattern(string pattern, DBEngine<string, DBElement<string, List<string>>> db)
        {

            List<string> foundKeys = new List<string>();
            IEnumerable<string> keys = db.Keys();
            foreach (string key in keys)
            {
                DBElement<string, List<string>> elem = new DBElement<string, List<string>>();
                db.getValue(key, out elem);
                if (elem.name.Contains(pattern))
                {
                    foundKeys.Add(key);
                }
                else if (elem.descr.Contains(pattern))
                {
                    foundKeys.Add(key);
                }
                else if (elem.timeStamp.ToString().Contains(pattern))
                {
                    foundKeys.Add(key);
                }
            }

            if (foundKeys.Count == 0)
                return null;
            else
                return foundKeys;
        }

        public List<string> dateTimeSearch(DateTime fromDate, DateTime toDate, DBEngine<string, DBElement<string, List<string>>> db)
        {
            if (toDate == default(DateTime))
            {
                toDate = DateTime.Now;
            }

            List<string> foundKeys = new List<string>();
            IEnumerable<string> keys = db.Keys();
            foreach (string key in keys)
            {
                DBElement<string, List<string>> elem = new DBElement<string, List<string>>();

                db.getValue(key, out elem);

                if (elem.timeStamp <= toDate && elem.timeStamp > fromDate)
                {
                    foundKeys.Add(key);
                }
            }


            if (foundKeys.Count == 0)
                return null;
            else
                return foundKeys;

        }
    }



}

