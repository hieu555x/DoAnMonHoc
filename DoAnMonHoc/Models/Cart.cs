using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DoAnMonHoc.Models;

namespace DoAnMonHoc.Models
{
    public class Cart
    {
        DienTuEntities data = new DienTuEntities();
        public int MaSP_cart { get; set; }
        public string TenSP_cart { get; set; }
        public double GiaBan_cart { get; set; }
        public string AnhBia_cart { get; set; }
        public int SoLuong_cart { get; set; }
        public double ThanhTien_cart
        {
            get
            {
                return SoLuong_cart * GiaBan_cart;
            }
        }
        public Cart(int MaSP)
        {
            MaSP_cart = MaSP;
            SanPham SanPham = data.SanPhams.Single(a => a.MaSP == MaSP);
            TenSP_cart = SanPham.TenSP;
            AnhBia_cart = SanPham.AnhBia;
            GiaBan_cart = double.Parse(SanPham.GiaBan.ToString());
            SoLuong_cart = 1;
        }

    }
}