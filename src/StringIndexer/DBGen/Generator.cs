using StringIndexer.DBGen.BinaryTree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace StringIndexer.DBGen {
    public class Generator {
        BTree _tree = new BTree();
        HashSet<StringPositions> _strings = new HashSet<StringPositions>();

        private Regex _rxword = new Regex("(\\w+)");

        private StringPositions GetPositions(string s) {
            UInt32 hash = s.Crc32();

            BLeaf last = _tree.Root;

            for (int bit = 0; bit < 32; bit++) {
                bool high = (hash & 1 << bit) != 0;
                last = last[high];
            }

            if (last.StringPositions is null) {
                last.StringPositions = new StringPositions(s);
                _strings.Add(last.StringPositions);
            }
            return last.StringPositions;
        }

        public void RegisterString(string s, string filename, UInt32 position) {

            var pos = GetPositions(s);

            pos.Positions.Add(
                new StringPosition() {
                    File = filename,
                    Position = position
                }
            );

            RegisterString(filename);
        }

        public void RegisterString(string s) {
            GetPositions(s);
        }

        public void ProcessFile(string filename) {
            var data = File.ReadAllText(filename);

            foreach(Match match in _rxword.Matches(data)) {
                RegisterString(match.Value.ToLower(), filename, (UInt32)match.Index);
            }
        }

        public void Write(Stream o) {
            o.Write(((UInt32)32).GetBytes());

            // Precalculating file positions
            uint offset = 4;

            foreach (var leaf in _tree.Leafs) {
                if (leaf.StringPositions is null) {
                    leaf.FileOffset = offset;
                    offset += 2 * 4;
                }
            }

            foreach(var pos in _strings) {
                pos.FileOffset = offset;
                offset += pos.GetSize();
            }

            // Write to file
            foreach (var leaf in _tree.Leafs) {
                if (leaf.StringPositions is null) {
                    o.WriteUInt32(leaf.Low?.GetFilePosition() ?? 0);
                    o.WriteUInt32(leaf.High?.GetFilePosition() ?? 0);
                }
            }

            foreach (var pos in _strings) {
                pos.Write(o);
            }

        }
    }
}
