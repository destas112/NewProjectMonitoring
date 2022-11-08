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

    public class ProkerModel
    {
        public string ID { get; set; }
        public string Nama { get; set; }
        public string Proker { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public string NoHp { get; set; }
        public string Kategori { get; set; }
        public string DanaPengajuan { get; set; }
        public string DanaPersetujuan { get; set; }
        public string Status { get; set; }
        public string Target { get; set; }
        public DateTime TanggalPelaksanaan { get; set; }
        public DateTime TanggalAkhir { get; set; }
        public HttpPostedFileBase File { get; set; }
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

    public class ListProkerModel
    {
        public string ID { get; set; }
        public string Nama { get; set; }
        public string Proker { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public string NoHp { get; set; }
        public string Kategori { get; set; }
        public string DanaPengajuan { get; set; }
        public string DanaPersetujuan { get; set; }
        public string Status { get; set; }
        public string Target { get; set; }
        public string TanggalPelaksanaan { get; set; }
        public string TanggalAkhir { get; set; }
        public string File { get; set; }

        public string StatusPengajuan { get; set; }
    }


    public class PengajuanDanaModel
    {
        public string ProkerID { get; set; }
        public string NamaProgramKerja { get; set; }
        public string NamaBank { get; set; }
        public string NamaRekening { get; set; }
        public string NomorRekening { get; set; }
        public HttpPostedFileBase File { get; set; }
    }



}