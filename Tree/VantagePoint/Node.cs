namespace NutzCode.Libraries.PerceptualImage.Tree.VantagePoint
{
    public class Node<T>
    {
        public T HashItem { get; set; }
        public Node<T> Left { get; set; }
        public Node<T> Right { get; set; }
        public int Distance { get; set; }
    }
}