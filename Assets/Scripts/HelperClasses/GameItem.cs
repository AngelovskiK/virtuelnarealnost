using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.HelperClasses
{
    public class GameItem
    {
        public string ItemName { get; private set; }
        public int Id { get; private set; }

        public GameItem(string itemName, int id)
        {
            Id = id;
            ItemName = itemName;
        }

        public override bool Equals(object obj)
        {
            return obj is GameItem item &&
                   Id == item.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }
    }
}
