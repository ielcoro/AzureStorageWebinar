using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserPictureStorer.Models;
using UserPictureStorer.Repositories;

namespace UserPictureStorer.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        public ActionResult List()
        {
            using (var userRepository = new UserRepository())
            {
                return View(userRepository.Users);
            }
        }

        //
        // GET: /User/Details/5
        public ActionResult Details(string partitionKey, string rowKey)
        {
            return View();
        }

        //
        // GET: /User/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create
        [HttpPost]
        public ActionResult Create(User user)
        {
            try
            {
                using (var userRepository = new UserRepository())
                {
                    userRepository.AddObject("Users", user);
                    userRepository.SaveChanges();
                }

                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }

        
        
    }
}
