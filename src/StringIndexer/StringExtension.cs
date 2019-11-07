using System;
using System.Collections.Generic;
using System.Text;

namespace StringIndexer {
    public static class StringExtension {
        public static UInt32 Crc32(this String s) {
            return StringIndexer.Crc32.Hash(s);
        }
    }
}
