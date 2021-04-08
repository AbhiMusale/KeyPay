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
using System.Web.Security;
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
        [AllowAnonymous]
        public ActionResult Login(User user)
        {
            using (UsersConfigModel usersConfigModel = new UsersConfigModel())
            {
                var isUserExists = usersConfigModel.Users.Any(x => x.UserName == user.UserName);
                if (isUserExists)
                {
                    byte[] dbPassword = usersConfigModel.Users.Where(x => x.UserName == user.UserName).Select(x => x.Password).FirstOrDefault();
                    string dbDecryptedPassword = DecryptData(dbPassword);
                    if (user.strPassword == dbDecryptedPassword)
                    {
                        FormsAuthentication.SetAuthCookie(user.UserName, true);
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
            ModelState.Clear();
            var data = usersConfigModel.Configurations.FirstOrDefault();
            data.strIntacctUserName = DecryptData(data.IntacctUserName);
            data.strIntacctPassword = DecryptData(data.IntacctPassword);
            data.strIntacctSenderPassword = DecryptData(data.IntacctSenderPassword);
            data.strKeyPayAPI = DecryptData(data.KeyPayAPI);
            data.strEmailPassword = DecryptData(data.EmailPassword);
            return View(data);
        }

        [HttpPost]
        public ActionResult Configuration(Configuration configuration)
        {
            try
            {
                //if (ModelState.IsValid)
                {
                    //if ((configuration.Department == configuration.Location) || (configuration.Department == configuration.Project) || (configuration.Location == configuration.Project) || ((configuration.Department == configuration.Location) && (configuration.Department == configuration.Project) && (configuration.Location == configuration.Project)))
                    if ((configuration.Department == configuration.Location) || (configuration.Department == configuration.Project) || (configuration.Location == configuration.Project))
                    {
                        ViewBag.Message = "Some Dimensions are similar!";
                        return View();
                    }
                    configuration.IntacctUserName = EncryptData(configuration.strIntacctUserName);
                    configuration.IntacctPassword = EncryptData(configuration.strIntacctPassword);
                    configuration.IntacctSenderPassword = EncryptData(configuration.strIntacctSenderPassword);
                    configuration.KeyPayAPI = EncryptData(configuration.strKeyPayAPI);
                    configuration.EmailPassword = EncryptData(configuration.strEmailPassword);
                    usersConfigModel.Configurations.AddOrUpdate(configuration);
                    usersConfigModel.SaveChanges();
                    ViewBag.Message = "Update successful!";
                    return RedirectToAction("Configuration");
                }
                //else
                //{
                //    return View();
                //}
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        //string DecryptData(string data)
        //{
        //    byte[] byteData = Encoding.UTF8.GetBytes(data);
        //    string encryptedData = string.Join("", ProtectedData.Unprotect(byteData, new byte[] { }, DataProtectionScope.CurrentUser));
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