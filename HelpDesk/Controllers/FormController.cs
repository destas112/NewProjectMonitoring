using System;
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
using System.Globalization;

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
        public ActionResult InsertDataForm(ProkerModel model)
        {
            var dtUser = HelpDeskData.User.GetByName(model.Nama);
            string ID = GenerateIDProker(dtUser.ID);
            

            HelpDeskData.ProgramKerja dt = new HelpDeskData.ProgramKerja(); //langsung deklarasi model 1 tabel ticket(1 row)
            HelpDeskData.PengajuanDana dtPengajuan = new HelpDeskData.PengajuanDana(); //langsung deklarasi model 1 tabel ticket(1 row)
            HelpDeskData.PertanggungJawaban dtLpj = new HelpDeskData.PertanggungJawaban(); //langsung deklarasi model 1 tabel ticket(1 row)
            dt.ProkerID = ID;
            dt.NamaLK = model.Nama;
            dt.NamaProker = model.Proker;
            dt.Category = model.Kategori;
            dt.Email = model.Email;
            dt.DanaPengajuan = model.DanaPengajuan;
            ///////////////////////////////////////////

            dt.Administrasi = model.Administrasi;
            dt.Akomodasi = model.Akomodasi;
            dt.Transportasi = model.Trasnportasi;
            dt.Konsumsi = model.Konsumsi;
            dt.Pembicara = model.Pembicara;
            dt.Hadiah = model.Hadiah;
            dt.Souvernir = model.Souvernir;

            dt.DanaPersetujuan = "";
            dt.Status = "";
            dt.PIC = "";
            dt.CreatedDate = model.TanggalPelaksanaan;
            dt.FinishDate = model.TanggalAkhir;
            dt.TargetPeserta = model.Target;
            dt.ContactPerson = model.ContactPerson;
            dt.Keterangan = "";
            dt.IsPengajuan = false;
            dt.TargetJumlahPeserta = model.TargetJumlah;
           // dt.Status = "0"
            if (model.File != null)
            {
                //proses nyimpen gambar ke dlm folder
                string filepath = "";
                filepath = Server.MapPath("~/File/");
                filepath = filepath + Path.GetFileName(model.File.FileName);
                model.File.SaveAs(filepath);
                dt.FileName = filepath;
            }
            dt.Insert("");//
            ViewBag.ID = ID;
            ///////////-------ADD PENGAJUAN DANAA---------///////////////
            string IDPengajuan = GenerateID();
            dtPengajuan.IDPengajuan = IDPengajuan;
            dtPengajuan.IDProker = ID;
            dtPengajuan.NamaProker = model.Proker;
            dtPengajuan.IsDelete = false;
            dtPengajuan.Insert("");

            ///////////-------ADD PERTANGGUNG JAWABAN---------///////////////
            string IDLpj = GenerateID();
            dtLpj.IDPertanggungJawaban = IDLpj;
            dtLpj.IDPengajuanDana = IDPengajuan;
            dtLpj.IDProker = ID;
            dtLpj.StatusKumpulOnline = "";
            dtLpj.StatusKumpulOffline = "";
            dtLpj.Insert("");


            // var email = SendEmailKonfirmationUser(model.Email,ID);
            return View("SuksesGeneral");
        }

        public ActionResult SearchTicket()
        {
            return View("SearchTicket");
        }

        [HttpGet]
        public ActionResult AddPengajuan(string ID)
        {

            ProgramKerja model = new ProgramKerja();
            var data = ProgramKerja.GetByID(ID);
            model = data;
            double Modal = double.Parse(model.DanaPengajuan);
            double totalPengeluaran = double.Parse(model.Administrasi) + double.Parse(model.Akomodasi) + double.Parse(model.Transportasi) + double.Parse(model.Konsumsi) + double.Parse(model.Pembicara) + double.Parse(model.Souvernir) + double.Parse(model.Hadiah);
            decimal Presentase = decimal.Parse(((totalPengeluaran / Modal) * 100).ToString());
          

            ViewBag.Data = model;
            ViewBag.Modal = CreateDecimalMoneyFormat(decimal.Parse(Modal.ToString()));
            ViewBag.Total = CreateDecimalMoneyFormat(decimal.Parse(totalPengeluaran.ToString()));
            ViewBag.Presentase = Presentase;

            return View("FormPengajuan");
        }

        public ActionResult InsertDataPengajuanDana(PengajuanDanaModel model)
        {
            string ID = GenerateID();
            var dtUser = HelpDeskData.User.GetByID("1100");
            HelpDeskData.PengajuanDana dt = PengajuanDana.GetByProkerID(model.ProkerID); //langsung deklarasi model 1 tabel ticket(1 row)
            HelpDeskData.ProgramKerja dtProker = ProgramKerja.GetByID(model.ProkerID);
          //  dt.IDPengajuan = ID;
            dt.DanaPengajuan = dtProker.DanaPengajuan;
            dt.NamaProker = dtProker.NamaProker;
            dt.DanaPersetujuan = "";
            dt.NoRekening = model.NomorRekening;
            dt.NamaBank = model.NamaBank;
            dt.NamaRekening = model.NamaRekening;
            dt.TanggalPengajuan = DateTime.Now;
            dt.TanggalTerealisasi = model.TanggalTerealisasi;
            dt.TanggalAkhirTerealisasi = model.TanggalAkhirTerealisasi;
            dt.TemaKegiatan = dtProker.Category;
            dt.JumlahPanitia = "";
            dt.TanggalProses = new DateTime();
            dt.IDProker = dtProker.ProkerID;

    
            if (model.File != null)
            {
                //proses nyimpen gambar ke dlm folder
                string filepath = "";
                filepath = Server.MapPath("~/File/");
                filepath = filepath + Path.GetFileName(model.File.FileName);
                model.File.SaveAs(filepath);
                dt.File = filepath;
            }
            dtProker.IsPengajuan = true;
            dtProker.Status = "0";
            dtProker.Update("");
            dt.Status = "0";
            dt.Update("");// 
            ViewBag.ID = ID;
            var email = SendEmailPengajuan(dtUser.Email, dtProker.NamaProker);
            return View("SuksesGeneral");
        }

        [HttpGet]
        public ActionResult AllPengajuanDana()
        {
            List<PengajuanDana> model = new List<PengajuanDana>();
            var data = PengajuanDana.GetByAllBiro().ToList();
            model = data;
            ViewBag.Data = model;
            return View("ListTicketAll");
        }

        [HttpGet]
        public ActionResult GetPengajuanDana(string ID)
        {

            PengajuanDana model = new PengajuanDana();
            var data = PengajuanDana.GetByID(ID);
            model = data;
            ViewBag.Data = model;

            return View("DetailPengajuanDana");
        }

        public ActionResult DaftarVerifikasiPengajuan()
        {

            List<PengajuanDana> model = new List<PengajuanDana>();
            var data = PengajuanDana.GetbyStatus("0").ToList();
            model = data;
            ViewBag.Data = model;

            return View("DaftarVerifikasi");
        }
        public ActionResult GetDetailVerifikasi(string ID)
        {

            PengajuanDana model = new PengajuanDana();
            var data = PengajuanDana.GetByID(ID);
            var dataproker = ProgramKerja.GetByID(data.IDProker);
            model = data;
            ViewBag.Data = model;
            ViewBag.DataProker = dataproker;
            //=================================================================
            double Modal = double.Parse(model.DanaPengajuan);
            double totalPengeluaran = double.Parse(dataproker.Administrasi) + double.Parse(dataproker.Akomodasi) + double.Parse(dataproker.Transportasi) + double.Parse(dataproker.Konsumsi) + double.Parse(dataproker.Pembicara) + double.Parse(dataproker.Souvernir) + double.Parse(dataproker.Hadiah);
            decimal Presentase = decimal.Parse(((totalPengeluaran / Modal) * 100).ToString());


            ViewBag.Modal = Modal;
            ViewBag.Total = totalPengeluaran;
            ViewBag.Presentase = Presentase;
            return View("DetailVerifikasi");
        }

        public ActionResult Verifikasi(PengajuanDana model)
        {
            var dtUser = HelpDeskData.User.GetByID("1000");
            PengajuanDana DtPengajuan = PengajuanDana.GetByID(model.IDPengajuan);
            ProgramKerja DtProker = ProgramKerja.GetByID(DtPengajuan.IDProker);
            DtPengajuan.Status = "1";
            DtPengajuan.DanaPersetujuan = model.DanaPersetujuan;
            DtPengajuan.Update("");

            DtProker.Status = "1";
            DtProker.Update("");
            var email = SendEmailPengajuanToBiro(dtUser.Email, DtProker.NamaProker);
            return View("Sukses");
        }


        [HttpGet]
        public ActionResult DaftarProkerLK(string pic)
        {
            List<ListProkerModel> model = new List<ListProkerModel>();
            // List <Ticket> model = new List<Ticket>();
            var data = ProgramKerja.GetByPIC(pic).OrderByDescending(x => x.CreatedDate);
            //model = data;
            foreach (var item in data)
            {
                ListProkerModel dt = new ListProkerModel();
                dt.ID = item.ProkerID;
                dt.TanggalPelaksanaan = item.CreatedDate.GetValueOrDefault().ToString("MM/dd/yyyy");
                dt.TanggalAkhir = item.FinishDate.GetValueOrDefault().ToString("MM/dd/yyyy");
                dt.Proker = item.NamaProker;
                dt.Email = item.Email;
                dt.Kategori = item.NamaProker;
                dt.DanaPengajuan =  CreateDecimalMoneyFormat(decimal.Parse(item.DanaPengajuan));
                dt.DanaPersetujuan = string.IsNullOrEmpty(item.DanaPersetujuan)? "" : CreateDecimalMoneyFormat(decimal.Parse(item.DanaPersetujuan));
                dt.Status = item.Status;
                dt.StatusPengajuan = item.IsPengajuan.ToString();

                model.Add(dt);
            }
            ViewBag.Data = model;
            return View("ListTicket");
        }
        [HttpGet]
        public ActionResult UpdateStatusPengajuan(PengajuanDana model)
        {
            PengajuanDana DtPengajuan = PengajuanDana.GetByID(model.IDPengajuan);
            ProgramKerja DtProker = ProgramKerja.GetByID(model.IDProker);
            DtPengajuan.Status = model.Status;
            DtPengajuan.Keterangan = model.Keterangan;
            DtPengajuan.TanggalProses = DateTime.Now;
            DtPengajuan.IsLPJ = false;
            DtPengajuan.Update("");

            DtProker.Status = model.Status;
            DtProker.Update("");
            return View("Sukses");
        }

        [HttpGet]
        public ActionResult GetDataLPJLK(string pic)
        {
            List<PertanggungJawabanModel> Result = new List<PertanggungJawabanModel>();
            var data = ProgramKerja.GetForListLPJ(pic).OrderByDescending(x => x.CreatedDate).ToList();
            //model = data;
            
            foreach (var item in data)
            {
                PertanggungJawabanModel model = new PertanggungJawabanModel();
                var dtPengajuan = PengajuanDana.GetByProkerID(item.ProkerID);
                var dt = PertanggungJawaban.GetByProkerID(item.ProkerID);
                DateTime taggalDeadline = dtPengajuan.TanggalPengajuan.GetValueOrDefault().AddDays(30);
               model.ProkerID = dt.IDProker;
                model.NamaProgramKerja = dtPengajuan.NamaProker;
                model.IsLpj = dtPengajuan.IsLPJ.ToString();
                model.StatusKumpulOffline = dt.StatusKumpulOffline;
                model.StatusKumpulOnline = dt.StatusKumpulOnline;
                model.Deadline = dtPengajuan.TanggalPengajuan.GetValueOrDefault().AddDays(30).ToString("MM/dd/yyyy");
                model.TanggalPengumpulan = dt.TanggalPengumpulan.GetValueOrDefault().ToString("MM/dd/yyyy");
                model.TanggalPengajuanDana = dtPengajuan.TanggalPengajuan.GetValueOrDefault().ToString("MM/dd/yyyy");
                model.DanaPengajuan =CreateDecimalMoneyFormat(decimal.Parse(dtPengajuan.DanaPengajuan));
                model.DanaPersetujuan = CreateDecimalMoneyFormat(decimal.Parse(dtPengajuan.DanaPersetujuan));
                model.Keterangan = string.IsNullOrEmpty(dt.Keterangan) ?"" : dt.Keterangan;
                model.IsDeadline =  DateTime.Now > taggalDeadline ? "1" : "";
                Result.Add(model);
            }
            ViewBag.Data = Result;
            return View("ListLPJLK");
        }
        public ActionResult AddLpj(string ID)
        {

            PengajuanDana model = new PengajuanDana();
            var data = PengajuanDana.GetByProkerID(ID);
            model = data;
            ViewBag.Data = model;

            return View("FormTambahLPJ");
        }
        public ActionResult TambahLpj(LPJModel model)
        {
            PertanggungJawaban dt = PertanggungJawaban.GetByProkerID(model.IDProker);
            PengajuanDana dtPengajuan = PengajuanDana.GetByProkerID(model.IDProker);
            dtPengajuan.IsLPJ = true;
            dtPengajuan.Update("");


            dt.DanaPemasukan = model.DanaPemasukan;
            dt.DanaPengeluaran = model.DanaPengeluaran;
            dt.PesertaTerealisasi = model.PesertaTerealisasi;
            dt.StatusKumpulOnline = "0";
            dt.StatusKumpulOffline = "0";

            if (model.File != null)
            {
                //proses nyimpen gambar ke dlm folder
                string filepath = "";
                filepath = Server.MapPath("~/File/");
                filepath = filepath + Path.GetFileName(model.File.FileName);
                model.File.SaveAs(filepath);
                dt.File = filepath;
            }
            dt.Update("");
            return View("SuksesGeneral");
        }

        public ActionResult DaftarVerifikasiLPJ()
        {

            List<PertanggungJawabanModel> model = new List<PertanggungJawabanModel>();
            List<PengajuanDana> modelPengajuanDana = new List<PengajuanDana>();
            var data = PertanggungJawaban.GetbyStatus("0").ToList();
            foreach (var item in data)
            {
                PertanggungJawabanModel dtModel = new PertanggungJawabanModel();
                var dt = PengajuanDana.GetByProkerID(item.IDProker);
                dtModel.DanaPemasukan = item.DanaPemasukan;
                dtModel.DanaPengeluaran = item.DanaPengeluaran;
                dtModel.NamaProgramKerja = dt.NamaProker;
                dtModel.ProkerID = item.IDProker;
                dtModel.FileName = item.File;
                model.Add(dtModel);
                modelPengajuanDana.Add(dt);
            }
            ViewBag.Data = model;
            return View("DaftarVerifikasiLPJ");
        }

        public ActionResult GetDetailVerifikasiLPJ(string ID)
        {

            PertanggungJawaban model = new PertanggungJawaban();
            var data = PertanggungJawaban.GetByProkerID(ID);
            var dataProker = ProgramKerja.GetByID(ID);
            model = data;
            ViewBag.Data = model;
            ViewBag.DataNamaProker = dataProker.NamaProker;


            return View("DetailVerifikasiLPJ");
        }

        public ActionResult VerifikasiLPJ(PertanggungJawaban model)
        {
            PertanggungJawaban DtLPJ = PertanggungJawaban.GetByProkerID(model.IDProker);
            DtLPJ.StatusKumpulOnline = model.StatusKumpulOnline;
            DtLPJ.Keterangan = model.Keterangan;
            DtLPJ.Update("");
            return View("Sukses");
        }

        public ActionResult AllLPJ()
        {
            List<PertanggungJawabanModel> model = new List<PertanggungJawabanModel>();
            var data = PertanggungJawaban.GetByAllForBiro().ToList();
            foreach (var item in data)
            {
                PertanggungJawabanModel dtModel = new PertanggungJawabanModel();
                var dt = PengajuanDana.GetByProkerID(item.IDProker);
                var dt1 = ProgramKerja.GetByID(item.IDProker);
                dtModel.IDPengajuanDana = item.IDPengajuanDana;
                dtModel.DanaPemasukan = item.DanaPemasukan;
                dtModel.DanaPengeluaran = item.DanaPengeluaran;
                dtModel.NamaProgramKerja = dt.NamaProker;
                dtModel.StatusKumpulOffline = item.StatusKumpulOffline;
                dtModel.StatusKumpulOnline = item.StatusKumpulOnline;
                dtModel.ProkerID = item.IDProker;
                dtModel.FileName = item.File;
                dtModel.NamaLK = dt1.NamaProker;
                model.Add(dtModel);
               
            }
            ViewBag.Data = model;
            ViewBag.Data = model;
            return View("LpjBiro");
        }

        public ActionResult VerifikasiLpjOffline(string ID)
        {
           
            PertanggungJawaban DtLPJ = PertanggungJawaban.GetByProkerID(ID);
            DtLPJ.StatusKumpulOffline = "1";
            DtLPJ.Update("");
            return View("Sukses");
        }

        public static string SendEmailPengajuan(string EmailTo, string Proker)
        {
            string body = "Pengajuan dana program kerja " + Proker + " diterima mohon untuk diverifikasi dana terealisasi";

            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            var credentials = new System.Net.NetworkCredential("sipenaukdw@gmail.com", "ebljupbkwfxcutqu");
            client.Credentials = credentials;
            client.TargetName = "STARTTLS/smtp.gmail.com";

            var msg = new MailMessage();
            msg.From = new MailAddress("sipenaukdw@gmail.com");
            msg.To.Add(EmailTo);
            msg.Subject = "Verifikasi Pengajuan Dana";
            msg.Body = body;
            msg.IsBodyHtml = true;
            client.Send(msg);

            return "";
        }

        public static string SendEmailPengajuanToBiro(string EmailTo, string Proker)
        {
            string body = "Pengajuan dana baru dengan proker " + Proker;

            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            var credentials = new System.Net.NetworkCredential("sipenaukdw@gmail.com", "ebljupbkwfxcutqu");
            client.Credentials = credentials;
            client.TargetName = "STARTTLS/smtp.gmail.com";

            var msg = new MailMessage();
            msg.From = new MailAddress("sipenaukdw@gmail.com");
            msg.To.Add(EmailTo);
            msg.Subject = "Verifikasi Pengajuan Dana";
            msg.Body = body;
            msg.IsBodyHtml = true;
            client.Send(msg);

            return "";
        }
        public ActionResult Summary()
        {
            List<PertanggungJawabanModel> model = new List<PertanggungJawabanModel>();
            var data = PertanggungJawaban.GetSummary().ToList();
            foreach (var item in data)
            {
                PertanggungJawabanModel dtModel = new PertanggungJawabanModel();
                var dt = PengajuanDana.GetByProkerID(item.IDProker);
                var dt1 = ProgramKerja.GetByID(item.IDProker);
                dtModel.IDPengajuanDana = item.IDPengajuanDana;
                dtModel.DanaPemasukan = item.DanaPemasukan;
                dtModel.DanaPengeluaran = item.DanaPengeluaran;
                dtModel.NamaProgramKerja = dt1.NamaProker;
                dtModel.StatusKumpulOffline = item.StatusKumpulOffline;
                dtModel.StatusKumpulOnline = item.StatusKumpulOnline;
                dtModel.Peserta = item.PesertaTerealisasi;
                dtModel.ProkerID = item.IDProker;
                dtModel.FileName = item.File;
                dtModel.NamaLK = dt1.NamaLK;
                dtModel.TanggalTerealisasi = dt.TanggalAkhirTerealisasi.GetValueOrDefault().ToString("MM/dd/yyyy");
                model.Add(dtModel);

            }
            ViewBag.Data = model;
            ViewBag.Data = model;
            return View("Summary");
        }
    
        public ActionResult CekRiwayat(string Nama)
        {
            List<PertanggungJawabanModel> model = new List<PertanggungJawabanModel>();
            var dataModel = ProgramKerja.GetByNamaLK(Nama).ToList();
            if (dataModel.Count() > 0)
            {
                var data = PertanggungJawaban.GetSummary().ToList();
                foreach (var item in data)
                {
                    PertanggungJawabanModel dtModel = new PertanggungJawabanModel();
                    var dt = PengajuanDana.GetByProkerID(item.IDProker);
                    var dt1 = ProgramKerja.GetByID(item.IDProker);
                    var dt2 = PertanggungJawaban.GetByProkerID(item.IDProker);
                    dtModel.IDPengajuanDana = item.IDPengajuanDana;
                    dtModel.DanaPemasukan = item.DanaPemasukan;
                    dtModel.DanaPengeluaran = item.DanaPengeluaran;
                    dtModel.NamaProgramKerja = dt1.NamaProker;
                    dtModel.StatusKumpulOffline = item.StatusKumpulOffline;
                    dtModel.StatusKumpulOnline = item.StatusKumpulOnline;
                    dtModel.Peserta = item.PesertaTerealisasi;
                    dtModel.ProkerID = item.IDProker;
                    dtModel.FileName = item.File;
                    dtModel.NamaLK = dt1.NamaLK;
                    dtModel.TanggalTerealisasi = dt.TanggalAkhirTerealisasi.GetValueOrDefault().ToString("MM/dd/yyyy");
                    if (int.Parse(dt1.TargetJumlahPeserta) < int.Parse(dt2.PesertaTerealisasi))
                    {
                        dtModel.KeteranganPeserta = "Melebihi Target";
                    
                    }
                    else if (int.Parse(dt1.TargetJumlahPeserta) == int.Parse(dt2.PesertaTerealisasi))
                    {
                        dtModel.KeteranganPeserta = "Sama Dengan Target";

                    }
                    else 
                    {
                        dtModel.KeteranganPeserta = "Target Tidak Tercapai";

                    }
                    if (dt1.CreatedDate > dt.TanggalTerealisasi)
                    {
                        dtModel.KeteranganWaktu = "Waktu Pelaksanaan Maju";
                    }
                    else if (dt1.CreatedDate == dt.TanggalTerealisasi)
                    {
                        dtModel.KeteranganWaktu = "Tepat Waktu";
                    }
                    else
                    {
                        dtModel.KeteranganWaktu = "Waktu Pelaksanaan Mundur";
                    }


                    model.Add(dtModel);

                }
             
            }
            else
            {
                model = null;

            }
            ViewBag.Data1 = model;
            return View("RiwayatLK");
        }


        public ActionResult ChartBiro()
        {
            return View("ChartBiro");
        }

        public ActionResult ChartIndex()
        {
            return View("ChartLK");
        }


            public ActionResult ChartLK()
        {
            var cookie = Request.Cookies["LoginHelpDesk"].Value.ToString();
            string ModelNama = "";
            if (cookie.Contains("&"))
            {
                var x = cookie.Substring(cookie.IndexOf('&') + 1).Split('=');
                ModelNama = x[1];
            }
            else // ada kemungkinan tanpa "&"
            {
                var x = cookie.Split('=');
                ModelNama = x[1];
            }
            List<ChartModelLK> model = new List<ChartModelLK>();
            var data = ProgramKerja.GetByNamaLK(ModelNama).ToList();
            var NamaProker = new List<string>();
            var Peserta = new List<int>();
            var PesertaTerealisasi = new List<int>();

            foreach (var item in data)
            {
                ChartModelLK dtModel = new ChartModelLK();
                var dt = PertanggungJawaban.GetByProkerID(item.ProkerID);
                if (dt.PesertaTerealisasi != null)
                {
                    dtModel.NamaProker = item.NamaProker;
                    dtModel.TargetPeserta = int.Parse(item.TargetJumlahPeserta);
                    dtModel.PesertaTerealisasi = int.Parse(dt.PesertaTerealisasi);

                    model.Add(dtModel);
                }


            }
            var data1 = model;


            foreach (var item1 in data1)
            {
                NamaProker.Add(item1.NamaProker);
                Peserta.Add(item1.TargetPeserta);
                PesertaTerealisasi.Add(item1.PesertaTerealisasi);
            }

            var myChart = new Chart(width: 600, height: 400, theme: ChartTheme.Vanilla)
          .AddTitle("Perbandingan Peserta").AddLegend("Details")
        .AddSeries(
        name: "Target Peserta",
        xValue: NamaProker,
        yValues: Peserta
       )
        .AddSeries(
        name: "Peserta Terealisasi",
        xValue: NamaProker,
        yValues: PesertaTerealisasi)
        .Write();

            return File(myChart.ToWebImage().GetBytes(),"image/jpeg");
        }



        public ActionResult ChartLKDana()
        {
            var cookie = Request.Cookies["LoginHelpDesk"].Value.ToString();
            string ModelNama = "";
            if (cookie.Contains("&"))
            {
                var x = cookie.Substring(cookie.IndexOf('&') + 1).Split('=');
                ModelNama = x[1];
            }
            else // ada kemungkinan tanpa "&"
            {
                var x = cookie.Split('=');
                ModelNama = x[1];
            }
            List<ChartModelLK> model = new List<ChartModelLK>();
            var data = ProgramKerja.GetByNamaLK(ModelNama).ToList();
            var NamaProker = new List<string>();
            var DanaPemasukan = new List<int>();
            var DanaPengeluaran = new List<int>();

            foreach (var item in data)
            {
                ChartModelLK dtModel = new ChartModelLK();
                var dt = PertanggungJawaban.GetByProkerID(item.ProkerID);
                if (dt.PesertaTerealisasi != null)
                {
                    dtModel.DanaPemasukan = int.Parse(dt.DanaPemasukan);
                    dtModel.DanaPengeluaran = int.Parse(dt.DanaPengeluaran);
                    dtModel.NamaProker = item.NamaProker;


                    model.Add(dtModel);
                }


            }
            var data1 = model;


            foreach (var item1 in data1)
            {
                NamaProker.Add(item1.NamaProker);
                DanaPemasukan.Add(item1.DanaPemasukan);
                DanaPengeluaran.Add(item1.DanaPengeluaran);
            }

            var myChart = new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
          .AddTitle("Perbandingan Pengeluaran & Pemasukan").AddLegend("Details")
        .AddSeries(
        name: "Dana Pemasukan",
        xValue: NamaProker,
        yValues: DanaPemasukan
       )
        .AddSeries(
        name: "Dana Pengeluaran",
        xValue: NamaProker,
        yValues: DanaPengeluaran)
        .Write();

            return File(myChart.ToWebImage().GetBytes(), "image/jpeg");
        }

        public ActionResult ChartLKDanaBiro()
        {

            List<ChartModelLK> model = new List<ChartModelLK>();
            var dataUser = HelpDeskData.User.GetByRoleAll().ToList();
            var NamaLK = new List<string>();
            var DanaPemasukan = new List<int>();
            var DanaPengeluaran = new List<int>();

            foreach (var item in dataUser)
            {
                string query = "Select * from \"ProgramKerja\" Where \"NamaLK\" = '" + item.Nama + "'";
                var data = HelpDeskData.ExtentionTransaction.ExecuteQuerys<ProgramKerja>(query);
                // var data = ProgramKerja.GetByAll()/*.Where(x => x.NamaLK == item.Nama).ToList()*/;
                var a = data.ToList();
                int totalPemasukan = 0;
                int totalPengeluaran = 0;
                ChartModelLK dtModel = new ChartModelLK();
                foreach (var item1 in a)
                {
                    if (item1.NamaLK == item.Nama)
                    {
                        var dt = PertanggungJawaban.GetByProkerIDToChart(item1.ProkerID).First();

                        //string query1 = "Select * from \"PertanggungJawaban\" Where \"IDProker\" = '" + item1.ProkerID + "'";
                        //var dt = HelpDeskData.ExtentionTransaction.ExecuteQuerys<PertanggungJawaban>(query1);

                        if (dt.PesertaTerealisasi != null)
                        {
                            totalPemasukan = totalPemasukan + int.Parse(dt.DanaPemasukan);
                            totalPengeluaran = totalPengeluaran + int.Parse(dt.DanaPengeluaran);
                            dtModel.DanaPemasukan =  totalPemasukan;
                            dtModel.DanaPengeluaran = totalPengeluaran;
                            ////dtModel.NamaProker = item1.NamaProker;
                            dtModel.NamaLK = item.Nama;


                            model.Add(dtModel);
                        }
                    }
                }
            }
            var data1 = model;

            foreach (var item1 in data1)
            {
                if (!NamaLK.Contains(item1.NamaLK))
                {
                    NamaLK.Add(item1.NamaLK);
                    DanaPemasukan.Add(item1.DanaPemasukan);
                    DanaPengeluaran.Add(item1.DanaPengeluaran);
                }
            }

            var myChart = new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
          .AddTitle("Perbandingan Pengeluaran & Pemasukan").AddLegend("Details")
        .AddSeries(
        name: "Dana Pemasukan",
        xValue: NamaLK,
        yValues: DanaPemasukan
       )
        .AddSeries(
        name: "Dana Pengeluaran",
        xValue: NamaLK,
        yValues: DanaPengeluaran)
        .Write();

            return File(myChart.ToWebImage().GetBytes(), "image/jpeg");
        }


        public ActionResult LineChartBiro()
        {

            List<ChartModelLK> model = new List<ChartModelLK>();
            var dataUser = HelpDeskData.User.GetByRoleAll().ToList();
            var NamaLK = new List<string>();
            var Keterlamabatan = new List<int>();

            foreach (var item in dataUser)
            {
                string query = "Select * from \"ProgramKerja\" Where \"NamaLK\" = '" + item.Nama + "'";
                var data = HelpDeskData.ExtentionTransaction.ExecuteQuerys<ProgramKerja>(query);
                // var data = ProgramKerja.GetByAll()/*.Where(x => x.NamaLK == item.Nama).ToList()*/;
                var a = data.ToList();
                int totalKeterlambatan = 0;
                ChartModelLK dtModel = new ChartModelLK();
                foreach (var item1 in a)
                {
                    if (item1.NamaLK == item.Nama)
                    {
                        var dt = PertanggungJawaban.GetByProkerIDToChart(item1.ProkerID).ToList().First();
                        var dtPengajuan = PengajuanDana.GetByProkerID(item1.ProkerID);

                        //string query1 = "Select * from \"PertanggungJawaban\" Where \"IDProker\" = '" + item1.ProkerID + "'";
                        //var dt = HelpDeskData.ExtentionTransaction.ExecuteQuerys<PertanggungJawaban>(query1);

                        if (dt.PesertaTerealisasi != null)
                        {
                            DateTime taggalDeadline = dtPengajuan.TanggalPengajuan.GetValueOrDefault().AddDays(30);
                            if (DateTime.Now > taggalDeadline)
                            {
                                totalKeterlambatan = totalKeterlambatan + 1;
                            }
                            dtModel.JumlahKeterlambatan = totalKeterlambatan;
                            dtModel.NamaLK = item.Nama;


                            model.Add(dtModel);
                        }
                    }
                }
            }
            var data1 = model;

            foreach (var item1 in data1)
            {
                if (!NamaLK.Contains(item1.NamaLK))
                {
                    NamaLK.Add(item1.NamaLK);
                    Keterlamabatan.Add(item1.JumlahKeterlambatan);
                }
            }

            var myChart = new Chart(width: 600, height: 400,theme: ChartTheme.Blue)
          .AddTitle("Tingkat Keterlambatan LK").AddLegend("Details")
        .AddSeries(
        chartType: "line",
        name: "Keterlambatan",
        xValue: NamaLK,
        yValues: Keterlamabatan
       );

            return File(myChart.ToWebImage().GetBytes(), "image/jpeg");
        }

        public ActionResult PieChartLKDanaBiro()
        {

            List<ChartModelLK> model = new List<ChartModelLK>();
            var dataUser = HelpDeskData.User.GetByRoleAll().ToList();
            var NamaLK = new List<string>();
            var DanaPemasukan = new List<int>();
            var DanaPengeluaran = new List<int>();

            foreach (var item in dataUser)
            {
                string query = "Select * from \"ProgramKerja\" Where \"NamaLK\" = '" + item.Nama + "'";
                var data = HelpDeskData.ExtentionTransaction.ExecuteQuerys<ProgramKerja>(query);
                // var data = ProgramKerja.GetByAll()/*.Where(x => x.NamaLK == item.Nama).ToList()*/;
                var a = data.ToList();
                int totalPemasukan = 0;
                int totalPengeluaran = 0;
                ChartModelLK dtModel = new ChartModelLK();
                foreach (var item1 in a)
                {
                    if (item1.NamaLK == item.Nama)
                    {
                        var dt = PertanggungJawaban.GetByProkerIDToChart(item1.ProkerID).First();

                        //string query1 = "Select * from \"PertanggungJawaban\" Where \"IDProker\" = '" + item1.ProkerID + "'";
                        //var dt = HelpDeskData.ExtentionTransaction.ExecuteQuerys<PertanggungJawaban>(query1);

                        if (dt.PesertaTerealisasi != null)
                        {
                            totalPemasukan = totalPemasukan + int.Parse(dt.DanaPemasukan);
                            totalPengeluaran = totalPengeluaran + int.Parse(dt.DanaPengeluaran);
                            dtModel.DanaPemasukan = totalPemasukan;
                            dtModel.DanaPengeluaran = totalPengeluaran;
                            ////dtModel.NamaProker = item1.NamaProker;
                            dtModel.NamaLK = item.Nama;


                            model.Add(dtModel);
                        }
                    }
                }
            }
            var data1 = model;

            foreach (var item1 in data1)
            {
                if (!NamaLK.Contains(item1.NamaLK))
                {
                    NamaLK.Add(item1.NamaLK);
                    DanaPemasukan.Add(item1.DanaPemasukan);
                    DanaPengeluaran.Add(item1.DanaPengeluaran);
                }
            }

            var myChart = new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
          .AddTitle("Tingkat Konsumsi Dana").AddLegend("Details")
        .AddSeries(

        chartType: "pie",
        name: "Konsumsi Dana",
        xValue: NamaLK,
        yValues: DanaPemasukan
       );

            return File(myChart.ToWebImage().GetBytes(), "image/jpeg");
        }

        public ActionResult ChartProkerBiro()
        {

            List<ChartModelLK> model = new List<ChartModelLK>();
            var dataUser = HelpDeskData.User.GetByRoleAll().ToList();
            var NamaLK = new List<string>();
            var JumlahProker = new List<int>();
            var PokerTercapai = new List<int>();

            foreach (var item in dataUser)
            {
                string query = "Select * from \"ProgramKerja\" Where \"NamaLK\" = '" + item.Nama + "'";
                var data = HelpDeskData.ExtentionTransaction.ExecuteQuerys<ProgramKerja>(query);
                // var data = ProgramKerja.GetByAll()/*.Where(x => x.NamaLK == item.Nama).ToList()*/;
                var a = data.ToList();
                int totalproker = 0;
                int totalprokertercapai = 0;
                ChartModelLK dtModel = new ChartModelLK();
                foreach (var item1 in a)
                {
                    if (item1.NamaLK == item.Nama)
                    {
                        var dt = PertanggungJawaban.GetByProkerIDToChart(item1.ProkerID).ToList().First();
                        var dtProker= ProgramKerja.GetByID(item1.ProkerID);

                        //string query1 = "Select * from \"PertanggungJawaban\" Where \"IDProker\" = '" + item1.ProkerID + "'";
                        //var dt = HelpDeskData.ExtentionTransaction.ExecuteQuerys<PertanggungJawaban>(query1);

                        if (dt.PesertaTerealisasi != null)
                        {
                            totalproker = totalproker + 1;
                            if (int.Parse(dtProker.TargetJumlahPeserta) < int.Parse(dt.PesertaTerealisasi) || int.Parse(dtProker.TargetJumlahPeserta) == int.Parse(dt.PesertaTerealisasi))
                            {
                                totalprokertercapai = totalprokertercapai + 1;
                            }
                            dtModel.JumlahProker = totalproker;
                            dtModel.ProkerTercapai = totalprokertercapai;
                            ////dtModel.NamaProker = item1.NamaProker;
                            dtModel.NamaLK = item.Nama;


                            model.Add(dtModel);
                        }
                    }
                }
            }
            var data1 = model;

            foreach (var item1 in data1)
            {
                if (!NamaLK.Contains(item1.NamaLK))
                {
                    NamaLK.Add(item1.NamaLK);
                    JumlahProker.Add(item1.JumlahProker);
                    PokerTercapai.Add(item1.ProkerTercapai);
                }
            }

            var myChart = new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
          .AddTitle("Tingkat Keberhasilan Program Kerja").AddLegend("Details")
        .AddSeries(
        name: "Jumlah Proker",
        xValue: NamaLK,
        yValues: JumlahProker
       )
        .AddSeries(
        name: "Proker Tercapai",
        xValue: NamaLK,
        yValues: PokerTercapai)
        .Write();

            return File(myChart.ToWebImage().GetBytes(), "image/jpeg");
        }


        //[HttpGet]
        //public ActionResult GetTicket(ReqGetTicket param)
        //{
        //    if (string.IsNullOrEmpty(param.ID))
        //    {
        //        ViewBag.ErrorGetTicket = "Harap Masukan Ticket ID Anda";
        //        return View("SearchTicket");
        //    }
        //    Ticket model = new Ticket();
        //    var data = Ticket.GetByID(param.ID);
        //    if (data == null)
        //    {
        //        ViewBag.ErrorGetTicket = "Ticket Tidak Ditemmukan";
        //        return View("SearchTicket");
        //    }
        //    model = data;
        //    ViewBag.Data = model;

        //    return View("TicketTableUser");
        //}

        //[HttpGet]
        //public ActionResult GetTicketById(string ID)
        //{

        //    Ticket model = new Ticket();
        //    var data = Ticket.GetByID(ID);
        //    model = data;
        //    ViewBag.Data = model;

        //    return View("TicketDetail");
        //}

        //[HttpGet]
        //public ActionResult GetTicketByIdAdmin(string ID)
        //{
        //    List<ParamUser> modelStaff = new List<ParamUser>();
        //    var dataStaff = HelpDeskData.User.GetByRole(2);
        //    Ticket model = new Ticket();
        //    var data = Ticket.GetByID(ID);
        //    model = data;
        //    foreach (var item in dataStaff)
        //    {
        //        ParamUser dt = new ParamUser();
        //        dt.Nama = item.Nama; //nama staff
        //        dt.ID = item.ID; //id staff

        //        modelStaff.Add(dt);
        //    }
        //    ViewBag.DataStaff = modelStaff;
        //    ViewBag.Data = model;

        //    return View("TicketDetailAdmin");
        //}

        //[HttpGet]
        //public ActionResult AllTicket()
        //{
        //    List<ListProkerModel> model = new List<ListProkerModel>();
        //    var data = ProgramKerja.GetByAll().OrderByDescending(x => x.CreatedDate);

        //    foreach (var item in data)
        //    {
        //        ListProkerModel dt = new ListProkerModel();
        //        dt.ID = item.ProkerID;
        //        dt.TanggalPelaksanaan = item.CreatedDate.GetValueOrDefault().ToString("MM/dd/yyyy");
        //        dt.TanggalAkhir = item.FinishDate.GetValueOrDefault().ToString("MM/dd/yyyy");
        //        dt.Proker = item.NamaProker;
        //        dt.Email = item.Email;
        //        dt.Kategori = item.NamaProker;
        //        dt.DanaPengajuan = item.DanaPengajuan;
        //        dt.DanaPersetujuan = item.DanaPersetujuan;
        //        dt.StatusPengajuan = item.IsPengajuan.ToString();

        //        model.Add(dt);
        //    }
        //    ViewBag.Data = model;
        //    return View("ListTicketAll");
        //}

        //[HttpPost]
        //public ActionResult UpdateDataForm(FormParam model)
        //{

        //    var dt = Ticket.GetByID(model.ID);
        //    dt.TicketID = model.ID;
        //    dt.Nama = model.Nama;
        //    dt.Email = model.Email;
        //    dt.Priority = model.Priority;
        //    dt.Subject = model.Subject;
        //    dt.Message = model.Message;
        //    dt.PIC = model.PIC;
        //    dt.Update(model.ID);
        //    if (Session["type"] != null && Session["resulttype"] != null)
        //        return View("TicketTableAdmin");
        //    else
        //        return Redirect(Request.UrlReferrer.ToString());

        //}
        //[HttpPost]
        //public ActionResult UpdateStatusTicket(FormParam model)
        //{
        //    var dt = Ticket.GetByID(model.ID);
        //    if (!string.IsNullOrEmpty(model.Category))
        //    {
        //        if (model.Category == "General")
        //        {
        //            dt.Category = "General";
        //            dt.Priority = "Low";
        //        }
        //        if (model.Category == "Sales")
        //        {
        //            dt.Category = "Sales";
        //            dt.Priority = "Critical";
        //        }
        //        if (model.Category == "Data")
        //        {
        //            dt.Category = "Data";
        //            dt.Priority = "Medium";
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(model.Message))
        //    {
        //        dt.Reply = model.Message;
        //    }
        //    if (!string.IsNullOrEmpty(model.PIC))
        //    {
        //        dt.PIC = model.PIC;
        //        var staff = HelpDeskData.User.GetByName(model.PIC);
        //        var email = SendEmailNewTicket(staff.Email, model.ID);
        //    }
        //    if (!string.IsNullOrEmpty(model.Status))
        //    {
        //        dt.Status = model.Status;
        //        if (model.Status == "Done")
        //        {
        //            dt.FinishDate = DateTime.Now;
        //            var email = SendEmailDone(dt.Email, model.ID);
        //        }
        //    }
        //    dt.Update(model.ID);
        //    if (Session["type"] != null && Session["resulttype"] != null)
        //        return View("TicketTableAdmin");
        //    else
        //        return Redirect(Request.UrlReferrer.ToString());
        //}
        //[HttpGet]
        //public ActionResult DeleteDataForm(string ID)
        //{
        //    var dt = Ticket.GetByID(ID);

        //    dt.IsDelete = true;
        //    dt.Update(ID);

        //    //cara ngerefresh
        //    if (Session["type"] != null && Session["resulttype"] != null)
        //        return View("TicketTableAdmin");
        //    else
        //        return Redirect(Request.UrlReferrer.ToString());

        //}

        //public static string SendEmailKonfirmationUser(string EmailTo, string ID)
        //{

        //    string Body = "Terimakasih atas laporan yang sudah diberikan. Anda telah mengirimkan ticket baru dengan Ticket ID : " + ID  + "<br>"+ "<a href = 'https://localhost:44306/' > Silahkan Cek Disini</a>";

        //    //proses mbikin koneksi ke email, template.
        //    SmtpClient client = new SmtpClient();
        //    client.Host = "smtp.gmail.com";
        //    client.Port = 587;
        //    client.EnableSsl = true;
        //    client.UseDefaultCredentials = false;
        //    var credentials = new System.Net.NetworkCredential("helpdeskkalbe@gmail.com", "qgmybjbwwypskcth");
        //    client.Credentials = credentials;
        //    client.TargetName = "STARTTLS/smtp.gmail.com";

        //    //proses mbikin konten emailnya
        //    var msg = new MailMessage();
        //    msg.From = new MailAddress("helpdeskkalbe@gmail.com");
        //    msg.To.Add(EmailTo);
        //    msg.Subject = "Konfirmasi Pelaporan Ticket Baru";
        //    msg.Body = Body;
        //    msg.IsBodyHtml = true;
        //    client.Send(msg);

        //    return "";
        //}

        //public static string SendEmailNewTicket(string EmailTo, string TicketID)
        //{
        //    string body = "Anda mempunya ticket baru dengan Ticket ID :" + TicketID + " Mohon segera cek dan selesaikan ticket anda!";

        //    SmtpClient client = new SmtpClient();
        //    client.Host = "smtp.gmail.com";
        //    client.Port = 587;
        //    client.EnableSsl = true;
        //    client.UseDefaultCredentials = false;
        //    var credentials = new System.Net.NetworkCredential("helpdeskkalbe@gmail.com", "qgmybjbwwypskcth");
        //    client.Credentials = credentials;
        //    client.TargetName = "STARTTLS/smtp.gmail.com";

        //    var msg = new MailMessage();
        //    msg.From = new MailAddress("helpdeskkalbe@gmail.com");
        //    msg.To.Add(EmailTo);
        //    msg.Subject = "Notifikasi Ticket Baru untuk Anda";
        //    msg.Body = body;
        //    client.Send(msg);

        //    return "";
        //}

        //public static string SendEmailDone(string EmailTo, string TicketID)
        //{
        //    string body = "Ticket anda dengan ID :" + TicketID + "sudah selesai dikerjakan terimakasih telah melaporkan" + "<br>" + "<a href = 'https://localhost:44306/' > Silahkan Cek Disini</a>";

        //    SmtpClient client = new SmtpClient();
        //    client.Host = "smtp.gmail.com";
        //    client.Port = 587;
        //    client.EnableSsl = true;
        //    client.UseDefaultCredentials = false;
        //    var credentials = new System.Net.NetworkCredential("helpdeskkalbe@gmail.com", "qgmybjbwwypskcth");
        //    client.Credentials = credentials;
        //    client.TargetName = "STARTTLS/smtp.gmail.com";

        //    var msg = new MailMessage();
        //    msg.From = new MailAddress("helpdeskkalbe@gmail.com");
        //    msg.To.Add(EmailTo);
        //    msg.Subject = "Ticket Selesai";
        //    msg.Body = body;
        //    msg.IsBodyHtml = true;
        //    client.Send(msg);

        //    return "";
        //}

        //[HttpGet]
        //public ActionResult OpenExportReport() //untuk buka view export report
        //{
        //    List<ParamUser> modelStaff = new List<ParamUser>(); // bentuknya list(array) karna datanya gak cuma 1
        //    var dataStaff = HelpDeskData.User.GetByRole(2);
        //    foreach (var item in dataStaff)
        //    {
        //        ParamUser dt = new ParamUser();
        //        dt.Nama = item.Nama;
        //        dt.ID = item.ID;

        //        modelStaff.Add(dt);
        //    }
        //    ViewBag.DataStaff = modelStaff;
        //    return View("ExportReport");
        //}

        //[HttpPost]
        //public string ExportReport(ReportModel model)
        //{
        //    List<string> ListStatus = new List<string>() { model.New, model.Done, model.Progres };
        //    string Staff = model.Staff;
        //    string Category = model.Category;

        //    var ticket = Ticket.GetForReport(ListStatus, Staff, Category,model.CreatedDate,model.FinishDate).ToList();

        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial; //install library excel
        //    ExcelPackage pck = new ExcelPackage();
        //    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
        //    ws.Cells["A1"].Value = "Report Ticket Keluhan IT Support";
        //    ws.Cells["A2"].Value = "Created Date";
        //    ws.Cells["B2"].Value = DateTime.Now.ToString();

        //    ws.Cells["A4"].Value = "TicketID";
        //    ws.Cells["B4"].Value = "Nama";
        //    ws.Cells["C4"].Value = "Email";
        //    ws.Cells["D4"].Value = "Subject";
        //    ws.Cells["E4"].Value = "Status"; 
        //    ws.Cells["F4"].Value = "Priority";
        //    ws.Cells["G4"].Value = "Category";
        //    ws.Cells["H4"].Value = "PIC";
        //    ws.Cells["I4"].Value = "Created Date Ticket";
        //    ws.Cells["J4"].Value = "Finish Date Ticket";
        //    ws.Cells["K4"].Value = "Message";
        //    ws.Cells["L4"].Value = "Message";
        //    int rowstart = 5;


        //    ///Styling
        //    ///
        //    ws.Row(1).Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //    ws.Row(1).Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //    ws.Row(1).Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //    ws.Row(1).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

        //    ws.Row(2).Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //    ws.Row(2).Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //    ws.Row(2).Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //    ws.Row(2).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

        //    ws.Row(4).Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //    ws.Row(4).Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //    ws.Row(4).Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //    ws.Row(4).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //    foreach (var item in ticket)
        //    {
        //        ws.Row(rowstart).Style.Border.Top.Style =  ExcelBorderStyle.Thin;
        //        ws.Row(rowstart).Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //        ws.Row(rowstart).Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //        ws.Row(rowstart).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //        ws.Cells[string.Format("A{0}", rowstart)].Value = item.TicketID;
        //        ws.Cells[string.Format("B{0}", rowstart)].Value = item.Nama;
        //        ws.Cells[string.Format("C{0}", rowstart)].Value = item.Email;
        //        ws.Cells[string.Format("D{0}", rowstart)].Value = item.Subject;
        //        ws.Cells[string.Format("E{0}", rowstart)].Value = item.Status; 
        //        ws.Cells[string.Format("F{0}", rowstart)].Value = item.Priority;
        //        ws.Cells[string.Format("G{0}", rowstart)].Value = item.Category;
        //        ws.Cells[string.Format("H{0}", rowstart)].Value = item.PIC;
        //        ws.Cells[string.Format("I{0}", rowstart)].Value = item.CreatedDate.GetValueOrDefault().ToString();
        //        ws.Cells[string.Format("J{0}", rowstart)].Value = item.FinishDate.GetValueOrDefault().ToString();
        //        ws.Cells[string.Format("K{0}", rowstart)].Value = item.Message;
        //        if (item.Status == "Done")
        //        {
        //            TimeSpan timeSpan = (item.FinishDate.GetValueOrDefault() - item.CreatedDate.GetValueOrDefault());
        //            var parts = new List<string>();

        //            int totalHours = (int)timeSpan.TotalHours;
        //            int workDays = totalHours / 8;
        //            int remainingHours = (int)timeSpan.TotalHours - 8 * workDays;

        //            if (workDays == 1) parts.Add("1 day");
        //            if (workDays > 1) parts.Add(workDays + " days");

        //            if (remainingHours == 1) parts.Add("1 hour");
        //            if (remainingHours > 1) parts.Add(timeSpan.Hours + " hours");

        //            if (timeSpan.Minutes == 1) parts.Add("1 minute");
        //            if (timeSpan.Minutes > 1) parts.Add(timeSpan.Minutes + " minutes");

        //          //  return string.Join(", ", parts);




        //            ws.Cells[string.Format("L{0}", rowstart)].Value = string.Join(", ", parts);

        //        }
        //        else
        //        {
        //            ws.Cells[string.Format("L{0}", rowstart)].Value = "Not Finished Yet";

        //        }
        //        rowstart++;
        //    }
        //    //proses generate file
        //    ws.Cells["A:AZ"].AutoFitColumns();
        //    Response.Clear();
        //    Response.ContentType = "Application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    Response.AddHeader("content-disposition", "attachment: filename=" + "TicketReport.xlsx");
        //    Response.BinaryWrite(pck.GetAsByteArray());
        //    Response.End();

        //    return null;
        //}

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
        public static string GenerateIDProker(string KodeLK)
        {
            Random random = new Random();
            int number = random.Next(1,100);
            string build = KodeLK + "22" + number;
            return build;
        }
        public static string CreateDecimalMoneyFormat(decimal value)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("id-ID");
            //return value.ToString("00.00", culture);
            string result = string.Format(culture, "{0:C0}", value);
            result = result.Replace("Rp", "");
            return result;
        }
        //[HttpGet]
        //public ActionResult AllTicketUser(string nama)
        //{
        //    List<ListTicketAll> model = new List<ListTicketAll>();
        //    // List <Ticket> model = new List<Ticket>();
        //    var data = Ticket.GetByNama(nama).OrderByDescending(x => x.CreatedDate);
        //    //model = data;
        //    foreach (var item in data)
        //    {
        //        ListTicketAll dt = new ListTicketAll();
        //        dt.Nama = item.Nama;
        //        dt.ID = item.TicketID;
        //        dt.Priority = item.Priority;
        //        dt.Email = item.Email;
        //        dt.Subject = item.Subject;
        //        dt.Message = item.Message;
        //        dt.PIC = item.PIC;
        //        dt.Status = item.Status;
        //        dt.Date = item.CreatedDate.GetValueOrDefault().ToString("MM/dd/yyyy");
        //        model.Add(dt);
        //    }
        //    ViewBag.Data = model;
        //    return View("AllTicketUser");
        //}

    }
}
