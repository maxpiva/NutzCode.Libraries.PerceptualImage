using NutzCode.Libraries.PerceptualImage.Hash;

namespace NutzCode.Libraries.PerceptualImage.VPTree
{
    public class Node
    {
        public IHashItem HashItem { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public int Distance { get; set; }
    }
}