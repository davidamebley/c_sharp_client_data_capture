using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace prjLiveProject
{
    static class LastInsertedIDClass
    {
        public static string lastId = "";

        public static int GetLastID()
        {
            MySqlConnection dbConnection = ConnectionClass.dbConnection;
            int lastId = 0;
            string selectQuery = "SELECT `last_id` FROM `last_id`";
            MySqlCommand dbCommand = new MySqlCommand(selectQuery, dbConnection);
            dbCommand.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader myDataReader = dbCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {

                    while (myDataReader.Read())
                    {
                        lastId = myDataReader.GetInt32(0);
                    }
                }
                else
                {
                    lastId = 0;
                }
                myDataReader.Close();
                dbConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbConnection.Close();
            return lastId;
        }


    }
}
