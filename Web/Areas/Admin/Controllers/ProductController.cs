using Model;
using Model.Dao;
using System;
using System.Web.Mvc;

namespace Web.Areas.Admin.Controllers
{
  public class ProductController : Controller
  {
    // GET: Admin/Product
    public ActionResult Index(string dropdownid, string searchString, int page = 1, int pageSize = 200)
    {
      var dao = new ProductDao();
      if (dropdownid == null)
      {
        dropdownid = "-1";
      }
      var model = dao.ListAllPaging(Convert.ToInt16(dropdownid), searchString, page, pageSize);

      ViewBag.SearchString = searchString;
      ViewBag.DropDownID = dropdownid;
      SetViewBag();
      return View(model);
    }

    [HttpDelete]
    public ActionResult Delete(int id)
    {
      new ProductDao().Delete(id);
      return RedirectToAction("Index");
    }
    public void SetViewBag(long? selectedId = null)
    {
      var dao = new ProductCategoryDao();
      ViewBag.CategoryID = dao.ListAll();
    }

    [HttpPost]
    public JsonResult AddProductAjax(string name, string code, string metatitle, string description, string image, string categoryid, string detail, string listtype, string listfile)
    {
      try
      {
        var dao = new ProductDao();
        Product product = new Product();

        product.Name = name;
        product.CreateDate = DateTime.Now;
        product.Code = code;
        product.MetaTitle = metatitle;
        product.Description = description;
        product.Image = image;
        product.CategoryID = Convert.ToInt16(categoryid);
        product.Status = true;
        product.Detail = detail;
        product.ListType = listtype;
        product.ListFile = listfile;

        long id = dao.Insert(product);
        if (id > 0)
        {
          return Json(new { status = true });
        }
        else
        {
          return Json(new { status = false });
        }
      }
      catch
      {
        return Json(new
        {
          status = false
        });
      }
    }
    [HttpPost]
    public JsonResult UpdateProductAjax(long id, string name, string code, string metatitle, string description, string detail, string image, string listtype, string listfile, string categoryid)
    {
      try
      {
        var dao = new ProductDao();
        Product product = new Product();

        product = dao.ViewDetail(Convert.ToInt16(id));
        product.Name = name;
        product.ModifiedDate = DateTime.Now;
        product.Code = code;
        product.MetaTitle = metatitle;
        product.Description = description;
        product.Image = image;

        if (detail.Length > 5)
        {
          product.Detail = detail;
        }

        product.ListType = listtype;
        product.ListFile = listfile;
        product.CategoryID = Convert.ToInt16(categoryid);

        bool editresult = dao.Update(product);
        if (editresult == true)
        {
          return Json(new { status = true });
        }
        else
        {
          return Json(new { status = false });
        }
      }
      catch
      {
        return Json(new
        {
          status = false
        });
      }
    }
  }
}