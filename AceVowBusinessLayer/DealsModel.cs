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
    public class DealsModel
    {
        DBEngine objDBEngine;
        DataTable dtResult;
        readonly Utility objUtility = new Utility();

        public List<PreviewDeals> GetSingleProductFlyer(string PreviewJson,int? DealId)
        {
            // int ReturnCode = 0;
            List<PreviewDeals> lstPreviewRes = null;

            try
            {

                SqlParameter[] Param = {
                                            new SqlParameter("@PreviewJson",SqlDbType.NVarChar),
                                            new SqlParameter("@DealId",SqlDbType.Int),

                                      };

                Param[0].Value = PreviewJson;
                Param[1].Value = DealId;

                using (objDBEngine = new DBEngine())
                {
                    dtResult = new DataTable();
                    dtResult = objDBEngine.GetDataTable("pGetFlyerProducts", Param);

                    if (dtResult.Rows.Count > 0)
                    {
                        lstPreviewRes = new List<PreviewDeals>();
                        DTtoListConverter.ConvertTo(dtResult, out lstPreviewRes);
                    }

                }

                // ReturnCode = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstPreviewRes;
        }

        public int GetFlyerPreview(int id, out List<PreviewDeals> lstPreview)
        {
            int ReturnCode = 0;
            lstPreview = null;

            try
            {

                SqlParameter[] Param = {
                                            new SqlParameter("@Id",SqlDbType.Int),

                                      };

                Param[0].Value = id;

                using (objDBEngine = new DBEngine())
                {
                    dtResult = new DataTable();
                    dtResult = objDBEngine.GetDataTable("pGetFlyerPreview", Param);

                    if (dtResult.Rows.Count > 0)
                    {
                        lstPreview = new List<PreviewDeals>();
                        DTtoListConverter.ConvertTo(dtResult, out lstPreview);
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

        public int GetImage(string source)
        {
            int i = 0;

            return i;
        }
    }
}
