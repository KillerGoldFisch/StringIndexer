using StringIndexer.DBGen;
using System;
using System.IO;

namespace StringIndexer {
    class Program {
        static void Main(string[] args) {
            var ms = new MemoryStream();

            var gen = new Generator();
            gen.ProcessFile("../../../../../data/RomeoAndJuliet.txt");
            gen.Write(ms);

            var r = Lookup.FindAccurances(ms, "Romeo");

            if (r is null)
                Console.WriteLine("Not Found");
            else
                foreach(var s in r) {
                    Console.WriteLine(s.File + "\t" + s.Position);
                }

            Console.ReadLine();
        }
    }
}
