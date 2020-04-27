using NutzCode.Libraries.PerceptualImage.Hash;

namespace NutzCode.Libraries.PerceptualImage
{
    public interface ITree
    {
        HashList Range(IHashItem query, int range);
    }
}