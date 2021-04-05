using KeyPay.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace KeyPay.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register/Index
        public ActionResult Index()
        {
            using (UsersConfigModel usersConfigModel = new UsersConfigModel())
            {
                return View(usersConfigModel.Users.ToList());
            }
        }

        // GET: Register/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Register/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Register/Create
        [HttpPost]
        public ActionResult Create(User user)
        {
            try
            {
                // TODO: Add insert logic here
                using (UsersConfigModel usersConfigModel = new UsersConfigModel())
                {
                    //string encryptedPassword = EncryptData(user.Password);
                    //user.Password = encryptedPassword;
                    var isExists = usersConfigModel.Users.Any(x => x.UserName == user.UserName);
                    if (!isExists)
                    {
                        usersConfigModel.Users.Add(user);
                        usersConfigModel.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "User exists!";
                    }
                }

                return View();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    //Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        //Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }
                ViewBag.ErrorMessage = "Registration failed!";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Registration failed!";
                return View();
            }
        }

        // GET: Register/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Register/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Register/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Register/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        string EncryptData(string data)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            string encryptedData = string.Join("", ProtectedData.Protect(byteData, new byte[] { }, DataProtectionScope.CurrentUser));
            return encryptedData;
        }
    }
}
