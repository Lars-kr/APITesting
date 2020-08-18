using APITest.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace APITest
{
    public class DBCommunicator
    {
        public DBCommunicator()
        {
           
        }
        public LicenceInfoModel FetchData(string licenceNo)
        {
            
            LicenceInfoModel licence = new LicenceInfoModel();
            using (SqlConnection con = new SqlConnection(GetConsString()))
            {
                string query = "SELECT * FROM LicenceInfo WHERE LicenceNo=" + Int32.Parse(licenceNo) + "";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    licence.LicenceNo = reader.GetInt32(1);
                    licence.UniqueKey = reader.GetString(2);
                    licence.CustomerNo = reader.GetInt32(3);
                    licence.DateTo = reader.GetDateTime(4);
                    licence.CompanyName = reader.GetString(5);
                }
            }
            return licence;
        }

        public string GetConsString()
        {
            //return String.Format(@"Server=DESKTOP-6SL0J8D\SQLEXPRESS;Database=ERPPOSLicenceDB;Trusted_Connection=True;");
            return String.Format(@"Server=DESKTOP-4Q6ALMB\LARS;Database=ERPPOSLicenceDB;Trusted_Connection=True;");
        }

    }
}

