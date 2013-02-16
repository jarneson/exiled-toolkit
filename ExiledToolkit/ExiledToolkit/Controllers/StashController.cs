using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using MvcContrib.UI.Grid;
using MvcContrib.Pagination;
using MvcContrib.Sorting;

namespace ExiledToolkit.Controllers
{
    public class StashController : Controller
    {
        private String StashItemListVar = "Stash";
        private String StashJsonVar = "StashJson";

        public ActionResult Index()
        {
            ExiledToolkit.Models.StashViewModel lModel = null;
            if (Session[StashItemListVar] != null)
            {
                List<ExiledToolkit.Models.ToolkitObjects.Item> lItemList = (List<ExiledToolkit.Models.ToolkitObjects.Item>)Session[StashItemListVar];
                lModel = new Models.StashViewModel();

                foreach (ExiledToolkit.Models.ToolkitObjects.Item item in lItemList)
                {
                    if (!lModel.Tabs.ContainsKey(item.BaseType))
                    {
                        lModel.Tabs[item.BaseType] = new Models.StashViewTab();
                    }
                    lModel.Tabs[item.BaseType].Items.Add(item);
                }
            }
            else
            {
                return RedirectToAction("LoadStash");
            }
            return View(lModel);
        }

        public ActionResult LoadStash()
        {
            return View();
        }

        public ActionResult CurrentJson()
        {
            String lJson = String.Empty;

            if (Session[StashJsonVar] != null)
            {
                lJson = (String)Session[StashJsonVar];
            }

            return View((object)lJson);
        }

        [HttpPost]
        public ActionResult SubmitJson(String inJson)
        {
            try
            {
                Session[StashJsonVar] = inJson;

                List<ExiledToolkit.Models.PathOfExileObjects.TempJson> thing = JsonConvert.DeserializeObject<List<ExiledToolkit.Models.PathOfExileObjects.TempJson>>(inJson);
                List<ExiledToolkit.Models.ToolkitObjects.Item> items = new List<Models.ToolkitObjects.Item>();
                thing.ForEach(t => t.items.ForEach(i => items.Add(new Models.ToolkitObjects.Item(i))));
                Session[StashItemListVar] = items;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Session[StashJsonVar] = null;
                Session[StashItemListVar] = null;
                return RedirectToAction("LoadStash");
            }

        }

        public ActionResult List(String BaseType,
                                 GridSortOptions sort, 
                                 int? page)
        {
            if (Session[StashItemListVar] != null)
            {
                List<ExiledToolkit.Models.ToolkitObjects.Item> lItemList = (List<ExiledToolkit.Models.ToolkitObjects.Item>)Session[StashItemListVar];
                ExiledToolkit.Models.ListViewModel model = new Models.ListViewModel();
                model.ItemPagedList = lItemList.Where(it => it.BaseType == BaseType).OrderBy(sort.Column, sort.Direction).ToList();
                return View(model);
            }
            return View();
        }
    }
}
