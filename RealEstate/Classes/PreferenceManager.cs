using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Classes
{
    class PreferenceManager
    {
        public bool AddPreference(int clientID, int minBedrooms, int maxBedrooms, int minBathrooms, int maxBathrooms, int minGarages, int maxGarages, int minPlotSize, int maxPlotSize, int minHouseSize, int maxHouseSize, int minPrice, int maxPrice, int hasPool)
        {
            DatabaseManager dbManager = new DatabaseManager();

            return dbManager.NonReturnQuery("INSERT INTO Preference (Preference_Client_ID, Preference_Min_Bedrooms, Preference_Max_Bedrooms, Preference_Min_Bathrooms, Preference_Max_Bathrooms, Preference_Min_Garages, Preference_Max_Garages, Preference_hasPool, Preference_Min_Plot_Size, Preference_Max_Plot_Size, Preference_Min_House_Size, Preference_Max_House_Size, Preference_Min_Price, Preference_Max_Price) VALUES (" + clientID + "," + minBedrooms + "," + maxBedrooms + "," + minBathrooms + "," + maxBathrooms + "," + minGarages + "," + maxGarages + "," + hasPool + "," + minPlotSize + "," + maxPlotSize + "," + minHouseSize + "," + maxHouseSize + "," + minPrice + "," + maxPrice + ");");
        }
        public bool AddPreferenceArea(int preferenceID, string areaIn)
        {
            string[] area;
            string[] dell = new string[] { ", " };
            area = areaIn.Split(dell, System.StringSplitOptions.None);
            DatabaseManager dbManager = new DatabaseManager();
            LocationManager locManager = new LocationManager();

            return (dbManager.NonReturnQuery("INSERT INTO Preference_Area (Preference_ID, Area_ID) values (" + preferenceID + "," + locManager.AreaID(locManager.CityID(area[1], locManager.ProvinceID(area[0])), area[2]) + ");"));
        }
        public bool EditPreference(int preferenceID, int clientID, int minBedrooms, int maxBedrooms, int minBathrooms, int maxBathrooms, int minGarages, int maxGarages, int minPlotSize, int maxPlotSize, int minHouseSize, int maxHouseSize, int minPrice, int maxPrice, int hasPool)
        {
            DatabaseManager dbManager = new DatabaseManager();

            return (dbManager.NonReturnQuery("UPDATE Preference SET Preference_Min_Bedrooms = " + minBedrooms + ", Preference_Max_Bedrooms = " + maxBedrooms + ", Preference_Min_Bathrooms = " + minBathrooms + ", Preference_Max_Bathrooms = " + maxBathrooms + ", Preference_Min_Garages = " + minGarages + ", Preference_Max_Garages = " + maxGarages + ", Preference_hasPool = " + hasPool + ", Preference_Min_Plot_Size = " + minPlotSize + ", Preference_Max_Plot_Size = " + maxPlotSize + ", Preference_Min_House_Size = " + minHouseSize + ", Preference_Max_House_Size = " + maxHouseSize + ", Preference_Min_Price = " + minPrice + ", Preference_Max_Price = " + maxPrice + " WHERE  (Preference_Client_ID = " + clientID + " AND Preference_ID = " + preferenceID + ");"));
        }
        public bool DeletePreference(int preferenceID)
        {
            DatabaseManager dbManager = new DatabaseManager();

            return (dbManager.NonReturnQuery("DELETE FROM Preference WHERE Preference_ID = " + preferenceID + ";"));
        }
        public bool DeletePreferenceArea(int preferenceID, string areaIn)
        {
            string[] area;
            string[] dell = new string[] { ", " };
            area = areaIn.Split(dell, System.StringSplitOptions.None);
            DatabaseManager dbManager = new DatabaseManager();
            LocationManager locManager = new LocationManager();

            return (dbManager.NonReturnQuery("DELETE FROM Preference_Area WHERE Preference_ID = " + preferenceID + " AND Area_ID = " + locManager.AreaID(locManager.CityID(area[1], locManager.ProvinceID(area[0])), area[2]) + ";"));
        }
        public bool CanAddPreference(int clientID, int minBedrooms, int maxBedrooms, int minBathrooms, int maxBathrooms, int minGarages, int maxGarages, int minPlotSize, int maxPlotSize, int minHouseSize, int maxHouseSize, int minPrice, int maxPrice, int hasPool)
        {
            DatabaseManager dbManager = new DatabaseManager();

            bool canAdd = true;


            var preferenceEmails = dbManager.ReturnQuery("SELECT Preference_ID FROM Preference WHERE (Preference_Client_ID = " + clientID + " AND Preference_Min_Bedrooms = " + minBedrooms + " AND Preference_Max_Bedrooms = " + maxBedrooms + " AND Preference_Min_Bathrooms = " + minBathrooms + " AND Preference_Max_Bathrooms = " + maxBathrooms + " AND Preference_Min_Garages = " + minGarages + " AND Preference_Max_Garages = " + maxGarages + " AND Preference_hasPool = " + hasPool + " AND Preference_Min_Plot_Size = " + minPlotSize + " AND Preference_Max_Plot_Size = " + maxPlotSize + " AND Preference_Min_House_Size = " + minHouseSize + " AND Preference_Max_House_Size = " + maxHouseSize + " AND Preference_Min_Price = " + minPrice + " AND Preference_Max_Price = " + maxPrice + ");");

            foreach (var preferenceEmail in preferenceEmails)
            {
                canAdd = false;
                break;
            }

            return canAdd;
        }
        public bool ClearPreferenceArea(int preferenceID)
        {
            DatabaseManager dbManager = new DatabaseManager();
            return (dbManager.NonReturnQuery("DELETE FROM Preference_Area WHERE Preference_ID = " + preferenceID + ";"));
        }
        public int GetPreferenceID(int clientID)
        {
            DatabaseManager dbManager = new DatabaseManager();
            int preferenceID = 0;
            var preferences = (dbManager.ReturnQuery("SELECT Preference_ID FROM Preference WHERE Preference_Client_ID = " + clientID.ToString() + " ORDER BY Preference_ID ASC;"));
            foreach (var i in preferences)
            {
                preferenceID = Convert.ToInt32(i[0]);
            }
            return preferenceID;
        }
    }
}
