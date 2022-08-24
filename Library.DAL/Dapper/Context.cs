using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace LibraryAPI.DAL.Dapper
{
    public class Context
    {
        public static string GlobalSqlConnectionString = "";
        public static string OtherSqlConnectionString = "";

        public static T ExeScalarQuery<T>(String QueryText, DynamicParameters paras)
        {
            try
            {
                GlobalSqlConnectionString = string.Format(GlobalSqlConnectionString);
                T Result;


                using (SqlConnection conn = new SqlConnection(GlobalSqlConnectionString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                        conn.Open();
                    Result = conn.Query<T>(QueryText, paras).FirstOrDefault();
                }
                return Result;
            }
            catch (Exception ex)
            {
                //Common.CreateLog(QueryText + ex.Message, "Db Call error", "ExeScalarQuery");
                return default(T);
            }
        }
        public static int ExeQuery(String QueryText, DynamicParameters paras)
        {
                


            GlobalSqlConnectionString = string.Format(GlobalSqlConnectionString);

            int Result;
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSqlConnectionString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                        conn.Open();
                    Result = conn.Execute(QueryText, paras);
                }

                return Result;
            }
            catch (Exception ex)
                {//found
             // Common.CreateLog(QueryText + ex.Message, "Db Call error", "ExeQuery");
                return 0;
            }
        }

        public static List<T> ExeQueryList<T>(String QueryText, DynamicParameters paras)
        {
            try
            {

                
                GlobalSqlConnectionString = string.Format(GlobalSqlConnectionString);

                List<T> Result;
                using (SqlConnection conn = new SqlConnection(GlobalSqlConnectionString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                        conn.Open();
                    Result = conn.Query<T>(QueryText, paras).ToList();
                }
                return Result;
            }
            catch (Exception ex)
            {
                //ePUB.Utility.Common.CreateLog(QueryText + ex.Message, "Db Call error", "ExeQueryList");
                return default(List<T>);
            }
        }

        public static T ExeScalarQuery<T>(String QueryText)
        {
            try
            {
                T Result;
                using (SqlConnection conn = new SqlConnection(GlobalSqlConnectionString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                        conn.Open();
                    Result = conn.Query<T>(QueryText).FirstOrDefault();
                }
                return Result;
            }
            catch (Exception ex)
            {
                // ePUB.Utility.Common.CreateLog(QueryText + ex.Message, "Db Call error", "ExeQueryList");
                return default(T);
            }

        }
        public static List<T> ExeQueryList<T>(String QueryText)
        {
            GlobalSqlConnectionString = string.Format(GlobalSqlConnectionString);

            List<T> Result;
            using (SqlConnection conn = new SqlConnection(GlobalSqlConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                Result = conn.Query<T>(QueryText).ToList();
            }
            return Result;
        }
        public static T ExeSPScaler<T>(String QueryText, DynamicParameters paras)
        {

            if (GlobalSqlConnectionString != "")
            {
                GlobalSqlConnectionString = OtherSqlConnectionString;
            }
            GlobalSqlConnectionString = string.Format(GlobalSqlConnectionString);


            T Result;
            using (SqlConnection conn = new SqlConnection(GlobalSqlConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                Result = conn.Query<T>(QueryText, paras, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            return Result;
        }
        public static List<T> ExeSPList<T>(String QueryText, DynamicParameters paras)
        {

            if (GlobalSqlConnectionString != "")
            {
                GlobalSqlConnectionString = OtherSqlConnectionString;
            }
            GlobalSqlConnectionString = string.Format(GlobalSqlConnectionString);


            List<T> Result;
            using (SqlConnection conn = new SqlConnection(GlobalSqlConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                Result = conn.Query<T>(QueryText, paras, commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
            return Result;
        }
        public static List<dynamic> ExeSPSclarMultiple(String QueryText, DynamicParameters paras, string SqlConnectionString = "")
        {
            if (SqlConnectionString == "")
                SqlConnectionString = GlobalSqlConnectionString;
            dynamic Result;
            //try
            //{
            using (SqlConnection conn = new SqlConnection(SqlConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                using (var multi = conn.QueryMultiple(QueryText, paras, commandType: System.Data.CommandType.StoredProcedure))
                {
                    List<dynamic> oLst = new List<dynamic>();

                    while (!multi.IsConsumed)
                    {

                        oLst.Add(multi.Read().ToList());

                    }
                    Result = oLst;
                }
                return Result;
            }

        }
        public static int ExeSP(String QueryText, DynamicParameters paras)
        {

            if (GlobalSqlConnectionString != "")
            {
                GlobalSqlConnectionString = OtherSqlConnectionString;
            }
            GlobalSqlConnectionString = string.Format(GlobalSqlConnectionString);


            int Result;
            using (SqlConnection conn = new SqlConnection(GlobalSqlConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                Result = conn.Execute(QueryText, paras, commandType: System.Data.CommandType.StoredProcedure);
            }
            return Result;
        }

    }
}
