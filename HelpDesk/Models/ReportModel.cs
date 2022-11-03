using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.Models
{
    public class ReportModel
    {
        public string New { get; set; }
        public string Done { get; set; }
        public string Progres { get; set; }
        public string All { get; set; }
        public string Low { get; set; }
        public string Medium { get; set; }
        public string Critical { get; set; }
        public string Staff { get; set; }
        public string Category { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime FinishDate { get; set; }

    }
}