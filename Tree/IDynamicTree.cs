using NutzCode.Libraries.PerceptualImage.Hash;

namespace NutzCode.Libraries.PerceptualImage.Tree
{
    public interface IDynamicTree<T> : ITree<IHashItem<T>>
    {
        void Add(IHashItem<T> item);
        void Delete(T id);
    }
}