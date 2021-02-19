using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CreatureClass2.Models
{
    public class Helper
    {
        private static string SqlConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static SqlConnection sqlCon = new SqlConnection(SqlConnectionString);

        public string getManager()
        {
            string manager = "";

            sqlCon.Open();

            string commandString = "SELECT * FROM AspNetUsers ";

            SqlCommand sqlCmd = new SqlCommand(commandString, sqlCon);
            SqlDataReader reader = sqlCmd.ExecuteReader();


            while (reader.Read())
            {
               string disc = reader["discriminator"].ToString();
                if(disc.Equals("Manager"))
                {
                    manager = reader["Id"].ToString();
                }
            }

            reader.Close();
            sqlCon.Close();

            return manager;
        }

        public Team getTeam(string teamName)
        {
            Team t = new Team();
            sqlCon.Open();

            string commandString = "SELECT * FROM Teams WHERE UserName= '" + teamName + "' ";

            SqlCommand sqlCmd = new SqlCommand(commandString, sqlCon);
            SqlDataReader reader = sqlCmd.ExecuteReader();


            while (reader.Read())
            {

                t.UserName = reader["UserName"].ToString();
                t.HouseName = reader["HouseName"].ToString();
                t.LocationId = int.Parse(reader["LocationId"].ToString());
                t.Score = int.Parse(reader["Score"].ToString());
                t.Counter = int.Parse(reader["Counter"].ToString());

            }

            reader.Close();
            sqlCon.Close();

            return t;
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
                m.Id = int.Parse(reader["Id"].ToString());
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


    }
}
