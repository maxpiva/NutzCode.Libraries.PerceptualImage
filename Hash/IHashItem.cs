using System;

namespace NutzCode.Libraries.PerceptualImage.Hash
{
    public interface IHashItem : IComparable
    {
        IIdentity Identity { get; set; }
        int SortDistance { get; set; }
        byte[] ByteArray { get; }
        int CalculateDistance(IHashItem dist);
    }
}