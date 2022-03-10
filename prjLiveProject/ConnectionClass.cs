using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjLiveProject
{
    class ConnectionClass
    {
        public static string conStr = "datasource=localhost;port=3306;username=root;password=Password1;database=uniyat_live_project";
        public static MySqlConnection dbConnection = new MySqlConnection(conStr);
    }
}
