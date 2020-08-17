using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using APITest.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APITest 
{

    [ApiController]
    public class LicenceController : ControllerBase
    {
        //API Routing table 
        //Google singleton
        //Add C# class to here in constructor

        public DBCommunicator dbcom = new DBCommunicator();

        [HttpGet]
        [Route("api/GetLicence/{licence}")]
        public IActionResult GetLicenceInfo(string licence) // Controll with licence string
        {

            #region WithC#
            DBCommunicator dbcom = new DBCommunicator();
            return StatusCode(200, dbcom.FetchData(licence));
            //Validate the object before sending.
            #endregion
            #region NoC#class
            //if (licence == "potato")
            //{
            //    DBCommunicator db = new DBCommunicator();
            //    //LicenceInfoModel infoModel = db.FetchData();
            //    LicenceInfoModel infoModel = new LicenceInfoModel
            //    {
            //        LicenceNo = "123",
            //        CustomerNo = "4124",
            //        UniqeKey = "124215215",
            //        DateTo = "2020-12-14"
            //    };
            //    if (string.IsNullOrWhiteSpace(infoModel.LicenceNo))
            //    {
            //        return StatusCode(400, "Shit's on fire");
            //    }

            //    return StatusCode(200, infoModel);
            //}
            //return StatusCode(404); 
            #endregion
        }
        [HttpPost]
        [Route("api/AddNewLicence")]
        public IActionResult AddNewLicence([FromBody] LicenceInfoModel licence)
        {
            bool newLicense = false;

            using (SqlConnection con = new SqlConnection(dbcom.GetConsString()))
            {
                string query = "SELECT Select LicenceNo From LicenceInfo WHERE LicenceNo =" + licence + ";";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int a = reader.GetInt32(0);
                    if(a != 0)
                    {
                        newLicense = true;
                    }
                }
                if(newLicense == true)
                {
                    query = "INSERT INTO LicenceInfo(LicenceNo, UniqueNo, CustomerNo, Experationdate, CompanyName) Values(" + licence.LicenceNo + "," + licence.UniqueKey + "," + licence.CustomerNo + "," + licence.DateTo + "," + licence.CompanyName + ")";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    return StatusCode(200, "Licence Created");
                }
                
            }
            return StatusCode(400, "Licence was not Created");
            
        }
        [HttpPost]
        [Route("api/AddNewDepartment/{licence}")]
        public IActionResult AddNewDepartment([FromBody] string licence)
        {


            using (SqlConnection con = new SqlConnection(dbcom.GetConsString()))
            {
                string query = "Select LicenceNo From LicenceInfo";


                query = "INSERT INTO Departments (DepartmentID,LicenceNo,Users,SupplierName,Address,Phone) Values (1,1000,3,'PCK','Nordre Kullerød','12345678')";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                }
            }
            return StatusCode(200, "Deptartment added to licence" + licence + "");

        }
        
        [HttpPut]
        [Route("api/UpdateLicence/{licence}")]
        public IActionResult UpdateLicence([FromBody] string licence) //Update a licence
        {
            LicenceInfoModel info = dbcom.FetchData(licence);
            string testDate = "2020-12-13";
            DateTime date = DateTime.ParseExact(testDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
            string companyName = "asdewwqe";
            if (string.IsNullOrWhiteSpace(info.LicenceNo))
            {
                return StatusCode(401, "Licence Not Found.");
            }
            using (SqlConnection con = new SqlConnection(dbcom.GetConsString()))
            {

                string query = "UPDATE LicenceInfo SET Experationdate =" + date + ", CompanyName =" + companyName + " WHERE LicenceNo =" + Int32.Parse(licence) + "";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                }
            }
            return StatusCode(200, "Licence Updated.");

        }

    }
}
