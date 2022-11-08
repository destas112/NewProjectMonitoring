using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Objects;

using HelpDeskData;


namespace HelpDeskData
{
    public partial class Ticket
    {
        //public void Insert(string by)
        //{
        //    ExtentionTransaction.Inserted(this);
        //}
        //public static Ticket GetByID(string ID)
        //{
        //    return CurrentDataContext.CurrentContext.Tickets.FirstOrDefault(x => x.TicketID == ID );
        //}
        //public static IQueryable<Ticket> GetByPIC(string pic)
        //{
        //    return CurrentDataContext.CurrentContext.Tickets.Where(x => x.PIC == pic && !x.IsDelete).OrderBy(x => x.CreatedDate);
        
        //}
        //public static IQueryable<Ticket> GetByNama(string nama)
        //{
        //    return CurrentDataContext.CurrentContext.Tickets.Where(x => x.Nama == nama && !x.IsDelete).OrderBy(x => x.CreatedDate);
        //}

        //public static IQueryable<Ticket> GetByAll()
        //{
        //    return CurrentDataContext.CurrentContext.Tickets.Where(x => !x.IsDelete );
        //}

        //public static IQueryable<Ticket> GetForReport(List<string> Status,string staff,string category,DateTime CreatedDate,DateTime FinishDate)
        //{
        //    return CurrentDataContext.CurrentContext.Tickets.Where(x => !x.IsDelete && (staff == "All" ? true : x.PIC == staff) && (category == "All" ? true : x.Category == category) && Status.Contains(x.Status) && x.CreatedDate >= CreatedDate && x.CreatedDate <= FinishDate ).OrderBy(x => x.CreatedDate);
        //}


        //public void Update(string by)
        //{
        //    ExtentionTransaction.Updated(this);
        //}
        //public void Delete(string by)
        //{
        //    ExtentionTransaction.Updated(this);
        //}
    }
}
