using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace ExiledToolkit.Controllers
{
    public class StashController : Controller
    {
        private String StashItemListVar = "Stash";
        private String StashJsonVar = "StashJson";

        public ActionResult Index()
        {
            List<ExiledToolkit.Models.ToolkitObjects.Item> lItemList = null;
            if (Session[StashItemListVar] != null)
            {
                lItemList = (List<ExiledToolkit.Models.ToolkitObjects.Item>)Session[StashItemListVar];
            }
            else
            {
                lItemList = new List<ExiledToolkit.Models.ToolkitObjects.Item>();
            }
            return View(lItemList);
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

        public ActionResult List()
        {
            return View();
        }
    }
}
