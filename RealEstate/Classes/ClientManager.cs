using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Classes
{
    class ClientManager
    {
        public bool AddClient(string name, string surname, string phone, string email, string password)
        {
            DatabaseManager dbManager = new DatabaseManager();
            Cryptography crypto = new Cryptography();

            return (dbManager.NonReturnQuery("INSERT INTO Clients (Client_Name , Client_Surname, Client_Phone, Client_Email, Client_Password) VALUES ('" + name + "','" + surname + "','" + phone + "','" + email + "','" + crypto.EncryptString(password) + "');"));
        }
        public bool DeleteClient(string email)
        {
            DatabaseManager dbManager = new DatabaseManager();

            return (dbManager.NonReturnQuery("DELETE FROM Clients WHERE Client_Email = '" + email + "';"));
        }

        public bool EditName(string email, string newName)
        {
            DatabaseManager dbManager = new DatabaseManager();

            return (dbManager.NonReturnQuery("UPDATE Clients SET Client_Name ='" + newName + "' WHERE Client_Email ='" + email + "';"));
        }

        public bool EditSurname(string email, string newSurname)
        {
            DatabaseManager dbManager = new DatabaseManager();

            return (dbManager.NonReturnQuery("UPDATE Clients SET Client_Surname ='" + newSurname + "' WHERE Client_Email ='" + email + "';"));
        }

        public bool EditPhone(string email, string newPhone)
        {
            DatabaseManager dbManager = new DatabaseManager();

            return (dbManager.NonReturnQuery("UPDATE Clients SET Client_Phone ='" + newPhone + "' WHERE Client_Email ='" + email + "';"));
        }

        public bool EditEmail(string oldEmail, string newEmail)
        {
            DatabaseManager dbManager = new DatabaseManager();

            return (dbManager.NonReturnQuery("UPDATE Clients SET Client_Email ='" + newEmail + "' WHERE Client_Email ='" + oldEmail + "';"));
        }

        public bool EditPassword(string email, string newPassword)
        {
            DatabaseManager dbManager = new DatabaseManager();
            Cryptography crypto = new Cryptography();

            return (dbManager.NonReturnQuery("UPDATE Clients set Client_Password = '" + crypto.EncryptString(newPassword) + "' WHERE Client_Email ='" + email + "';"));
        }

        public bool CanAddClient(string email)
        {
            DatabaseManager dbManager = new DatabaseManager();

            bool canAdd = true;


            var clientEmails = dbManager.ReturnQuery("SELECT Client_Email FROM Clients WHERE Client_Email = '" + email + "';");

            foreach (var clientEmail in clientEmails)
            {
                canAdd = false;
                break;
            }

            return canAdd;
        }
    }
}
