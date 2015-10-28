using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Classes
{
    class Session
    {
        public bool IsAdmin { get; private set; }
        public bool LoginSuccessfull { get; private set; }

        public string username { get; private set; }
        string password = "";

        public Session(string loginUsername, string loginPassword)
        {
            username = "";
            password = "";

            IsAdmin = UsernameInAdminTable(loginUsername);

            password = GetPassword(loginUsername, IsAdmin);
            username = GetUsername(loginUsername, IsAdmin);

            if (password != "" && username != "")
            {
                if ((username == loginUsername) && (password == loginPassword))
                {
                    LoginSuccessfull = true;
                }
            }

        }

        private string GetPassword(string username, bool admin)
        {
            string retValue = "";

            DatabaseManager dbManager = new DatabaseManager();

            if (admin)
            {
                var result = dbManager.ReturnQuery("SELECT Admin_Password FROM Admin WHERE Admin_Username = '" + username + "';");

                foreach (var r in result)
                {
                    retValue = r[0].ToString();
                    break;
                }
            }
            else
            {
                var result = dbManager.ReturnQuery("SELECT Agent_Password FROM Agent WHERE Agent_Email = '" + username + "';");

                foreach (var r in result)
                {
                    retValue = r[0].ToString();
                    break;
                }
            }

            return retValue;
        }

        private string GetUsername(string username, bool admin)
        {
            string retValue = "";

            DatabaseManager dbManager = new DatabaseManager();

            if (admin)
            {
                var result = dbManager.ReturnQuery("SELECT Admin_Username FROM Admin WHERE Admin_Username = '" + username + "';");

                foreach (var r in result)
                {
                    retValue = r[0].ToString();
                    break;
                }
            }
            else
            {
                var result = dbManager.ReturnQuery("SELECT Agent_Email FROM Agent WHERE Agent_Email = '" + username + "';");

                foreach (var r in result)
                {
                    retValue = r[0].ToString();
                    break;
                }
            }

            return retValue;
        }

        private bool UsernameInAdminTable(string username)
        {
            DatabaseManager dbManger = new DatabaseManager();

            var result = dbManger.ReturnQuery("SELECT Admin_Username FROM Admin WHERE Admin_Username = '" + username + "';");

            bool retValue = false;
            foreach (var r in result)
            {
                retValue = true;
                break;
            }
            return retValue;
        }

    }
}
