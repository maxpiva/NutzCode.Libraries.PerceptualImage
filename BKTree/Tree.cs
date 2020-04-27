using System.Linq;
using NutzCode.Libraries.PerceptualImage.Hash;

namespace NutzCode.Libraries.PerceptualImage.BKTree
{
    public class Tree : ITree
    {
        private Node _root;

        public Tree(HashList co)
        {
            co.ForEach(Add);
        }

        public HashList Range(IHashItem item, int d)
        {
            HashList rtn = new HashList();
            RecursiveSearch(_root, rtn, item, d);
            rtn.CustomSort(0, rtn.Count - 1);
            return rtn;
        }

        public void Add(IHashItem item)
        {
            if (_root == null)
            {
                _root = new Node(item);
                return;
            }

            var curNode = _root;

            var dist = curNode.HashItem.CalculateDistance(item);
            while (curNode.ContainsKey(dist))
            {
                if (dist == 0) return;

                curNode = curNode[dist];
                dist = curNode.HashItem.CalculateDistance(item);
            }

            curNode.AddChild(dist, item);
        }


        private void RecursiveSearch(Node node, HashList rtn, IHashItem item, int d)
        {
            var curDist = node.HashItem.CalculateDistance(item);
            var minDist = curDist - d;
            var maxDist = curDist + d;

            if (curDist <= d)
                rtn.Add(node.HashItem);

            foreach (var key in node.Keys.Cast<int>().Where(key => minDist <= key && key <= maxDist))
            {
                RecursiveSearch(node[key], rtn, item, d);
            }
        }
    }
}