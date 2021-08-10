using AceVowEntities;
using AceVowUtility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AceVowBusinessLayer
{
    public class DataModel
    {
        DBEngine objDBEngine;
        DataTable dtResult;
        readonly Utility objUtility = new Utility();
        public int GetClientInfo(string ClientName, string StoreUrl, string City, out List<ClientUser> lstClient)
        {
            int ReturnCode = 0;
            lstClient = null;

            try
            {

                SqlParameter[] Param = {
                                            new SqlParameter("@ClientName",SqlDbType.NVarChar),
                                            new SqlParameter("@StoreUrl",SqlDbType.NVarChar),
                                            new SqlParameter("@City",SqlDbType.NVarChar),

                                      };

                Param[0].Value = ClientName;
                Param[1].Value = StoreUrl;
                Param[2].Value = City;


                using (objDBEngine = new DBEngine())
                {
                    dtResult = new DataTable();
                    dtResult = objDBEngine.GetDataTable("pGetClientInfo", Param);

                    if (dtResult.Rows.Count > 0)
                    {
                        DTtoListConverter.ConvertTo(dtResult, out lstClient);
                    }

                }

                ReturnCode = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ReturnCode;
        }
        public long BulkInsertProdMaster(string Action, string JPramValue, long Createdby, out string Msg)
        {
            int InsRow = 0;
            Msg = string.Empty;
            SqlCommand sqlCommand = new SqlCommand();
            try
            {

                SqlParameter[] Param = {
                                            new SqlParameter("@Action",SqlDbType.NVarChar),
                                            new SqlParameter("@JParamVal",SqlDbType.NVarChar),
                                            new SqlParameter("@UserId",SqlDbType.BigInt),
                                            new SqlParameter("@Message",SqlDbType.NVarChar,5000)
                                      };
                Param[0].Value = Action;
                Param[1].Value = JPramValue;
                Param[2].Value = Createdby;
                Param[3].Direction = ParameterDirection.Output;

                using (objDBEngine = new DBEngine())
                {
                    sqlCommand = objDBEngine.DMLOperationOutPutParam("pBulkInsertProductMaster", Param, out InsRow);

                }

                Msg = (string)sqlCommand.Parameters["@Message"].Value;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return InsRow;
        }

        public int GetLoginInfo(string uname, string pswd)
        {
            int i = 0;
            SqlConnection sqlCon = null;
            String SqlconString = ConfigurationManager.ConnectionStrings["AceVowAdmin"].ConnectionString;
            using (sqlCon = new SqlConnection(SqlconString))
            {
                sqlCon.Open();
                SqlCommand Cmnd = new SqlCommand("SELECT UserName FROM Login WHERE UserName=@Uname and Password=@Pswd", sqlCon);
                Cmnd.Parameters.AddWithValue("@Uname", uname);
                Cmnd.Parameters.AddWithValue("@Pswd", pswd);
                object result = Cmnd.ExecuteScalar();
                if (result != null)
                {
                    string name = result.ToString();
                    i = 1;
                }
                sqlCon.Close();
            }
            return i;
        }

        public int GetAutocompleteCat(string prefixText, string Action, out List<Category> lstCat)
        {
            int ReturnCode = 0;
            lstCat = null;

            try
            {
                SqlParameter[] Param = {
                                            new SqlParameter("@prefixText",SqlDbType.NVarChar),
                                            new SqlParameter("@Action",SqlDbType.NVarChar),

                                      };

                Param[0].Value = prefixText;
                Param[1].Value = Action;

                objDBEngine = new DBEngine();

                using (objDBEngine = new DBEngine())
                {
                    dtResult = new DataTable();

                    dtResult = objDBEngine.GetDataTable("pGetAutocompleteCatName", Param);

                    if (dtResult.Rows.Count > 0)
                    {
                        lstCat = dtResult.AsEnumerable().Select(U => new Category()
                        {
                            Name = U.Field<string>("Name"),
                            ParentCategory = U.Field<string>("ParentCategory")
                        }
                        ).ToList();
                    }

                }

                ReturnCode = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ReturnCode;
        }
        public long DMLCatMaster(string JPramValue)
        {
            long RIMAsterID = 0;
            int InsRow = 0;
            SqlCommand sqlCommand = new SqlCommand();
            try
            {
                SqlParameter[] Param = {
                                            new SqlParameter("@JParamVal",SqlDbType.NVarChar),
                                            new SqlParameter("@ReturnRIid",SqlDbType.BigInt)
                                      };

                Param[0].Value = JPramValue;
                Param[1].Direction = ParameterDirection.Output;
                using (objDBEngine = new DBEngine())
                {
                    sqlCommand = objDBEngine.DMLOperationOutPutParam("pDMLCategoryMaster", Param, out InsRow);
                }
                RIMAsterID = (long)sqlCommand.Parameters["@ReturnRIid"].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RIMAsterID;
        }
        public int GetClientUser(int Id, out ClientUser obj)
        {
            int ReturnCode = 0;
            obj = null;
            List<Category> lst = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                SqlParameter[] Param = {
                                            new SqlParameter("@Id",SqlDbType.Int),

                                      };

                Param[0].Value = Id;

                objDBEngine = new DBEngine();

                using (objDBEngine = new DBEngine())
                {
                    DataSet ds = new DataSet();

                    ds = objDBEngine.GetDataSet("pGetClientInfo_Update", Param);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row =ds.Tables[0].Rows[0];
                        obj = new ClientUser
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            Industry = row["Industry"].ToString(),
                            StoreName = row["StoreName"].ToString(),
                            StoreURL = row["StoreURL"].ToString(),
                            ContactName = row["ContactName"].ToString(),
                            Positon = row["Positon"].ToString(),
                            PirmaryContact = row["PirmaryContact"].ToString(),
                            EmailId = row["EmailId"].ToString(),
                            Password = row["Password"].ToString(),
                            AddressLine1 = row["AddressLine1"].ToString(),
                            AddressLine2 = row["AddressLine2"].ToString(),
                            City = row["City"].ToString(),
                            TargetedCities = row["TargetedCities"].ToString(),
                            TargetedCommunities = row["TargetedCommunities"].ToString(),
                            WelcomeMessage = row["WelcomeMessage"].ToString(),
                            TrolleryCount = Convert.ToInt32(row["TrolleryCount"]),
                            BasketCount = Convert.ToInt32(row["BasketCount"]),
                            UserId = Convert.ToInt32(row["UserId"]),
                            PostalCode = row["PostalCode"].ToString(),
                            ClientLogo = row["ClientLogo"].ToString(),
                            ClientBanner = row["ClientBanner"].ToString(),
                            ClientFBUrl = row["ClientFBUrl"].ToString(),
                            ClientInstaUrl = row["ClientInstaUrl"].ToString(),
                            ClientTwitterUrl = row["ClientTwitterUrl"].ToString()
                        };

                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        lst = new List<Category>();
                        DTtoListConverter.ConvertTo(ds.Tables[1], out lst);
                    }
                    obj.lstCategory = lst;
                }

                ReturnCode = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ReturnCode;
        }
        public long DMLUserMaster(string JPramValue)
        {
            long RIMAsterID = 0;
            int InsRow = 0;
            SqlCommand sqlCommand = new SqlCommand();
            try
            {
                SqlParameter[] Param = {
                                            new SqlParameter("@JParamVal",SqlDbType.NVarChar),
                                            new SqlParameter("@ReturnRIid",SqlDbType.BigInt)
                                      };

                Param[0].Value = JPramValue;
                Param[1].Direction = ParameterDirection.Output;
                using (objDBEngine = new DBEngine())
                {
                    sqlCommand = objDBEngine.DMLOperationOutPutParam("pDMLClient", Param, out InsRow);
                }
                RIMAsterID = (long)sqlCommand.Parameters["@ReturnRIid"].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RIMAsterID;
        }
        public long GetQRCount(int? UId, DateTime FromDate, DateTime ToDate)
        {
            long returnCode = -1;
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection con = new SqlConnection(objUtility.GetConnectionString()))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand
                    {
                        CommandText = "GetQRCountforDashBoard" 
                    };

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;

                  
                    cmd.Parameters.Add("@FDate", SqlDbType.DateTime).Value = FromDate;
                    cmd.Parameters.Add("@TDate", SqlDbType.DateTime).Value = ToDate;
                    cmd.Parameters.AddWithValue("@UserId", UId);
                    returnCode = Convert.ToInt64(cmd.ExecuteScalar());
                    cmd.Dispose();

                    //SqlDataAdapter sdaAdapter = new SqlDataAdapter
                    //{
                    //    SelectCommand = cmd
                    //};
                    //sdaAdapter.Fill(ds);

                    //if (ds.Tables[0].Rows.Count > 0)
                    //{
                    //    DataRow row = ds.Tables[0].Rows[0];

                    //    returnCode = Convert.ToInt64(row["Id"]);
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnCode;
        }
    }
}
