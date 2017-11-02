using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Controllers
{
    public class LogRegController : Controller
    {
        private WeddingPlannerContext _context;
        public LogRegController(WeddingPlannerContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {

            if (HttpContext.Session.GetInt32("user_id") != null)
            {
                return RedirectToAction("Dashboard", "Main");
            }
            return View();
        }

        [HttpPost]
        [Route("Register")]

        public IActionResult Register(RegisterViewModel model)
        {
            List<User> existingEmails = _context.users.Where(item => item.email == model.email).ToList();
            if (existingEmails.Count > 0)
            {
                TempData["emailError"] = "A user with that email already exists";
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                User user = new User
                {
                    firstName = model.firstName,
                    lastName = model.lastName,
                    email = model.email,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                    password = ""
                };
                user.password = Hasher.HashPassword(user, model.password);
                _context.users.Add(user);
                _context.SaveChanges();

                
                User addedUser = _context.users.SingleOrDefault(item => item.email == model.email);
                HttpContext.Session.SetInt32("user_id", addedUser.id);
                return RedirectToAction("Dashboard", "Main");
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        [Route("Login")]

        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<User> users = _context.users.Where(item => item.email == model.email).ToList();
                if (users.Count < 1)
                {
                    TempData["loginError"] = "No user with that email found";
                    return RedirectToAction("Index");
                }
                User user = users[0];
                var Hasher = new PasswordHasher<User>();
                if (Hasher.VerifyHashedPassword(user, user.password, model.password) != 0)
                {
                    HttpContext.Session.SetInt32("user_id", user.id);
                    return RedirectToAction("Dashboard", "Main");
                }
                else
                {
                    TempData["loginError"] = "That password does not match what is on record";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View(model);
            }
        }

        
    }
}