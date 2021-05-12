using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnMonHoc.Models;

namespace DoAnMonHoc.Controllers
{
    public class HomeController : Controller
    {
        DienTuEntities data = new DienTuEntities();
        public ActionResult Index()
        {
            return View(data.SanPhams.OrderByDescending(a => a.NgayCapNhat));
        }
    }
}