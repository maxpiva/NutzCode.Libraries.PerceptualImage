using System.Collections.Generic;

namespace NutzCode.Libraries.PerceptualImage.Hash
{
    public class HashList<T> : List<T> where T : IHashItem
    {
        public void CustomSort(int low, int high)
        {
            List<T> list = new List<T>();
            for (int x = low; x <= high; x++)
                list.Add(this[x]);
            list.Sort();
            int cnt = 0;
            for (int x = low; x <= high; x++)
                this[x] = list[cnt++];
        }

        public void CustomSort()
        {
            CustomSort(0, Count - 1);
        }
    }

}