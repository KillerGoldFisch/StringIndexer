using StringIndexer.DBGen;
using System;
using System.IO;

namespace StringIndexer {
    class Program {
        static void Main(string[] args) {
            //var ms = new MemoryStream();
            Console.Write("Rebild index.bin? [Yn]:");
            var rebuild = Console.ReadLine().ToLower() != "n";

            var ms = new FileStream("./index.bin", rebuild ? FileMode.Create : FileMode.Open);

            Console.WriteLine("Directory:" + Environment.CurrentDirectory);

            if (rebuild) {
                var gen = new Generator();
                foreach (var file in Directory.GetFiles(".")) {
                    if (file.EndsWith(".txt")) {
                        Console.WriteLine("Indexing: " + file);
                        gen.ProcessFile(file);
                    }
                }

                Console.WriteLine("DONE");
                gen.Write(ms);
            }

            while (true) {
                Console.Write("> ");
                var searchString = Console.ReadLine();

                var start = DateTime.Now;
                var r = Lookup.FindAccurances(ms, searchString);
                var elapsedTime = DateTime.Now - start;

                if (r is null)
                    Console.WriteLine("Not Found");
                else {
                    Console.WriteLine($"Found {r.Length} entrys");
                    //foreach (var s in r) {
                    //    Console.WriteLine(s.File + "\t" + s.Position);
                    //}
                }

                Console.WriteLine($"Lookup took {elapsedTime.TotalMilliseconds} ms");
            }
        }
    }
}
