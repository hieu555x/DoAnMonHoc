using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DoAnMonHoc.Models;

namespace DoAnMonHoc.Models
{
    public class CommonInfo
    {
        DienTuEntities data;

        public CommonInfo()
        {
            data = new DienTuEntities();
        }

        public IEnumerable<DoAnMonHoc.Models.TheLoai> TheLoai()
        {
            return data.TheLoais;
        }
        public IEnumerable<DoAnMonHoc.Models.SanPham> SanPham()
        {
            return data.SanPhams;
        }
        public IEnumerable<DoAnMonHoc.Models.Hang> Hang()
        {
            return data.Hangs;
        }
        public IEnumerable<DoAnMonHoc.Models.PhanHoi> PhanHoi()
        {
            return data.PhanHois;
        }
        public IEnumerable<DoAnMonHoc.Models.DonDatHang> DonDatHang()
        {
            return data.DonDatHangs;
        }
        public IEnumerable<DoAnMonHoc.Models.KhachHang> KhachHang()
        {
            return data.KhachHangs;
        }
    }
}