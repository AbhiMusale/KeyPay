using KeyPay.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace KeyPay.Controllers
{
    public class UserController : Controller
    {

        UsersConfigModel usersConfigModel = new UsersConfigModel();
        // GET: User/Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            using (UsersConfigModel usersConfigModel = new UsersConfigModel())
            {
                var isUserExists = usersConfigModel.Users.Any(x => x.UserName == user.UserName);
                if (isUserExists)
                {
                    //string dbEncryptedPassword = usersConfigModel.Users.Where(x => x.UserName == user.UserName).Select(x => x.Password).FirstOrDefault();
                    //string dbDecryptedPassword = DecryptData(dbEncryptedPassword);
                    string Password = usersConfigModel.Users.Where(x => x.UserName == user.UserName).Select(x => x.Password).FirstOrDefault();
                    if (user.Password == Password)
                    {
                        return RedirectToAction("Configuration", "User");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Password incorrect!";
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "User does not exists!";
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Configuration()
        {
            //using (UsersConfigModel usersConfigModel = new UsersConfigModel())
            {
                //var data = usersConfigModel.Configurations.FirstOrDefault();
                if (ViewBag.Data == null)
                    ViewBag.Data = usersConfigModel.Configurations.FirstOrDefault();
                return View(ViewBag.Data);
            }
        }

        [HttpPost]
        public ActionResult Configuration(Configuration configuration)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //if ((configuration.Department == configuration.Location) || (configuration.Department == configuration.Project) || (configuration.Location == configuration.Project) || ((configuration.Department == configuration.Location) && (configuration.Department == configuration.Project) && (configuration.Location == configuration.Project)))
                    if ((configuration.Department == configuration.Location) || (configuration.Department == configuration.Project) || (configuration.Location == configuration.Project))
                    {
                        ViewBag.Message = "Some Dimensions are similar!";
                        return View();
                    }
                    usersConfigModel.Configurations.AddOrUpdate(configuration);
                    usersConfigModel.SaveChanges();
                    ViewBag.Message = "Update successful!";
                    return RedirectToAction("Configuration");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        string DecryptData(string data)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            string encryptedData = string.Join("", ProtectedData.Unprotect(byteData, new byte[] { }, DataProtectionScope.CurrentUser));
            return encryptedData;
        }
    }
}