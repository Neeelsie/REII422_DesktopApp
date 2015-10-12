using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Classes
{
    class AdminManager
    {
        public bool AddAdmin(string username, string password)
        {
            DatabaseManager dbManager = new DatabaseManager();
            Cryptography crypto = new Cryptography();

            return (dbManager.NonReturnQuery("INSERT INTO Admin (Admin_Username , Admin_Password) VALUES ('" + username + "','" + crypto.EncryptString(password) + "');"));
        }
        public bool DeleteAdmin(string username)
        {
            DatabaseManager dbManager = new DatabaseManager();

            return (dbManager.NonReturnQuery("DELETE FROM Admin WHERE Admin_Username = '" + username + "';"));
        }

        public bool EditUsername(string oldUsername, string newUsername)
        {
            DatabaseManager dbManager = new DatabaseManager();

            return (dbManager.NonReturnQuery("UPDATE Admin SET Admin_Username ='" + newUsername + "' WHERE Admin_Username ='" + oldUsername + "';"));
        }

        public bool EditPassword(string username, string newPassword)
        {
            DatabaseManager dbManager = new DatabaseManager();
            Cryptography crypto = new Cryptography();

            return (dbManager.NonReturnQuery("UPDATE Admin set Admin_Password = '" + crypto.EncryptString(newPassword) + "' WHERE Admin_Username ='" + username + "';"));
        }

        public bool CanAddAdmin(string username)
        {
            DatabaseManager dbManager = new DatabaseManager();

            bool canAdd = true;


            var adminUsernames = dbManager.ReturnQuery("SELECT Admin_Username FROM Admin WHERE Admin_Username = '" + username + "';");

            foreach (var adminUsername in adminUsernames)
            {
                canAdd = false;
                break;
            }

            return canAdd;

        }
    }
}
