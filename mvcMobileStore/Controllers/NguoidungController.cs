using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvcMobileStore.Controllers;
using mvcMobileStore.Models;

namespace mvcMobileStore.Controllers
{
    public class NguoidungController : Controller
    {
        // GET: Nguoidung
        public ActionResult Index()
        {
            return View();

        }
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }
        dbQLMobileDataContext db = new dbQLMobileDataContext();
        public ActionResult Dangky(FormCollection collection, KhachHang kh)
        {
            // Gán các giá tị người dùng nhập liệu cho các biến
            var hoten = collection["TenKh"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var cccd = collection["CCCD"];
            var diachi = collection["Diachi"];
            var dienthoai = collection["Dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống";
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi4"] = "Phải nhập lại mật khẩu";
            }
            else if (String.IsNullOrEmpty(cccd))
            {
                ViewData["Loi5"] = "Phải nhập số căn cước công dân";
            }
            else if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi6"] = "Phải nhập địa chỉ";
            }
            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi7"] = "Phải nhập số điện thoai";
            }
            else
            {
                //Gần giá trị cho đối tượng được tạo mới (kh)
                kh.TENKH = hoten;
                kh.Taikhoan = tendn;
                kh.Matkhau = matkhau;
                kh.CCCD = cccd;
                kh.DiaChi = diachi;
                kh.SDT = dienthoai;
                kh.Ngaysinh = DateTime.Parse(ngaysinh);
                db.KhachHangs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap");
                
            }
            return this.Dangky();
        }

        public ActionResult Dangnhap(FormCollection collection)
        {
            // Gần các giá trị người dùng nhập liệu cho các biến
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loil"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                //Gần giá trị cho đối tượng được tạo mới (kh)
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.Taikhoan == tendn && n.Matkhau == matkhau);
                if (kh != null)
                {
                    
                    ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Index", "MobileStore");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
    }
}