 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HelpDesk.Models;
using HelpDeskData;

namespace HelpDesk.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Register()
        {
            return View("register");
        }

        public ActionResult RegisterStaff()
        {
            return View("registerStaff");
        }

        [HttpGet]
        public ActionResult CreateUser(ParamUser model)
            {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Nama) || string.IsNullOrEmpty(model.Password))
            {
                ViewBag.ErrorRegisterUser = "Mohon Data Dipastikan Lengkap Dan Sesuai";
                return View("Register");
            }
            if (model.Password.Length < 8)
            {
                ViewBag.ErrorRegisterUser = "Password Minimal 8 Karakter";
                return View("Register");
            }
            if (model.Password.Length > 8)
            {
                ViewBag.ErrorRegisterUser = "Password Maksimal 8 Karakter";
                return View("Register");
            }
            HelpDeskData.User dt = new HelpDeskData.User();
            string ID = GenerateID();
            model.ID = ID.ToString();
            model.Role = 3;
            dt.ID = model.ID.ToString();
            dt.Nama = model.Nama;
            dt.Email = model.Email;
            dt.Password = model.Password;
            dt.Role = model.Role;
            dt.Insert("");
            return View("Login");
        }

        [HttpPost]
        public ActionResult CreateStaff(ParamUser model)
        {
            if (model.Password.Length != 8)
            {
                ViewBag.ErrorStaff = "Password Harus 8 Karakter";
                return View("RegisterStaff");
            }
            if (!model.Email.Contains("@"))
            {
                ViewBag.ErrorStaff = "Masukan Email Dengan Format Yang Benar";
                return View("RegisterStaff");
            }

            HelpDeskData.User dt = new HelpDeskData.User();
            string ID = GenerateID();
            model.ID = ID.ToString();
            model.Role = 2;
            dt.ID = model.ID;
            dt.Nama = model.Nama;
            dt.Email = model.Email;
            dt.Password = model.Password;
            dt.Role = model.Role;
            dt.Insert("");
            return Redirect("https://localhost:44306/User/ManageStaffAccount");
        }

        [HttpGet]
        public ActionResult ManageStaffAccount()
        {
            List<ParamUser> model = new List<ParamUser>();
            var data = HelpDeskData.User.GetByRole(2);
            foreach (var item in data)
            {
                ParamUser dt = new ParamUser();
                dt.ID = item.ID;
                dt.Nama = item.Nama;
                dt.Email = item.Email;
                dt.Password = item.Password;
                dt.Keterangan = item.Keterangan;

                model.Add(dt);
            }
            ViewBag.DataStaff = model;

            return View("ManageStaffAccount");
        }

        [HttpPost]
        public ActionResult UpdateDataStaff(ParamUser model)
        {

            var dt = HelpDeskData.User.GetByID(model.ID);
            dt.ID = model.ID;
            dt.Nama = model.Nama;
            dt.Email = model.Email;
            dt.Password = model.Password;
            dt.Keterangan = model.Keterangan;
            dt.Update("");
            if (Session["type"] != null && Session["resulttype"] != null)
                return View("ManageStaffAccount");
            else
                return Redirect(Request.UrlReferrer.ToString());
        }
        [HttpPost]
        public ActionResult DeleteDataStaff(ParamUser model)
        {
            //string baseurl = "https://localhost:44306/Form/TicketTableAdmin?pic=";
            var dt = HelpDeskData.User.GetByID(model.ID);
            dt.ID = model.ID;
            dt.Nama = model.Nama;
            dt.Email = model.Email;
            dt.Keterangan = model.Keterangan;
            dt.Password = model.Password;
            dt.IsDelete = true;
            dt.Update(model.ID);
            //  return Redirect(baseurl+model.PIC);
            if (Session["type"] != null && Session["resulttype"] != null)
                return View("UpdateDataStaff");
            else
                return Redirect(Request.UrlReferrer.ToString());
        }

        public static string GenerateID() 
        {
            int length = 7;

            // creating a StringBuilder object()
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }
            return str_build.ToString();
        }

    }
}