using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StringIndexer.DBGen {
    public class StringPositions {
        public byte[] String { get; private set; }

        public UInt32 FileOffset { get; set; }
        public HashSet<StringPosition> Positions { get; private set; } = new HashSet<StringPosition>();

        public StringPositions(String s) {
            this.String = Encoding.UTF8.GetBytes(s);
        }

        public UInt32 GetSize() {
            return (UInt32) (
                4 + // lenth of String
                this.String.Length +
                4 + // Lenth of list
                Positions.Count * 2 * 4
            );
        }

        public void Write(Stream o) {
            o.WriteUInt32((uint)this.String.Length);

            o.Write(this.String);

            o.WriteUInt32((uint)this.Positions.Count);

            foreach(var pos in Positions) {
                o.WriteUInt32(pos.File.Crc32());
                o.WriteUInt32(pos.Position);
            }
        }
    }
}
