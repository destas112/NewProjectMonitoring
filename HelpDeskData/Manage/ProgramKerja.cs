using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Objects;

using HelpDeskData;


namespace HelpDeskData
{
    public partial class ProgramKerja
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
        public static IQueryable<ProgramKerja> GetByAll()
        {
            return CurrentDataContext.CurrentContext.ProgramKerjas.Where(x => !x.IsDelete);
        }
        public static IQueryable<ProgramKerja> GetByPIC(string pic)
        {
            return CurrentDataContext.CurrentContext.ProgramKerjas.Where(x => x.NamaLK == pic && !x.IsDelete && x.Status != "3").OrderBy(x => x.CreatedDate);

        }
        public static ProgramKerja GetByID(string ID)
        {
            return CurrentDataContext.CurrentContext.ProgramKerjas.FirstOrDefault(x => x.ProkerID == ID);
        }
        public static IQueryable<ProgramKerja> GetForListLPJ(string pic)
        {
            return CurrentDataContext.CurrentContext.ProgramKerjas.Where(x => x.NamaLK == pic && !x.IsDelete && x.Status == "3").OrderBy(x => x.CreatedDate);

        }
        public static IQueryable<ProgramKerja> GetByNamaLK(string LK)
        {
            return CurrentDataContext.CurrentContext.ProgramKerjas.Where(x => x.NamaLK == LK);

        }
    }
}
