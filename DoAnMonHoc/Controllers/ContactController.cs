using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnMonHoc.Models;

namespace DoAnMonHoc.Controllers
{
    public class ContactController : Controller
    {
        DienTuEntities data = new DienTuEntities();
        // GET: Contact
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            string ten = collection["name"];
            string email = collection["email"];
            string sodienthoai = collection["subject"];
            string ykien = collection["message"];
            using(var context = new DienTuEntities())
            {
                var DongGop = context.Set<PhanHoi>();
                int MaPhanHois;
                if (data.PhanHois.ToList().Count <= 0)
                {
                    MaPhanHois = 0;
                }
                else
                {
                    MaPhanHois = data.PhanHois.OrderByDescending(a => a.MaPhanHoi).ToList()[0].MaPhanHoi + 1;
                }
                DongGop.Add(new PhanHoi
                {
                    MaPhanHoi = MaPhanHois,
                    TenKH = ten,
                    Email = email,
                    DienThoaiKH = sodienthoai,
                    NoiDung = ykien,
                    NgayDang = DateTime.Now
                });
                context.SaveChanges();
            }
            ViewBag.ThongBao = "Ý kiến phản hồi của quý khách về dịch vụ của chúng tôi đã được gửi đi. Chân thành cảm ơn những đóng góp của quý khách để chúng tôi có thể phục vụ quý khách tốt hơn nữa. Xin chân thành cảm ơn";
            return this.Index();
        }
        
    }
}