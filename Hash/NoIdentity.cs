using System;

namespace NutzCode.Libraries.PerceptualImage.Hash
{
    public class NoIdentity : IIdentity
    {
    }

    public class GuidIdentity : IIdentity
    {
        public GuidIdentity(Guid uid)
        {
            Guid = uid;
        }

        public Guid Guid { get; }
    }
    public class IntIdentity : IIdentity
    {
        public IntIdentity(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
    public class StringIdentity : IIdentity
    {
        public StringIdentity(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}