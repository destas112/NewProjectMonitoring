using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.Models
{
        public class FormParam
        {
            public string ID { get; set; }
            public string Nama { get; set; }
            public string Email { get; set; }
            public string Priority { get; set; }
            public string Subject { get; set; }
            public string Message { get; set; }
            public string PIC { get; set; }
            public string Status { get; set; }
            public string Category { get; set; }
            public DateTime Date { get; set; }
            public HttpPostedFileBase ImageFile { get; set; }
    }

        public class ReqGetTicket
        {
            public string ID { get; set; }
        }
        public class LoginModel
        {
            public string Nama { get; set; }
            public string Email { get; set; }
            public int role { get; set; }
            public string Password { get; set; }
    }

    public class ListTicketAll
    {
        public string ID { get; set; }
        public string Nama { get; set; }
        public string Email { get; set; }
        public string Priority { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string PIC { get; set; }
        public string Status { get; set; }
        public string Category { get; set; }
        public string Date { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }



}