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

        [HttpPost]
        public ActionResult Filter(string name)
        {
            using (var userRepository = new UserRepository())
            {
                var query = userRepository.Users.Where(u => u.FirstName == name);

                ViewBag.CurrentFilter = name;
                return View("List", query); 
            }
        }

        public ActionResult ClearFilter()
        {
            ViewBag.CurrentFilter = String.Empty;
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFile file)
        {
            var imageStorage = new ImageRepository();

            Uri tempBlobUri = imageStorage.SaveTempFile(file.InputStream);

            ViewBag.TempBlobUri = tempBlobUri;

            return View("Create");
        }
        
    }
}
