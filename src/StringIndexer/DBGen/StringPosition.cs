using System;
using System.Collections.Generic;
using System.Text;

namespace StringIndexer.DBGen {
    public class StringPosition {
        public String File { get; set; }
        public UInt32 FileNameHash { get; set; }
        public UInt32 Position { get; set; }
    }
}
