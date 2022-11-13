using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Objects;

using HelpDeskData;


namespace HelpDeskData
{
    public partial class PertanggungJawaban
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
        public static PertanggungJawaban GetByProkerID(string ID)
        {
            return CurrentDataContext.CurrentContext.PertanggungJawabans.FirstOrDefault(x => x.IDProker == ID);
        }

    }
}
