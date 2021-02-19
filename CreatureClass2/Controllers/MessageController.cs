using CreatureClass2.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace CreatureClass2.Controllers
{
    public class MessageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Helper man = new Helper();





        private static string SqlConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static SqlConnection sqlCon = new SqlConnection(SqlConnectionString);



        [HttpGet]
        [Authorize(Roles = "Player")]
        public ActionResult MessageScreen(string score)
        {

            string userId = User.Identity.GetUserId();
            Team team = db.Teams.Find(userId);
            if (score != null)
            {
                team.Score = int.Parse(score);
            }
            

            if (Request["Send"] != null)
            {
                string body = Request["sendBody"];
                string user = team.UserName +"Sent at " + DateTime.Now;

                if (body != "")
                {
                    SendMessage(userId, body);
                    sendEmail(user);
                }


            }

            //Team t = db.Teams.Find(user);
            //t.Score = score;
            //List<Message> messages = db.Messages.ToList();
            List<Message> messages = man.GetUserMessages(userId);
            foreach (Message m in messages)
            {
                Message dbMessage = db.Messages.Find(m.Id);
                dbMessage.status = "Read";
                db.SaveChanges();
            }
           

            ViewBag.Messages = messages;// GetRecentMessage(user);

            ViewBag.SabiId = man.getManager();
            ViewBag.team = team;
            


            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult SabiMessages()
        {

            List<Team> teams = new List<Team>();

            foreach (Team t in db.Teams.ToList())
            {
                if (t.RegisteredAt == DateTime.Today)
                {
                    teams.Add(t);
                    t.Messages = GetUserMessages(t.Id);
                }
            }

            if(teams.Count==0)
            {
                ViewBag.noTeamsMessage = "There are no messages today";
            }

            ViewBag.Teams = teams;

            string teamName = Request["teamName"];

            if (teamName != null)
            {
                Team tt= db.Teams.Find(teamName);
                return RedirectToAction("SabiSend", "Message", tt);
            }
            return View();
        }

        [Authorize(Roles = "Manager")]
        public ActionResult SabiSend(Team t)
        {
            if (Request["sendBody"] != null)
            {
                //string teamId = Request["team"];
                //t = db.Teams.Find(t.Id);
                string body = Request["sendBody"];

                Message message = new Message();
                message.Body = body;
                message.SentTo = t.Id;
                message.SentFrom = User.Identity.GetUserId(); 
               
                message.TimeSent = DateTime.Now;
                message.status = "Sent";
                db.Messages.Add(message);
                db.SaveChanges();


            }
            List<Message> messages = GetUserMessages(t.Id);
            

            ViewBag.sabiId = User.Identity.GetUserId();
            ViewBag.Messages = messages;
            ViewBag.TeamName = t.Id;



            return View();
        }


        public void SendMessage(string user, string body)
        {
            Message m = new Message();

            m.Body = body;
            m.SentFrom = user;
            m.SentTo = User.Identity.GetUserId(); 
            m.TimeSent = DateTime.Now;
            m.status = "Sent";

            db.Messages.Add(m);

            db.SaveChanges();


        }
        public List<Message> GetUserMessages(string user)
        {
            List<Message> messages = new List<Message>();


            sqlCon.Open();

            string commandString = "SELECT * FROM Messages WHERE SentFrom = '" + user + "' OR SentTo = '" + user + "'";

            SqlCommand sqlCmd = new SqlCommand(commandString, sqlCon);
            SqlDataReader reader = sqlCmd.ExecuteReader();


            while (reader.Read())
            {
                Message m = new Message();
                m.Body = reader["Body"].ToString();
                m.SentTo = reader["SentTo"].ToString();
                m.SentFrom = reader["SentFrom"].ToString();
                m.TimeSent = DateTime.Parse(reader["TimeSent"].ToString());
                m.status = reader["status"].ToString();

                messages.Add(m);
            }

            reader.Close();
            sqlCon.Close();



            return messages;
        }
        public Message GetRecentMessage(string user)
        {
            Message m = new Message();


            sqlCon.Open();

            string commandString = "SELECT * FROM Messages WHERE SentFrom = '" + user + "' AND TimeSent = " +
                "(SELECT MAX(TimeSent) as Sent FROM Messages WHERE SentFrom = '" + user + "'  )";

            SqlCommand sqlCmd = new SqlCommand(commandString, sqlCon);
            SqlDataReader reader = sqlCmd.ExecuteReader();


            while (reader.Read())
            {

                m.Body = reader["Body"].ToString();



            }

            reader.Close();
            sqlCon.Close();



            return m;
        }

        public List<Team> GetTeams()
        {
            List<Team> teams = new List<Team>();

            //.ToString("yyyy/MM/dd")
            sqlCon.Open();

            string commandString = "SELECT * FROM Teams WHERE RegisteredAt = '" + DateTime.Today + "' ";

            SqlCommand sqlCmd = new SqlCommand(commandString, sqlCon);
            SqlDataReader reader = sqlCmd.ExecuteReader();


            while (reader.Read())
            {

                Team t = new Team();

                t.UserName = reader["UserName"].ToString();
                t.HouseName = reader["HouseName"].ToString();
                t.LocationId = int.Parse(reader["LocationId"].ToString());
                t.Score = int.Parse(reader["Score"].ToString());
                t.Counter = int.Parse(reader["Counter"].ToString());

                teams.Add(t);


            }

            reader.Close();
            sqlCon.Close();



            return teams;
        }

        public void sendEmail(string usr)
        {
            
            var message = new MailMessage();
            message.To.Add(new MailAddress("sherlockssecretchallenge@gmail.com"));  // replace with valid value 
            message.From = new MailAddress("fantasticcreaturesapp@gmail.com");  // replace with valid value
            message.Subject = "FantasticCreatures Message Center";
            message.Body = "new message from "+usr +" ";
            //message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "fantasticcreaturesapp@gmail.com" , // replace with valid value
                    Password = "Taegan11"  // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(message);

            }
        }

        public void SendText(string msg)
        {
            // Find your Account Sid and Token at twilio.com/console
            // DANGER! This is insecure. See http://twil.io/secure
            const string accountSid = "AC683126488d44d72d97d40281b23fb9db";
            const string authToken = "fef799a8705362c707b26ffa66ac1bee";

            TwilioClient.Init(accountSid, authToken);

            var to = new Twilio.Types.PhoneNumber("+447449714511");
            var from = new Twilio.Types.PhoneNumber("+447401201624");
            var message = MessageResource.Create
                (
                to: to,
                from: from,
                body: "You have a new message from " + msg 



            ) ;

            Content(message.Sid);
        }
    }
}
