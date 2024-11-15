using BOOK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace BOOK.Controllers
{
    public class DefaultController : Controller
    {

        QLBANSACHEntities data = new QLBANSACHEntities();
        // GET: Default
        private List<SACH> Laysachmoi(int count)
        {
            return data.SACHes.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }

        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            
            var sachmoi = Laysachmoi(6);

            return View(sachmoi.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult Chude()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }

        public ActionResult NhaXB()
        {
            var xb = from cd in data.NHAXUATBANs select cd;
            return PartialView(xb);
        }

        public ActionResult Details(int id)
        {
            var sach = data.SACHes.SingleOrDefault(s => s.Masach == id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }


        public ActionResult SPTheochude(int id)
        {
           
                var sach = from s in data.SACHes where s.MaCD == id select s;
                return View(sach);
            

        }
        public ActionResult SPTheoNhaXB(int id )
        {
                var sach = from s in data.SACHes where s.MaNXB == id select s;
                return View(sach);
            

        }
    }
}