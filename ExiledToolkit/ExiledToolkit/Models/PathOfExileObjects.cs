using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExiledToolkit.Models
{
    namespace PathOfExileObjects
    {
        public class StashTab
        {
            public List<Item> items;
            public int numTabs;
        }

        public class Item
        {
            public bool verified;
            public int w;
            public int h;
            public String icon;
            public bool support;
            public String league;
            public List<Object> sockets;
            public String name;
            public String typeLine;
            public bool identified;
            public List<Property> properties;
            public List<Property> requirements;
            public List<String> implicitMods;
            public List<String> explicitMods;
            public String descrText;
            public int frameType;
            public int x;
            public int y;
            public String inventoryId;
            public List<Item> socketedItems;
        }

        public class Property
        {
            public String name;
            public List<String[]> values;
            public int displayMode;
        }
    }
}