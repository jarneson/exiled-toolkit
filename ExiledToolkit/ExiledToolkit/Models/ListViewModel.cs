using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcContrib.Pagination;

namespace ExiledToolkit.Models
{
    public class ListViewModel
    {
        public List<ExiledToolkit.Models.ToolkitObjects.Item> ItemPagedList { get; set; }
    }
}