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
            Console.WriteLine("INSERT INTO Listing (Property_ID, Agent_ID, List_Price, List_isNegotiable, List_isSold) VALUES (" + propertyID + "," + agentID + "," + listPrice + "," + isNegotiable + "," + isSold + ");");
            return dbManager.NonReturnQuery("INSERT INTO Listing (Property_ID, Agent_ID, List_Price, List_isNegotiable, List_isSold VALUES (" + propertyID + "," + agentID + "," + listPrice + "," + isNegotiable + "," + isSold + ");");
        }
        public bool AddListingProperty(int clientID, int addressID, int complexID, int propertyUnitNo, int bedroomCount, int bathroomCount, int garageCount, int hasPool, int plotSize, int houseSize, int propertyValue)
        {
            DatabaseManager dbManager = new DatabaseManager();
            return (dbManager.NonReturnQuery("INSERT INTO Property (Client_ID, Address_ID, Complex_ID, Property_Unit_No, Property_Bedroom_Count, Property_Bathroom_Count, Property_Garage_Count, Property_hasPool, Property_Plot_size, Property_House_Size, Property_Value) values (" + clientID + "," + addressID + "," + complexID + "," + propertyUnitNo + "," + bedroomCount + "," + bathroomCount + "," + garageCount +"," + hasPool + "," + plotSize + "," + houseSize + "," + propertyValue + ");"));
        }
        public bool AddListingAddress(int areaID, string streetName, int streetNo)
        {
            DatabaseManager dbManager = new DatabaseManager();
            return (dbManager.NonReturnQuery("INSERT INTO Address (Area_ID, Address_Streetname, Address_Streetno) values (" + areaID + ",'" + streetName + "'," + streetNo +");"));
        }
        public bool AddListingComplex(string complexName, int addressID)
        {
            DatabaseManager dbManager = new DatabaseManager();
            return (dbManager.NonReturnQuery("INSERT INTO Complex (Complex_Name, Address_ID) values ('" + complexName + "'," + addressID + ");"));
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

        public List<string> GetClients()
        {
            DatabaseManager dbManager = new DatabaseManager();
            List<string> clients_Email = new List<string>();
            var clients = (dbManager.ReturnQuery("SELECT Client_Name , Client_Email FROM Clients"));
            string clientName, clientEmail;
            foreach (var i in clients)
            {
                clientName = Convert.ToString(i[0]);
                clientEmail = Convert.ToString(i[1]);
                clients_Email.Add(clientName + ", " + clientEmail);
            }
            return clients_Email;
        }

        public List<string> GetProvinces()
        {
            DatabaseManager dbManager = new DatabaseManager(); 
            List<string> province_Names = new List<string>();
            var provinces = (dbManager.ReturnQuery("SELECT Province_Name FROM Province"));
            string provinceName;
            foreach (var i in provinces)
            {
                provinceName = Convert.ToString(i[0]);
                province_Names.Add(provinceName);
            }
            return province_Names;
        }
        public List<string> GetCities(int city_Province_ID)
        {
            DatabaseManager dbManager = new DatabaseManager(); 
            List<string> city_Names = new List<string>();
            var citiesName = (dbManager.ReturnQuery("SELECT City_Name FROM City WHERE City_Province_ID = " + city_Province_ID + ""));
            string cityName;
            foreach (var i in citiesName)
            {
                cityName = Convert.ToString(i[0]);
                city_Names.Add(cityName);
            }
            return city_Names;
        }
        public List<string> GetAreas(int area_City_ID)
        {
            DatabaseManager dbManager = new DatabaseManager();
            List<string> area_Names = new List<string>();
            var areasName = (dbManager.ReturnQuery("SELECT Area_Name FROM Area WHERE Area_City_ID = " + area_City_ID + ""));
            string areaName;
            foreach (var i in areasName)
            {
                areaName = Convert.ToString(i[0]);
                area_Names.Add(areaName);
            }
            return area_Names;
        }
        public List<int> GetClientsID()
        {
            DatabaseManager dbManager = new DatabaseManager();
            List<int> clients_ID = new List<int>();
            var clientsID = (dbManager.ReturnQuery("SELECT Client_ID FROM Clients"));
            int clientID;
            foreach (var i in clientsID)
            {
                clientID = Convert.ToInt32(i[0]);
                clients_ID.Add(clientID);
            }
            return clients_ID;
        }
        public List<int> GetProvincesID()
        {
            DatabaseManager dbManager = new DatabaseManager();
            List<int> provinces_ID = new List<int>();
            var provincesID = (dbManager.ReturnQuery("SELECT Province_ID FROM Province"));
            int provinceID;
            foreach (var i in provincesID)
            {
                provinceID = Convert.ToInt32(i[0]);
                provinces_ID.Add(provinceID);
            }
            return provinces_ID;
        }
        public List<int> GetCitiesID(int city_Province_ID)
        {
            DatabaseManager dbManager = new DatabaseManager();
            List<int> cities_ID = new List<int>();
            var citiesID = (dbManager.ReturnQuery("SELECT City_ID FROM City WHERE City_Province_ID = " + city_Province_ID + ""));
            int cityID;
            foreach (var i in citiesID)
            {
                cityID = Convert.ToInt32(i[0]);
                cities_ID.Add(cityID);
            }
            return cities_ID;
        }
        public List<int> GetAreasID(int area_City_ID)
        {
            DatabaseManager dbManager = new DatabaseManager();
            List<int> areas_ID = new List<int>();
            var areasID = (dbManager.ReturnQuery("SELECT Area_ID FROM Area WHERE Area_City_ID = " + area_City_ID + ""));
            int areaID;
            foreach (var i in areasID)
            {
                areaID = Convert.ToInt32(i[0]);
                areas_ID.Add(areaID);
            }
            return areas_ID;
        }
        public int GetAddressID(int AreaID, string streetName, int streetNo)
        {
            DatabaseManager dbManager = new DatabaseManager();
            int address_ID = -1;
            var addressID = (dbManager.ReturnQuery("SELECT Address_ID FROM Address WHERE (Area_ID = " + AreaID + " AND Address_Streetname = '" + streetName + "' AND Address_StreetNo = " + streetNo + ")"));
            foreach(var i in addressID)
            {
                address_ID = Convert.ToInt32(i[0]);
            }
            return address_ID;
        }
        public int GetComplexID(string complexName,int addressID)
        {
            DatabaseManager dbManager = new DatabaseManager();
            int complex_ID = -1;
            var complexID = (dbManager.ReturnQuery("SELECT Complex_ID FROM Complex WHERE (Complex_Name = '" + complexName + "' AND Address_ID = " + addressID + ")"));
            foreach (var i in complexID)
            {
                complex_ID = Convert.ToInt32(i[0]);
            }
            return complex_ID;
          
        }
        public int GetPropertyID(int clientID)
        {
            DatabaseManager dbManager = new DatabaseManager();
            int propertyID = 0;
            var properties = (dbManager.ReturnQuery("SELECT Property_ID FROM Property WHERE Client_ID = " + clientID.ToString() + " ORDER BY Property_ID ASC;"));
            foreach (var i in properties)
            {
                propertyID = Convert.ToInt32(i[0]);
            }
            return propertyID;
        }
        public int GetAgentID(string agentEmail)
        {
            DatabaseManager dbManager = new DatabaseManager();
            int agentID = -1;
            var agent = (dbManager.ReturnQuery("SELECT Agent_ID FROM Agent WHERE Agent_Email = '" + agentEmail + "'"));
            foreach (var i in agent)
            {
                agentID = Convert.ToInt32(i[0]);
            }
            return agentID;
        }
    }
}
