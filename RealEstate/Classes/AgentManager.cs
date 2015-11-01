using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Classes
{
    class AgentManager
    {
        public bool AddAgent(string name, string surname, string phone, string email, string password)
        {
            DatabaseManager dbManager = new DatabaseManager();
            Cryptography crypto = new Cryptography();

            MySql.Data.MySqlClient.MySqlCommand com = new MySql.Data.MySqlClient.MySqlCommand();

            return (dbManager.NonReturnQuery("INSERT INTO Agent (Agent_Name , Agent_Surname, Agent_Phone, Agent_Email, Agent_Password) VALUES ('" + name + "','" + surname + "','" + phone + "','" + email + "','" + Cryptography.CreateHash(password).ToString() + "');"));
        }
        public bool DeleteAgent(string email)
        {
            DatabaseManager dbManager = new DatabaseManager();

            return (dbManager.NonReturnQuery("DELETE FROM Agent WHERE Agent_Email = '" + email + "';"));
        }

        public bool EditName(string email, string newName)
        {
            DatabaseManager dbManager = new DatabaseManager();

            return (dbManager.NonReturnQuery("UPDATE Agent SET Agent_Name ='" + newName + "' WHERE Agent_Email ='" + email + "';"));
        }

        public bool EditSurname(string email, string newSurname)
        {
            DatabaseManager dbManager = new DatabaseManager();

            return (dbManager.NonReturnQuery("UPDATE Agent SET Agent_Surname ='" + newSurname + "' WHERE Agent_Email ='" + email + "';"));
        }

        public bool EditPhone(string email, string newPhone)
        {
            DatabaseManager dbManager = new DatabaseManager();

            return (dbManager.NonReturnQuery("UPDATE Agent SET Agent_Phone ='" + newPhone + "' WHERE Agent_Email ='" + email + "';"));
        }

        public bool EditEmail(string oldEmail, string newEmail)
        {
            DatabaseManager dbManager = new DatabaseManager();

            return (dbManager.NonReturnQuery("UPDATE Agent SET Agent_Email ='" + newEmail + "' WHERE Agent_Email ='" + oldEmail + "';"));
        }

        public bool EditPassword(string email, string newPassword)
        {
            DatabaseManager dbManager = new DatabaseManager();
            Cryptography crypto = new Cryptography();

            return (dbManager.NonReturnQuery("UPDATE Agent set Agent_Password = '" + Cryptography.CreateHash(newPassword) + "' WHERE Agent_Email ='" + email + "';"));
        }

        public bool CanAddAgent(string email)
        {
            DatabaseManager dbManager = new DatabaseManager();

            bool canAdd = true;


            var agentEmails = dbManager.ReturnQuery("SELECT Agent_Email FROM Agent WHERE Agent_Email = '" + email + "';");

            foreach (var agentEmail in agentEmails)
            {
                canAdd = false;
                break;
            }

            return canAdd;
        }
    }
}
