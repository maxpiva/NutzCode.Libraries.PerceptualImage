namespace NutzCode.Libraries.PerceptualImage.Hash
{
    public class HashInfo<T> : IIdentity
    {
        public HashInfo(T id)
        {
            Id = id;
        }

        public T Id { get; set; }
    }
}