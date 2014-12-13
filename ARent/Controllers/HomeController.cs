using ARent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ARent.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchByZipCode(ARent.Models.Address address)
        {
            Task<IList<HouseEntity>> query = ARent.DAO.HouseDAO.GetInformationByZipCode(address.Zip);
            return Json(query.Result);
        }
    }
}