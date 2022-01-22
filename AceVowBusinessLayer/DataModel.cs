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
        public long BulkInsertSchedulePosts(string Action, string JPramValue, string Createdby, out string Msg)
        {
            int InsRow = 0;
            Msg = string.Empty;
            SqlCommand sqlCommand = new SqlCommand();
            try
            {

                SqlParameter[] Param = {
                                            new SqlParameter("@Action",SqlDbType.NVarChar),
                                            new SqlParameter("@JParamVal",SqlDbType.NVarChar),
                                            new SqlParameter("@UserId",SqlDbType.NVarChar),
                                            new SqlParameter("@Message",SqlDbType.NVarChar,5000)
                                      };
                Param[0].Value = Action;
                Param[1].Value = JPramValue;
                Param[2].Value = Createdby;
                Param[3].Direction = ParameterDirection.Output;

                using (objDBEngine = new DBEngine())
                {
                    sqlCommand = objDBEngine.DMLOperationOutPutParam("pBulkInsertPostMaster", Param, out InsRow);

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
        public int GetClientUser(int Id, long IndustryId, out ClientUser obj)
        {
            int ReturnCode = 0;
            obj = new ClientUser();
            List<Category> lst = null;
            List<Industry> lstIndures = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                SqlParameter[] Param = {
                                            new SqlParameter("@Id",SqlDbType.Int),
                                            new SqlParameter("@IndustryId",SqlDbType.BigInt),

                                      };

                Param[0].Value = Id;
                Param[1].Value = IndustryId;


                objDBEngine = new DBEngine();

                using (objDBEngine = new DBEngine())
                {
                    DataSet ds = new DataSet();

                    ds = objDBEngine.GetDataSet("pGetClientInfo_Update", Param);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        List<ClientUser> lstCount = new List<ClientUser>();
                        DTtoListConverter.ConvertTo(ds.Tables[0], out lstCount);
                        
                        obj = lstCount[0];


                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        lst = new List<Category>();
                        DTtoListConverter.ConvertTo(ds.Tables[1], out lst);
                        
                        obj.lstCategory = lst;
                    }
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        lstIndures = new List<Industry>();
                        DTtoListConverter.ConvertTo(ds.Tables[2], out lstIndures);
                        
                        obj.lstIndustry = lstIndures;
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
                        CommandText = "pGetQRCountforDashBoard"
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
        public int GetDeals(out List<ClientUser> lstDeals)
        {
            int ReturnCode = 0;
            lstDeals = null;

            try
            {

                SqlParameter[] Param = {
                                            new SqlParameter("@ClientName",SqlDbType.NVarChar),
                                      };

                Param[0].Value = "admin";
                using (objDBEngine = new DBEngine())
                {
                    dtResult = new DataTable();
                    dtResult = objDBEngine.GetDataTable("pGetDealsInfo", Param);

                    if (dtResult.Rows.Count > 0)
                    {
                        DTtoListConverter.ConvertTo(dtResult, out lstDeals);
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
        public long BulkInsertRecipeMaster(string Action, string JPramValue, out string Msg)
        {
            int InsRow = 0;
            Msg = string.Empty;
            SqlCommand sqlCommand = new SqlCommand();
            try
            {

                SqlParameter[] Param = {
                                            new SqlParameter("@Action",SqlDbType.NVarChar),
                                            new SqlParameter("@JParamVal",SqlDbType.NVarChar),
                                            new SqlParameter("@Message",SqlDbType.NVarChar,5000)
                                      };
                Param[0].Value = Action;
                Param[1].Value = JPramValue;
                Param[2].Direction = ParameterDirection.Output;

                using (objDBEngine = new DBEngine())
                {
                    sqlCommand = objDBEngine.DMLOperationOutPutParam("pBulkInsertRecipeMaster", Param, out InsRow);

                }

                Msg = (string)sqlCommand.Parameters["@Message"].Value;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return InsRow;
        }
        public int GetComments(int Id, out List<ClientUser> lstClientUser)
        {
            int ReturnCode = 0;
            lstClientUser = null;

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

                    ds = objDBEngine.GetDataSetSM_Testing("pGetPostComments", Param);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lstClientUser = new List<ClientUser>();
                        DTtoListConverter.ConvertTo(ds.Tables[0], out lstClientUser);
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
        public long UpdatePostComment(int Id, string ReplyComments, out string Msg)
        {
            int InsRow = 0;
            Msg = string.Empty;
            SqlCommand sqlCommand = new SqlCommand();
            try
            {

                SqlParameter[] Param = {
                                            new SqlParameter("@Id",SqlDbType.Int),
                                            new SqlParameter("@ReplyComments",SqlDbType.NVarChar),
                                            new SqlParameter("@Message",SqlDbType.NVarChar,5000)
                                      };
                Param[0].Value = Id;
                Param[1].Value = ReplyComments ?? "";

                Param[2].Direction = ParameterDirection.Output;

                using (objDBEngine = new DBEngine())
                {
                    sqlCommand = objDBEngine.DMLOperationOutPutParam_SMTesting("pUpdatePostComment", Param, out InsRow);

                }

                Msg = (string)sqlCommand.Parameters["@Message"].Value;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return InsRow;
        }
        public long DMLRecipeMaster(string JPramValue)
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
                    sqlCommand = objDBEngine.DMLOperationOutPutParam("pDMLRecipe", Param, out InsRow);
                }
                RIMAsterID = (long)sqlCommand.Parameters["@ReturnRIid"].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RIMAsterID;
        }
        public int GetRecipeforUpdate(int UserId, out List<Recipes> lstRecipe)
        {
            int ReturnCode = 0;
            lstRecipe = null;
            try
            {

                SqlParameter[] Param = { new SqlParameter("@UserId", SqlDbType.Int) };

                Param[0].Value = UserId;

                using (objDBEngine = new DBEngine())
                {
                    dtResult = new DataTable();
                    dtResult = objDBEngine.GetDataTable("pGetRecipeforUpdate", Param);

                    if (dtResult.Rows.Count > 0)
                    {
                        lstRecipe = new List<Recipes>();
                        DTtoListConverter.ConvertTo(dtResult, out lstRecipe);
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
        public int GetUserList(out List<ClientUser> lstUsers)
        {
            int ReturnCode = 0;
            lstUsers = null;

            SqlCommand cmd = new SqlCommand();
            try
            {
                SqlParameter[] Param = {
                                            new SqlParameter("@Id",SqlDbType.Int),

                                      };

                Param[0].Value = 0;

                objDBEngine = new DBEngine();

                using (objDBEngine = new DBEngine())
                {
                    DataSet ds = new DataSet();

                    ds = objDBEngine.GetDataSet("pGetUsers", Param);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lstUsers = new List<ClientUser>();
                        DTtoListConverter.ConvertTo(ds.Tables[0], out lstUsers);
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
        public int GetCategoryList(out List<Category> lstCategory, int UserId)
        {
            int ReturnCode = 0;
            lstCategory = null;

            SqlCommand cmd = new SqlCommand();
            try
            {
                SqlParameter[] Param = {
                                            new SqlParameter("@UserId",SqlDbType.Int),

                                      };

                Param[0].Value = UserId;

                objDBEngine = new DBEngine();

                using (objDBEngine = new DBEngine())
                {
                    DataSet ds = new DataSet();

                    ds = objDBEngine.GetDataSet("pGetCategory", Param);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lstCategory = new List<Category>();
                        DTtoListConverter.ConvertTo(ds.Tables[0], out lstCategory);
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

        public int DeactivateClient(ClientUser objUser)
        {
            int intResult = 0;
            try
            {

                SqlParameter[] Param = {
                                            new SqlParameter("@UserId",SqlDbType.Int),
                                            new SqlParameter("@Status",SqlDbType.Bit),

                                      };
                Param[0].Value = objUser.UserId;
                Param[1].Value = objUser.IsActive;
                using (objDBEngine = new DBEngine())
                {
                    intResult = objDBEngine.DMLOperation("pDeactivateClient", Param);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return intResult;
        }
        public int DeactivateRecipe(Recipes objRecipe)
        {
            int intResult = 0;
            try
            {

                SqlParameter[] Param = {
                                            new SqlParameter("@RecipeId",SqlDbType.Int),
                                            new SqlParameter("@Status",SqlDbType.Bit),

                                      };
                Param[0].Value = objRecipe.RecipeId;
                Param[1].Value = objRecipe.IsActive;
                using (objDBEngine = new DBEngine())
                {
                    intResult = objDBEngine.DMLOperation("pDeactivateRecipe", Param);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return intResult;
        }
        public long DMLProductMaster(string JPramValue)
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
                    sqlCommand = objDBEngine.DMLOperationOutPutParam("pDMLProductMaster", Param, out InsRow);
                }
                RIMAsterID = (long)sqlCommand.Parameters["@ReturnRIid"].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RIMAsterID;
        }
    }
}
