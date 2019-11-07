using System;
using System.Collections.Generic;
using System.Text;

namespace StringIndexer {
    public static class UInt32Extensions {
        public static byte[] GetBytes(this UInt32 i) {
            byte[] ret = BitConverter.GetBytes(i);
            return ret;
        }
    }
}
