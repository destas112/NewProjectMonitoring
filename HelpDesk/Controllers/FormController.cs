﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;
using HelpDesk.Models;
using HelpDeskData;
using OfficeOpenXml;
using System.Web.Helpers;
using System.IO;
using System.Text;
using OfficeOpenXml.Style;

namespace HelpDesk.Controllers
{
    public class FormController : Controller
    {
        // GET: Form
        public ActionResult FormComplaint()
        {//Open View Form
            return View();
        }

        [HttpPost]
        public ActionResult InsertDataForm(FormParam model)
        {
            string ID = GenerateID();
            HelpDeskData.Ticket dt = new HelpDeskData.Ticket(); //langsung deklarasi model 1 tabel ticket(1 row)
            dt.TicketID = ID;
            dt.Nama = model.Nama;
            dt.Email = model.Email;
            if (model.Category == "General")
            {
                dt.Category = "General";
                dt.Priority = "Low";
            }
            if (model.Category == "Sales")
            {
                dt.Category = "Sales";
                dt.Priority = "Critical";
            }
            if (model.Category == "Data")
            {
                dt.Category = "Data";
                dt.Priority = "Medium";
            }
            dt.Subject = model.Subject;
            dt.Message = model.Message;
            dt.Status = "New";
            dt.PIC = "";
            dt.CreatedDate = DateTime.Now;
            dt.FinishDate = new DateTime();
            if (model.ImageFile != null)
            {
                //proses nyimpen gambar ke dlm folder
                string filepath = "";
                filepath = Server.MapPath("~/File/");
                filepath = filepath + Path.GetFileName(model.ImageFile.FileName);
                model.ImageFile.SaveAs(filepath);
                dt.FileName = filepath;
            }
            dt.Insert("");//
            ViewBag.ID = ID;
            var email = SendEmailKonfirmationUser(model.Email,ID);
            return View("Succses");
        }

        public ActionResult SearchTicket()
        {
            return View("SearchTicket");
        }
        [HttpGet]
        public ActionResult GetTicket(ReqGetTicket param)
        {
            if (string.IsNullOrEmpty(param.ID))
            {
                ViewBag.ErrorGetTicket = "Harap Masukan Ticket ID Anda";
                return View("SearchTicket");
            }
            Ticket model = new Ticket();
            var data = Ticket.GetByID(param.ID);
            if (data == null)
            {
                ViewBag.ErrorGetTicket = "Ticket Tidak Ditemmukan";
                return View("SearchTicket");
            }
            model = data;
            ViewBag.Data = model;

            return View("TicketTableUser");
        }

        [HttpGet]
        public ActionResult GetTicketById(string ID)
        {

            Ticket model = new Ticket();
            var data = Ticket.GetByID(ID);
            model = data;
            ViewBag.Data = model;

            return View("TicketDetail");
        }

        [HttpGet]
        public ActionResult GetTicketByIdAdmin(string ID)
        {
            List<ParamUser> modelStaff = new List<ParamUser>();
            var dataStaff = HelpDeskData.User.GetByRole(2);
            Ticket model = new Ticket();
            var data = Ticket.GetByID(ID);
            model = data;
            foreach (var item in dataStaff)
            {
                ParamUser dt = new ParamUser();
                dt.Nama = item.Nama; //nama staff
                dt.ID = item.ID; //id staff

                modelStaff.Add(dt);
            }
            ViewBag.DataStaff = modelStaff;
            ViewBag.Data = model;

            return View("TicketDetailAdmin");
        }

