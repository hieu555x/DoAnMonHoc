using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnMonHoc.Models;

namespace DoAnMonHoc.Controllers
{
    public class UserController : Controller
    {
        DienTuEntities data = new DienTuEntities();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(FormCollection collection)
        {
            if (!collection["MatKhau"].Equals(collection["NhapLai"]))
            {
                ViewData["Loi1"] = "Nhập Lại Mật Khẩu Không Khớp Với Mật Khẩu";
            }
            else
            {
                using (var context = new DienTuEntities())
                {
                    string TenDN = collection["TenDN"];
                    if (context.KhachHangs.Any(a => a.TaiKhoan == TenDN) == true)    
                    {
                        ViewBag.ThongBao = "Tên Đăng Nhập Đã Tồn Tại Vui Lòng Chọn Tên Đăng Nhập Khác";
                    }
                    else
                    {
                        var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);
                        var account = context.Set<KhachHang>();
                        int maKH;
                        if (data.KhachHangs.ToList().Count <= 0)
                        {
                            maKH = 0;
                            account.Add(new KhachHang
                            {
                                MaKH = maKH,
                                HoTen = collection["HoTenKH"],
                                TaiKhoan = collection["TenDN"],
                                MatKhau = collection["MatKhau"],
                                Email = collection["Email"],
                                DiaChiKH = collection["DiaChi"],
                                DienThoaiKH = collection["DienThoai"],
                                NgaySinh = DateTime.Parse(ngaysinh),
                            });
                            context.SaveChanges();
                        }
                        else
                        {
                            maKH = data.KhachHangs.OrderByDescending(a => a.MaKH).ToList()[0].MaKH + 1;
                            account.Add(new KhachHang
                            {
                                MaKH = maKH,
                                HoTen = collection["HoTenKH"],
                                TaiKhoan = collection["TenDN"],
                                MatKhau = collection["MatKhau"],
                                Email = collection["Email"],
                                DiaChiKH = collection["DiaChi"],
                                DienThoaiKH = collection["DienThoai"],
                                NgaySinh = DateTime.Parse(ngaysinh),
                            });
                            context.SaveChanges();
                        }
                        
                        return RedirectToAction("DangNhap");
                    }
                }
            }
            return this.DangKy();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            string TenDN = collection["TenDN"], MatKhau = collection["MatKhau"];
            var kh = data.KhachHangs.SingleOrDefault(a => a.TaiKhoan.Equals(TenDN) && a.MatKhau.Equals(MatKhau));
            if (kh != null)
            {
                //ViewBag.ThongBao = "Chúc Mừng Đăng Nhập Thành Công";
                TempData["Ten"] = kh.HoTen;
                Session["TaiKhoan"] = kh;
                return RedirectToAction("Index", "Cart");
            }
            else
            {
                ViewBag.ThongBao = "Tên Đăng Nhập Hoặc Mật Khẩu Không Đúng";
            }
            return View();
        }

        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            TempData["Ten"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}