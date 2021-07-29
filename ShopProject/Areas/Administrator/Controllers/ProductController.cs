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
    public class ProductController : Controller
    {
        Models.AdminContext dbPro = new Models.AdminContext();
        ShopProject.Repository.ShopDAO shopDAO = new Repository.ShopDAO();
        //
        // GET: /Administrator/Product/
        [HandleError]
        public ActionResult Index(string error, string name)
        {
            if (Session["accname"] == null)
            {
                Session["accname"] = null;
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.ProError = error;
                var model = dbPro.Products.ToList();
                if (!string.IsNullOrEmpty(name))
                {
                    return View(dbPro.Products.Where(p => p.proName.Contains(name)).OrderByDescending(x => x.proName).ToList());
                }
                else
                {
                    return View(model);
                }
                
            }
        }

        [HandleError]
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["accname"] == null)
            {
                Session["accname"] = null;
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.pdcListCreate = new SelectList(dbPro.Producers, "pdcID", "pdcName");
                ViewBag.typeListCreate = new SelectList(dbPro.ProductTypes, "typeID", "typeName");
                return View();
            }
        }

        [HandleError]
        [HttpPost]
        public ActionResult Create(Models.Product createPro, HttpPostedFileBase file, HttpPostedFileBase file2, HttpPostedFileBase file3, HttpPostedFileBase file4, HttpPostedFileBase file5, HttpPostedFileBase file6)
        {
            if (Session["accname"] == null)
            {
                Session["accname"] = null;
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.pdcListCreate = new SelectList(dbPro.Producers, "pdcID", "pdcName");
                ViewBag.typeListCreate = new SelectList(dbPro.ProductTypes, "typeID", "typeName");
                var pro = dbPro.Products.SingleOrDefault(c => c.proID.Equals(createPro.proID));
                if (file != null)
                {
                    if (file.ContentLength > 0)
                    {
                        try
                        {
                            string nameFile = "";
                            string nameFile2 = "";
                            string nameFile3 = "";
                            string nameFile4 = "";
                            string nameFile5 = "";
                            string nameFile6 = "";
                            nameFile = Path.GetFileName(file.FileName);
                            file.SaveAs(Path.Combine(Server.MapPath("/Image"), nameFile));

                            /*Ngoại trừ hình chính thì những hình khác không được thêm thì sẽ được để màu trắng*/

                            if (file2 == null)
                            {
                                nameFile2 = "giaytrang.jpg";
                            }
                            else
                            {
                                nameFile2 = Path.GetFileName(file2.FileName);
                                file2.SaveAs(Path.Combine(Server.MapPath("/Image"), nameFile2));
                            }

                            if (file3 == null)
                            {
                                nameFile3 = "giaytrang.jpg";
                            }
                            else
                            {
                                nameFile3 = Path.GetFileName(file3.FileName);
                                file3.SaveAs(Path.Combine(Server.MapPath("/Image"), nameFile3));
                            }

                            if (file4 == null)
                            {
                                nameFile4 = "giaytrang.jpg";
                            }
                            else
                            {
                                nameFile4 = Path.GetFileName(file4.FileName);
                                file4.SaveAs(Path.Combine(Server.MapPath("/Image"), nameFile4));
                            }

                            if (file5 == null)
                            {
                                nameFile5 = "giaytrang.jpg";
                            }
                            else
                            {
                                nameFile5 = Path.GetFileName(file5.FileName);
                                file5.SaveAs(Path.Combine(Server.MapPath("/Image"), nameFile5));
                            }

                            if (file6 == null)
                            {
                                nameFile6 = "giaytrang.jpg";
                            }
                            else
                            {
                                nameFile6 = Path.GetFileName(file6.FileName);
                                file6.SaveAs(Path.Combine(Server.MapPath("/Image"), nameFile6));
                            }
                            /*file2.SaveAs(Path.Combine(Server.MapPath("/Image"), nameFile2));
                            file3.SaveAs(Path.Combine(Server.MapPath("/Image"), nameFile3));
                            file4.SaveAs(Path.Combine(Server.MapPath("/Image"), nameFile4));
                            file5.SaveAs(Path.Combine(Server.MapPath("/Image"), nameFile5));
                            file6.SaveAs(Path.Combine(Server.MapPath("/Image"), nameFile6));*/
                            createPro.proPhoto = "/Image/" + nameFile;
                            createPro.proPhoto2 = "/Image/" + nameFile2;
                            createPro.proPhoto3 = "/Image/" + nameFile3;
                            createPro.proPhoto4 = "/Image/" + nameFile4;
                            createPro.proPhoto5 = "/Image/" + nameFile5;
                            createPro.proPhoto6 = "/Image/" + nameFile6;
                        }
                        catch (Exception)
                        {
                            ViewBag.CreateProError = "Không thể chọn ảnh.";
                        }
                    }
                    createPro.proUpdateDate = DateTime.Now.ToString();
                    try
                    {
                        if (pro != null)
                        {
                            ViewBag.CreateProError = "Mã sản phẩm đã tồn tại.";
                        }
                        else
                        {
                            dbPro.Products.Add(createPro);
                            dbPro.SaveChanges();
                            ViewBag.CreateProError = "Thêm sản phẩm thành công.";
                        }
                    }
                    catch (Exception)
                    {
                        ViewBag.CreateProError = "Không thể thêm sản phẩm.";
                    }
                }
                else
                {
                    ViewBag.HinhAnh = "Vui lòng chọn hình ảnh.";
                }
                return View();
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
                ViewBag.pdcListEdit = new SelectList(dbPro.Producers, "pdcID", "pdcName");
                ViewBag.typeListEdit = new SelectList(dbPro.ProductTypes, "typeID", "typeName");
                var model = dbPro.Products.SingleOrDefault(p => p.proID.Equals(id));
                return View(model);
            }
        }

        [HandleError]
        [HttpPost]
        public ActionResult Edit(Models.Product editPro, HttpPostedFileBase file, HttpPostedFileBase file2, HttpPostedFileBase file3, HttpPostedFileBase file4, HttpPostedFileBase file5, HttpPostedFileBase file6)
        {
            if (Session["accname"] == null)
            {
                Session["accname"] = null;
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.pdcListEdit = new SelectList(dbPro.Producers, "pdcID", "pdcName");
                ViewBag.typeListEdit = new SelectList(dbPro.ProductTypes, "typeID", "typeName");
                if (file != null)
                {
                    if (file.ContentLength > 0)
                    {
                        try
                        {
                            string nameFile = "";
                            string nameFile2 = "";
                            string nameFile3 = "";
                            string nameFile4 = "";
                            string nameFile5 = "";
                            string nameFile6 = "";
                            nameFile = Path.GetFileName(file.FileName);
                            file.SaveAs(Path.Combine(Server.MapPath("/Image"), nameFile));

                            /*Ngoại trừ hình chính thì những hình khác không được thêm thì sẽ được để màu trắng*/

                            if (file2 == null)
                            {
                                nameFile2 = "giaytrang.jpg";
                            }
                            else
                            {
                                nameFile2 = Path.GetFileName(file2.FileName);
                                file2.SaveAs(Path.Combine(Server.MapPath("/Image"), nameFile2));
                            }

                            if (file3 == null)
                            {
                                nameFile3 = "giaytrang.jpg";
                            }
                            else
                            {
                                nameFile3 = Path.GetFileName(file3.FileName);
                                file3.SaveAs(Path.Combine(Server.MapPath("/Image"), nameFile3));
                            }

                            if (file4 == null)
                            {
                                nameFile4 = "giaytrang.jpg";
                            }
                            else
                            {
                                nameFile4 = Path.GetFileName(file4.FileName);
                                file4.SaveAs(Path.Combine(Server.MapPath("/Image"), nameFile4));
                            }

                            if (file5 == null)
                            {
                                nameFile5 = "giaytrang.jpg";
                            }
                            else
                            {
                                nameFile5 = Path.GetFileName(file5.FileName);
                                file5.SaveAs(Path.Combine(Server.MapPath("/Image"), nameFile5));
                            }

                            if (file6 == null)
                            {
                                nameFile6 = "giaytrang.jpg";
                            }
                            else
                            {
                                nameFile6 = Path.GetFileName(file6.FileName);
                                file6.SaveAs(Path.Combine(Server.MapPath("/Image"), nameFile6));
                            }
                            /*file2.SaveAs(Path.Combine(Server.MapPath("/Image"), nameFile2));
                            file3.SaveAs(Path.Combine(Server.MapPath("/Image"), nameFile3));
                            file4.SaveAs(Path.Combine(Server.MapPath("/Image"), nameFile4));
                            file5.SaveAs(Path.Combine(Server.MapPath("/Image"), nameFile5));
                            file6.SaveAs(Path.Combine(Server.MapPath("/Image"), nameFile6));*/
                            editPro.proPhoto = "/Image/" + nameFile;
                            editPro.proPhoto2 = "/Image/" + nameFile2;
                            editPro.proPhoto3 = "/Image/" + nameFile3;
                            editPro.proPhoto4 = "/Image/" + nameFile4;
                            editPro.proPhoto5 = "/Image/" + nameFile5;
                            editPro.proPhoto6 = "/Image/" + nameFile6;
                        }
                        catch (Exception)
                        {
                            ViewBag.EditProError = "Không thể chọn ảnh.";
                        }
                    }
                    editPro.proUpdateDate = DateTime.Now.ToString();
                    try
                    {
                        dbPro.Entry(editPro).State = System.Data.Entity.EntityState.Modified;
                        dbPro.SaveChanges();
                        ViewBag.EditProError = "Cập nhật sản phẩm thành công.";
                    }
                    catch (Exception)
                    {
                        ViewBag.EditProError = "Không thể cập nhật sản phẩm.";
                    }
                }
                else
                {
                    ViewBag.HinhAnh = "Vui lòng chọn hình ảnh.";
                }
                return View();
            }
        }

        [HandleError]
        public ActionResult Delete(string id)
        {
            if (Session["accname"] == null)
            {
                Session["accname"] = null;
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var model = dbPro.Products.SingleOrDefault(p => p.proID.Equals(id));
                if (model != null)
                    {

                    foreach (var item in dbPro.Rates.Where(x => x.proID == id))
                    {
                        dbPro.Rates.Remove(item);
                    }
                    foreach (var item in dbPro.Comments.Where(x => x.proID == id))
                    {
                        dbPro.Comments.Remove(item);
                    }
                    foreach (var item in dbPro.OrderDetails.Where(x => x.proID == id))
                    {
                        dbPro.OrderDetails.Remove(item);
                    }
                    dbPro.Products.Remove(model);
                    
                    
                    dbPro.SaveChanges();
                        return RedirectToAction("Index", "Product", new { error = "Xoá sản phẩm thành công." });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Product", new { error = "Sản phẩm không tồn tại." });
                    }
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
                var model = dbPro.Products.SingleOrDefault(p => p.proID.Equals(id));
                return View(model);
            }
        }
    }
}