        [HttpGet]
        public ActionResult TicketTableAdmin(string pic)
        {
            List<ListTicketAll> model = new List<ListTicketAll>();
            // List <Ticket> model = new List<Ticket>();
            var data = Ticket.GetByPIC(pic).OrderByDescending(x => x.CreatedDate);
            //model = data;
            foreach (var item in data)
            {
                ListTicketAll dt = new ListTicketAll();
                dt.Nama = item.Nama;
                dt.ID = item.TicketID;
                dt.Priority = item.Priority;
                dt.Email = item.Email;
                dt.Subject = item.Subject;
                dt.Message = item.Message;
                dt.PIC = item.PIC;
                dt.Status = item.Status;
                dt.Date = item.CreatedDate.GetValueOrDefault().ToString("MM/dd/yyyy");
                model.Add(dt);
            }
            ViewBag.Data = model;
            return View("ListTicket");
        }

        [HttpGet]
        public ActionResult AllTicket()
        {
            List<ListTicketAll> model = new List<ListTicketAll>();
            var data = Ticket.GetByAll().OrderByDescending(x => x.CreatedDate);

            foreach (var item in data)
            {
                ListTicketAll dt = new ListTicketAll();
                dt.Nama = item.Nama;
                dt.Date = item.CreatedDate.GetValueOrDefault().ToString("MM/dd/yyyy");
                dt.ID = item.TicketID;
                dt.Priority = item.Priority;
                dt.Email = item.Email;
                dt.Subject = item.Subject;
                dt.Message = item.Message;
                dt.PIC = item.PIC;
                dt.Status = item.Status;

                model.Add(dt);
            }
            ViewBag.Data = model; 
            return View("ListTicketAll");
        }

        [HttpPost]
        public ActionResult UpdateDataForm(FormParam model)
        {

            var dt = Ticket.GetByID(model.ID);
            dt.TicketID = model.ID;
            dt.Nama = model.Nama;
            dt.Email = model.Email;
            dt.Priority = model.Priority;
            dt.Subject = model.Subject;
            dt.Message = model.Message;
            dt.PIC = model.PIC;
            dt.Update(model.ID);
            if (Session["type"] != null && Session["resulttype"] != null)
                return View("TicketTableAdmin");
            else
                return Redirect(Request.UrlReferrer.ToString());

        }
        [HttpPost]
        public ActionResult UpdateStatusTicket(FormParam model)
        {
            var dt = Ticket.GetByID(model.ID);
            if (!string.IsNullOrEmpty(model.Category))
            {
                if (model.Category == "General")
                {
                    dt.Category = "General";
                    dt.Priority = "Low";
                }
                if (model.Category == "Sales")
                {
                    dt.Category = "Sales";
                    dt.Priority = "Critical";
                }
                if (model.Category == "Data")
                {
                    dt.Category = "Data";
                    dt.Priority = "Medium";
                }
            }
            if (!string.IsNullOrEmpty(model.Message))
            {
                dt.Reply = model.Message;
            }
            if (!string.IsNullOrEmpty(model.PIC))
            {
                dt.PIC = model.PIC;
                var staff = HelpDeskData.User.GetByName(model.PIC);
                var email = SendEmailNewTicket(staff.Email, model.ID);
            }
            if (!string.IsNullOrEmpty(model.Status))
            {
                dt.Status = model.Status;
                if (model.Status == "Done")
                {
                    dt.FinishDate = DateTime.Now;
                    var email = SendEmailDone(dt.Email, model.ID);
                }
            }
            dt.Update(model.ID);
            if (Session["type"] != null && Session["resulttype"] != null)
                return View("TicketTableAdmin");
            else
                return Redirect(Request.UrlReferrer.ToString());
        }
        [HttpGet]
        public ActionResult DeleteDataForm(string ID)
        {
            var dt = Ticket.GetByID(ID);

            dt.IsDelete = true;
            dt.Update(ID);

            //cara ngerefresh
            if (Session["type"] != null && Session["resulttype"] != null)
                return View("TicketTableAdmin");
            else
                return Redirect(Request.UrlReferrer.ToString());

        }

