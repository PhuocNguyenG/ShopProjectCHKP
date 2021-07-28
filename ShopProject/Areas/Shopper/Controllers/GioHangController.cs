using ShopProject.Areas.Shopper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopProject.Areas.Shopper.Controllers
{
    public class GioHangController : Controller
    {
        UserContext db = new UserContext();
        // GET: Shopper/GioHang
        public ActionResult Index()
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            return View(giohang);

        }
        public ActionResult ThemVaoGio(string SanPhamID, string sizemoi,int soluongmoi)
        {
            if (Session["giohang"] == null) // Nếu giỏ hàng chưa được khởi tạo
            {
                Session["giohang"] = new List<CartItem>();  // Khởi tạo Session["giohang"] là 1 List<CartItem>
            }

            List<CartItem> giohang = Session["giohang"] as List<CartItem>;  // Gán qua biến giohang dễ code

            // Kiểm tra xem sản phẩm khách đang chọn đã có trong giỏ hàng chưa
            if (giohang.FirstOrDefault(m => m.SanPhamID == SanPhamID) == null) // ko co sp nay trong gio hang
            {               
                Models.Product sp = db.Products.Find(SanPhamID);  // tim sp theo sanPhamID
                CartItem newItem = new CartItem()
                {
                    SanPhamID = SanPhamID,
                    TenSanPham = sp.proName,
                    SoLuong = soluongmoi,
                    Type = sp.ProductType.cateID,
                    Size = sizemoi,
                    Hinh = sp.proPhoto,
                    DonGia = (Int32.Parse(sp.proPrice) - (Int32.Parse(sp.proPrice) * sp.proDiscount)/100).ToString()

                };  // Tạo ra 1 CartItem mới

                giohang.Add(newItem);  // Thêm CartItem vào giỏ 
            }
            else
            {
                // Nếu sản phẩm khách chọn đã có trong giỏ hàng thì không thêm vào giỏ nữa mà tăng số lượng lên.
                CartItem cardItem = giohang.FirstOrDefault(m => m.SanPhamID == SanPhamID);
                cardItem.SoLuong += soluongmoi;
                cardItem.Size = sizemoi;
            }

            // Action này sẽ chuyển hướng về trang chi tiết sp khi khách hàng đặt vào giỏ thành công. chuyển về chính trang khách hàng vừa đứng bằng lệnh return Redirect(Request.UrlReferrer.ToString()); 
            return Redirect(Request.UrlReferrer.ToString());
        }
        //Sửa số lượng
        public ActionResult SuaSoLuong(string SanPhamID, int soluongmoi, string sizemoi)
        {
            // tìm carditem muon sua
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            CartItem itemSua = giohang.FirstOrDefault(m => m.SanPhamID.Equals(SanPhamID));
            if (itemSua != null)
            {
                if (soluongmoi < 1 || soluongmoi > 100)
                {

                }
                else
                {
                    @ViewBag.GioError = "";
                    itemSua.SoLuong = soluongmoi;
                    itemSua.Size = sizemoi;
                }
            }
            return Redirect(Request.UrlReferrer.ToString());
            /*return RedirectToAction("Index");*/

        }
        //Xoá khỏi giỏ
        public ActionResult XoaKhoiGio(string SanPhamID)
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            CartItem itemXoa = giohang.FirstOrDefault(m => m.SanPhamID.Equals(SanPhamID));
            if (itemXoa != null)
            {
                giohang.Remove(itemXoa);
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}