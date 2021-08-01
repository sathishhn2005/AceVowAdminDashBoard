using AceVowEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceVowBusinessLayer
{
    public class DataModel
    {
        DBEngine objDBEngine;
        DataTable dtResult;
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
                        lstClient = dtResult.AsEnumerable().Select(U => new ClientUser()
                        {
                            Id = U.Field<int>("Id"),
                            StoreName = U.Field<string>("StoreName"),
                            ContactName = U.Field<string>("ContactName"),
                            StoreURL = U.Field<string>("StoreURL"),
                            Positon = U.Field<string>("Positon"),
                            EmailId = U.Field<string>("EmailId"),
                            AddressLine1 = U.Field<string>("Address"),
                            City = U.Field<string>("City"),
                           

                        }).ToList();
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
    }
}
