using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcContrib.Pagination;
using MvcContrib.UI.Grid;

namespace ExiledToolkit.Models
{
    public class ListViewModel
    {
        public IPagination<ExiledToolkit.Models.ToolkitObjects.Item> ItemPagedList { get; set; }
        public GridSortOptions ItemSorter { get; set; }
    }
}