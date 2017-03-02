///////////////////////////////////////////////////////////////
// DBFactory.cs - Immutable database with no augment fucntion//
// Ver 1.0                                                   //
// Author:      Ishan Gupta , CSE, Syracuse University       //  
//              (315) 935-1141, igupt100@syr.edu             //
// Source:      Jim Fawcett, CST 4-187, Syracuse University  //
//              (315) 443-3948, jfawcett@twcny.rr.com        //
///////////////////////////////////////////////////////////////

/*
 * Package Operations:
 * -------------------
 * This Package provides functionlaities of
 * Persisting Database to XML and XML data to Database.
 */
/*
 * Maintenance:
 * ------------
 * 
 * Required Files: 
 *   PersistXML.cs, DBElement.cs, DBEngine, 
 *   QueryEngine.cs,
 *
 * Build Process:  devenv Project2Starter.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Maintenance History:
 * --------------------
 * ver 1.0 10/08/2015
 * - first release
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project4Starter
{
     public class DBFactory<Key, Value>
{
    private Dictionary<Key, Value> dbStoreIm;
    public DBFactory()
    {
        dbStoreIm = new Dictionary<Key, Value>();
    }
}

#if (TEST_TESTDBFACTORY)

  public class TestDBFactory
  {
    static void Main(string[] args)
    {
      //"Testing DBFactory Package".title('=');
      Console.WriteLine();

      
      Console.Write("\n\n DBFactory Incomplete");
    }
  }
#endif
}
