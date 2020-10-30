using SurveySystem.Models;
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
            var model = db.Answer.Where(x => x.UserCode == Code).ToList();

            return View(model);
        }
        public ActionResult Create(string code)
        {
            if (code == null)
            {
                List<SelectListItem> personList = (from person in db.Person
                                                   where person.Code != Code
                                                   select new SelectListItem
                                                   {
                                                       Text = person.NameSurname,
                                                       Value = person.Code.ToString()
                                                   }).ToList();
                ViewBag.Person = new SelectList(personList.OrderBy(x => x.Text), "Value", "Text");

                var questionModel = db.Question.ToList();
                return View(questionModel);
            }
            else
            {
                CalculateScore(code);
                return RedirectToAction("Index");
            }
        }
        public void CalculateScore(string code)
        {
            double yes = 0, no = 0, result = 0;
            var answer = db.Answer.FirstOrDefault(x => x.PersonCode == code && x.UserCode == Code);
            var answerLine = db.AnswerLine.Where(x => x.AnswerId == answer.Id).ToList();
            foreach (var item in answerLine)
            {
                if (item.Answer == Constants.AnswerType.Yes)
                    yes++;
                else
                    no++;
            }
            result = (yes / (yes + no)) * 100;
            _ = result > 79 ? answer.IsComplete = true : answer.IsComplete = false;
            answer.Score = result.ToString();
            db.SaveChanges();
        }
        public String SendData(AnswerModel answerModel)
        {
            int? month = DateTime.Now.Month;
            var model = db.Answer.FirstOrDefault(x => x.PersonCode == answerModel.Code && x.UserCode == Code && x.CreateDate.Value.Month == month);

            if (model != null)
            {
                SaveAnswerLine(answerModel.Question, answerModel.Answer, model.Id);
            }
            else
            {
                Answer answer = new Answer();
                answer.PersonCode = answerModel.Code;
                answer.PersonName = answerModel.NameSurname;
                answer.UserCode = Code;
                answer.CreateDate = DateTime.Now;
                answer.CreateBy = NameSurname;

                db.Answer.Add(answer);
                db.SaveChanges();

                SaveAnswerLine(answerModel.Question, answerModel.Answer, answer.Id);
            }

            return "true";
        }
        public void SaveAnswerLine(string question, string answer, int answerId)
        {
            var model = db.AnswerLine.FirstOrDefault(x => x.AnswerId == answerId && x.Question == question);

            if (model != null)
            {
                model.Answer = answer;
                db.SaveChanges();
            }
            else
            {
                AnswerLine answerLine = new AnswerLine();
                answerLine.AnswerId = answerId;
                answerLine.Answer = answer;
                answerLine.Question = question;

                db.AnswerLine.Add(answerLine);
                db.SaveChanges();
            }

        }
        public ActionResult Detail(int? id)
        {
            var model = db.AnswerLine.Where(x => x.AnswerId == id).ToList();
            return View(model);
        }
    }
}