using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Objects;

using HelpDeskData;


namespace HelpDeskData
{
    public partial class PengajuanDana
    {
        public void Insert(string by)
        {
            ExtentionTransaction.Inserted(this);
        }

        public void Update(string by)
        {
            ExtentionTransaction.Updated(this);
        }
        public void Delete(string by)
        {
            ExtentionTransaction.Updated(this);
        }
        public static IQueryable<PengajuanDana> GetByAll()
        {
            return CurrentDataContext.CurrentContext.PengajuanDanas.Where(x => !x.IsDelete);
        }
        public static PengajuanDana GetByID(string ID)
        {
            return CurrentDataContext.CurrentContext.PengajuanDanas.FirstOrDefault(x => x.IDPengajuan == ID);
        }

    }
}
