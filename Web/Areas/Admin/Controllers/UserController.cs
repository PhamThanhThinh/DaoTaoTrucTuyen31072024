using Model;
using Model.Dao;
using System;
using System.Web.Mvc;
using Web.Common;

namespace Web.Areas.Admin.Controllers
{
  public class UserController : Controller
  {
    // GET: Admin/User
    public ActionResult Index(string searchString, int page = 1, int pageSize = 200)
    {
      var dao = new UserDao();
      var model = dao.ListAllPaging(searchString, page, pageSize);
      ViewBag.SearchString = searchString;
      return View(model);
    }
    [HttpDelete]
    public ActionResult Delete(int id)
    {
      new UserDao().Delete(id);
      return RedirectToAction("Index");
    }

    //[HttpPost]
    //public JsonResult AddUserAjax(string username, string name, string password, string address, string email, string phone)
    //{
    //  try
    //  {
    //    var dao = new UserDao();
    //    User user = new User();

    //    var encryptedMd5Pas = Encryptor.MD5Hash(password);
    //    user.Password = encryptedMd5Pas;
    //    user.CreateDate = DateTime.Now;
    //    user.UserName = username;
    //    user.Name = name;
    //    user.Address = address;
    //    user.Email = email;
    //    user.Phone = phone;
    //    user.Status = true;

    //    long id = dao.Insert(user);
    //    if (id > 0)
    //    {
    //      return Json(new { status = true });
    //    }
    //    else
    //    {
    //      return Json(new { status = false });
    //    }
    //  }
    //  catch
    //  {
    //    return Json(new
    //    {
    //      status = false
    //    });
    //  }
    //}

    //[HttpPost]
    //public JsonResult AddUserAjax(string username, string name, string password, string address, string email, string phone)
    //{
    //  try
    //  {
    //    var dao = new UserDao();
    //    User user = new User();

    //    var encryptedMd5Pas = Encryptor.MD5Hash(password);
    //    user.Password = encryptedMd5Pas;
    //    user.CreateDate = DateTime.Now;
    //    user.UserName = username;
    //    user.Name = name;
    //    user.Address = address;
    //    user.Email = email;
    //    user.Phone = phone;
    //    user.Status = true;

    //    long id = dao.Insert(user);
    //    if (id > 0)
    //    {
    //      return Json(new { status = true });
    //    }
    //    else
    //    {
    //      return Json(new { status = false });
    //    }
    //  }
    //  catch
    //  {
    //    return Json(new
    //    {
    //      status = false
    //    });
    //  }
    //}

    [HttpPost]
    public JsonResult AddUserAjax(string username, string name, string password, string address, string email, string phone)
    {
      try
      {
        // Kiểm tra dữ liệu đầu vào
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password))
        {
          return Json(new { status = false, message = "Chưa nhập thông tin cần thiết" });
        }

        // Tạo đối tượng User và thiết lập thuộc tính
        var user = new User
        {
          UserName = username,
          Name = name,
          Password = Encryptor.MD5Hash(password), // Giả định Encryptor nằm trong Web.Common
          Address = address,
          Email = email,
          Phone = phone,
          CreateDate = DateTime.Now,
          Status = true
        };

        // Chèn người dùng vào cơ sở dữ liệu
        var dao = new UserDao(); // Giả định UserDao nằm trong Model.Dao
        long id = dao.Insert(user);

        // Kiểm tra kết quả chèn dữ liệu
        if (id > 0)
        {
          return Json(new { status = true });
        }
        else
        {
          return Json(new { status = false, message = "Không thể thêm người dùng vào cơ sở dữ liệu." });
        }
      }
      catch (Exception ex)
      {
        // Ghi lại chi tiết lỗi (có thể sử dụng thư viện ghi log)
        // Ví dụ: Log.Error(ex.Message);

        return Json(new
        {
          status = false,
          message = "Có lỗi xảy ra: " + ex.Message // Gửi thông báo lỗi chi tiết (chỉ dùng cho gỡ lỗi)
        });
      }
    }

    [HttpPost]
    public JsonResult UpdateUserAjax(string userid, string name, string address, string email, string phone)
    {
      try
      {
        var dao = new UserDao();
        User user = new User();

        user = dao.ViewDetail(Convert.ToInt16(userid));

        user.Name = name;
        user.Address = address;
        user.Email = email;
        user.Phone = phone;

        bool editresult = dao.Update(user);

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