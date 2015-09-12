using EricUtility.Iterators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;

namespace Gerard.Tests
{
   [TestClass]
   public class IterSortTests
   {
      [TestMethod]
      public void TestIterSortCollection()
      {
         Console.WriteLine();
         Console.WriteLine( "Testing IterSortCollection" );

         Hashtable hash = new Hashtable();
         hash.Add( "Dog", 45 );
         hash.Add( "Cat", 45 );
         hash.Add( "Aardvark", 3 );
         hash.Add( "Whale", 0 );

         // old way
         string[] keys = new string[ hash.Count ];
         hash.Keys.CopyTo( keys, 0 );
         Array.Sort( keys );
         foreach ( string s in keys )
         {
            Console.WriteLine( "{0} = {1}", s, hash[ s ] );
         }

         foreach ( string s in new IterSortCollection( hash.Keys ) )
         {
            Console.WriteLine( "{0} = {1}", s, hash[ s ] );
         }
      }
   }
}