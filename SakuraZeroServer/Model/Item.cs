using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SakuraZeroServer.Model
{
    public class Item
    {
        public int ID { get; private set; }
        public int Count { get; private set; }
        public bool IsDressed { get; private set; }

        public Item(int id, int count, bool isDressed = false)
        {
            ID = id;
            Count = count;
            IsDressed = isDressed;
        }

        public override string ToString()
        {
            int tempIsDressed = IsDressed ? 1 : 0;
            return String.Format("{0}-{1}-{2}", ID.ToString(), Count.ToString(), tempIsDressed.ToString());
        }
    }
}
