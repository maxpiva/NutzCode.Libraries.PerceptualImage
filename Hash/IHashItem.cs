using System;

namespace NutzCode.Libraries.PerceptualImage.Hash
{
    public interface IHashItem : IComparable
    {
        bool IsDeleted { get; set; }
        int SortDistance { get; set; }
        byte[] ByteArray { get; }
        int CalculateDistance(IHashItem dist);
    }

    public interface IHashItem<T> : IHashItem
    {
        T Id { get; }
    }
}