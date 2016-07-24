using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdeaMgmt.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {


            ////remove cookie
            //var remcookie = new HttpCookie("cookieName");
            //cookie.Expires = DateTime.Now.AddDays(-1d);
            //Response.Cookies.Add(remcookie);

            //To Request the cookies value
            //var val = Request.Cookies["cookieName"].Value;
            ///////////////////////////////////////////////

            //ViewData["Message"] = "Welcome to ASP.NET MVC!";
            //string cookie = "There is no cookie!";
            //if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("Cookie"))
            //{
            //    cookie = "Yeah - Cookie: " + this.ControllerContext.HttpContext.Request.Cookies["Cookie"].Value;
            //}
            //ViewData["Cookie"] = cookie;
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string un, string p)
        {

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            SqlCommand cmd = new SqlCommand("Select * from UserMgmt where userid = '" + un + "' and password = '" + p + "'");
            SqlTransaction trans;
            conn.Open();
            trans = conn.BeginTransaction("Validate");
            cmd.Connection = conn;
            cmd.Transaction = trans;
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                da.SelectCommand = cmd;
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    TempData["notice"] = "Login Sucessfull!!!";
                    //Creating Cookie
                    var cookie = new HttpCookie("logincookie");
                    cookie.Value = un;
                    Response.Cookies.Add(cookie);
                    string k=un;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["notice"] = "Login Failed!!! Please try again.";
                }
            }
            catch (Exception ex)
            {
                Console.Write("Commit Exception Type: {0}", ex.GetType());
                Console.Write("  Message: {0}", ex.Message);
                try
                {
                    //trans.Rollback();
                    return Content("Error");
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred 
                    // on the server that would cause the rollback to fail, such as 
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(string email, string username, string password, string firstname, string lastname, string mobileno)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            conn.Open();
            string query = "INSERT INTO UserMgmt (userid,password,firstname,lastname,email,mobileno) VALUES(@userid,@password,@firstname,@lastname,@email,@mobileno)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userid", username);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@firstname", firstname);
            cmd.Parameters.AddWithValue("@lastname", lastname);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@mobileno", mobileno);
            SqlTransaction trans = conn.BeginTransaction("Transaction");
            cmd.Connection = conn;
            cmd.Transaction = trans;

            try
            {
                cmd.ExecuteNonQuery();
                trans.Commit();
                conn.Close();
                TempData["notice"] = "SignUp Sucessfull!!! Have a nice day :)";
                return RedirectToAction("Index","Home");
            }
            catch (Exception ex)
            {
                Console.Write("Commit Exception Type: {0}", ex.GetType());
                Console.Write("  Message: {0}", ex.Message);
                try
                {
                    trans.Rollback();
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred 
                    // on the server that would cause the rollback to fail, such as 
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        public ActionResult Idea()
        {
            return RedirectToAction("Index","Ideas");
        }
        //[HttpPost]
        //public ActionResult Idea()
        //{
        //    //string username, string date,string title, string category, string description,string status, string members, string votes
        //    //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        //    //conn.Open();
        //    //string query = "INSERT INTO Idea (userid,date,title,category,description,status,members,votes) VALUES(@userid,@date,@title,@category,@description,@status,@members,@votes)";
        //    //SqlCommand cmd = new SqlCommand(query, conn);
        //    //cmd.Parameters.AddWithValue("@userid", username);
        //    //cmd.Parameters.AddWithValue("@date", date);
        //    //cmd.Parameters.AddWithValue("@title", title);
        //    //cmd.Parameters.AddWithValue("@category", category);
        //    //cmd.Parameters.AddWithValue("@description", description);
        //    //cmd.Parameters.AddWithValue("@status", status);
        //    //cmd.Parameters.AddWithValue("@members", members);
        //    //cmd.Parameters.AddWithValue("@votes", votes);
        //    //SqlTransaction trans = conn.BeginTransaction("Transaction");
        //    //cmd.Connection = conn;
        //    //cmd.Transaction = trans;

        //    //try
        //    //{
        //    //    cmd.ExecuteNonQuery();
        //    //    trans.Commit();
        //    //    conn.Close();
        //    //    TempData["notice"] = "Idea Submitted!!! Don't Worry if idea is not that best, Well Execution of your work comes first :)";
        //    //    return RedirectToAction("Index", "Home");
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    Console.Write("Commit Exception Type: {0}", ex.GetType());
        //    //    Console.Write("  Message: {0}", ex.Message);
        //    //    try
        //    //    {
        //    //        trans.Rollback();
        //    //        TempData["notice"] = "Idea Submission Failed!!! Sorry, We just had a bad time :(";
        //    //        return RedirectToAction("Index", "Home");
        //    //    }
        //    //    catch (Exception ex2)
        //    //    {
        //    //        // This catch block will handle any errors that may have occurred 
        //    //        // on the server that would cause the rollback to fail, such as 
        //    //        // a closed connection.
        //    //        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
        //    //        Console.WriteLine("  Message: {0}", ex2.Message);
        //    //    }
        //    //}
        //    return View();
        //}
        public ActionResult Logout()
        {
            var remcookie = new HttpCookie("logincookie");
            remcookie.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(remcookie);
            TempData["notice"] = "Logout Sucessfull!!! Have a nice day :)";
            return RedirectToAction("Index");
        }
    }
}