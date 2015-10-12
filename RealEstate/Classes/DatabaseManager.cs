using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace RealEstate.Classes
{
    class DatabaseManager
    {
        ConfigManager configManager = new ConfigManager();
        MySqlConnection connection;

        public DatabaseManager()
        {
            if (configManager.ConfigLoaded)
            {
                connection = new MySqlConnection(configManager.ConnectionString);
            }
        }

        /// <summary>
        /// Opens database
        /// </summary>
        /// <returns>Boolean if database is open</returns>
        public bool OpenDatabase()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException e)
            {
                Console.WriteLine("OpenDatabase MySqlException : " + e.ToString());
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("OpenDatabase Exception : " + e.ToString());
                return false;
            }
        }

        /// <summary>
        /// Closes database
        /// </summary>
        /// <returns>Boolean if closed</returns>
        public bool CloseDatabase()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// Test database connection
        /// </summary>
        /// <returns>Returns boolean if connection is working</returns>
        public bool ConnectionWorking()
        {
            if (OpenDatabase())
            {
                CloseDatabase();
                return true;
            }
            else
            {
                return false;
            }
        }

       
        public bool TestConnectionSettings(string server, string username, string password, string database, string portno)
        {
            string connection_string = "server=" + server + ";uid=" + username + ";database=" + database + ";port=" + portno + ";pwd=" + password + ";";
            MySqlConnection temp_conn = new MySqlConnection(connection_string);

            try
            {
                temp_conn.Open();

                temp_conn.Close();

                temp_conn.Dispose();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Executes MySQL return query eg. SELECT
        /// </summary>
        /// <param name="query">MySQL query</param>
        /// <returns>string array with rows and columns corresponding to query result</returns>
        public IEnumerable<string[]> ReturnQuery(string query)
        {
            if (OpenDatabase())
            {
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = string.Format(query);

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string[] row = new string[reader.FieldCount];

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[i] = reader.GetString(i);
                    }
                    yield return row;
                }
                reader.Close();
                CloseDatabase();
            }
        }

        /// <summary>
        /// Executes non return MySQL query eg MODIFY,INSERT,DELETE
        /// </summary>
        /// <param name="query">MySQL query</param>
        /// <returns>Boolean if query is executed</returns>
        public bool NonReturnQuery(string query)
        {
            bool querySuccess = false;

            if (OpenDatabase())
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                try
                {
                    command.ExecuteNonQuery();
                    CloseDatabase();
                    querySuccess = true;
                }
                catch
                {
                    querySuccess = false;
                }
            }

            return querySuccess;
        }
    }
}
