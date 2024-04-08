using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UAM.Model.Entity;
using UAM.Model.Models;

namespace UAM.Plugin.Common
{
    public class DBConnection
    {
        public static string ConnectString;
        public static string UserConnectString;
        public DBConnection()
        {
            ConnectString = ConfigurationManager.AppSettings["DatabaseConnString"].ToString();
            if (string.IsNullOrEmpty(ConnectString))
            {
                MessageBox.Show("No Connection!");
                return;
            }
        }


        /// <summary>
        /// 查询用户信息列表
        /// </summary>
        /// <returns></returns>
        public List<EntityUser> QueryUsers()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectString))
            {
                string sql = "select userid,userpassword,username,authorityid from public.uam_user ";

                List<EntityUser> UserList = connection.Query<EntityUser>(sql).ToList();

                if (UserList.Count > 0 && UserList != null)
                {
                    return UserList;
                }
                else
                {
                    return null;
                }
            }
        }

        public DataSet SearchCommand(string aircraftName)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectString))
            {
                string sql = @"select * from uam_vertiport  WHERE aircraft_name='" + aircraftName + @"' ORDER BY City;";

                DataSet ds = (DataSet)connection.Query<DataSet>(sql);

                if (ds.Tables.Count > 0 && ds != null)
                {
                    return ds;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<VertiportsModel> SearchAllVertiportCommand()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectString))
            {
                string sql = @"select * from uam_vertiport";

                List<VertiportsModel> vertiports = connection.Query<VertiportsModel>(sql).ToList();

                if (vertiports.Count > 0 && vertiports != null)
                {
                    return vertiports;
                }
                else
                {
                    return null;
                }
            }
        }

        public DataSet SearchLa(string icName, string name)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectString))
            {
                string sql = @"select value from ic_request_parameters  WHERE ic_set_name='" + icName + "' AND " + "name='" + name + "';";

                DataSet ds = (DataSet)connection.Query<DataSet>(sql);

                if (ds.Tables.Count > 0 && ds != null)
                {
                    return ds;
                }
                else
                {
                    return null;
                }
            }
        }


    }
}
