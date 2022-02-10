using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToDoListWebApplications.Models;

namespace ToDoListWebApplications.Controllers
{
    public class TaskController : Controller
    {
        private ApplicationDbContext task = new ApplicationDbContext();

        public IEnumerable<ToDo> GetMyTasks()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = task.Users.FirstOrDefault(model => model.Id == currentUserId);

            return task.ToDos.ToList().Where(model => model.User == currentUser);
        }
        public ActionResult BuildToDoTable()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = task.Users.FirstOrDefault(model => model.Id == currentUserId);

            return PartialView("_TodosTable", task.ToDos.ToList().Where(model => model.User == currentUser));
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        // GET: Task
        public ActionResult Index(string sortOrder, string searchString = "")
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = task.Users.FirstOrDefault(model => model.Id == currentUserId);            

            ViewBag.PrioritySortParm = String.IsNullOrEmpty(sortOrder) ? "priority_desc" : "";
            ViewBag.DateSortParm = sortOrder == "CreationDate" ? "" : "creationdate_desc";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name" : "";
            ViewBag.searchString = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                return View(task.ToDos.Where(temp => temp.ToDoName.Contains(searchString) || temp.ToDoDescription.Contains(searchString)).ToList());

            }
       
            switch (sortOrder)
            {
                case "priority_desc":
                    return View(task.ToDos.OrderBy(temp => temp.Priority).ToList()
                                                                        .Where(temp => temp.User == currentUser)
                                                                                  .Where(temp => temp.ToDoName.Contains(searchString)
                                                                                                              || temp.ToDoDescription.Contains(searchString)).ToList());
                case "creationdate_desc":
                    return View(task.ToDos.OrderBy(temp => temp.DueDate).ToList()
                                                                        .Where(temp => temp.User == currentUser)
                                                                                  .Where(temp => temp.ToDoName.Contains(searchString)
                                                                                                              || temp.ToDoDescription.Contains(searchString)).ToList());
                case "name":
                    return View(task.ToDos.OrderBy(temp => temp.ToDoName).ToList()
                                                                        .Where(temp => temp.User == currentUser)
                                                                                  .Where(temp => temp.ToDoName.Contains(searchString)
                                                                                                              || temp.ToDoDescription.Contains(searchString)).ToList());
                default:
                    return View(task.ToDos.OrderByDescending(temp => temp.CreationDate).ToList()
                                                                        .Where(temp => temp.User == currentUser)
                                                                                  .Where(temp => temp.ToDoName.Contains(searchString)
                                                                                                              || temp.ToDoDescription.Contains(searchString)).ToList());
            }
        }
        // GET: Task/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = task.ToDos.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            return View(toDo);
        }

        // GET: Task/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Task/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "ToDoID,ToDoName,ToDoDescription,CreationDate,DueDate,Priority,Status,Image,ImageFile")] */ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                //get current user object
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = task.Users.FirstOrDefault(model => model.Id == currentUserId);

                toDo.User = currentUser;
                toDo.Status = false;
                toDo.CreationDate = DateTime.Now;

                string fileName = Path.GetFileName(toDo.ImageFile.FileName);

                string _filename = DateTime.Now.ToString("yymmssfff") + fileName;
                string extension = Path.GetExtension(toDo.ImageFile.FileName);

                string path = Path.Combine(Server.MapPath("../Image/"), _filename);

                toDo.Image = "../Image/" + _filename;

                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png" || extension.ToLower() == ".gif")
                {
                    task.ToDos.Add(toDo);
                    toDo.ImageFile.SaveAs(path);
                    task.SaveChanges();
                    ModelState.Clear();
                }
                else
                {
                    ViewBag.msg = "Invalid File Format";
                    return View();
                }

                return RedirectToAction("Index");
            }

            else
            {
                return View();
            }
        }

        // GET: Task/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = task.ToDos.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            return View(toDo);
        }

        // POST: Task/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(/*[Bind(Include = "ToDoID,ToDoName,ToDoDescription,DueDate,Priority,Status,Image,ImageFile")] */ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                var edit = task.ToDos.Where(temp => temp.ToDoID == toDo.ToDoID).FirstOrDefault();
                edit.ToDoName = toDo.ToDoName;
                edit.ToDoDescription = toDo.ToDoDescription;
                edit.DueDate = toDo.DueDate;
                edit.Priority = toDo.Priority;
                edit.Status = toDo.Status;
                task.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IndexEdit(int? id, bool statusValue) //Checkbox click gets id 
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = task.ToDos.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            else
            {
                toDo.Status = statusValue;

                task.Entry(toDo).State = EntityState.Modified;
                task.SaveChanges();
                return PartialView("_TodosTable", GetMyTasks());
            }
        }

        // GET: Task/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = task.ToDos.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            return View(toDo);
        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ToDo toDo = task.ToDos.Find(id);
            task.ToDos.Remove(toDo);
            task.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                task.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
