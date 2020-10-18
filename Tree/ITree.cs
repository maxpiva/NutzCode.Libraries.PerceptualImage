using NutzCode.Libraries.PerceptualImage.Hash;

namespace NutzCode.Libraries.PerceptualImage.Tree
{
    public interface ITree<T> where T : IHashItem
    {
        HashList<T> Range(T query, int range);
    }
}