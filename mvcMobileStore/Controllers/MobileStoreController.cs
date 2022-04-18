using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvcMobileStore.Models;
using mvcMobileStore.Controllers;

using PagedList;
using PagedList.Mvc;
namespace mvcMobileStore.Controllers
{
    public class MobileStoreController : Controller
    {
        dbQLMobileDataContext data = new dbQLMobileDataContext();
        private List<SanPham> LaySP (int count){
            return data.SanPhams.OrderByDescending(a => a.MASP).Take(count).ToList();
        }
        public ActionResult Index(int ?  page)
        {
            //Tao bien quy dinh so san pham tren moi trang
            int pageSize = 9;
            //Tao bien so trang
            int pageNum = (page ?? 1);
            //Lấy top 5 Album bán chạy nhất
            var dienthoaimoi = LaySP(100);
            return View(dienthoaimoi.ToPagedList(pageNum, pageSize));
        }
        // GET: MobileStore
        public ActionResult Dong()
        {
            var dong = from d in data.Dongs select d;
            return PartialView(dong);
        }
        public ActionResult SPTheoDong(int id)
        {
            var dienthoai = from sptd in data.SanPhams where sptd.MaDong == id select sptd;
            return View(dienthoai);
        }
        public ActionResult Details(int id)
        {
            var chitiet = from ct in data.SanPhams
                          where ct.MASP == id
                          select ct;
            return View(chitiet.Single());
        }
        
    }
}