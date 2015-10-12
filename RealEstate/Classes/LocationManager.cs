using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Classes
{
    class LocationManager
    {

        #region Province

        public bool CanAddProvince(string provinceName)
        {
            bool canAdd = true;

            DatabaseManager dbManager = new DatabaseManager();

            var provinceNames = dbManager.ReturnQuery("SELECT Province_Name FROM Province WHERE Province_Name = '" + provinceName + "';");

            foreach(var province in provinceNames )
            {
                canAdd = false;
                break;
            }

            return canAdd;
        }

        public bool AddProvince(string provinceName)
        {
            DatabaseManager dbManger = new DatabaseManager();

            return dbManger.NonReturnQuery("INSERT INTO Province (Province_name) VALUES ('" + provinceName + "');");
        }

        public bool ChangeProvinceName(string oldProvince, string newProvince)
        {
            DatabaseManager dbManager = new DatabaseManager();

            return dbManager.NonReturnQuery("UPDATE Province SET Province_Name ='" + newProvince + "' WHERE Province_Name = '" + oldProvince +"';");
        }

        private int ProvinceID(string provinceName)
        {
            DatabaseManager dbManager = new DatabaseManager();

            var result = dbManager.ReturnQuery("SELECT Province_ID FROM Province WHERE Province_Name = '" + provinceName + "';");

            int provinceID = -1;

            foreach(var i in result)
            {
                provinceID = int.Parse(i[0]);
                break;
            }

            return provinceID;
        }

        #endregion

        #region City

        public bool CanAddCity(string cityName, string provinceName)
        {
            bool canAdd = true;
            
            DatabaseManager dbManager = new DatabaseManager();

            var results = dbManager.ReturnQuery("SELECT City_Name FROM City WHERE City_Name ='" + cityName + "' AND City_Province_ID = (SELECT Province_ID FROM Province WHERE Province_Name ='" + provinceName + "');" );

            foreach(var i in results)
            {
                canAdd = false;
                break;
            }

            return canAdd;
        }

        public bool AddCity(string cityName, string provinceName)
        {
            DatabaseManager dbManager = new DatabaseManager();
            //return dbManager.NonReturnQuery("INSERT INTO City (City_Name,City_Province_ID) VALUES ('" + cityName + "',(SELECT Province_ID FROM Province WHERE Province_Name = '" + provinceName + "');");        
            return dbManager.NonReturnQuery("INSERT INTO City (City_Name, City_Province_ID) VALUES ('" + cityName + "'," + ProvinceID(provinceName).ToString() + ");");
        }

        public bool EditCity(string oldCityName, string oldProvince, string newCityName, string newProvince)
        {
            DatabaseManager dbManager = new DatabaseManager();

//          UPDATE City SET City_Name = 'Pretoria2', City_Province_ID = (SELECT Province_ID FROM Province WHERE Province_Name = 'Mpumalanga') WHERE City_Name = 'Pretoria' AND City_Province_ID = (SELECT Province_ID FROM Province WHERE Province_Name = 'Gauteng');
            return dbManager.NonReturnQuery("UPDATE City SET City_Name='" + newCityName + "', City_Province_ID = (SELECT Province_ID FROM Province WHERE Province_Name ='" + newProvince + "') WHERE City_Name ='" + oldCityName + "' AND City_Province_ID = (SELECT Province_ID FROM Province WHERE Province_Name = '" + oldProvince +"');");
        }

        private int CityID(string cityName, int provinceID)
        {
            DatabaseManager dbManger = new DatabaseManager();

            int cityID = -1;

            var restult = dbManger.ReturnQuery("SELECT City_ID FROM City WHERE City_Name = '" + cityName + "' AND City_Province_ID = " + provinceID.ToString() + ";");

            foreach( var i in restult )
            {
                cityID = int.Parse(i[0]);
                break;
            }

            return cityID;
            
        }

        #endregion

        #region Area
        public bool CanAddArea(string areaName,string cityName, string provinceName)
        {
            DatabaseManager dbManager = new DatabaseManager();

            bool canAdd = true;

            var result = dbManager.ReturnQuery("SELECT Area_Name FROM Area WHERE Area_Name ='" + areaName + "' AND Area_City_ID = " + CityID(cityName,ProvinceID(provinceName)).ToString() + ";");

            foreach (var i in result)
            {
                canAdd = false;
                break;
            }

            return canAdd;
        }

        public bool AddArea(string areaName, string cityName, string provinceName)
        {
            DatabaseManager dbManager = new DatabaseManager();

            return dbManager.NonReturnQuery("INSERT INTO Area (Area_Name,Area_City_ID) VALUES ('" + areaName + "'," + CityID(cityName,ProvinceID(provinceName)) + " );");
        }

        public bool EditArea(string oldAreaName, string newAreaName, string oldCityName, string newCityName, string oldProvinceName, string newProvinceName)
        {
            DatabaseManager dbManger = new DatabaseManager();

            Console.WriteLine(oldAreaName + "," + oldCityName + "," + oldProvinceName + " to " + newAreaName + "," + newCityName + "," + newProvinceName);

            return dbManger.NonReturnQuery("UPDATE Area SET Area_Name = '" + newAreaName + "' , Area_City_ID = " + CityID(newCityName, ProvinceID(newProvinceName)) + " WHERE Area_Name = '" + oldAreaName + "' AND Area_City_ID = " + CityID(oldCityName, ProvinceID(oldProvinceName)) + ";");
        }

        #endregion
    }
}
