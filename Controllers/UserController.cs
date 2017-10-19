using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using the_project.Models;
using System.Linq;

namespace the_project.Controllers
{
    public class UserController : Controller
    {
        private ActivityContext _context;
        public UserController(ActivityContext context)
        {
            _context = context;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.Error = TempData["Login"];
            return View();
        }
        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                //Queries through the database and checks if that email registered already exists in the database if so it will throw an error
                User DbUser = _context.Users.SingleOrDefault(User => User.Email == model.Email);
                if(DbUser != null)
                {
                    ViewBag.Error = "Email already exist please try another one";
                    return View("Index");
                }
                User NewUser = new User
                {
                    First_Name = model.First_Name,
                    Last_Name = model.Last_Name,
                    Email = model.Email,
                    Password = model.Password
                };
                _context.Users.Add(NewUser);
                _context.SaveChanges();
                return View("Index");
            }
            return View("Index", model);
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(string Email, string Password)
        {
            //Queries through the database looks inside user table and looks if the email passed through login is the same as the one from the database
            User DbUser = _context.Users.SingleOrDefault(User => User.Email == Email);
            if(DbUser == null)
            {
                ViewBag.Error = "Please check if everything is correct";
                return View("Index");
            }
            //If both email and password in the login match from the user database then its a success and store user in session and redirect the user to dashboard and 
            if((string)DbUser.Email == Email && (string)DbUser.Password == Password)
            {
                HttpContext.Session.SetInt32("User_Id", (int)DbUser.User_Id);
                return RedirectToAction("Home", "Activities");
            }
            ViewBag.Error = "Please check if everything is correct";
            return View("Index");
        }
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
