///////////////////////////////////////////////////////////////
// Scheduler.cs - a constructor that is used to persist DB   //
//                to XML on a regular schedule              //
// Ver 1.1                                                  //
// Author:      Ishan Gupta , CSE, Syracuse University       //  
//              (315) 935-1141, igupt100@syr.edu             //
// Source:      Jim Fawcett, CST 4-187, Syracuse University  //
//              (315) 443-3948, jfawcett@twcny.rr.com        //
///////////////////////////////////////////////////////////////

/*
 * Package Operations:
 * -------------------
 * This Package provides functionlaities of scheduling the
 * persistence of Database to XML on a given time interval
 */
/*
 * Maintenance:
 * ------------
 * 
 * Required Files: 
 *   PersistXML.cs, DBElement.cs, DBEngine, 
 *   Display.cs,DBExtensions.cs, Scheduler.cs
 *
 * Build Process:  devenv Project2Starter.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Maintenance History:
 * --------------------
 * addition of function that schedules timely
 * persistence of DB to XML
 * ver 1.0 10/07/2015
 * - first release
 *
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Project4Starter
{
    public class Scheduler
    {
        public Timer scheduler{ get; set; } = new Timer();
         PersistXML c = new PersistXML();
        
        public Scheduler(DBEngine<int,DBElement<int, string>> db1, int time)
        {
            scheduler.Interval = time;
            scheduler.AutoReset = true;

            // Note use of timer's Elapsed delegate, binding to subscriber lambda
            // This delegate is invoked when the internal timer thread has waited
            // for the specified Interval.
           
            scheduler.Elapsed += (object source, ElapsedEventArgs e) =>
            {
                
                c.writeToXML(db1);
                Console.Write("\n  an event occurred at {0}" + e.SignalTime);
            };
        }
        static void Main(string[] args)
        {
            DBEngine<int, DBElement<int, string>> db = new DBEngine<int, DBElement<int, string>>();
            int timediff = 1000;
            Scheduler s = new Scheduler(db,timediff);
            s.scheduler.Enabled = true;
            Console.ReadKey();

        }

    
    }
}
