using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

using System.IO;

namespace ShopProject.Areas.Administrator.Controllers
{
    public class OrderController : Controller
    {
        Models.AdminContext dbPro = new Models.AdminContext();
        ShopProject.Repository.ShopDAO shopDAO = new Repository.ShopDAO();
        // GET: Administrator/Order
        public ActionResult Index(string error, string phone, string hdid)
        {
            if (Session["accname"] == null)
            {
                Session["accname"] = null;
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.ProError = error;
                if (!string.IsNullOrEmpty(phone))
                {
                    return View(dbPro.Orders.Where(p => p.cusPhone.Contains(phone)).OrderByDescending(x => x.orderDateTime).ToList());

                }
                else if (!string.IsNullOrEmpty(hdid))
                {
                    return View(dbPro.Orders.Where(p => p.orderID .Contains(hdid)).OrderByDescending(x => x.orderDateTime).ToList());
                }
                else
                {
                    return View(dbPro.Orders.OrderByDescending(x => x.orderDateTime).ToList());
                }

            }
        }
        [HandleError]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (Session["accname"] == null)
            {
                Session["accname"] = null;
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return View(dbPro.Orders.SingleOrDefault(e => e.orderID.Contains(id)));
            }
        }

        [HandleError]
        [HttpPost]
        public ActionResult Edit(Models.Order editOrder)
        {
            if (Session["accname"] == null)
            {
                Session["accname"] = null;
                return RedirectToAction("Login", "Account");
            }
            else
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        dbPro.Entry(editOrder).State = System.Data.Entity.EntityState.Modified;
                        dbPro.SaveChanges();
                        ViewBag.EditTypeError = "Cập nhật hóa đơn thành công.";
                    }
                }
                catch (Exception)
                {
                    ViewBag.EditTypeError = "Không thể cập nhật hóa đơn này.";
                }
                 return View(dbPro.Orders.OrderByDescending(x => x.orderDateTime).ToList());
            }
        }
        public ActionResult Details(string id)
        {
            if (Session["accname"] == null)
            {
                Session["accname"] = null;
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return View(dbPro.OrderDetails.Where(p => p.Order.orderID.Contains(id)).OrderByDescending(x => x.Order.orderDateTime).ToList());
            }
        }

    }
}