namespace NutzCode.Libraries.PerceptualImage.Hash
{
    public class Hash256Item<T> : IHashItem<T>
    {
        public Hash256Item(T id, byte[] hash)
        {
            Id = id;
            Hash1 = ((ulong) hash[0] << 56) | ((ulong) hash[1] << 48) | ((ulong) hash[2] << 40) | ((ulong) hash[3] << 32) | ((ulong) hash[4] << 24) | ((ulong) hash[5] << 16) | ((ulong) hash[6] << 8) | hash[7];
            Hash2 = ((ulong) hash[8] << 56) | ((ulong) hash[9] << 48) | ((ulong) hash[10] << 40) | ((ulong) hash[11] << 32) | ((ulong) hash[12] << 24) | ((ulong) hash[13] << 16) | ((ulong) hash[14] << 8) | hash[15];
            Hash3 = ((ulong) hash[16] << 56) | ((ulong) hash[17] << 48) | ((ulong) hash[18] << 40) | ((ulong) hash[19] << 32) | ((ulong) hash[20] << 24) | ((ulong) hash[21] << 16) | ((ulong) hash[22] << 8) | hash[23];
            Hash4 = ((ulong) hash[24] << 56) | ((ulong) hash[25] << 48) | ((ulong) hash[26] << 40) | ((ulong) hash[27] << 32) | ((ulong) hash[28] << 24) | ((ulong) hash[29] << 16) | ((ulong) hash[30] << 8) | hash[31];
        }

        public Hash256Item(T id, ulong[] hash)
        {
            Id = id;
            Hash1 = hash[0];
            Hash2 = hash[1];
            Hash3 = hash[2];
            Hash4 = hash[3];
        }

        public Hash256Item(T identity, ulong hash1, ulong hash2, ulong hash3, ulong hash4)
        {
            Id = identity;
            Hash1 = hash1;
            Hash2 = hash2;
            Hash3 = hash3;
            Hash4 = hash4;
        }

        public ulong Hash1 { get; internal set; }
        public ulong Hash2 { get; internal set; }
        public ulong Hash3 { get; internal set; }
        public ulong Hash4 { get; internal set; }

        public ulong[] Hashes
        {
            get
            {
                ulong[] b = new ulong[4];
                b[0] = Hash1;
                b[1] = Hash2;
                b[2] = Hash3;
                b[3] = Hash4;
                return b;
            }
        }

        public T Id { get; private set; }

        public int SortDistance { get; set; }

        public bool IsDeleted { get; set; }

        public byte[] ByteArray
        {
            get
            {
                byte[] b = new byte[32];
                ulong[] hs = Hashes;
                for (int x = 0; x < 4; x++)
                {
                    int p = (x << 3) + 7;
                    ulong hash = hs[x];
                    b[p--] = (byte) (hash & 0xff);
                    hash >>= 8;
                    b[p--] = (byte) (hash & 0xff);
                    hash >>= 8;
                    b[p--] = (byte) (hash & 0xff);
                    hash >>= 8;
                    b[p--] = (byte) (hash & 0xff);
                    hash >>= 8;
                    b[p--] = (byte) (hash & 0xff);
                    hash >>= 8;
                    b[p--] = (byte) (hash & 0xff);
                    hash >>= 8;
                    b[p--] = (byte) (hash & 0xff);
                    hash >>= 8;
                    b[p] = (byte) (hash & 0xff);
                }

                return b;
            }
        }

        public int CalculateDistance(IHashItem dist)
        {
            ulong x = ((Hash256Item<T>) dist).Hash1 ^ Hash1;
            ulong y = ((Hash256Item<T>) dist).Hash2 ^ Hash2;
            ulong u = ((Hash256Item<T>) dist).Hash3 ^ Hash3;
            ulong v = ((Hash256Item<T>) dist).Hash4 ^ Hash4;
            ulong m1 = 0x5555555555555555;
            ulong m2 = 0x3333333333333333;
            ulong m3 = 0x0F0F0F0F0F0F0F0F;
            ulong m4 = 0x000000FF000000FF;
            x = x - ((x >> 1) & m1);
            y = y - ((y >> 1) & m1);
            u = u - ((u >> 1) & m1);
            v = v - ((v >> 1) & m1);
            x = (x & m2) + ((x >> 2) & m2);
            y = (y & m2) + ((y >> 2) & m2);
            u = (u & m2) + ((u >> 2) & m2);
            v = (v & m2) + ((v >> 2) & m2);
            x = x + y;
            u = u + v;
            x = (x & m3) + ((x >> 4) & m3);
            u = (u & m3) + ((u >> 4) & m3);
            x = x + u;
            x = x + (x >> 8);
            x = x + (x >> 16);
            x = x & m4;
            x = x + (x >> 32);
            return (int) (x & 0x000001FF);
        }

        /*
                public int CalculateDistance(IHashItem dist)
        {

            ulong x = ((Hash256Item) dist).Hash1 ^ Hash1;
            ulong w = ((Hash256Item) dist).Hash2 ^ Hash2;
            ulong y = ((Hash256Item) dist).Hash3 ^ Hash3;
            ulong z = ((Hash256Item) dist).Hash4 ^ Hash4;

            x -= (x >> 1) & 0x5555555555555555;
            w -= (w >> 1) & 0x5555555555555555;
            y -= (y >> 1) & 0x5555555555555555;
            z -= (z >> 1) & 0x5555555555555555;
            x = (x & 0x3333333333333333) + ((x >> 2) & 0x3333333333333333);
            w = (w & 0x3333333333333333) + ((w >> 2) & 0x3333333333333333);
            y = (y & 0x3333333333333333) + ((y >> 2) & 0x3333333333333333);
            z = (z & 0x3333333333333333) + ((z >> 2) & 0x3333333333333333);
            x = (x + (x >> 4)) & 0x0f0f0f0f0f0f0f0f;
            w = (w + (w >> 4)) & 0x0f0f0f0f0f0f0f0f;
            y = (y + (y >> 4)) & 0x0f0f0f0f0f0f0f0f;
            z = (z + (z >> 4)) & 0x0f0f0f0f0f0f0f0f;
            x += x >> 8;
            w += w >> 8;
            y += y >> 8;
            z += z >> 8;
            x += x >> 16;
            w += w >> 16;
            y += y >> 16;
            z += z >> 16;
            x += x >> 32;
            w += w >> 32;
            y += y >> 32;
            z += z >> 32;
            return (int) (x & 0x7f) + (int) (w & 0x7f) + (int) (y & 0x7f) + (int) (z & 0x7f);

        }*/
        public int CompareTo(object obj)
        {
            Hash256Item<T> b = (Hash256Item<T>) obj;
            return SortDistance.CompareTo(b.SortDistance);
        }
    }
}