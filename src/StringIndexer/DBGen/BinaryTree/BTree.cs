using System;
using System.Collections.Generic;
using System.Text;

namespace StringIndexer.DBGen.BinaryTree {
    public class BTree {
        public BLeaf Root { get; set; }

        public HashSet<BLeaf> Leafs { get; private set; } = new HashSet<BLeaf>();

        public BTree() {
            Root = new BLeaf();
            Root.Tree = this;
            Leafs.Add(Root);
        }
    }
}
