
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB;
using Npgsql;
using System.IO;
using System.Data;
using System.Reflection;

namespace HelpDeskData
{
    public partial class ExtentionTransaction
    {

        public static int ExecuteNonQuerys(string sql)
        {
            int result = 0;
            Npgsql.NpgsqlConnection conn = new Npgsql.NpgsqlConnection(ConnectionString());
            conn.Open();
            Npgsql.NpgsqlCommand da = new Npgsql.NpgsqlCommand(sql, conn);
            result = da.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
            return result;
        }

        public static List<T> ExecuteQuerys<T>(string sql)
        {
            Npgsql.NpgsqlConnection conn = new Npgsql.NpgsqlConnection(ConnectionString());
            conn.Open();
            Npgsql.NpgsqlCommand da = new Npgsql.NpgsqlCommand(sql, conn);
            var dataReader = da.ExecuteReader();
            var response = DataReaderMapToList<T>(dataReader);
            conn.Close();
            conn.Dispose();
            return response;
        }



        static string ConnectionString()
        {
            var connection = System.Configuration.ConfigurationManager.ConnectionStrings["HelpDeskData"].ConnectionString;
            return connection;
        }


        public static List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!object.Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }

    }
}
