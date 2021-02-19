using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CreatureClass2.Models
{
    public class DbSeed: DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext db)
        {
            base.Seed(db);//this recreates the database every time the application is launched


            //this is the role manager included in the framework
            //this creates the different roles the system will use
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            //roleManager.Create(new IdentityRole("Team"));
            roleManager.Create(new IdentityRole("Manager"));
            roleManager.Create(new IdentityRole("Player"));

            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));



            // Super liberal password validation for password for seeds
            userManager.PasswordValidator = new PasswordValidator
            {
                RequireDigit = false,
                RequiredLength = 1,
                RequireLowercase = false,
                RequireNonLetterOrDigit = false,
                RequireUppercase = false,
            };

            Manager Sabi = new Manager()
            {
                Email = "Sabi@FantasticCreatures.com",
                UserName = "Sabi@FantasticCreatures.com",

            };

            userManager.Create(Sabi, "Sabi1234");//the user manager add to the database
            userManager.AddToRole(Sabi.Id, "Manager");// assigns to manager role

            db.SaveChanges();




           

            Location N = new Location()
            {
                Id=1,
                Letter = "N",
                Creature = "Western Dragon",
                
                Question = "I remember the old times, when we used to burn ... around here",
                Answer = "Witches",
                


            };

            

          
            db.Locations.Add(N);
            
            Location W = new Location()
            {
                Id = 2,
                Letter = "W",
                Creature = "Phoenix",
                
                Question = "Atkinson never needed my tears; he could cure people using simple ...",
                Answer = "Herbs"
               


            };

            

            
            db.Locations.Add(W);
            

           
            Location H = new Location()
            {
                Id = 3,
                Letter = "H",
                Creature = "Griffin",
                
                Question = "We are indeed guarding a great treasure. Not gold but ...",
                Answer = "Knowledge",
                


            };

           

           
            db.Locations.Add(H);
           

            
            Location K = new Location()
            {
                Id = 4,
                Letter = "k",
                Creature = "Beithir",
                
                Question = "I never thought you’d find me here. Never thought you’d come to this part of the world, right at the ...",
                Answer = "End",
               
            };

           

           
            db.Locations.Add(K);
          

           
            Location E = new Location()
            {
                Id = 5,
                Letter = "E",
                Creature = "Cat sith",
                
                Question = "I like my companion. He too is a creature of the night. He doesn’t look like a faerie though. What is he?",
                Answer = "Owl",
               
            };

           

            
            db.Locations.Add(E);
           

            
            Location O = new Location()
            {
                Id = 6,
                Letter = "O",
                Creature = "Sphinx",
                
                Question = "Here is my riddle: what is yours but everyone uses more than you?",
                Answer = "Name",
               
            };

           


            
            db.Locations.Add(O);


           

            Professor P = new Professor()
            {
                Id=11,
                Letter = "P",
               
                Question = "",
                Answer = "X",



            };

            N.PointsTo = W.Id;
            W.PointsTo = H.Id;
            H.PointsTo = K.Id;
            K.PointsTo = E.Id;
            E.PointsTo = O.Id;
            O.PointsTo = N.Id;

            db.Professors.Add(P);
            db.SaveChanges();

            House Pixie = new House()
            {
                Name = "Pixie",
                StartLocationId = N.Id,


            };
            House Alba = new House()
            {
                Name = "Alba",
                StartLocationId = W.Id,


            };
            House Wallace = new House()
            {
                Name = "Wallace",
                StartLocationId = H.Id,


            };
            House Unseelie = new House()
            {
                Name = "Unseelie",
                StartLocationId = K.Id,


            };
            House Skye = new House()
            {
                Name = "Skye",
                StartLocationId = E.Id,


            };

            //db.Houses.Add(Pixie);
            //db.Houses.Add(Alba);
            //db.Houses.Add(Wallace);
            //db.Houses.Add(Unseelie);
            //db.Houses.Add(Skye);

            //db.SaveChanges();



            Team Participants = new Team()
            {
                UserName = "Participants",
                House = Pixie,
                HouseName = Pixie.Name,
                Counter = 3,
                LocationId = 6,
                RegisteredAt = DateTime.Today,
                Score = 6
            };

            Team newTeam = new Team()
            {
                UserName = "p",
                House = Alba,
                HouseName = Alba.Name,
                Counter = 7,
                LocationId = 4,
                RegisteredAt = DateTime.Today,
                Score = 30

            };

            db.Teams.Add(Participants);
            db.Teams.Add(newTeam);
            db.Houses.Add(Pixie);
            db.Houses.Add(Alba);
            db.Houses.Add(Wallace);
            db.Houses.Add(Unseelie);
            db.Houses.Add(Skye);


            db.SaveChanges();


            Message message = new Message()
            {
                SentFrom = Participants.Id,
                SentTo = "Sabi",
                TimeSent = DateTime.Now.AddMinutes(-2),
                Body = "We are stuck at the firs hurdle",
                status = "Sent",


            };

            Message response = new Message()
            {
                Body = "youse are stupid",
                SentFrom = "Sabi",
                SentTo = Participants.Id,
                TimeSent = DateTime.Now,
                status = "Read"

            };


            db.Messages.Add(message);
            db.Messages.Add(response);
            db.SaveChanges();

        }
    }
}
