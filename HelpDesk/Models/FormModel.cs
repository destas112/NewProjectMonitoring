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
        public string LK { get; set; }
        public string Nama { get; set; }
        public string Proker { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public string NoHp { get; set; }
        public string Kategori { get; set; }
        public string DanaPengajuan { get; set; }


        public string Administrasi { get; set; }
        public string Akomodasi { get; set; }
        public string Trasnportasi { get; set; }
        public string Konsumsi { get; set; }
        public string Pembicara { get; set; }
        public string Hadiah { get; set; }
        public string Souvernir { get; set; }


        public string DanaPersetujuan { get; set; }
        public string Status { get; set; }
        public string Target { get; set; }
        public string TargetJumlah { get; set; }
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
        public string LPJ { get; set; }
    }


    public class PengajuanDanaModel
    {
        public string ProkerID { get; set; }
        public string NamaProgramKerja { get; set; }
        public string NamaBank { get; set; }
        public string NamaRekening { get; set; }
        public string NomorRekening { get; set; }

        public DateTime TanggalTerealisasi { get; set; }
        public DateTime TanggalAkhirTerealisasi { get; set; }
        public HttpPostedFileBase File { get; set; }
    }

    public class PertanggungJawabanModel
    {
        public string ProkerID { get; set; }
        public string IDPengajuanDana { get; set; }
        public string NamaProgramKerja { get; set; }
        public string NamaLK { get; set; }
        public string StatusPengumpulan { get; set; }
        public string IsLpj { get; set; }
        public string StatusKumpulOnline { get; set; }
        public string StatusKumpulOffline { get; set; }
        public string Deadline { get; set; }
        public string TanggalPengumpulan { get; set; }
        public string TanggalPengajuanDana { get; set; }
        public string TanggalTerealisasi{ get; set; }
        public string Keterangan { get; set; }
        public string DanaPengajuan { get; set; }
        public string DanaPersetujuan { get; set; }
        public string DanaPemasukan { get; set; }
        public string DanaPengeluaran { get; set; }
        public string Peserta { get; set; }
        public string IsDeadline { get; set; }

        public string FileName { get; set; }

        public string KeteranganPeserta { get; set; }

        public string KeteranganWaktu { get; set; }

        public HttpPostedFileBase File { get; set; }
    }

    public class LPJModel
    {
        public string IDProker { get; set; }
        public string DanaPemasukan { get; set; }
        public string DanaPengeluaran { get; set; }
        public string PesertaTerealisasi { get; set; }
        public HttpPostedFileBase File { get; set; }
    }

    public class ChartModelLK
    {
        public string IDProker { get; set; }
        public string NamaProker { get; set; }
        public int DanaPemasukan { get; set; }
        public int DanaPengeluaran { get; set; }
        public int PesertaTerealisasi { get; set; }
        public int TargetPeserta { get; set; }
    }




}