        public static string SendEmailKonfirmationUser(string EmailTo, string ID)
        {

            string Body = "Terimakasih atas laporan yang sudah diberikan. Anda telah mengirimkan ticket baru dengan Ticket ID : " + ID  + "<br>"+ "<a href = 'https://localhost:44306/' > Silahkan Cek Disini</a>";
            
            //proses mbikin koneksi ke email, template.
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            var credentials = new System.Net.NetworkCredential("helpdeskkalbe@gmail.com", "qgmybjbwwypskcth");
            client.Credentials = credentials;
            client.TargetName = "STARTTLS/smtp.gmail.com";

            //proses mbikin konten emailnya
            var msg = new MailMessage();
            msg.From = new MailAddress("helpdeskkalbe@gmail.com");
            msg.To.Add(EmailTo);
            msg.Subject = "Konfirmasi Pelaporan Ticket Baru";
            msg.Body = Body;
            msg.IsBodyHtml = true;
            client.Send(msg);

            return "";
        }

        public static string SendEmailNewTicket(string EmailTo, string TicketID)
        {
            string body = "Anda mempunya ticket baru dengan Ticket ID :" + TicketID + " Mohon segera cek dan selesaikan ticket anda!";

            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            var credentials = new System.Net.NetworkCredential("helpdeskkalbe@gmail.com", "qgmybjbwwypskcth");
            client.Credentials = credentials;
            client.TargetName = "STARTTLS/smtp.gmail.com";

            var msg = new MailMessage();
            msg.From = new MailAddress("helpdeskkalbe@gmail.com");
            msg.To.Add(EmailTo);
            msg.Subject = "Notifikasi Ticket Baru untuk Anda";
            msg.Body = body;
            client.Send(msg);

            return "";
        }

        public static string SendEmailDone(string EmailTo, string TicketID)
        {
            string body = "Ticket anda dengan ID :" + TicketID + "sudah selesai dikerjakan terimakasih telah melaporkan" + "<br>" + "<a href = 'https://localhost:44306/' > Silahkan Cek Disini</a>";

            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            var credentials = new System.Net.NetworkCredential("helpdeskkalbe@gmail.com", "qgmybjbwwypskcth");
            client.Credentials = credentials;
            client.TargetName = "STARTTLS/smtp.gmail.com";

            var msg = new MailMessage();
            msg.From = new MailAddress("helpdeskkalbe@gmail.com");
            msg.To.Add(EmailTo);
            msg.Subject = "Ticket Selesai";
            msg.Body = body;
            msg.IsBodyHtml = true;
            client.Send(msg);

            return "";
        }

        [HttpGet]
        public ActionResult OpenExportReport() //untuk buka view export report
        {
            List<ParamUser> modelStaff = new List<ParamUser>(); // bentuknya list(array) karna datanya gak cuma 1
            var dataStaff = HelpDeskData.User.GetByRole(2);
            foreach (var item in dataStaff)
            {
                ParamUser dt = new ParamUser();
                dt.Nama = item.Nama;
                dt.ID = item.ID;

                modelStaff.Add(dt);
            }
            ViewBag.DataStaff = modelStaff;
            return View("ExportReport");
        }

