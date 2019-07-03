using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DroneWebConsole.Controllers
{
    [Route("Auth")]
    public class AuthController : Controller
    {
        //check for admin account create admin account if one does not exist
        public void adminAccount()
        {
            var ucol = globals.getDB().GetCollection<mUsers>("mUsers");
            var admin = ucol.Find(i => i._id != null).FirstOrDefault();
            if (admin == null)
            {
                var na = new mUsers();
                na.username = "admin";
                na.password = globals.getmd5("admin");
                ucol.InsertOne(na);
            }
        }


        [HttpGet("Login")]
        public IActionResult Login(string msg)
        {
            adminAccount();
            ViewBag.msg = msg;
            return View();
        }


        [HttpPost("Login")]
        public IActionResult Login(string username,string password)
        {
            var ucol = globals.getDB().GetCollection<mUsers>("mUsers");
            var user = ucol.Find(i => i.username == username && i.password == globals.getmd5(password)).FirstOrDefault();
            if(user==null)
            {
                return RedirectToAction("Login", "Auth", new { msg = "invalid credentials" });
            }
            else
            {
                HttpContext.Session.SetString("email", username);
               return RedirectToAction("Dashboard", "Admin");
            }
        }
    }
}
