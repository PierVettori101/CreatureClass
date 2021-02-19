using CreatureClass2.Models;
using System;
using System.Collections.Generic;

using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using Microsoft.AspNet.Identity.EntityFramework;



namespace CreatureClass2.Controllers
{

   

    public class HomeController : Controller
    {
       // private static int scoreCheck = 0;
        private Helper man = new Helper();
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult Index()
        {
            return RedirectToAction("Welcome", "Home");
        }


        public ActionResult Welcome(string msg)
        {
            ViewBag.msg = msg;
            return View();
        }

      
        

       [HttpGet]
       [Authorize(Roles = "Player")]
        public ActionResult Questions(string score)
        {
            List<Message> messages = new List<Message>();
            string refresh = Request["RefreshCheck"];

            string userId = User.Identity.GetUserId();
            Team team = db.Teams.Find(userId);
            team.House = db.Houses.Find(team.HouseName);

            if(score!=null)
            {
                team.Score = int.Parse(score);
            }
            

           

                team.Location = db.Locations.Find(team.LocationId);
               

                foreach (Message m in man.GetUserMessages(team.Id))
                {
                    if (m.status == "Sent")
                    {
                        messages.Add(m);
                    }
                }

                if (team.Counter >= 6)
                {
                    return RedirectToAction("Professor", new { UserName = team.UserName, Score = team.Score, HouseName = team.HouseName, LocationId = team.LocationId });

                }

                ViewBag.Messages = messages;


            db.SaveChanges();
            ViewBag.team = team;
            return View();
        }

       
        [Authorize(Roles = "Player")]
        public ActionResult Questions2(int score )
        {
            
            List<Message> messages = new List<Message>();
            string userId = User.Identity.GetUserId();
            Team team = (Team)db.Users.Find(userId);
            string refresh = Request["RefreshCheck"];
            
            team.Score = score;




            Location comeFrom = db.Locations.Find(team.LocationId);

            if (refresh == "True")
            {
                team.LocationId = comeFrom.Id;
                team.Location = db.Locations.Find(comeFrom.Id);

                team.Counter = team.Counter;
                db.SaveChanges();
            }
            else
            {
                

                team.LocationId = comeFrom.PointsTo;
                team.Location = db.Locations.Find(comeFrom.PointsTo);

                team.Counter = team.Counter + 1;

                db.SaveChanges();
            }

            if (team.Counter >= 6)
            {
                return RedirectToAction("Professor", new { UserName = team.UserName, Score = team.Score, HouseName = team.HouseName, LocationId = team.LocationId });

            }

            


            foreach (Message m in man.GetUserMessages(team.Id))
            {
                if (m.status == "Sent")
                {
                    messages.Add(m);
                }
            }

            ViewBag.Messages = messages;

            
            ViewBag.team = team;
            //scoreCheck = score;
            return View();
        }


        [Authorize(Roles = "Player")]
        public ActionResult Professor(Team t)
        {
           // t.LocationId = int.Parse(Request["locationId"]);
            if (t.LocationId == 0 )
            {
                t.HouseName = Request["houseName"];
                t.Score = int.Parse(Request["score"]);
                t.UserName = Request["teamName"];


                return RedirectToAction("Finish", new { UserName = t.UserName, Score = t.Score, HouseName = t.HouseName });

            }

            
            
            List<Message> messages = new List<Message>();

            foreach (Message m in man.GetUserMessages(t.Id))
            {
                if (m.status == "Sent")
                {
                    messages.Add(m);
                }
            }
            ViewBag.Messages = messages;
            ViewBag.Prof = db.Professors.Find(11);
            ViewBag.team = t;

            return View();
        }


        [Authorize(Roles = "Player")]
        public ActionResult Finish(Team t)
        {


            ViewBag.Team = t;
            return View();
        }

        [Authorize(Roles = "Player")]
        public ActionResult Score()
        {
            string userId = User.Identity.GetUserId();
            Team t = (Team)db.Users.Find(userId);

            ViewBag.Team = t;

            
            return View();
        }
       

    }

}
