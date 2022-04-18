using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvcMobileStore.Models;

using PagedList;
using PagedList.Mvc;
namespace mvcMobileStore.Controllers
{
    public class HomeController : Controller
    {
        dbQLMobileDataContext data = new dbQLMobileDataContext();
        private List<SanPham> LaySP(int count)
        {
            return data.SanPhams.OrderByDescending(a => a.MASP).Take(count).ToList();
        }
        public ActionResult Index()
        {
            var SPMoi = LaySP(9);
            return View(SPMoi);
        }
        
    }
}