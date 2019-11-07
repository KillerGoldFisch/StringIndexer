using StringIndexer.DBGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StringIndexer {
    public class Lookup {

        private static Dictionary<UInt32, String> _filenameCache = new Dictionary<uint, string>();

        public static int FindStringpositionsOffset(Stream db, UInt32 hash) {
            int offset = 4;
            db.Position = offset;

            for (int bit = 0; bit < 32; bit++) {
                bool high = (hash & 1 << bit) != 0;

                if (high)
                    offset += 4;

                db.Position = offset;
                offset = (int)db.ReadUInt32();
                if (offset == 0)
                    return 0;
            }

            return offset;
        }

        public static StringPosition[] FindAccurances(Stream db, String s) {
            var hash = s.ToLower().Crc32();

            int offset = FindStringpositionsOffset(db, hash);
            if (offset == 0)
                return null;

            db.Position = offset;

            var str = db.ReadString(db.ReadUInt32());
            var len = db.ReadUInt32();

            var re = new List<StringPosition>();

            for(int i = 0; i < len; i++) {
                re.Add(new StringPosition() {
                    FileNameHash = db.ReadUInt32(),
                    Position = db.ReadUInt32()
                });
            }

            foreach(var loc in re) {
                loc.File = FindString(db, loc.FileNameHash);
            }

            return re.ToArray();
        }

        public static String FindString(Stream db, UInt32 hash) {
            if (_filenameCache.ContainsKey(hash))
                return _filenameCache[hash];

            int offset = FindStringpositionsOffset(db, hash);
            if (offset == 0)
                return null;

            db.Position = offset;

            var str = db.ReadString(db.ReadUInt32());

            _filenameCache[hash] = str;

            return str;
        }
    }
}
