using BOOK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Data.Entity;
using EntityState = System.Data.Entity.EntityState;
using System.Runtime.Remoting.Contexts;
using System.Data.Entity.Migrations;
namespace BOOK.Controllers
{
    public class AdminController : Controller
    {
        QLBANSACHEntities db = new QLBANSACHEntities();
        // GET: Admin

        public ActionResult Sach(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.SACHes.ToList());
            return View(db.SACHes.ToList().OrderBy(n => n.Masach).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Thongke()
        {

         var booksByCategory = db.SACHes
        .GroupBy(s => s.CHUDE.TenChuDe)  // Assuming `Tenchude` is the category name in CHUDE
        .Select(g => new { Category = g.Key, Count = g.Count() })
        .ToList();

            ViewBag.ChartLabels = booksByCategory.Select(b => b.Category).ToArray();
            ViewBag.ChartData = booksByCategory.Select(b => b.Count).ToArray();

            return View();
        }

        [HttpGet]
        public ActionResult Suasach(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            if (sach == null)
            {
                return HttpNotFound("The requested book was not found in the database.");
            }

            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);

            return View(sach); // Ensure sach is passed here
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasach(SACH sach, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");


            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa ";
                return View(sach);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);

                    var path = Path.Combine(Server.MapPath("~/HinhSanPham/"), fileName);

                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        return View(sach);
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }

                    sach.Anhbia = fileName;
                    db.SACHes.AddOrUpdate(sach);
                    db.SaveChanges();
                }
                return RedirectToAction("Sach");
            }

        }

        //}

        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Suasach(SACH sach, HttpPostedFileBase fileUpload)
        //{
        //    // Kiểm tra xem có file được upload không
        //    if (fileUpload != null && fileUpload.ContentLength > 0)
        //    {
        //        // Lấy tên file từ uploaded file
        //        var fileName = Path.GetFileName(fileUpload.FileName);

        //        // Xây dựng đường dẫn lưu file trên server
        //        var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName);

        //        // Kiểm tra nếu file đã tồn tại hay chưa
        //        if (!System.IO.File.Exists(path))
        //        {
        //            // Lưu file vào thư mục Hinhsanpham
        //            fileUpload.SaveAs(path);
        //            sach.Anhbia = fileName; // Cập nhật tên ảnh vào model
        //        }
        //        else
        //        {
        //            ViewBag.Thongbao = "Ảnh đã tồn tại!";

        //        }
        //    }

        //    // Cập nhật dữ liệu sách vào database
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(sach).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Sach");
        //    }

        //    // Trả lại view nếu có lỗi
        //    ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
        //    ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
        //    return View(sach);
        //}


        [HttpGet]
        public ActionResult Xoasach(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
          if(sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        [HttpPost, ActionName("Xoasach")]
        public ActionResult Xacnhanxoa(int id)
        {
            // Lay ra doi tuong sach can xoa theo ma    
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach?.Masach; // Safe navigation in case 'sach' is null

            if (sach == null)
            {
                // Return HTTP 404 status code if the item is not found
                return HttpNotFound();
            }

            // Remove the 'sach' object from the database
            db.SACHes.Remove(sach);

            // Save the changes to the database to persist the deletion
            db.SaveChanges();

            // Redirect to the "Sach" action after successful deletion
            return RedirectToAction("Sach");
        }





        public ActionResult Chitietsach(int id)
        {
            // Lay ra doi tuong sach theo ma
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach?.Masach; // Safe navigation if 'sach' is null

            if (sach == null)
            {
                // Return HTTP 404 status code if the item is not found
                return HttpNotFound(); // Automatically sets 404 status code
            }

            return View(sach);
        }



        [HttpGet]
        public ActionResult Themoisach()
        {
            // Đưa dữ liệu vào DropDownList
            // Lấy danh sách từ bảng CHUDE sắp xếp tăng dần theo TenChuDe và chọn MaCD
            ViewBag.MaCD = new SelectList(db.CHUDEs.OrderBy(n => n.TenChuDe).ToList(), "MaCD", "TenChuDe");

            // Lấy danh sách từ bảng NHAXUATBAN sắp xếp tăng dần theo TenNXB và chọn MaNXB
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.OrderBy(n => n.TenNXB).ToList(), "MaNXB", "TenNXB");

            return View();
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themoisach(SACH sach, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownlist
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui long chon anh bia";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten file, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName);
                    //Kiem tra hinh anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hinh anh da ton tai";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    sach.Anhbia = fileName;
                    //Luu vao CSDL
                    db.SACHes.Add(sach);
                    db.SaveChanges();
                }
                return RedirectToAction("Sach");
            }
        }



        public ActionResult SanPham()
        {
            return View(db.CHUDEs.ToList());
        }

        public ActionResult Nhaxuatban()
        {
            return View(db.NHAXUATBANs.ToList());
        }
       
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            // Gán các giá trị người dùng nhập liệu cho các biến
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                // Gán giá trị cho đối tượng được tạo mới (ad)
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                     ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }

       
    }
}