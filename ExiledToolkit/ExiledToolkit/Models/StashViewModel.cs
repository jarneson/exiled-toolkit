using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExiledToolkit.Models
{
    public class StashViewModel
    {
        public Dictionary<String, StashViewTab> Tabs;

        public StashViewModel()
        {
            Tabs = new Dictionary<string, StashViewTab>();
        }
    }

    public class StashViewTab
    {
        public List<ExiledToolkit.Models.ToolkitObjects.Item> Items;

        public StashViewTab()
        {
            Items = new List<ToolkitObjects.Item>();
        }
    }
}