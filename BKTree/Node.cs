using System.Collections;
using System.Collections.Specialized;
using NutzCode.Libraries.PerceptualImage.Hash;

namespace NutzCode.Libraries.PerceptualImage.BKTree
{
    public class Node
    {
        public Node()
        {
        }

        public Node(IHashItem item)
        {
            HashItem = item;
        }

        public IHashItem HashItem { get; set; }
        public HybridDictionary Children { get; set; }

        public Node this[int key] => (Node) Children[key];

        public ICollection Keys
        {
            get
            {
                if (Children == null)
                    return new ArrayList();
                return Children.Keys;
            }
        }

        public bool ContainsKey(int key)
        {
            return Children != null && Children.Contains(key);
        }

        public void AddChild(int key, IHashItem item)
        {
            if (Children == null)
                Children = new HybridDictionary();
            Children[key] = new Node(item);
        }
    }
}