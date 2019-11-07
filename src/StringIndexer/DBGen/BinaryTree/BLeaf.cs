using System;
using System.Collections.Generic;
using System.Text;

namespace StringIndexer.DBGen.BinaryTree {
    public class BLeaf {
        public UInt32 FileOffset { get; set; }
        public BTree Tree { get; set; }

        public BLeaf High { get; set; } = null;
        public BLeaf Low { get; set; } = null;

        public StringPositions StringPositions { get; set; } = null;

        public BLeaf this[bool high] {
            get {
                var ret = high ? High : Low;

                if (ret is null) {
                    ret = new BLeaf();
                    this[high] = ret;
                }

                return ret;
            }
            set {
                value.Tree = this.Tree;
                Tree.Leafs.Add(value);
                if (high)
                    High = value;
                else
                    Low = value;
            }
        }

        public UInt32 GetFilePosition() {
            if (this.StringPositions is null)
                return FileOffset;
            return this.StringPositions.FileOffset;
        }
    }
}
