﻿using System.Web.Mvc;

namespace Web.Areas.Admin.Controllers
{
  public class HomeController : Controller
  {
    // GET: Admin/Home
    public ActionResult Index()
    {
      return View();
    }
  }
}