using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using YoutubeDashboard.Models;

namespace YoutubeDashboard.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ForgotPassword()
        {
            return View();
        }
       

        public ActionResult About()
        {
           SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["YoutubeDashboardContext"].ConnectionString);

           //Open your connection prior to executing the Command
               Conn.Open();

                //Build your actual query text here
                string query = "select  username , password from app_users";

                //Build a command that will execute your SQL
                SqlCommand cmdselectUser = new SqlCommand(query, Conn);

                //Populate your parameters listed above
                //sqlCommand.Parameters.AddWithValue("@A", "Your Value");
                //Continue with additional parameters here
                //sqlCommand.Parameters.AddWithValue("@A","Your Value");

                
                string uname = "";
                string pwd = "";

         
             
                SqlDataReader reader = cmdselectUser.ExecuteReader();

                while (reader.Read())
            {
                uname = uname + reader["username"];
                pwd = pwd + reader["password"];
            }
                reader.Close();
                Conn.Close();

                ViewBag.Message = uname;
                
            

            
            return View();
        }


        /* Method to authenticate user */
        public string AuthenticateUser(string username, string password)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["YoutubeDashboardContext"].ConnectionString);

            //Open your connection prior to executing the Command
            conn.Open();

            string out_msg ="";
            string err_msg="";
            string O_auth = "";
            string Ocmd_msg = "";
            string O_tkn = "";

            SqlCommand cmd = new SqlCommand("userAuthentication", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@uname", username);
            cmd.Parameters.AddWithValue("@pwd", password);

           // cmd.Parameters.Add("@authenticate_flag", SqlDbType.VarChar).Direction = ParameterDirection.Output;
            //cmd.Parameters.Add("@login_msg", SqlDbType.VarChar).Direction = ParameterDirection.Output;
            
            SqlParameter auth = new SqlParameter();
            auth.ParameterName = "@authenticate_flag";
            //auth varchar(200) = null output;
            auth.SqlDbType = System.Data.SqlDbType.VarChar;
            auth.Direction = System.Data.ParameterDirection.Output;
            auth.Size = 200;
            cmd.Parameters.Add(auth);

            SqlParameter cmd_msg = new SqlParameter();
            cmd_msg.ParameterName = "@login_msg";
            cmd_msg.SqlDbType = System.Data.SqlDbType.VarChar;
            cmd_msg.Direction = System.Data.ParameterDirection.Output;
            cmd_msg.Size = 200;
            cmd.Parameters.Add(cmd_msg);

            SqlParameter tkn_msg = new SqlParameter();
            tkn_msg.ParameterName = "@login_token";
            tkn_msg.SqlDbType = System.Data.SqlDbType.VarChar;
            tkn_msg.Direction = System.Data.ParameterDirection.Output;
            tkn_msg.Size = 200;
            cmd.Parameters.Add(tkn_msg);

            cmd.ExecuteNonQuery();

             O_auth = cmd.Parameters["@authenticate_flag"].Value.ToString();
            Ocmd_msg = cmd.Parameters["@login_msg"].Value.ToString();
            O_tkn = cmd.Parameters["@login_token"].Value.ToString();

            conn.Close();

            switch (O_auth)
            {

                case "Yes":
                    out_msg = "Login Successful";
                    Session["username"] = username;
                    Session["Authenticate"] = "Yes";
                    break;
                case "No":
                    out_msg = "User authentication failed with the below error";
                    err_msg = Ocmd_msg;
                    break;
                default:
                    out_msg = Ocmd_msg;
                    err_msg = Ocmd_msg;
                    break;
            }

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //ViewBag.Login_Message = out_msg;
            //ViewBag.error_msg = err_msg;

            methodOutput MethodOutput = new methodOutput();
            MethodOutput.output_message = out_msg;
            MethodOutput.error_message = err_msg;
            MethodOutput.status = O_auth;
            MethodOutput.tken = O_tkn;
            return serializer.Serialize(MethodOutput);

            

        }


        public string checkPreviousLogin(string uname, string utoken)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["YoutubeDashboardContext"].ConnectionString);

            conn.Open();

            string token_valid = "";

            string query = "select  case when login_token ='" +  @utoken + "'  then 'Yes' else 'No' end token_status from app_users where username ='" + @uname + "'";

            //Build a command that will execute your SQL
            SqlCommand cmdselectUser = new SqlCommand(query, conn);

            SqlDataReader reader = cmdselectUser.ExecuteReader();

            while (reader.Read())
            {
                token_valid = token_valid + reader["token_status"];
            }
            reader.Close();
            conn.Close();

            if(token_valid == "Yes")
            {
                Session["username"] = uname;
                Session["Authenticate"] = "Yes";
            }
            else 
            {
                Session.Remove("username");
                Session.Remove("Authenticate");
            }
            
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            methodOutput MethodOutput = new methodOutput();
            MethodOutput.status = token_valid;
            return serializer.Serialize(MethodOutput);

        }

        public ActionResult Logout()
        {
            Session.RemoveAll(); 
            return RedirectToAction("Index", "Home");
        }
    }
}