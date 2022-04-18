using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mvcMobileStore.Controllers;
using mvcMobileStore.Models;
using mvcMobileStore;

namespace mvcMobileStore.Models
{
    public class GioHang
    {
        dbQLMobileDataContext data = new dbQLMobileDataContext();
        public int iMaSP { set; get; }
        public string sTenSP { set; get; }
        public string sAnhbia { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }
        }
        public GioHang(int MaSP)
        {
            iMaSP = MaSP;
            SanPham dt = data.SanPhams.Single(n => n.MASP == iMaSP);
            sTenSP = dt.TENSP;
            sAnhbia = dt.AnhSP;
            dDongia = double.Parse(dt.GIABAN.ToString());
            iSoluong = 1;
        }
    }
}