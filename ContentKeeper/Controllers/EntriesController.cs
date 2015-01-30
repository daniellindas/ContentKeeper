using System.Web.Mvc;
using ContentKeeperService;

namespace ContentKeeper.Controllers
{
    public class EntriesController : Controller
    {
        private readonly Service _service = new Service();

        public JsonResult ListAll()
        {
            var entries = _service.ListUsers();

            return Json(entries, JsonRequestBehavior.AllowGet);
        }

    }
}
