using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnMonHoc.Models;
using Newtonsoft.Json;
using System.IO;

namespace DoAnMonHoc.Areas.Admin.Controllers
{
    public class AdminHomeController : Controller
    {
        DienTuEntities data = new DienTuEntities();
        // GET: Admin/Home
        public ActionResult Index()
        {
            if (Session["TaiKhoanAD"] == null)
            {
                return RedirectToAction("Login", "AdminHome", "Admin");
            }
            List<ThongKe> dataPoints = new List<ThongKe>();
            foreach(var item in data.SanPhams)
            {
                dataPoints.Add(new ThongKe(item.TenSP, (int)item.SoLuongTon));
                
            }
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            return View();
        }
        
        [HttpGet]
        public ActionResult Login()
        {
            Session["TaiKhoanAD"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var user = collection["username"];
            var pass = collection["password"];
            var ad = data.Admins.SingleOrDefault(a => a.UserAdmin == user && a.PassAdmin == pass);
            if (ad != null)
            {
                TempData["admin"] = ad.UserAdmin;
                Session["TaiKhoanAD"] = ad;
                return RedirectToAction("Index", "AdminHome", "Admin");
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu bị sai";
            }
            return View();
        }

        public ActionResult SanPham()
        {
            if (Session["TaiKhoanAD"] == null)
            {
                return RedirectToAction("Login", "AdminHome", "Admin");
            }
            return View(data.SanPhams.ToList());
        }

        [HttpGet]
        public ActionResult ThemMoi()
        {
            if (Session["TaiKhoanAD"] == null)
            {
                return RedirectToAction("Login", "AdminHome", "Admin");
            }
            ViewBag.MaTheLoai = new SelectList(data.TheLoais.ToList().OrderBy(a => a.TenTheLoai), "MaTheLoai", "TenTheLoai");
            ViewBag.MaHang = new SelectList(data.Hangs.ToList().OrderBy(a => a.TenHang), "MaHang", "TenHang");
            return View();
        }

        [HttpPost]
        public ActionResult ThemMoi(SanPham sanPham,HttpPostedFileBase fileupload)
        {
            if (Session["TaiKhoanAD"] == null)
            {
                return RedirectToAction("Login", "AdminHome", "Admin");
            }
            int maSP;
            if (ModelState.IsValid)
            {
                var fileName = Path.GetFileName(fileupload.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                if (System.IO.File.Exists(path))
                {
                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                }
                else
                {
                    fileupload.SaveAs(path);
                }
                sanPham.AnhBia = fileName;
                using(var context = new DienTuEntities())
                {
                    var sanpham = context.Set<SanPham>();
                    if (data.SanPhams.ToList().Count <= 0)
                    {
                        maSP = 0;
                        sanpham.Add(new SanPham
                        {
                            MaSP = maSP,
                            TenSP = sanPham.TenSP,
                            GiaBan = sanPham.GiaBan,
                            MoTa = sanPham.MoTa,
                            AnhBia = sanPham.AnhBia,
                            NgayCapNhat = sanPham.NgayCapNhat,
                            SoLuongTon = sanPham.SoLuongTon,
                            MaTheLoai = sanPham.MaTheLoai,
                            MaHang = sanPham.MaHang
                        });
                        context.SaveChanges();
                    }
                    else
                    {
                        maSP = data.SanPhams.OrderByDescending(a => a.MaSP).ToList()[0].MaSP + 1;
                        sanpham.Add(new SanPham
                        {
                            MaSP = maSP,
                            TenSP = sanPham.TenSP,
                            GiaBan = sanPham.GiaBan,
                            MoTa = sanPham.MoTa,
                            AnhBia = sanPham.AnhBia,
                            NgayCapNhat = sanPham.NgayCapNhat,
                            SoLuongTon = sanPham.SoLuongTon,
                            MaTheLoai = sanPham.MaTheLoai,
                            MaHang = sanPham.MaHang
                        });
                        context.SaveChanges();
                    }
                    
                }
            }
            
            ViewBag.MaTheLoai = new SelectList(data.TheLoais.ToList().OrderBy(a => a.TenTheLoai), "MaTheLoai", "TenTheLoai");
            ViewBag.MaHang = new SelectList(data.Hangs.ToList().OrderBy(a => a.TenHang), "MaHang", "TenHang");
            return RedirectToAction("SanPham");
        }

        public ActionResult ChiTiet(int id)
        {
            if (Session["TaiKhoanAD"] == null)
            {
                return RedirectToAction("Login", "AdminHome", "Admin");
            }
            SanPham sanpham = data.SanPhams.SingleOrDefault(a => a.MaSP == id);
            ViewBag.MaSP = sanpham.MaSP;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }

        [HttpGet]
        public ActionResult Xoa(int id)
        {
            if (Session["TaiKhoanAD"] == null)
            {
                return RedirectToAction("Login", "AdminHome", "Admin");
            }
            SanPham sanpham = data.SanPhams.SingleOrDefault(a => a.MaSP == id);
            ViewBag.MaSP = sanpham.MaSP;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }

        [HttpPost,ActionName("Xoa")]
        public ActionResult XacNhanXoa(int id)
        {
            SanPham sanpham = data.SanPhams.SingleOrDefault(a => a.MaSP == id);
            ViewBag.MaSP = sanpham.MaSP;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            using(var context = new DienTuEntities())
            {
                var xoa = new SanPham { MaSP = sanpham.MaSP };
                context.SanPhams.Attach(xoa);
                context.SanPhams.Remove(xoa);
                context.SaveChanges();
            }
            return RedirectToAction("SanPham");
        }

        [HttpGet]
        public ActionResult Sua(int id)
        {
            ViewBag.MaTheLoai = new SelectList(data.TheLoais.ToList().OrderBy(a => a.TenTheLoai), "MaTheLoai", "TenTheLoai");
            ViewBag.MaHang = new SelectList(data.Hangs.ToList().OrderBy(a => a.TenHang), "MaHang", "TenHang");
            if (Session["TaiKhoanAD"] == null)
            {
                return RedirectToAction("Login", "AdminHome", "Admin");
            }
            SanPham sanpham = data.SanPhams.SingleOrDefault(a => a.MaSP == id);
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            else
            {
                return View(sanpham);
            }
        }

        [HttpPost]
        public ActionResult Suah(int id,HttpPostedFileBase fileupload)
        {
            ViewBag.MaTheLoai = new SelectList(data.TheLoais.ToList().OrderBy(a => a.TenTheLoai), "MaTheLoai", "TenTheLoai");
            ViewBag.MaHang = new SelectList(data.Hangs.ToList().OrderBy(a => a.TenHang), "MaHang", "TenHang");
            if (Session["TaiKhoanAD"] == null)
            {
                return RedirectToAction("Login", "AdminHome", "Admin");
            }
            
            return RedirectToAction("SanPham");
        }
    }
}