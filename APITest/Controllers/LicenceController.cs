using System;
using System.Collections.Generic;
using System.Data;
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
        [Route("api/GetLicenceInfo/{licence}")]
        public IActionResult GetLicenceInfo(string licence) // Controll with licence string
        {
            #region WithC#

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
        [Route("api/GetLicenceWithUniqueKey/{uniqueKey}")]
        public IActionResult GetLicenceWithUniqueKey(string uniqueKey) // Controll with licence string
        {
            return StatusCode(200, dbcom.FetchDataOnUniqueKey(uniqueKey));
        }
        [HttpPost]
        [Route("api/AddNewLicence")]
        public IActionResult AddNewLicence([FromBody] LicenceInfoModel licence)
        {
            //Console.WriteLine($"Got a InfoModel: {licence.LicenceNo}!");
            //return new AcceptedResult();
            bool newLicense = true;

            using (SqlConnection con = new SqlConnection(dbcom.GetConsString()))
            {
                con.Open();
                string query = "SELECT LicenceNo From LicenceInfo WHERE LicenceNo =" + licence.LicenceNo + ";";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    newLicense = false;
                    return StatusCode(401, "Licence already exists");
                }
                reader.Close();
                if (newLicense == true)
                {

                    query = "INSERT INTO LicenceInfo(LicenceNo, UniqueNo, CustomerNo, Experationdate, CompanyName) Values(@param1,@param2,@param3,@param4,@param5)";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.Add("@param1", SqlDbType.Int).Value = licence.LicenceNo;
                    cmd.Parameters.Add("@param2", SqlDbType.NVarChar).Value = licence.UniqueKey;
                    cmd.Parameters.Add("@param3", SqlDbType.Int).Value = licence.CustomerNo;
                    cmd.Parameters.Add("@param4", SqlDbType.Date).Value = licence.DateTo;
                    cmd.Parameters.Add("@param5", SqlDbType.NVarChar).Value = licence.CompanyName;
                    cmd.ExecuteNonQuery();
                    return StatusCode(200, "Licence Created");
                }

            }
            return StatusCode(400, "Licence was not Created");

        }
        [HttpPost]
        [Route("api/AddNewDepartment")]
        public IActionResult AddNewDepartment([FromBody] DepartmentModel department)
        {
            bool newDepartment = false;
            using (SqlConnection con = new SqlConnection(dbcom.GetConsString()))
            {
                string query = "Select LicenceInfo.LicenceNo, Departments.DepartmentID From LicenceInfo LEFT JOIN Departments ON LicenceInfo.LicenceNo = Departments.LicenceNo WHERE LicenceInfo.LicenceNo=" + department.LicenceNo;

                con.Open();

                SqlCommand cmd = new SqlCommand(query, con);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if(reader.IsDBNull(1) == true)
                    {
                        newDepartment = true;
                    }
                    if(reader.IsDBNull(1) == false)
                    {
                        if(reader.GetInt32(1) == department.DepartmentId)
                        {
                            newDepartment = false;
                            return StatusCode(401, "Deparmentnumber already exists for that licence");
                        }
                        
                    } 
                    newDepartment = true;
                }
                reader.Close();
                if (newDepartment == true)
                {
                    query = "INSERT INTO Departments (DepartmentID,LicenceNo,Users,SupplierName,Address,Email,Phone) Values (@param1,@param2,@param3,@param4,@param5,@param6,@param7)";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.Add("@param1", SqlDbType.Int).Value = department.DepartmentId;
                    cmd.Parameters.Add("@param2", SqlDbType.Int).Value = department.LicenceNo;
                    cmd.Parameters.Add("@param3", SqlDbType.Int).Value = department.Users;
                    cmd.Parameters.Add("@param4", SqlDbType.NVarChar).Value = department.SupplierName;
                    cmd.Parameters.Add("@param5", SqlDbType.NVarChar).Value = department.StoreAddress;
                    cmd.Parameters.Add("@param6", SqlDbType.NVarChar).Value = department.Email;
                    cmd.Parameters.Add("@param7", SqlDbType.NVarChar).Value = department.Phone;
                    cmd.ExecuteNonQuery();
                    return StatusCode(200, "Department created for licence: " + department.LicenceNo + "");
                }
            }
            
            return StatusCode(400, "Deptartment: " + department.DepartmentId + " could not be added to licence" + department.LicenceNo + ". Licence might not exist");

        }
        [HttpPut]
        [Route("api/UpdateLicence")]
        public IActionResult UpdateLicence([FromBody] LicenceInfoModel licence) //Update a licence
        {
            LicenceInfoModel info = dbcom.FetchData(licence.LicenceNo.ToString());
            //string testDate = "2020-12-13";
            //DateTime date = DateTime.ParseExact(testDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
            if (string.IsNullOrWhiteSpace(info.LicenceNo.ToString()))
            {
                return StatusCode(401, "Licence Not Found.");
            }
            using (SqlConnection con = new SqlConnection(dbcom.GetConsString()))
            {
                con.Open();
                string query = "UPDATE LicenceInfo " +
                    "SET Experationdate = @expdate WHERE LicenceNo = " + licence.LicenceNo + "";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.Add("@expdate", SqlDbType.Date).Value = licence.DateTo;
                cmd.ExecuteNonQuery();
                return StatusCode(200, "Experationdate has been updated to:" + licence.DateTo + "");
                
            }
            
        }
        [HttpPut]
        [Route("api/UpdateDepartment")]
        public IActionResult UpdateDepartment([FromBody] DepartmentModel department) //Update a licence
        {
            LicenceInfoModel info = dbcom.FetchData(department.LicenceNo.ToString());
            //string testDate = "2020-12-13";
            //DateTime date = DateTime.ParseExact(testDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
            if (string.IsNullOrWhiteSpace(info.LicenceNo.ToString())) //TEST HERE -> 0 or null instead?
            {
                return StatusCode(401, "Licence Not Found.");
            }
            using (SqlConnection con = new SqlConnection(dbcom.GetConsString()))
            {
                con.Open();
                string query = "UPDATE Departments " +
                    "SET Users = @users WHERE LicenceNo =" + department.LicenceNo + " AND DepartmentId = " + department.DepartmentId + "";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.Add("@Users", SqlDbType.Int).Value = department.Users;
                cmd.ExecuteNonQuery();
                return StatusCode(200, "Department info updated!");

            }

        }

    }

}

