﻿using SurveySystem.Models;
using SurveySystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveySystem.Controllers
{
    public class PersonController : BaseController
    {
        public ActionResult Index()
        {
            var model = db.Person.ToList();
            return View(model);
        }

        public ActionResult Create(Person person)
        {
            if (person.NameSurname != null)
            {
                person.CreateDate = DateTime.Now;
                person.CreateBy = NameSurname;

                db.Person.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }

        }

        public ActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return HttpNotFound();
            }
            var model = db.Person.Find(Id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Person person)
        {
            db.Entry(person).State = System.Data.Entity.EntityState.Modified;
            db.Entry(person).Property(e => e.CreateBy).IsModified = false;
            db.Entry(person).Property(e => e.CreateDate).IsModified = false;

            person.ModifyBy =NameSurname;
            person.ModifyDate = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return HttpNotFound();
            }
            var person = db.Person.Find(id);
            db.Person.Remove(person);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}