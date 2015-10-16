using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace RealEstate.Classes
{
    class ConfigManager
    {
        DataSets.Config config = new DataSets.Config();
        DataRow r;

        private string serverKey = "Server";
        private string userKey = "Username";
        private string passwordKey = "Password";
        private string dbKey = "DB";
        private string portKey = "Port";
        private string ftpWebdirKey = "FTP";
        private string ftpUserKey = "FTPUser";
        private string ftpPasswordKey = "FTPPassword";
        

        const string configDir = "RealEstate";
        const string configFile = "config.xml";
        string configDirPath = null;
        string configFilePath = null;

        bool ConfigExists;
        public string ServerIP { get; private set; }
        public string ServerUser { get; private set; }
        string ServerPassword;
        public string ServerDB { get; private set; }
        public string ServerPort { get; private set; }

        public string ConnectionString { get; private set; }
        public bool ConfigLoaded { get; private set; }

        public string FtpWebDirectory { get; private set; }
        public string FtpUser { get; private set; }
        public string FtpPassword { get; private set; }


        public ConfigManager()
        {
            configDirPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + '/' + configDir;
            configFilePath = configDirPath + '/' + configFile;
            ConfigExists = CheckConfigExists();
            ConfigLoaded = false;

            if (!ConfigExists)
            {
                CreateConfigFile();
            }

            LoadConfig();
        }

        /// <summary>
        /// Check if config file exists
        /// </summary>
        /// <returns>Boolean value</returns>
        private bool CheckConfigExists()
        {
            return File.Exists(configFilePath);
        }

        /// <summary>
        /// Creates config file with keys and empty values
        /// </summary>
        private void CreateConfigFile()
        {
            Cryptography crypto = new Cryptography();

            if (!Directory.Exists(configDirPath))
            {
                Directory.CreateDirectory(configDirPath);
            }

            config.Options.AddOptionsRow(crypto.EncryptString(serverKey), "");
            config.Options.AddOptionsRow(crypto.EncryptString(userKey), "");
            config.Options.AddOptionsRow(crypto.EncryptString(passwordKey), "");
            config.Options.AddOptionsRow(crypto.EncryptString(dbKey), "");
            config.Options.AddOptionsRow(crypto.EncryptString(portKey), "");
            config.Options.AddOptionsRow(crypto.EncryptString(ftpWebdirKey), "");
            config.Options.AddOptionsRow(crypto.EncryptString(ftpUserKey), "");
            config.Options.AddOptionsRow(crypto.EncryptString(ftpPasswordKey), "");
            WriteConfig();
        }

        /// <summary>
        /// Writes config to XML file
        /// </summary>
        private void WriteConfig()
        {
            config.WriteXml(configFilePath);
        }

        /// <summary>
        /// Loads values from config XML file
        /// </summary>
        private void LoadConfig()
        {
            try
            {
                config.ReadXml(configFilePath);
                Cryptography crypto = new Cryptography();

                r = config.Options.Select("Key ='" + crypto.EncryptString(serverKey) + "'")[0];
                ServerIP = crypto.DecryptString(r[1].ToString());

                r = config.Options.Select("Key ='" + crypto.EncryptString(userKey) + "'")[0];
                ServerUser = crypto.DecryptString(r[1].ToString());

                r = config.Options.Select("Key ='" + crypto.EncryptString(passwordKey) + "'")[0];
                ServerPassword = crypto.DecryptString(r[1].ToString());

                r = config.Options.Select("Key ='" + crypto.EncryptString(dbKey) + "'")[0];
                ServerDB = crypto.DecryptString(r[1].ToString());

                r = config.Options.Select("Key ='" + crypto.EncryptString(portKey) + "'")[0];
                ServerPort = crypto.DecryptString(r[1].ToString());

                r = config.Options.Select("Key = '" + crypto.EncryptString(ftpWebdirKey) + "'")[0];
                FtpWebDirectory = crypto.DecryptString(r[1].ToString());

                r = config.Options.Select("Key = '" + crypto.EncryptString(ftpUserKey) + "'")[0];
                FtpUser = crypto.DecryptString(r[1].ToString());

                r = config.Options.Select("Key = '" + crypto.EncryptString(ftpPasswordKey) + "'")[0];
                FtpPassword = crypto.DecryptString(r[1].ToString());

                ConnectionString = "server=" + ServerIP + ";uid=" + ServerUser + ";database=" + ServerDB + ";port=" + ServerPort + ";pwd=" + ServerPassword + ";";

                ConfigLoaded = true;
            }
            /*catch
            {
                ConfigLoaded = false;
            }*/
            catch (Exception e)
            {
                Console.WriteLine("LoadConfig Exception " + e.ToString());
                ConfigLoaded = false;
            }

        }

        /// <summary>
        /// Saves data and writes it encrypted to config XML file
        /// </summary>
        /// <param name="server">Server IP address string</param>
        /// <param name="username">Server username</param>
        /// <param name="password">Server password</param>
        /// <param name="database">Server database</param>
        /// <param name="portno">Server port number</param>
        /// <returns>Boolean if saved</returns>
        public bool SaveDBConfig(string server, string username, string password, string database, string portno)
        {
            try
            {
                Cryptography crypto = new Cryptography();
                ServerIP = server;
                ServerUser = username;
                ServerPassword = password;
                ServerDB = database;
                ServerPort = portno;

                r = config.Options.Select("Key = '" + crypto.EncryptString(serverKey) + "'")[0];
                r[1] = crypto.EncryptString(ServerIP);

                r = config.Options.Select("Key = '" + crypto.EncryptString(userKey) + "'")[0];
                r[1] = crypto.EncryptString(ServerUser);

                r = config.Options.Select("Key = '" + crypto.EncryptString(passwordKey) + "'")[0];
                r[1] = crypto.EncryptString(ServerPassword);

                r = config.Options.Select("Key = '" + crypto.EncryptString(dbKey) + "'")[0];
                r[1] = crypto.EncryptString(ServerDB);

                r = config.Options.Select("Key = '" + crypto.EncryptString(portKey) + "'")[0];
                r[1] = crypto.EncryptString(ServerPort);

                WriteConfig();
                LoadConfig();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SaveFTPConfig(string webdir, string username, string password)
        {
            try
            {
                Cryptography crypto = new Cryptography();
                FtpWebDirectory = webdir;
                FtpUser = username;
                FtpPassword = password;

                r = config.Options.Select("Key = '" + crypto.EncryptString(ftpWebdirKey) + "'")[0];
                r[1] = crypto.EncryptString(webdir);

                r = config.Options.Select("Key = '" + crypto.EncryptString(ftpUserKey) + "'")[0];
                r[1] = crypto.EncryptString(FtpUser);

                r = config.Options.Select("Key = '" + crypto.EncryptString(ftpPasswordKey) + "'")[0];
                r[1] = crypto.EncryptString(FtpPassword);

                WriteConfig();
                LoadConfig();

                return true;
            }
            catch
            {
                return false;
            }
        }



    }
}
