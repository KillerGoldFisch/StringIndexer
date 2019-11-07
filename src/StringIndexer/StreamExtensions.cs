using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StringIndexer {
    public static class StreamExtensions {
        public static void WriteUInt32(this Stream s, UInt32 i) {
            s.Write(i.GetBytes());
        }

        public static byte[] ReadBytes(this Stream s, UInt32 len) {
            var buffer = new byte[len];
            s.Read(buffer, 0, (int)len);
            return buffer;
        }

        public static UInt32 ReadUInt32(this Stream s) {
            return BitConverter.ToUInt32(s.ReadBytes(4));
        }

        public static String ReadString(this Stream s, UInt32 rawLength) {
            return Encoding.UTF8.GetString(s.ReadBytes(rawLength));
        }
    }
}
