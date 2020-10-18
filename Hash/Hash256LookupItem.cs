namespace NutzCode.Libraries.PerceptualImage.Hash
{
    public class Hash256LookUpItem<T> : IHashItem<T>
    {
        private static readonly byte[] Table = new byte[65536];

        static Hash256LookUpItem()
        {
            for (int x = 0; x < 65535; x++)
            {
                int count = 0;
                int value = x;
                while (value > 0)
                {
                    if ((value & 1) == 1)
                        count++;
                    value >>= 1;
                }

                Table[x] = (byte) count;
            }
        }

        public Hash256LookUpItem(T id, byte[] hash)
        {
            Id = id;
            Hashes[0] = (ushort) ((hash[0] << 8) | hash[1]);
            Hashes[1] = (ushort) ((hash[2] << 8) | hash[3]);
            Hashes[2] = (ushort) ((hash[4] << 8) | hash[5]);
            Hashes[3] = (ushort) ((hash[6] << 8) | hash[7]);
            Hashes[4] = (ushort) ((hash[8] << 8) | hash[9]);
            Hashes[5] = (ushort) ((hash[10] << 8) | hash[11]);
            Hashes[6] = (ushort) ((hash[12] << 8) | hash[13]);
            Hashes[7] = (ushort) ((hash[14] << 8) | hash[15]);
            Hashes[8] = (ushort) ((hash[16] << 8) | hash[17]);
            Hashes[9] = (ushort) ((hash[18] << 8) | hash[19]);
            Hashes[10] = (ushort) ((hash[20] << 8) | hash[21]);
            Hashes[11] = (ushort) ((hash[22] << 8) | hash[23]);
            Hashes[12] = (ushort) ((hash[24] << 8) | hash[25]);
            Hashes[13] = (ushort) ((hash[26] << 8) | hash[27]);
            Hashes[14] = (ushort) ((hash[28] << 8) | hash[29]);
            Hashes[15] = (ushort) ((hash[30] << 8) | hash[31]);
        }

        public ushort[] Hashes { get; internal set; } = new ushort[16];
        public T Id { get; }
        public int SortDistance { get; set; }
        public bool IsDeleted { get; set; }

        public byte[] ByteArray
        {
            get
            {
                byte[] b = new byte[32];
                for (int x = 0; x < 16; x++)
                {
                    int p = (x << 1) + 1;
                    ushort hash = Hashes[x];
                    b[p--] = (byte) (hash & 0xff);
                    hash >>= 8;
                    b[p] = (byte) (hash & 0xff);
                }

                return b;
            }
        }

        public int CalculateDistance(IHashItem dist)
        {
            Hash256LookUpItem<T> l = (Hash256LookUpItem<T>) dist;
            int val = Table[l.Hashes[0] ^ Hashes[0]];
            val += Table[l.Hashes[1] ^ Hashes[1]];
            val += Table[l.Hashes[2] ^ Hashes[2]];
            val += Table[l.Hashes[3] ^ Hashes[3]];
            val += Table[l.Hashes[4] ^ Hashes[4]];
            val += Table[l.Hashes[5] ^ Hashes[5]];
            val += Table[l.Hashes[6] ^ Hashes[6]];
            val += Table[l.Hashes[7] ^ Hashes[7]];
            val += Table[l.Hashes[8] ^ Hashes[8]];
            val += Table[l.Hashes[9] ^ Hashes[9]];
            val += Table[l.Hashes[10] ^ Hashes[10]];
            val += Table[l.Hashes[11] ^ Hashes[11]];
            val += Table[l.Hashes[12] ^ Hashes[12]];
            val += Table[l.Hashes[13] ^ Hashes[13]];
            val += Table[l.Hashes[14] ^ Hashes[14]];
            val += Table[l.Hashes[15] ^ Hashes[15]];
            return val;
        }

        public int CompareTo(object obj)
        {
            Hash256LookUpItem<T> b = (Hash256LookUpItem<T>) obj;
            return SortDistance.CompareTo(b.SortDistance);
        }
    }
}