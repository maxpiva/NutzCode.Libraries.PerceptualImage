namespace NutzCode.Libraries.PerceptualImage.Hash
{
    public class Hash64Item : IHashItem
    {
        public Hash64Item(IIdentity id, byte[] hash)
        {
            Identity = id;
            Hash = ((ulong) hash[0] << 56) | ((ulong) hash[1] << 48) | ((ulong) hash[2] << 40) | ((ulong) hash[3] << 32) | ((ulong) hash[4] << 24) | ((ulong) hash[5] << 16) | ((ulong) hash[6] << 8) | hash[7];
        }

        public Hash64Item(IIdentity id, ulong hash)
        {
            Identity = id;
            Hash = hash;
        }

        public ulong Hash { get; internal set; }
        public IIdentity Identity { get; set; }
        public int SortDistance { get; set; }

        public int CalculateDistance(IHashItem dist)
        {
            ulong x = ((Hash64Item) dist).Hash ^ Hash;
            x -= (x >> 1) & 0x5555555555555555;
            x = (x & 0x3333333333333333) + ((x >> 2) & 0x3333333333333333);
            x = (x + (x >> 4)) & 0x0f0f0f0f0f0f0f0f;
            x += x >> 8;
            x += x >> 16;
            x += x >> 32;
            return (int) (x & 0x7f);
        }

        public byte[] ByteArray
        {
            get
            {
                ulong hash = Hash;
                byte[] b = new byte[8];
                b[7] = (byte) (hash & 0xff);
                hash >>= 8;
                b[6] = (byte) (hash & 0xff);
                hash >>= 8;
                b[5] = (byte) (hash & 0xff);
                hash >>= 8;
                b[4] = (byte) (hash & 0xff);
                hash >>= 8;
                b[3] = (byte) (hash & 0xff);
                hash >>= 8;
                b[2] = (byte) (hash & 0xff);
                hash >>= 8;
                b[1] = (byte) (hash & 0xff);
                hash >>= 8;
                b[0] = (byte) (hash & 0xff);
                return b;
            }
        }

        public int CompareTo(object obj)
        {
            Hash64Item b = (Hash64Item) obj;
            return SortDistance.CompareTo(b.SortDistance);
        }
    }
}