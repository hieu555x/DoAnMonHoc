using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnMonHoc.Models;

namespace DoAnMonHoc.Controllers
{
    public class CartController : Controller
    {
        DienTuEntities data = new DienTuEntities();

        public List<Cart> LayGioHang()
        {
            List<Cart> GioHang_list = Session["GioHang"] as List<Cart>;
            if (GioHang_list == null)
            {
                GioHang_list = new List<Cart>();
                Session["GioHang"] = GioHang_list;
            }
            return GioHang_list;
        }

        public ActionResult ThemGioHang(int MaSP_cart,string strUrl)
        {
            List<Cart> GioHang_list = LayGioHang();
            Cart SanPham = GioHang_list.Find(a => a.MaSP_cart == MaSP_cart);
            if (SanPham == null)
            {
                SanPham = new Cart(MaSP_cart);
                GioHang_list.Add(SanPham);
                return Redirect(strUrl);
            }
            else
            {
                SanPham.SoLuong_cart++;
                return Redirect(strUrl);
            }
        }

        private double TongTien()
        {
            double TongTien_cart = 0;
            List<Cart> GioHang_list = Session["GioHang"] as List<Cart>;
            if (GioHang_list != null)
            {
                TongTien_cart = GioHang_list.Sum(a => a.ThanhTien_cart);
            }
            return TongTien_cart;
        }

        private int TongSoLuong()
        {
            int TongSoLuong_cart = 0;
            List<Cart> GioHang_list = Session["GioHang"] as List<Cart>;
            if (GioHang_list != null)
            {
                TongSoLuong_cart = GioHang_list.Sum(a => a.SoLuong_cart);
            }
            return TongSoLuong_cart;
        }

        // GET: Cart
        public ActionResult Index()
        {
            List<Cart> GioHang_list = LayGioHang();
            if (GioHang_list.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(GioHang_list);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            return PartialView();
        }

        public ActionResult XoaGioHang(int id)
        {
            List<Cart> GioHang_list = LayGioHang();
            Cart SanPham = GioHang_list.SingleOrDefault(a => a.MaSP_cart == id);
            if (SanPham != null)
            {
                GioHang_list.RemoveAll(a => a.MaSP_cart == id);
                return RedirectToAction("Index");
            }
            if (GioHang_list.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index");
        }

        public ActionResult CapNhatGioHang(int MaSP,FormCollection collection)
        {
            List<Cart> GioHang_list = LayGioHang();
            Cart SanPham = GioHang_list.SingleOrDefault(a => a.MaSP_cart == MaSP);
            if (SanPham != null)
            {
                SanPham.SoLuong_cart = int.Parse(collection["txtSoLuong"].ToString());
            }
            return RedirectToAction("Index");
        }

        public ActionResult XoaTatCaGioHang()
        {
            List<Cart> GioHang_list = LayGioHang();
            GioHang_list.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("KhongThongTin", "Cart");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Shop");
            }
            List<Cart> GioHang_list = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(GioHang_list);
        }

        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            DonDatHang ddh = new DonDatHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            List<Cart> gh = LayGioHang();
            if (data.DonDatHangs.ToList().Count<=0)
            {
                ddh.SoDH = 0;
            }
            else
            {
                ddh.SoDH = data.DonDatHangs.OrderByDescending(a => a.SoDH).ToList()[0].SoDH + 1;
            }
            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.NgayGiao = DateTime.Parse(String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]));
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            using(var context = new DienTuEntities())
            {
                var DonDatHang = context.Set<DonDatHang>();
                DonDatHang.Add(new DonDatHang
                {
                    SoDH = ddh.SoDH,
                    MaKH = ddh.MaKH,
                    NgayDat = ddh.NgayDat,
                    NgayGiao = ddh.NgayGiao,
                    DaThanhToan = ddh.DaThanhToan,
                    TinhTrangGiaoHang = ddh.TinhTrangGiaoHang
                });
                foreach(var item in gh)
                {
                    ChiTietDatHang ctdh = new ChiTietDatHang();
                    ctdh.SoDH = ddh.SoDH;
                    ctdh.MaSP = item.MaSP_cart;
                    ctdh.SoLuong = item.SoLuong_cart;
                    ctdh.DonGia = (decimal)item.ThanhTien_cart;
                    var chitiet = context.Set<ChiTietDatHang>();
                    chitiet.Add(new ChiTietDatHang
                    {
                        SoDH = ctdh.SoDH,
                        MaSP = ctdh.MaSP,
                        SoLuong = ctdh.SoLuong,
                        DonGia = ctdh.DonGia
                    });
                }
                context.SaveChanges();
            }
            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDonHang", "Cart");
        }

        [HttpGet]
        public ActionResult KhongThongTin()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Shop");
            }
            List<Cart> GioHang_list = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(GioHang_list);
        }

        [HttpPost]
        public ActionResult KhongThongTin(FormCollection collection)
        {
            DonDatHang ddh = new DonDatHang();
            List<Cart> gh = LayGioHang();
            KhachHang kh = new KhachHang();
            kh.HoTen = collection["HoTen"];
            kh.DiaChiKH = collection["DiaChiKH"];
            kh.DienThoaiKH = collection["DienThoaiKH"];
            if (data.DonDatHangs.ToList().Count <= 0)
            {
                ddh.SoDH = 0;
            }
            else
            {
                ddh.SoDH = data.DonDatHangs.OrderByDescending(a => a.SoDH).ToList()[0].SoDH + 1;
            }
            if (data.KhachHangs.ToList().Count <= 0)
            {
                kh.MaKH = 0;
            }
            else
            {
                kh.MaKH = data.KhachHangs.OrderByDescending(a => a.MaKH).ToList()[0].MaKH + 1;
            }
            using(var context = new DienTuEntities())
            {
                var donhang = context.Set<DonDatHang>();
                var khachhang = context.Set<KhachHang>();
                khachhang.Add(new KhachHang
                {
                    MaKH = kh.MaKH,
                    HoTen = kh.HoTen,
                    TaiKhoan = null,
                    MatKhau = null,
                    Email = null,
                    DiaChiKH = kh.DiaChiKH,
                    DienThoaiKH = kh.DienThoaiKH,
                    NgaySinh = null
                });
                donhang.Add(new DonDatHang
                {
                    SoDH = ddh.SoDH,
                    MaKH = kh.MaKH,
                    NgayDat = DateTime.Now,
                    NgayGiao = ddh.NgayGiao,
                    DaThanhToan = false,
                    TinhTrangGiaoHang = false
                });
                foreach(var item in gh)
                {
                    ChiTietDatHang ctdh = new ChiTietDatHang();
                    ctdh.SoDH = ddh.SoDH;
                    ctdh.MaSP = item.MaSP_cart;
                    ctdh.SoLuong = item.SoLuong_cart;
                    ctdh.DonGia = (decimal)item.ThanhTien_cart;
                    var chitiet = context.Set<ChiTietDatHang>();
                    chitiet.Add(new ChiTietDatHang
                    {
                        SoDH = ctdh.SoDH,
                        MaSP = ctdh.MaSP,
                        SoLuong = ctdh.SoLuong,
                        DonGia = ctdh.DonGia
                    });
                }
                context.SaveChanges();
            }
            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDonHang", "Cart");
        }

        public ActionResult XacNhanDonHang()
        {
            return View();
        }
    }
}