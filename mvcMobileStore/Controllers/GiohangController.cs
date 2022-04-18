using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvcMobileStore.Models;
using mvcMobileStore.Controllers;
using System.Windows.Documents;
namespace mvcMobileStore.Controllers
{
    public class GiohangController : Controller
    {

        dbQLMobileDataContext data = new dbQLMobileDataContext();

        public List<GioHang> Laygiohang()
        {
            List<GioHang> lsGioHang = Session["GioHang"] as List<GioHang>;
            if (lsGioHang == null)
            {
                lsGioHang = new List<GioHang>();
                Session["GioHang"] = lsGioHang;
            }
            return lsGioHang;
        }
        public ActionResult ThemGioHang(int iMaSP, string strURL)
        {
            List<GioHang> lsGioHang = Laygiohang();
            GioHang sp = lsGioHang.Find(n => n.iMaSP == iMaSP);
            if (sp == null)
            {
                sp = new GioHang(iMaSP);
                lsGioHang.Add(sp);
                return Redirect(strURL);
            }
            else
            {
                sp.iSoluong++;
                return Redirect(strURL);
            }

        }
        private int TongSoLuong()
        {
            int TongSL = 0;
            List<GioHang> lsGioHang = Session["GioHang"] as List<GioHang>;
            if (lsGioHang != null)
            {
                TongSL = lsGioHang.Sum(n => n.iSoluong);
            }
            return TongSL;
        }
        private double TongTien()
        {
            double iTongTien = 0;
            List<GioHang> lsGioHang = Session["GioHang"] as List<GioHang>;
            if (lsGioHang != null)
            {
                iTongTien = lsGioHang.Sum(n => n.dThanhtien);
            }
            return iTongTien;
        }
        public ActionResult GioHang()
        {
            List<GioHang> lsGioHang = Laygiohang();
            if (lsGioHang.Count == 0)
            {
                return RedirectToAction("Index", "MobileStore");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lsGioHang);
        }
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }
        public ActionResult XoaGiohang(int iMaSP)
        {
            List<GioHang> lsGioHang = Laygiohang();
            GioHang sanpham = lsGioHang.SingleOrDefault(n => n.iMaSP == iMaSP);
            if (sanpham != null)
            {
                lsGioHang.RemoveAll(n => n.iMaSP == iMaSP);
                return RedirectToAction("GioHang");
            }
            if (lsGioHang.Count == 0)
            {
                return RedirectToAction("Index", "MobileStore");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapnhatGiohang(int iMaSP, FormCollection f)
        {
            //Lay gio hang tu Session
            List<GioHang> lsGioHang = Laygiohang();
            //Kiem tra sach da co trong Session["Giohang"]
            GioHang sanpham = lsGioHang.SingleOrDefault(n => n.iMaSP == iMaSP);
            //Neu ton tai thi cho sua Soluong
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());

            }
            return RedirectToAction("Giohang");
        }
        public ActionResult XoaTatcaGiohang()
        {
            //Lay gio hang tu Session
            List<GioHang> lsGioHang = Laygiohang();
            lsGioHang.Clear();
            return RedirectToAction("Index", "MobileStore");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            //Kiem tra dang nhap
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "MobileStore");
            }
            //Lay gio hang tu Session
            List<GioHang> lsGioHang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lsGioHang);
        }
        public ActionResult DatHang(FormCollection collection)
        {
            //Them Don hang
            DONDATHANG ddh = new DONDATHANG();
            KhachHang kh = (KhachHang)Session["Taikhoan"];
            List<GioHang> gh = Laygiohang();
            ddh.MaKH = kh.MAKH;
            ddh.Ngaydat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.Ngaygiao = DateTime.Parse(ngaygiao);
            ddh.Tinhtranggiaohang = false;
            ddh.Dathanhtoan = false;
            data.DONDATHANGs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            //Them chi tiet don hang
            foreach (var item in gh)
            {
                CHITIETDONTHANG ctdh = new CHITIETDONTHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaSP = item.iMaSP;
                ctdh.Soluong = item.iSoluong;
                ctdh.Dongia = (decimal)item.dDongia;
                data.CHITIETDONTHANGs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}