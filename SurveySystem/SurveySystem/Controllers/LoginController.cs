using SurveySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveySystem.Controllers
{
    public class LoginController : Controller
    {
        SurveyEntities db = new SurveyEntities();
        public ActionResult SignIn(string Code, string Password)
        {
            if (Code == null)
            {
                return View();
            }
            else
            {
                var person = db.Person.FirstOrDefault(x => x.Code == Code && x.Password == Password);

                if (person != null)
                {
                    Session["Code"] = person.Code;
                    Session["NameSurname"] = person.NameSurname;
                    return RedirectToAction("Index", "Person");
                }
                else
                {
                    ViewBag.Error = "Kullanıcı kodu veya şifre hatalı!!";
                    return View();
                }
            }
        }
    }
}