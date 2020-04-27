using System;
using NutzCode.Libraries.PerceptualImage.Hash;
// ReSharper disable VirtualMemberCallInConstructor

namespace NutzCode.Libraries.PerceptualImage.VPTree
{
    public class Tree : ITree
    {
        private HashList _items;
        private readonly Random generator = new Random();

        public Tree(HashList cols)
        {
            Populate(cols);
        }

        public Tree()
        {
        }

        public Node Root { get; internal set; }

        public virtual HashList Range(IHashItem query, int range)
        {
            HashList r = RangeTraversal(query, range, Root, new HashList());
            r.CustomSort(0, r.Count - 1);
            return r;
        }

        public virtual void Populate(HashList cols)
        {
            _items = cols;
            Root = AddNode(0, cols.Count - 1);
        }

        private Node AddNode(int begin, int end)
        {
            if (begin == end)
                return null;
            Node node = new Node();
            node.HashItem = _items[begin];
            if (end - begin > 1)
            {
                int i = generator.Next(end - begin) + begin;
                IHashItem tmp = _items[begin];
                _items[begin] = _items[i];
                _items[i] = tmp;
                int median = (end + begin) / 2;
                node.HashItem = _items[begin];
                CalculateDistances(node, begin + 1, end);
                OrderDistances(begin + 1, end);
                node.Distance = _items[begin].CalculateDistance(_items[median]);
                node.Left = AddNode(begin + 1, median);
                node.Right = AddNode(median, end);
            }

            return node;
        }

        private void CalculateDistances(Node pivot, int begin, int end)
        {
            for (int i = begin; i <= end; i++)
                _items[i].SortDistance = pivot.HashItem == _items[i] ? 0 : pivot.HashItem.CalculateDistance(_items[i]);
        }

        private void OrderDistances(int begin, int end)
        {
            _items.CustomSort(begin, end);
        }

        private HashList RangeTraversal(IHashItem query, int range, Node tNode, HashList results)
        {
            if (tNode != null)
            {
                int sortdistance = query.CalculateDistance(tNode.HashItem);
                if (sortdistance < range)
                {
                    results.Add(tNode.HashItem);
                    tNode.HashItem.SortDistance = sortdistance;
                }

                if (sortdistance + range < tNode.Distance)
                    RangeTraversal(query, range, tNode.Left, results);
                else if (sortdistance - range > tNode.Distance)
                    RangeTraversal(query, range, tNode.Right, results);
                else
                {
                    RangeTraversal(query, range, tNode.Left, results);
                    RangeTraversal(query, range, tNode.Right, results);
                }
            }

            return results;
        }
    }
}