

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB;

namespace HelpDeskData
{
    public partial class ExtentionTransaction
    {

        public static void BeginTransaction()
        {

            //READ COMMITTED
            //A statement can only see rows committed before it began. This is the default.
            CurrentDataContext.CurrentContext.BeginTransaction();
        }


        public static void CommitTransaction()
        {
            CurrentDataContext.CurrentContext.CommitTransaction();
        }


        public static void RollbackTransaction()
        {
             CurrentDataContext.CurrentContext.RollbackTransaction();
        }



        public static void Inserted<T>(T dataObject)
        {

            CurrentDataContext.CurrentContext.Insert(dataObject);

        }

        public static void Updated<T>(T dataObject)
        {
             CurrentDataContext.CurrentContext.Update(dataObject);

        }


        public static object InsertedWithIdentity<T>(T dataObject)
        {

            return  CurrentDataContext.CurrentContext.InsertWithIdentity(dataObject);

        }


        public static void Deleted<T>(T dataObject)
        {



             CurrentDataContext.CurrentContext.Delete(dataObject);

        }

        //public static int ExecuteNonQuerys(string sql)
        //{
        //    string connectionString = ReportingKlik.Common.SiteSettings.ConnectionString;
        //    int result = 0;
        //    Npgsql.NpgsqlConnection conn = new Npgsql.NpgsqlConnection(connectionString);
        //    conn.Open();
        //    // string sql = "SELECT * FROM simple_table";
        //    // data adapter making request from our connection
        //    Npgsql.NpgsqlCommand da = new Npgsql.NpgsqlCommand(sql, conn);

        //    result = da.ExecuteNonQuery();

        //    //kalo butuh datanya
        //    //var check123c = da.ExecuteReader();

        //    conn.Close();
        //    conn.Dispose();

        //    return result;
        //}


    }
}
