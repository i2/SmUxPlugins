using System.Collections.Generic;
using PluginSystem.API;

namespace ItemsExchange.SmPack
{
    public class ItemCollection : List<Item>
    {
        public int GetAllItemsCount()
        {
            int result = 0;

            foreach (Item item in this)
            {
                result++;
                result += item.Subitems.GetAllItemsCount();
            }

            return result;
        }
    }
}