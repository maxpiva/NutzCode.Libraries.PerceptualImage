using System.Collections.Generic;
using System.Linq;
using NutzCode.Libraries.PerceptualImage.Hash;

// ReSharper disable VirtualMemberCallInConstructor
// ReSharper disable PossibleStructMemberModificationOfNonVariableStruct

namespace NutzCode.Libraries.PerceptualImage.Tree.VantagePoint
{
    public class DynamicVPTree<T, S> : VPTree<T> where T : IHashItem<S>
    {
        private readonly int _autogrow;
        private int _degrow;
        private int _grow;
        internal Dictionary<S, T> _lookup;
        internal HashList<T> Secondary = new HashList<T>();

        public DynamicVPTree(HashList<T> cols, int autogrow = 100)
        {
            _autogrow = autogrow;
            Populate(cols);
        }

        public DynamicVPTree(int autogrow = 100)
        {
            _autogrow = autogrow;
            _items = new HashList<T>();
        }

        public sealed override void Populate(HashList<T> cols)
        {
            base.Populate(cols);
            _lookup = cols.ToDictionary(a => a.Id, a => a);
        }

        public override HashList<T> Range(T query, int range)
        {
            HashList<T> ls = new HashList<T>();
            HashList<T> r = base.Range(query, range);
            ls.AddRange(r.Where(a => !a.IsDeleted));
            ls.AddRange(Secondary.Where(a => !a.IsDeleted && a.CalculateDistance(query) < range));
            ls.CustomSort(0, ls.Count - 1);
            return ls;
        }


        public void Add(T item)
        {
            Secondary.Add(item);
            _lookup.Add(item.Id, item);
            _grow++;
            if (_grow > _autogrow)
                Rebuild();
        }

        private void Rebuild()
        {
            HashList<T> l = new HashList<T>();
            l.AddRange(_items.Where(a => !a.IsDeleted));
            l.AddRange(Secondary.Where(a => !a.IsDeleted));
            Secondary.Clear();
            Populate(l);
            _grow = 0;
            _degrow = 0;
        }

        public void Delete(S id)
        {
            if (_lookup.ContainsKey(id))
            {
                _lookup[id].IsDeleted = true;
                _degrow++;
                if (_degrow > _autogrow)
                    Rebuild();
            }
        }
    }
}