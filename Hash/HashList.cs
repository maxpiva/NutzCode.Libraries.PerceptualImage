using System.Collections.Generic;

namespace NutzCode.Libraries.PerceptualImage.Hash
{
    public class HashList : List<IHashItem>
    {
        public void CustomSort(int low, int high)
        {
            List<IHashItem> list = new List<IHashItem>();
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