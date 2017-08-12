using Exercise.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exercise.Controllers
{
    public class HomeController : Controller
    {

        private WorkoutEntities db = new WorkoutEntities();

        public ActionResult Index(String workoutBodyPart, String searchString)
        {
            var workoutbodyPart = new List<string>();

            var workoutQRY = from wd in db.Keywords
                          orderby wd.BodyPart
                          select wd.BodyPart;

            workoutbodyPart.AddRange(workoutQRY.Distinct());

            ViewBag.workoutBodyPart = new SelectList(workoutbodyPart);

            //collection of model
            var workout = from w in db.Keywords
                          select w;

            //Bodypart search

            if (!String.IsNullOrEmpty(workoutBodyPart))
            {
                workout = workout.Where(x => x.BodyPart == workoutBodyPart);
            }

            //Name search
            if (!String.IsNullOrEmpty(searchString))
            {
                workout = workout.Where(ws => ws.Exercise.Contains(searchString));
            }          

            return View(workout);
        }

        public ActionResult Details(int? id)
        {
            Keyword workout = db.Keywords.Find(id);
            return View(workout);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            Keyword workout = db.Keywords.Find(id);
            return View(workout);
        }

        [HttpPost]
        public ActionResult Edit(Keyword workout)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workout).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(workout);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "id,Picture,Exercise,Instructions,BodyPart")] Keyword workout)
        {
            if (ModelState.IsValid)
            {
                db.Keywords.Add(workout);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(workout);
            
        }

        public ActionResult Delete(int id)
        {
            Keyword workout = db.Keywords.Find(id);
            return View(workout);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Keyword workout = db.Keywords.Find(id);
            db.Keywords.Remove(workout);
            db.SaveChanges();
            return RedirectToAction("index");
        }

        public ActionResult Like(int id)
        {
            Keyword workout = db.Keywords.Find(id);

            int currentLikes = workout.Blike;
            workout.Blike = currentLikes + 1;

            if (ModelState.IsValid)
            {
                db.Entry(workout).State = EntityState.Modified;
                db.SaveChanges();                
            }

            return RedirectToAction("Index");
        }

        public ActionResult Dislike(int id)
        {
            Keyword workout = db.Keywords.Find(id);

            int currentDislikes = workout.Bdislike;
            workout.Bdislike = currentDislikes + 1;

            if (ModelState.IsValid)
            {
                db.Entry(workout).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