        [HttpPost]
        public string ExportReport(ReportModel model)
        {
            List<string> ListStatus = new List<string>() { model.New, model.Done, model.Progres };
            string Staff = model.Staff;
            string Category = model.Category;
            
            var ticket = Ticket.GetForReport(ListStatus, Staff, Category,model.CreatedDate,model.FinishDate).ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; //install library excel
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
            ws.Cells["A1"].Value = "Report Ticket Keluhan IT Support";
            ws.Cells["A2"].Value = "Created Date";
            ws.Cells["B2"].Value = DateTime.Now.ToString();

            ws.Cells["A4"].Value = "TicketID";
            ws.Cells["B4"].Value = "Nama";
            ws.Cells["C4"].Value = "Email";
            ws.Cells["D4"].Value = "Subject";
            ws.Cells["E4"].Value = "Status"; 
            ws.Cells["F4"].Value = "Priority";
            ws.Cells["G4"].Value = "Category";
            ws.Cells["H4"].Value = "PIC";
            ws.Cells["I4"].Value = "Created Date Ticket";
            ws.Cells["J4"].Value = "Finish Date Ticket";
            ws.Cells["K4"].Value = "Message";
            ws.Cells["L4"].Value = "Message";
            int rowstart = 5;


            ///Styling
            ///
            ws.Row(1).Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Row(1).Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Row(1).Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Row(1).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            ws.Row(2).Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Row(2).Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Row(2).Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Row(2).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            ws.Row(4).Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Row(4).Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Row(4).Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Row(4).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            foreach (var item in ticket)
            {
                ws.Row(rowstart).Style.Border.Top.Style =  ExcelBorderStyle.Thin;
                ws.Row(rowstart).Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Row(rowstart).Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Row(rowstart).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A{0}", rowstart)].Value = item.TicketID;
                ws.Cells[string.Format("B{0}", rowstart)].Value = item.Nama;
                ws.Cells[string.Format("C{0}", rowstart)].Value = item.Email;
                ws.Cells[string.Format("D{0}", rowstart)].Value = item.Subject;
                ws.Cells[string.Format("E{0}", rowstart)].Value = item.Status; 
                ws.Cells[string.Format("F{0}", rowstart)].Value = item.Priority;
                ws.Cells[string.Format("G{0}", rowstart)].Value = item.Category;
                ws.Cells[string.Format("H{0}", rowstart)].Value = item.PIC;
                ws.Cells[string.Format("I{0}", rowstart)].Value = item.CreatedDate.GetValueOrDefault().ToString();
                ws.Cells[string.Format("J{0}", rowstart)].Value = item.FinishDate.GetValueOrDefault().ToString();
                ws.Cells[string.Format("K{0}", rowstart)].Value = item.Message;
                if (item.Status == "Done")
                {
                    TimeSpan timeSpan = (item.FinishDate.GetValueOrDefault() - item.CreatedDate.GetValueOrDefault());
                    var parts = new List<string>();

                    int totalHours = (int)timeSpan.TotalHours;
                    int workDays = totalHours / 8;
                    int remainingHours = (int)timeSpan.TotalHours - 8 * workDays;

                    if (workDays == 1) parts.Add("1 day");
                    if (workDays > 1) parts.Add(workDays + " days");

                    if (remainingHours == 1) parts.Add("1 hour");
                    if (remainingHours > 1) parts.Add(timeSpan.Hours + " hours");

                    if (timeSpan.Minutes == 1) parts.Add("1 minute");
                    if (timeSpan.Minutes > 1) parts.Add(timeSpan.Minutes + " minutes");

                  //  return string.Join(", ", parts);




                    ws.Cells[string.Format("L{0}", rowstart)].Value = string.Join(", ", parts);

                }
                else
                {
                    ws.Cells[string.Format("L{0}", rowstart)].Value = "Not Finished Yet";

                }
                rowstart++;
            }
            //proses generate file
            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "Application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "TicketReport.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();

            return null;
        }

        public static string GenerateID()
        {
            int length = 7;

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

        [HttpGet]
        public ActionResult AllTicketUser(string nama)
        {
            List<ListTicketAll> model = new List<ListTicketAll>();
            // List <Ticket> model = new List<Ticket>();
            var data = Ticket.GetByNama(nama).OrderByDescending(x => x.CreatedDate);
            //model = data;
            foreach (var item in data)
            {
                ListTicketAll dt = new ListTicketAll();
                dt.Nama = item.Nama;
                dt.ID = item.TicketID;
                dt.Priority = item.Priority;
                dt.Email = item.Email;
                dt.Subject = item.Subject;
                dt.Message = item.Message;
                dt.PIC = item.PIC;
                dt.Status = item.Status;
                dt.Date = item.CreatedDate.GetValueOrDefault().ToString("MM/dd/yyyy");
                model.Add(dt);
            }
            ViewBag.Data = model;
            return View("AllTicketUser");
        }

    }
}