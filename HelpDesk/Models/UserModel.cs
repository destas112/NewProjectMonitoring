using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.Models
{
    public class ParamUser
    {
        public string ID { get; set; }
        public string Nama { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public string Keterangan { get; set; }
        public string Email { get; set; }
    }
}