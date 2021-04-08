using KeyPay.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
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
                using (UsersConfigModel usersConfigModel = new UsersConfigModel())
                {
                    //byte[] encryptedPassword = EncryptData(user.strPassword);
                    //user.Password = encryptedPassword;
                    var isExists = usersConfigModel.Users.Any(x => x.UserName == user.UserName);
                    if (!isExists)
                    {
                        user.Password = EncryptData(user.strPassword);
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
                ViewBag.ErrorMessage = $"Registration failed! ({ex.Message})";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Registration failed! ({ex.Message})";
                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(int ID)
        {
            using (UsersConfigModel usersConfigModel = new UsersConfigModel())
            {
                ModelState.Clear();
                var user = usersConfigModel.Users.Where(x => x.ID == ID).FirstOrDefault();
                return View(user);
            }
        }

        // POST: Register/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // TODO: Add update logic here
                    using (UsersConfigModel usersConfigModel = new UsersConfigModel())
                    {
                        user.Password = EncryptData(user.strPassword);
                        //usersConfigModel.Users.AddOrUpdate(user);
                        usersConfigModel.Entry(user).State = EntityState.Modified;
                        usersConfigModel.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
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

        //string EncryptData(string data)
        //{
        //    byte[] byteData = Encoding.UTF8.GetBytes(data);
        //    string encryptedData = string.Join("", ProtectedData.Protect(byteData, new byte[] { }, DataProtectionScope.CurrentUser));
        //    return encryptedData;
        //}

        //byte[] EncryptData(string data)
        //{
        //    byte[] byteData = Encoding.UTF8.GetBytes(data);
        //    byte[] encryptedData = ProtectedData.Protect(byteData, new byte[] { }, DataProtectionScope.CurrentUser);
        //    return encryptedData;
        //}

        byte[] EncryptData(string data)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            byte[] encryptedData = ProtectedData.Protect(byteData, new byte[] { }, DataProtectionScope.CurrentUser);
            return encryptedData;
        }

        string DecryptData(byte[] data)
        {
            byte[] decryptedData = ProtectedData.Unprotect(data, new byte[] { }, DataProtectionScope.CurrentUser);
            string decryptedString = Encoding.UTF8.GetString(decryptedData);
            return decryptedString;
        }

    }
}
