using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelpDesk.Models;
using HelpDeskData;


namespace HelpDesk.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login() //buat buka halaman login
        {
            HttpCookie cookie = Request.Cookies["LoginHelpDesk"]; //isinya nama
            if (cookie != null)
            {
                return View("WelcomePage");
            }

            return View("Login");
        }

        public ActionResult Logout()
        {
            //kalo logout itu ngapus cookie
            HttpCookie aCookie;
            string cookieName;
            int limit = Request.Cookies.Count;
            for (int i = 0; i < limit; i++)
            {
                cookieName = Request.Cookies[i].Name;
                aCookie = new HttpCookie(cookieName);
                aCookie.Expires = DateTime.Now.AddDays(-1); //dibikin expirednya kemarin
                Response.Cookies.Add(aCookie);
            }

            return View("Login");
        }
        [HttpGet]
        public ActionResult GetData(string Email, string Password, string IsEmail)
        //Penjagaan Email atau Password Kosong
        {
            if (!string.IsNullOrEmpty(IsEmail))
            {
                if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
                {
                    ViewBag.Error = "Email / Password Salah";
                    return View("Login");
                }
            }
            LoginModel model = new LoginModel();
            var dt = !string.IsNullOrEmpty(IsEmail) ? HelpDeskData.User.GetByEmail(Email) : HelpDeskData.User.GetByName(Email);
            if (!string.IsNullOrEmpty(IsEmail))
            {
                if (dt == null || dt.Email != Email || dt.Password != Password)
                {
                    ViewBag.Error = "Email / Password Salah";
                    return View("Login");
                }
            }
            //penjagaan akun yang sudah di delete
            if (dt.IsDelete == true)
            {
                ViewBag.Error = "Akun Tidak Ditemukan";
                return View("Login");
            }
            // Penjagaan Email atau Password Salah atau user tidak ditemukan 

            HttpCookie cookie = Request.Cookies["LoginHelpDesk"]; //nge get data isinya nama
            HttpCookie cookieRole = Request.Cookies["Role"]; // Cookie Role Menentukan Tampilan Dashboard
            HttpCookie cookieEmail = Request.Cookies["Email"]; // Cookie isi email
            model.role = dt.Role.GetValueOrDefault();
            model.Nama = dt.Nama;
            model.Email = dt.Email;
            ViewBag.role = model.role;
            ViewData["Message"] = model; // gak kepake, tp mau dicek lg

            if (cookieEmail == null)
            {
                HttpCookie myCookie = new HttpCookie("Email");
                myCookie.Values.Add("Email", model.Email.ToString());
                myCookie.Expires = DateTime.Now.AddMonths(2);
                Response.Cookies.Add(myCookie);
            }
            else //jagaan ketika cookie nya masih nyangkut
            {
                string newCookie = model.Email.ToString();
                cookieEmail.Values["Email"] = newCookie;
                cookieEmail.Expires = DateTime.Now.AddMonths(2);
                Response.Cookies.Add(cookieEmail);
            }

            if (cookieRole == null)
            {
                HttpCookie myCookie = new HttpCookie("Role");
                myCookie.Values.Add("Role", model.role.ToString());
                myCookie.Expires = DateTime.Now.AddMonths(2);
                Response.Cookies.Add(myCookie);
            }
            else
            {
                string newCookie = model.role.ToString();
                cookieRole.Values["Role"] = newCookie;
                cookieRole.Expires = DateTime.Now.AddMonths(2);
                Response.Cookies.Add(cookieRole);
            }

            if (cookie == null)
            {
                HttpCookie myCookie = new HttpCookie("LoginHelpDesk");
                myCookie.Values.Add("Nama", dt.Nama);
                myCookie.Expires = DateTime.Now.AddMonths(2);
                Response.Cookies.Add(myCookie);
            }
            else
            {
                string newCookie = dt.Nama;
                cookie.Values["LoginHelpDesk"] = newCookie;
                cookie.Expires = DateTime.Now.AddMonths(2);
                Response.Cookies.Add(cookie);
            }

            if (dt.Role != null)
            {
                if (!string.IsNullOrEmpty(IsEmail))
                {
                    return View("WelcomePage");
                }
                else
                {

                    return View("FormComplaint");
                }
            }
            return View("Index");


        }
        public ActionResult Form()
        {
            return View("FormComplaint");
        }
    }
}