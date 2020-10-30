using SurveySystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace SurveySystem.Controllers
{
    public class AnswerController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            List<SelectListItem> personList = (from person in db.Person
                                               where person.Code != Code
                                               select new SelectListItem
                                               {
                                                   Text = person.NameSurname,
                                                   Value = person.Code.ToString()
                                               }).ToList();
            ViewBag.Person = new SelectList(personList.OrderBy(x => x.Text),"Value","Text");

            var questionModel = db.Question.ToList();
            return View(questionModel);
        }
    }
}