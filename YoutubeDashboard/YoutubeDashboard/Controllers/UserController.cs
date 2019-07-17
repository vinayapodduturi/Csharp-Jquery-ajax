using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Net.Mail;

namespace YoutubeDashboard.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        public ActionResult Index()
        {

             if (Session["Authenticate"]=="Yes")
             {

                 return View();
             }
             else
             {
                /// destroy all session variables
                 Session.Remove("username");
                 Session.Remove("Authenticate");
                 return RedirectToAction("Index", "Home");
             } 
            //return View();
        }

        public ActionResult RecentSearch()
        {
            return View();
        }

        public ActionResult userActivity()
        {
            return View();
        }

        public ActionResult OntologySearch()
        {
            return View();
        }



        public string loadVideoSearchLog(
string p_video_id,
string p_Search_keyword,
string p_title,
string p_url,
string p_channel_title,
string p_published_at,
string p_view_count,
string p_like_count,
string p_favourite_count,
string p_dislike_count,
string p_comment_count,
string p_thumbnail_url)
        {

            SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["YoutubeDashboardContext"].ConnectionString);

            Conn.Open();

            string user_id = "";
            string uname = Session["username"].ToString(); 
            string get_userid = "select  id from app_users where username ='" + uname + "'";

            SqlCommand cmdselectUser = new SqlCommand(get_userid, Conn);

            SqlDataReader reader = cmdselectUser.ExecuteReader();

            while (reader.Read())
            {
                user_id = user_id + reader["id"];
            }
            reader.Close();

            //Query to insert data
            string query = "insert into video_search_log(user_id, video_id, Search_keyword, title, url, channel_title, published_at, view_count, like_count , favourite_count, dislike_count,comment_count,thumbnail_url,date_created)values('" + user_id + "','" + p_video_id + "','" + p_Search_keyword + "','" + p_title + "','" + p_url + "','" + p_channel_title + "','" + p_published_at + "','" + p_view_count + "','" + p_like_count + "','" + p_favourite_count + "','" + p_dislike_count + "','" + p_comment_count + "','" + p_thumbnail_url + "'," + "convert(varchar(19) , GETDATE(), 20))";

            //Build a command that will execute your SQL
            SqlCommand cmdInsert = new SqlCommand(query, Conn);

            cmdInsert.ExecuteNonQuery();

            return "success";
        }

        public string RankVideos(
string p_Search_keyword,
string p_rank_keyword)
        {

            SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["YoutubeDashboardContext"].ConnectionString);

            Conn.Open();

            string user_id = "";
            string uname = Session["username"].ToString();
            string get_userid = "select  id from app_users where username ='" + uname + "'";

            SqlCommand cmdselectUser = new SqlCommand(get_userid, Conn);

            SqlDataReader reader = cmdselectUser.ExecuteReader();

            while (reader.Read())
            {
                user_id = user_id + reader["id"];
            }
            reader.Close();

            //Query to insert data
            string query = "";
            if (p_rank_keyword == "Rank")
            {
                 query = "select * from video_search_log where rtrim(ltrim(Search_keyword))  ='" + p_Search_keyword + "' and user_id=" + user_id + " order by  view_count  desc , like_count desc, comment_count desc , dislike_count asc";
            }
            else
            {
                 query = "select * from video_search_log where rtrim(ltrim(Search_keyword))  ='" + p_Search_keyword + "'  and user_id =" + user_id + " order by " + p_rank_keyword + " desc";
            }
            //Conn.Close();
            SqlCommand cmd1 = new SqlCommand(query, Conn);
            //Build a command that will execute your SQL
            DataTable dt = new DataTable();
            cmd1.ExecuteNonQuery();
            SqlDataAdapter objDataAdapter = new SqlDataAdapter(cmd1);
            objDataAdapter.Fill(dt);
            Conn.Close();

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            String result = serializer.Serialize(rows);
            return result;

        }

        public string recentSearchController()
        {
            SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["YoutubeDashboardContext"].ConnectionString);

            Conn.Open();

            string user_id = "";
            string uname = Session["username"].ToString();
            string get_userid = "select  id from app_users where username ='" + uname + "'";


            SqlCommand cmdselectUser = new SqlCommand(get_userid, Conn);

            SqlDataReader reader = cmdselectUser.ExecuteReader();

            while (reader.Read())
            {
                user_id = user_id + reader["id"];
            }
            reader.Close();

            //Query to get recent search data
            string query = "select top 10 b.* from (select a.* , rank() over (partition by user_id , Search_keyword  order by date_created desc , view_count desc) rnk from video_search_log a)b where user_id=" + user_id + " and rnk=1 order by date_created desc , view_count desc, like_count desc";
            SqlCommand cmd1 = new SqlCommand(query, Conn);
            DataTable dt = new DataTable();
            cmd1.ExecuteNonQuery();
            SqlDataAdapter objDataAdapter = new SqlDataAdapter(cmd1);
            objDataAdapter.Fill(dt);
            Conn.Close();

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            String result = serializer.Serialize(rows);
            return result;

        }


         public string userLogActivity()
        {
            SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["YoutubeDashboardContext"].ConnectionString);

            Conn.Open();

            //Query to get recent search data
            string query = "select user_id , username , user_action , Login_count , case when Login_count>50 then 'Frequent User' when login_count > 0 then 'Normal User' when login_count = 0 then 'Idle User' end user_activity_type from ( select b.id user_id , b.username , user_action , count(action_result) Login_count from  app_users b  left outer join user_activity_log a on ( a.user_id=b.id)  group by b.id ,b.username ,  user_action)c order by login_count desc";
   SqlCommand cmd1 = new SqlCommand(query, Conn);
            DataTable dt = new DataTable();
            cmd1.ExecuteNonQuery();
            SqlDataAdapter objDataAdapter = new SqlDataAdapter(cmd1);
            objDataAdapter.Fill(dt);
            Conn.Close();

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            String result = serializer.Serialize(rows);
            return result;

        }


        /*
        private string send_email(username string)
        {

            SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["YoutubeDashboardContext"].ConnectionString);

            Conn.Open();

            string to_address="";
             string query = "select  email_id from app_users where username ='" + username + "'";


            SqlCommand cmdselectUser = new SqlCommand(query, Conn);

            SqlDataReader reader = cmdselectUser.ExecuteReader();

            while (reader.Read())
            {
                to_address = to_address + reader["email_id"];
            }

            reader.Close();
            Conn.close();
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("shravss123@gmail.com");
                mail.To.Add(to_address);
                mail.Subject = "Test Mail";
                mail.Body = "This is for testing SMTP mail from GMAIL";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("shravss123@gmail.com", "NaglgP*()");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                MessageBox.Show("mail Send");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
              return "success";
        }
         
         */
        

    }

}
