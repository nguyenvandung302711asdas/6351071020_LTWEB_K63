using BOOK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOOK.Controllers
{
    public class GiohangController : Controller
    {
        // GET: Giohang

        //Tao doi tuong data chua du lieu tu model dbBansach da tao
        QLBANSACHEntities data = new QLBANSACHEntities();
        //Lay gio hang
        //public List<Giohang> Laygiohang()
        //{
        //    List<Giohang> lstGioHang = Session["Giohang"] as List<Giohang>;
        //    if(lstGioHang != null )
        //    {
        //        //Neu gio hang chua ton tai thi khooi tao listGiohang
        //        lstGioHang = new List<Giohang>();
        //        Session["Giohang"] = lstGioHang;
        //    }
        //    return lstGioHang;
        //}

        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGioHang = Session["Giohang"] as List<Giohang>;
            if (lstGioHang == null) // Change the condition to == null
            {
                // If the cart does not exist, initialize a new list
                lstGioHang = new List<Giohang>();
                Session["Giohang"] = lstGioHang;
            }
            return lstGioHang;
        }

        public ActionResult Index()
        {
            return View();
        }

        // Thêm hàng vào giỏ
        public ActionResult ThemGiohang(int iMasach, string strURL)
        {
            List<Giohang> lstGiohang = Laygiohang();
            // Kiểm tra sách này tồn tại trong Session["Giohang"] chưa?
            Giohang sanpham = lstGiohang.Find(n => n.iMasach == iMasach);
            if (sanpham == null)
            {
                sanpham = new Giohang(iMasach);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }

        // Tính tổng số lượng
        private int TongSoluong()
        {
            int iTongSoluong = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongSoluong = lstGiohang.Sum(n => n.iSoluong);
            }
            return iTongSoluong;
        }

        // Tính tổng tiền
        private double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhtien);
            }
            return iTongTien;
        }

        //Xoa gio hang
        public ActionResult XoaGiohang(int iMaSP)
        {
            //Lay gio hang
            List<Giohang> lstGiohang = Laygiohang();
            //Kiem tra sach da co trong Session["Gionghang"]
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMasach == iMaSP);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.iMasach == iMaSP);
                return RedirectToAction("GioHang");
            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Default");
            }
            return RedirectToAction("GioHang");

        }


        //Cap nhat gio hang

        public ActionResult CapnhatGiohang(int iMaSP, FormCollection f)
        {       
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMasach == iMaSP);
            if (sanpham != null)
            {
                if (f["txtSoluong"] != null && int.TryParse(f["txtSoluong"].ToString(), out int soluong))
                {
                    sanpham.iSoluong = soluong;
                }
                else
                {
                    
                    ViewBag.ErrorMessage = "Số lượng không hợp lệ.";
                }
            }
            return RedirectToAction("Giohang");
        }

        //Xoa tat ca gio hang
        public ActionResult XoaTatcaGiohang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();

            return RedirectToAction("Index", "Default");
        }


        // Cập nhật phương thức xử lý hiển thị Giỏ hàng : GioHang()
        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Default");
            }
            ViewBag.Tongsoluong = TongSoluong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }

        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoluong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }

        //Hien thi view dat hang de cap naht cac thong tin cho don hang
        [HttpGet]

        public ActionResult Dathang()
        {
            //Kiem tra dang nhap
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "Default");

            }

            //Lay gio hang tu Session
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoluong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);

          
        }

        public ActionResult DatHang(FormCollection collection)
        {
            //Them Don hang
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<Giohang> gh = Laygiohang();
            ddh.MaKH = kh.MaKH;
            ddh.Ngaydat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.Ngaygiao = DateTime.Parse(ngaygiao);
            ddh.Tinhtranggiaohang = false;
            ddh.Dathanhtoan = false;
            data.DONDATHANGs.Add(ddh);
            data.SaveChanges();
            
            //data.DONDATHANGs.InsertOnSubmit(ddh);
            //data.SubmitChanges();

            //Them chi tiet don hang
            foreach (var item in gh)
            {
                CHITIETDONHANG ctdh = new CHITIETDONHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.Masach = item.iMasach;
                ctdh.Soluong = item.iSoluong;
                ctdh.Dongia = (decimal)item.dDongia;
                data.CHITIETDONHANGs.Add(ctdh);
            }

            data.SaveChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }

        //Xac nhan don dat hang

        public ActionResult Xacnhandonhang()
        {
            return View();
        }


    }


}