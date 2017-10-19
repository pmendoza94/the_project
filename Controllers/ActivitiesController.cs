using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using the_project.Models;

namespace the_project.Controllers
{
    public class ActivitiesController : Controller
    {
        private ActivityContext _context;
        public ActivitiesController(ActivityContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("Home")]
        public IActionResult Home()
        {
            int? ThisUser = HttpContext.Session.GetInt32("User_Id");
            if(ThisUser == null)
            {
                TempData["Login"] = "Must be logged in to view the page. Please login";
                return RedirectToAction("Index", "User");
            }
            User CurrentUser = _context.Users.SingleOrDefault(u => u.User_Id == (int)ThisUser);
            List <Activity> AllActivities = _context.Activities.Include(a => a.Participants).OrderBy(a => a.Date).ToList();
            ViewBag.CurrentUser = CurrentUser;
            ViewBag.AllActivities = AllActivities;
            return View();
        }
        [HttpGet]
        [Route("New")]
        public IActionResult Create()
        {
            int? ThisUser = HttpContext.Session.GetInt32("User_Id");
            if(ThisUser == null)
            {
                TempData["Login"] = "Must be logged in to view the page. Please login";
                return RedirectToAction("Index", "User");
            }
            return View();
        }
        [HttpPost]
        [Route("NewActivity")]
        public IActionResult NewActivity(ActivityViewModel model, string units)
        {
            int? ThisUser = HttpContext.Session.GetInt32("User_Id");
            User CurrentUser = _context.Users.SingleOrDefault(u => u.User_Id == (int)ThisUser);
            if(ModelState.IsValid)
            {
                if(model.Date < DateTime.Now)
                {
                    ViewBag.Error = "Date must be in the future";
                    return View("Create");
                }
                Activity NewActivity = new Activity
                {
                    Title = model.Title,
                    Date = model.Date,
                    Time = model.Time,
                    Duration = model.Duration,
                    Value = units,
                    Description = model.Description,
                    Coordinator = CurrentUser.First_Name,
                    User_Id = (int)ThisUser
                };
                _context.Activities.Add(NewActivity);
                _context.SaveChanges();
                Activity ThisActivity = _context.Activities.Where(a => a.Activity_Id == NewActivity.Activity_Id).SingleOrDefault();
                return RedirectToAction("Info", new {id = ThisActivity.Activity_Id});
            }
            return View("Create", model);
        }
        [HttpGet]
        [Route("activity/{id}")]
        public IActionResult Info(int? id)
        {
            int? ThisUser = HttpContext.Session.GetInt32("User_Id");
            if(ThisUser == null)
            {
                TempData["Login"] = "Must be logged in to view the page. Please login";
                return RedirectToAction("Index", "User");
            }
            Activity ThisActivity = _context.Activities.Where(a => a.Activity_Id == id).Include(a => a.Participants).ThenInclude(a => a.User).SingleOrDefault();
            ViewBag.ThisActivity = ThisActivity;
            return View();
        }
        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            List<Participant> DeleteParticipant = _context.Participants.Where(p => p.Activity_Id == id).ToList();
            _context.Participants.RemoveRange(DeleteParticipant);
            Activity DeleteActivity = _context.Activities.Where(a => a.Activity_Id == id).SingleOrDefault();
            _context.Activities.Remove(DeleteActivity);
            _context.SaveChanges();
            return RedirectToAction("Home");
        }
        [HttpGet]
        [Route("join/{id}")]
        public IActionResult Join(int id)
        {
            int? ThisUser = HttpContext.Session.GetInt32("User_Id");
            Participant NewParticipant = new Participant
            {
                User_Id = (int)ThisUser,
                Activity_Id = id
            };
            _context.Participants.Add(NewParticipant);
            _context.SaveChanges();
            return RedirectToAction("Home");
        }
        [HttpGet]
        [Route("leave/{id}")]
        public IActionResult Leave(int id)
        {
            int? ThisUser = HttpContext.Session.GetInt32("User_Id");
            Participant ThisParticipant = _context.Participants.Where(p => p.User_Id == (int)ThisUser && p.Activity_Id == id).SingleOrDefault();
            _context.Participants.Remove(ThisParticipant);
            _context.SaveChanges();
            return RedirectToAction("Home");
        }
    }
}