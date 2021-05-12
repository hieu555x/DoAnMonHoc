using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnMonHoc.Models;
using PagedList;
using PagedList.Mvc;

namespace DoAnMonHoc.Controllers
{
    public class ShopController : Controller
    {
        DienTuEntities data = new DienTuEntities();
        // GET: Shop
        public ActionResult Index(int ? page)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);
            return View(data.SanPhams.OrderByDescending(a=>a.NgayCapNhat).ToPagedList(pageNum,pageSize));
        }

        public ActionResult SPTheoLoai(int id_TheLoai,int id_Hang,int ? page)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);
            return View(data.SanPhams.OrderByDescending(a => a.NgayCapNhat).Where(a => a.MaTheLoai == id_TheLoai && a.MaHang == id_Hang).ToPagedList(pageNum, pageSize));
        }

        public ActionResult ChiTiet(int id)
        {
            return View(data.SanPhams.SingleOrDefault(a => a.MaSP == id));
        }

        public ActionResult SPTheoTheLoai(int id,int ? page)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);
            return View(data.SanPhams.OrderByDescending(a => a.NgayCapNhat).Where(a => a.MaTheLoai == id).ToPagedList(pageNum, pageSize));
        }
    }
}