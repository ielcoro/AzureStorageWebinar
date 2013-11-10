using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Net;
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
            using (var userRepository = new UserRepository())
            {
                var userQuery = userRepository.Users.Where(u => u.PartitionKey == partitionKey && u.RowKey == rowKey);
                
                return View(userQuery.Single());
            }
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

                    var imageRepository = new ImageRepository();

                    var postedUserPicture = Request.Files.Get("picture");

                    if (postedUserPicture.ContentLength > 0)
                    {
                        Uri pictureUri = imageRepository.SaveFile(user.PartitionKey, user.RowKey, "me.jpg", postedUserPicture.InputStream);

                        user.PictureUrl = pictureUri.ToString();
                    }

                    userRepository.SaveChanges();
                }

                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(string partitionKey, string rowKey)
        {
            using (var userRepository = new UserRepository())
            {
                var userQuery = userRepository.Users.Where(u => u.PartitionKey == partitionKey && u.RowKey == rowKey);

                return View(userQuery.Single());
            }
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            try
            {
                using (var userRepository = new UserRepository())
                {
                    var imageRepository = new ImageRepository();

                    Uri pictureUri = null;
                    var postedUserPicture = Request.Files.Get("picture");

                    if (postedUserPicture.ContentLength > 0)
                        pictureUri = imageRepository.SaveFile(user.PartitionKey, user.RowKey, "me.jpg", postedUserPicture.InputStream);
                    else if (Request.Form["pictureOptions"] != null)
                        pictureUri = new Uri(Request.Form["pictureOptions"].ToString());

                    var currentUser = userRepository.Users
                                                    .Where(u => u.PartitionKey == user.PartitionKey && u.RowKey == user.RowKey)
                                                    .Single();

                    TryUpdateModel(currentUser);
                    currentUser.PictureUrl = pictureUri.ToString();

                    userRepository.UpdateObject(currentUser);
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
                var query = userRepository.Users.Where(u => u.PartitionKey == "Users" && u.RowKey == name);

                ViewBag.CurrentFilter = name;
                return View("List", query); 
            }
        }

        public ActionResult ClearFilter()
        {
            ViewBag.CurrentFilter = String.Empty;
            return RedirectToAction("List");
        }
        
        public JsonResult LoadPreviousPictures(string partitionKey, string rowKey)
        {
            var imageRepository = new ImageRepository();

            IEnumerable<Uri> snapshots = imageRepository.GetSnapshots(partitionKey, rowKey, "me.jpg");

            return Json(snapshots.Select(s => new { Url = s.ToString() }).ToArray(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteAll()
        {
            var userRepository = new UserRepository();

            foreach (var user in userRepository.Users)
            {
                userRepository.DeleteObject(user);
            }

            userRepository.SaveChanges(SaveChangesOptions.Batch);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}
