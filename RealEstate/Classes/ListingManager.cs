using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Classes
{
    class ListingManager
    {
        #region Add
        public bool AddListing(int propertyID, int agentID, int listPrice, int isNegotiable, int isSold)
        {
            DatabaseManager dbManager = new DatabaseManager();

            return dbManager.NonReturnQuery("INSERT INTO Listing (Property_ID, Agent_ID, List_Price, List_isNegotiable, List_isSold VALUES (" + propertyID + "," + agentID + "," + listPrice + "," + isNegotiable + "," + isSold + ");");
        }
        public bool AddListingProperty(int clientID, int addressID, int complexID, int propertyUnitNo, int bedroomCount, int bathroomCount, int garageCount, int hasPool, int plotSize, int houseSize, int propertyValue)
        {
            DatabaseManager dbManager = new DatabaseManager();
            return (dbManager.NonReturnQuery("INSERT INTO Property (Client_ID, Address_ID, Complex_ID, Property_Unit_No, Property_Bedroom_Count, Property_Bathroom_Count, Property_hasPool, Property_Plot_size, Property_House_Size, Property_Value) values (" + clientID + "," + addressID + "," + complexID + "," + propertyUnitNo + "," + bedroomCount + "," + bathroomCount + "," + hasPool + "," + plotSize + "," + houseSize + "," + propertyValue + ");"));
        }
        public bool AddListingAddress(int addressID, int areaID, string streetName, int streetNo, int latitude, int longitude)
        {
            DatabaseManager dbManager = new DatabaseManager();
            return (dbManager.NonReturnQuery("INSERT INTO Address (Area_ID, Adress_Streetname, Adress_Streetno, Adress_Latitude, Adress_Longitude) values (" + areaID + "," + streetName + "," + streetNo + "," + latitude + "," + longitude +");"));
        }
        public bool AddListingComplex(int complexName, int addressID)
        {
            DatabaseManager dbManager = new DatabaseManager();
            return (dbManager.NonReturnQuery("INSERT INTO Complex (Complex_Name, Adress_ID) values (" + complexName + "," + addressID + ");"));
        }
        public bool AddListingImage(int propertyID, int url, string caption)
        {
            DatabaseManager dbManager = new DatabaseManager();
            return (dbManager.NonReturnQuery("INSERT INTO Image (Property_ID, Image_URL, Image_Caption) values (" + propertyID + "," + url + "," + caption + ");"));
        }
        #endregion
        
        
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
