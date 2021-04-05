using KeyPay.Models;
using System;
using System.Collections.Generic;
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
                    string dbEncryptedPassword = usersConfigModel.Users.Where(x => x.UserName == user.UserName).Select(x => x.Password).FirstOrDefault();
                    string dbDecryptedPassword = DecryptData(dbEncryptedPassword);
                    if (user.Password == dbDecryptedPassword)
                    {
                        return RedirectToAction("Configuration", "User");
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Configuration(UsersConfigModel usersConfigModel)
        {
            //var data = usersConfigModel.Configurations.FirstOrDefault();
            if (ViewBag.Data == null)
                ViewBag.Data = usersConfigModel.Configurations.FirstOrDefault();
            return View(ViewBag.Data);
        }

        [HttpPost]
        public ActionResult Configuration(Configuration configuration)
        {
            return View();
        }

        string DecryptData(string data)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            string encryptedData = string.Join("", ProtectedData.Unprotect(byteData, new byte[] { }, DataProtectionScope.CurrentUser));
            return encryptedData;
        }
    }
}