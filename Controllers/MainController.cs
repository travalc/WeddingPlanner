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
    public class MainController : Controller
    {
        private WeddingPlannerContext _context;
        public MainController(WeddingPlannerContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetInt32("user_id") == null)
            {
                return RedirectToAction("Index", "LogReg");
            }

            List<Wedding> weddings = _context.weddings
                .Include(w => w.rsvps)
                    .ThenInclude(r => r.user)
                .ToList();
            
            ViewBag.weddings = weddings;
            ViewBag.currentUserId = (int)HttpContext.Session.GetInt32("user_id");
            return View();
        }

        [HttpGet]
        [Route("Wedding/{id}")]

        public IActionResult Wedding(string id)
        {
            int intid = Convert.ToInt32(id);
            Wedding wedding = _context.weddings.Where(item => item.id == intid).Include(item => item.rsvps).ThenInclude(r => r.user).SingleOrDefault();
            ViewBag.wedding = wedding;
            return View();
        }

        [HttpGet]
        [Route("NewWedding")]

        public IActionResult NewWedding()
        {
            return View();
        }

        [HttpGet]
        [Route("Logout")]

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "LogReg");
        } 

        [HttpPost]
        [Route("Create")]

        public IActionResult Create(WeddingViewModel model)
        {
            if (ModelState.IsValid)
            {
                Wedding wedding = new Wedding() {
                    creator_id = (int)HttpContext.Session.GetInt32("user_id"),
                    wedderOne = model.wedderOne,
                    wedderTwo = model.wedderTwo,
                    address = model.address,
                    date = model.date,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now
                };

                _context.weddings.Add(wedding);
                _context.SaveChanges();
                
                string stringid = wedding.id.ToString();
                //Wedding addedWedding = _context.weddings.Where(item => item.wedderOne == model.wedderOne).Where(item => item.wedderTwo == model.wedderTwo).SingleOrDefault();
                Dictionary<string, string> obj = new Dictionary<string, string>() {
                    {"id", stringid}
                };
                return RedirectToAction("Wedding", obj);
            }
            else
            {
                return View("NewWedding", model);
            }
        }

        [HttpGet]
        [Route("Delete/{id}")]

        public IActionResult Delete(int id)
        {
            Wedding wedding = _context.weddings.Where(item => item.id == id).SingleOrDefault();
            _context.weddings.Remove(wedding);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("RSVP/{id}")]

        public IActionResult RSVP(int id)
        {
            User user = _context.users.SingleOrDefault(item => item.id == (int)HttpContext.Session.GetInt32("user_id"));
            Wedding wedding = _context.weddings.SingleOrDefault(item => item.id == id);

            RSVP rsvp = new RSVP() {
                users_id = user.id,
                user = user,
                weddings_id = wedding.id,
                wedding = wedding
            };

            _context.rsvps.Add(rsvp);
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("UNRSVP/{id}")]

        public IActionResult UNRSVP(int id)
        {
            User user = _context.users.SingleOrDefault(item => item.id == (int)HttpContext.Session.GetInt32("user_id"));
            Wedding wedding = _context.weddings.SingleOrDefault(item => item.id == id);
            RSVP rsvp = _context.rsvps.Where(item => item.users_id == user.id).Where(item => item.weddings_id == wedding.id).SingleOrDefault();

            _context.rsvps.Remove(rsvp);
            return RedirectToAction("Dashboard");
        }
    }
}